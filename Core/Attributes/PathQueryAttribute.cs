namespace DotnetTest.Core.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class PathQueryAttribute : Attribute
{
    public string Name { get; set; }

    public PathQueryAttribute()
    {
    }

    public PathQueryAttribute(string name) => Name = name;
}