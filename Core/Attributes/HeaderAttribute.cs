namespace Project.Core.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class HeaderAttribute : Attribute
{
    public string Name { get; set; }

    public HeaderAttribute()
    {
    }

    public HeaderAttribute(string name) => Name = name;
}