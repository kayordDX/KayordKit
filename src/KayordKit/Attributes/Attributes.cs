namespace KayordKit.Attributes;


[AttributeUsage(AttributeTargets.Class)]
public class TableAttribute : Attribute
{
    public TableAttribute(string tableName)
    {
        Name = tableName;
    }
    public string Name { get; set; }
}

[AttributeUsage(AttributeTargets.Property)]
public class KeyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class ExplicitKeyAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class WriteAttribute : Attribute
{
    public WriteAttribute(bool write)
    {
        Write = write;
    }
    public bool Write { get; }
}
