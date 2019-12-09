using DevExpress.XtraLayout;

namespace FIS.AppClient.Interface
{
    public interface ICommonFieldSupportedModule
    {
        bool ValidateRequire { get; }
        LayoutControl CommonLayout { get; }
        string CommonLayoutStoredData { get; }
    }
}
