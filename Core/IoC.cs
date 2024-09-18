// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Project.Core;

public class ServiceContainer : ServiceContainer.IScope
{
    private static Lazy<ServiceContainer> _current;

    // Map of registered types
    private readonly Dictionary<Type, Func<ILifetime, object>> _registeredTypes = new Dictionary<Type, Func<ILifetime, object>>();

    // Lifetime management
    private readonly ContainerLifetime _lifetime;

    /// <summary>
    /// Creates a new instance of IoC Container
    /// </summary>
    public ServiceContainer() => _lifetime = new ContainerLifetime(t => _registeredTypes[t]);

    /// <summary>
    /// Represents a scope in which per-scope objects are instantiated a single time
    /// </summary>
    public interface IScope : IDisposable, IServiceProvider
    {
    }

    /// <summary>
    /// IRegisteredType is return by Container.Register and allows further configuration for the registration
    /// </summary>
    public interface IRegisteredType
    {
        /// <summary>
        /// Make registered type a singleton
        /// </summary>
        void AsSingleton();

        /// <summary>
        /// Make registered type a per-scope type (single instance within a Scope)
        /// </summary>
        void PerScope();
    }

    // ILifetime management adds resolution strategies to an IScope
    private interface ILifetime : IScope
    {
        object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory);

        object GetServicePerScope(Type type, Func<ILifetime, object> factory);
    }

    public static ServiceContainer Current
    {
        get
        {
            if (_current == null)
            {
                _current = new(() => new());
            }

            return _current.Value;
        }
    }

    /// <summary>
    /// Registers a factory function which will be called to resolve the specified interface
    /// </summary>
    /// <param name="interface">Interface to register</param>
    /// <param name="factory">Factory function</param>
    /// <returns></returns>
    public IRegisteredType Register(Type @interface, Func<object> factory)
        => RegisterType(@interface, _ => factory());

    /// <summary>
    /// Registers an implementation type for the specified interface
    /// </summary>
    /// <param name="interface">Interface to register</param>
    /// <param name="implementation">Implementing type</param>
    /// <returns></returns>
    public IRegisteredType Register(Type @interface, Type implementation)
        => RegisterType(@interface, FactoryFromType(implementation));

    /// <summary>
    /// Returns the object registered for the given type, if registered
    /// </summary>
    /// <param name="type">Type as registered with the container</param>
    /// <returns>Instance of the registered type, if registered; otherwise <see langword="null"/></returns>
    public object GetService(Type type)
    {
        Func<ILifetime, object> registeredType;

        if (!_registeredTypes.TryGetValue(type, out registeredType))
        {
            //ToDo: replace with activator.createinstance with default ctor
            return FactoryFromType(type).Invoke(_lifetime);
        }

        return registeredType(_lifetime);
    }

    /// <summary>
    /// Creates a new scope
    /// </summary>
    /// <returns>Scope object</returns>
    public IScope CreateScope() => new ScopeLifetime(_lifetime);

    /// <summary>
    /// Disposes any <see cref="IDisposable"/> objects owned by this container.
    /// </summary>
    public void Dispose() => _lifetime.Dispose();

    // Compiles a lambda that calls the given type's first constructor resolving arguments
    private static Func<ILifetime, object> FactoryFromType(Type itemType)
    {
        // Get first constructor for the type
        var constructors = itemType.GetConstructors();
        if (constructors.Length == 0)
        {
            // If no public constructor found, search for an internal constructor
            constructors = itemType.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
        }
        var constructor = constructors.First();

        // Compile constructor call as a lambda expression
        var arg = Expression.Parameter(typeof(ILifetime));
        return (Func<ILifetime, object>)Expression.Lambda(
            Expression.New(constructor, constructor.GetParameters().Select(
                param =>
                {
                    var resolve = new Func<ILifetime, object>(
                        lifetime => lifetime.GetService(param.ParameterType));
                    return Expression.Convert(
                        Expression.Call(Expression.Constant(resolve.Target), resolve.Method, arg),
                        param.ParameterType);
                })),
            arg).Compile();
    }

    private IRegisteredType RegisterType(Type itemType, Func<ILifetime, object> factory)
                        => new RegisteredType(itemType, f => _registeredTypes[itemType] = f, factory);

    // ObjectCache provides common caching logic for lifetimes
    private abstract class ObjectCache
    {
        // Instance cache
        private readonly ConcurrentDictionary<Type, object> _instanceCache = new ConcurrentDictionary<Type, object>();

        public void Dispose()
        {
            foreach (var obj in _instanceCache.Values)
                (obj as IDisposable)?.Dispose();
        }

        // Get from cache or create and cache object
        protected object GetCached(Type type, Func<ILifetime, object> factory, ILifetime lifetime)
            => _instanceCache.GetOrAdd(type, _ => factory(lifetime));
    }

    // Container lifetime management
    private class ContainerLifetime : ObjectCache, ILifetime
    {
        public ContainerLifetime(Func<Type, Func<ILifetime, object>> getFactory) => GetFactory = getFactory;

        // Retrieves the factory functino from the given type, provided by owning container
        public Func<Type, Func<ILifetime, object>> GetFactory { get; private set; }

        public object GetService(Type type) => GetFactory(type)(this);

        // Singletons get cached per container
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
            => GetCached(type, factory, this);

        // At container level, per-scope items are equivalent to singletons
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
            => GetServiceAsSingleton(type, factory);
    }

    // Per-scope lifetime management
    private class ScopeLifetime : ObjectCache, ILifetime
    {
        // Singletons come from parent container's lifetime
        private readonly ContainerLifetime _parentLifetime;

        public ScopeLifetime(ContainerLifetime parentContainer) => _parentLifetime = parentContainer;

        public object GetService(Type type) => _parentLifetime.GetFactory(type)(this);

        // Singleton resolution is delegated to parent lifetime
        public object GetServiceAsSingleton(Type type, Func<ILifetime, object> factory)
            => _parentLifetime.GetServiceAsSingleton(type, factory);

        // Per-scope objects get cached
        public object GetServicePerScope(Type type, Func<ILifetime, object> factory)
            => GetCached(type, factory, this);
    }

    // RegisteredType is supposed to be a short lived object tying an item to its container
    // and allowing users to mark it as a singleton or per-scope item
    private class RegisteredType : IRegisteredType
    {
        private readonly Type _itemType;
        private readonly Action<Func<ILifetime, object>> _registerFactory;
        private readonly Func<ILifetime, object> _factory;

        public RegisteredType(Type itemType, Action<Func<ILifetime, object>> registerFactory, Func<ILifetime, object> factory)
        {
            _itemType = itemType;
            _registerFactory = registerFactory;
            _factory = factory;

            registerFactory(_factory);
        }

        public void AsSingleton()
            => _registerFactory(lifetime => lifetime.GetServiceAsSingleton(_itemType, _factory));

        public void PerScope()
            => _registerFactory(lifetime => lifetime.GetServicePerScope(_itemType, _factory));
    }
}
public static class ServiceContainerExtensions
{
    public static void RegisterForPlatform<T>(this ServiceContainer scope, OSPlatform platform, T impl)
    {
        if (RuntimeInformation.IsOSPlatform(platform))
        {
            scope.Register<T>(impl);
        }
    }

