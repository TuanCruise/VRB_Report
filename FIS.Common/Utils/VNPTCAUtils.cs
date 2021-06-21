namespace FIS.Utils
{
    public static class VNPTCAUtils
    {
        public static int ShowMessVNPTCA(int ErrCode)
        {
            int iErr = 996;
            switch (ErrCode)
            {
                case 100:
                    iErr = 990;
                    break;
                case 101:
                    iErr = 991;
                    break;
                case 102:
                    iErr = 992;
                    break;
                case 400:
                    iErr = 993;
                    break;
                case 402:
                    iErr = 994;
                    break;
                case 403:
                    iErr = 995;
                    break;
                case 500:
                    iErr = 996;
                    break;
            }
            return iErr;
        }
    }
}
