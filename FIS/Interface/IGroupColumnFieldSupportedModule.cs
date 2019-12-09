namespace FIS.AppClient.Interface
{
    public interface IGroupColumnFieldSupportedModule :
        IColumnFieldSupportedModule
    {
        string GroupLayoutStoredData { get; }
    }
}