    /// <summary>
    /// Registers an implementation type for the specified interface
    /// </summary>
    /// <typeparam name="T">Interface to register</typeparam>
    /// <param name="serviceContainer">This container instance</param>
    /// <param name="type">Implementing type</param>
    /// <returns>IRegisteredType object</returns>
    public static ServiceContainer.IRegisteredType Register<T>(this ServiceContainer serviceContainer, Type type)
        => serviceContainer.Register(typeof(T), type);

    /// <summary>
    /// Registers an implementation type for the specified interface
    /// </summary>
    /// <typeparam name="TInterface">Interface to register</typeparam>
    /// <typeparam name="TImplementation">Implementing type</typeparam>
    /// <param name="serviceContainer">This container instance</param>
    /// <returns>IRegisteredType object</returns>
    public static ServiceContainer.IRegisteredType Register<TInterface, TImplementation>(this ServiceContainer serviceContainer)
        where TImplementation : TInterface
        => serviceContainer.Register(typeof(TInterface), typeof(TImplementation));

    /// <summary>
    /// Registers a factory function which will be called to resolve the specified interface
    /// </summary>
    /// <typeparam name="T">Interface to register</typeparam>
    /// <param name="serviceContainer">This container instance</param>
    /// <param name="factory">Factory method</param>
    /// <returns>IRegisteredType object</returns>
    public static ServiceContainer.IRegisteredType Register<T>(this ServiceContainer serviceContainer, Func<T> factory)
        => serviceContainer.Register(typeof(T), () => factory());

    public static ServiceContainer.IRegisteredType Register<T>(this ServiceContainer serviceContainer, T impl) =>
        serviceContainer.Register<T>(() => impl);

    /// <summary>
    /// Registers a type
    /// </summary>
    /// <param name="serviceContainer">This container instance</param>
    /// <typeparam name="T">Type to register</typeparam>
    /// <returns>IRegisteredType object</returns>
    public static ServiceContainer.IRegisteredType Register<T>(this ServiceContainer serviceContainer)
        => serviceContainer.Register(typeof(T), typeof(T));

    /// <summary>
    /// Returns an implementation of the specified interface
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="scope">This scope instance</param>
    /// <returns>Object implementing the interface</returns>
    public static T Resolve<T>(this ServiceContainer.IScope scope) => (T)scope.GetService(typeof(T));

    public static T Resolve<T>(this ServiceContainer.IScope scope, Type type) => (T)scope.GetService(type);
}