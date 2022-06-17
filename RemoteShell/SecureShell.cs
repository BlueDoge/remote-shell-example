// Originally created by Elizabeth Clements
// Copyright and License can be found in the LICENSE file or at the github (https://github.com/BlueDoge/remote-shell-example/)


using Renci.SshNet;

namespace BlueDogeTools.RemoteShell
{
    public class SecureShell
    {
        string serverIpAddress;
        int serverPort;

        // Waits for .Run() to be called with UserInterface reference passed.
        public SecureShell(string ipAddress, int port = 22)
        {
            serverIpAddress = ipAddress;
            serverPort = port;
        }

        // Auto runs the shell
        public SecureShell(ref UserInterface ui)
        {
            serverIpAddress = ui.GetIp();
            serverPort = ui.GetPort();
            Run(ref ui);
        }

        public void Run(ref UserInterface ui)
        {
            Utilities.WriteLog("SecureShell", String.Format("Connecting to {0}...", ui.GetIp()));

            using (var sshClient = new SshClient(serverIpAddress, serverPort, Utilities.SecurityStringToString(ref ui.GetUsername()), Utilities.SecurityStringToString(ref ui.GetPassword())))
            {
                try
                {
                    sshClient.Connect();
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Utilities.HardError<Exception>(this, String.Format("Error: failed to connect ({0})", ex.Message).ToString());
                }

                using (ShellStream shellStream = sshClient.CreateShellStream("xterm", 240, 50, 0, 0, 1024))
                {
                    // inject the command, and add the sentinel string to be echoed after the command is completed
                    shellStream.WriteLine(String.Format("{0} && echo \"bdt-end-stream\"", ui.GetCommand()));

                    Utilities.WriteLog(String.Format("SecureShell", serverIpAddress), $"Executing on {serverIpAddress}...");


                    // if you want to listen for a response, and know the length of the response, or have a sentinel to look for...
                    // do it here...

                    // loop until the sentinel string is returned to us
                    for(var d = shellStream.ReadLine(); d != null && d != "bdt-end-stream"; d = shellStream.ReadLine())
                    {
                        if (d == "\n") continue;
                        Utilities.WriteLog(String.Format("{0}", serverIpAddress), d);
                    }

                    shellStream.Close();
                } // shellStream.Dispose();

                sshClient.Disconnect();
            } // sshClient.Dispose();
            Utilities.WriteLog("SecureShell", String.Format("Finished."));
        }
    }
}
