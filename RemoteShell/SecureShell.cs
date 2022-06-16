



using System.Security;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Security;
using Renci.SshNet.Security.Cryptography;
using Renci.SshNet.Sftp;
using Renci.SshNet.Compression;

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
            Console.WriteLine("Connecting to {0}...", ui.GetIp());

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
                    // do the thing
                    shellStream.WriteLine(ui.GetCommand());

                    // if you want to listen for a response, and know the length of the response, or have a sentinel to look for...
                    // do it here...


                    shellStream.Close();
                } // shellStream.Dispose();

                sshClient.Disconnect();
            } // sshClient.Dispose();
        }
    }
}
