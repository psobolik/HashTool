
namespace HashTool.Model
{
    public interface IAssemblyInfo
    {
        string Title { get; }
        string Description { get; }
        string Configuration { get; }
        string Product { get; }
        string Company { get; }
        string Copyright { get; }
        string Trademark { get; }
        string Culture { get; }
        string Version { get; }
        string DisplayName { get; }
        string ProcessorArchecture { get; }
    }
}
