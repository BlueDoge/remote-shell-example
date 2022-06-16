using System;
using Renci.SshNet;

namespace BlueDogeTools.RemoteShell
{
    public class Program
    {
        static int Main(string[] args)
        {
            // boot up the user interface
            var ui = new UserInterface(false);
            int uiResult = ui.Run();
            if(uiResult != 0)
            {
                Utilities.HardError<Exception>(null, "Error: UI failed");
            }

            // jump into the secure shell with the data learned from the ui
            var secureShell = new SecureShell(ref ui);

            ui.DisposeCredentials();

            return uiResult;
        }
    }
}