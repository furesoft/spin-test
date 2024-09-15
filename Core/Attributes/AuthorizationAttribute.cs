namespace Project.Core.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class AuthorizationAttribute(params string[] roles) : Attribute
{
    public string[] Roles { get; } = roles;
}