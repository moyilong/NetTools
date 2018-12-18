namespace 诊断工具
{
    public interface AutoLoadTemplate
    {
        string TabName { get; }
        string Catalog { get; }
    }

    public interface HelpedAutoLoad : AutoLoadTemplate
    {
        string HelpDoc { get; }
    }

    public interface WIPTemplate
    {
    }
}