namespace Project.Core.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class PathParameterAttribute : Attribute
{
    public string Name { get; set; }

    public PathParameterAttribute()
    {
    }

    public PathParameterAttribute(string name) => Name = name;
}