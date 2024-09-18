namespace Project.Core.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class AuthorizationAttribute(params string[] roles) : Attribute
{
    public string[] Roles { get; } = roles;
}
