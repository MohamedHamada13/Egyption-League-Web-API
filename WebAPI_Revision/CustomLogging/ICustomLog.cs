namespace WebAPI_Revision.CustomLogging
{
    public interface ICustomLog
    {
        void AddLog(string message, CustomLogLevel level = CustomLogLevel.Info);
    }
}
