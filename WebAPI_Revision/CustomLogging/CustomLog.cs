namespace WebAPI_Revision.CustomLogging
{
    public class CustomLog : ICustomLog
    {
        public void AddLog(string message, CustomLogLevel level = CustomLogLevel.Info)
        {
            if (level is CustomLogLevel.Info)
                Console.Write("Log_Info - ");
            else if(level is CustomLogLevel.Warn)
                Console.Write("Log_Warn - ");
            else if(level is CustomLogLevel.Error)
                Console.Write("Log_Error - ");

            Console.WriteLine(message);
        }
    }
}
