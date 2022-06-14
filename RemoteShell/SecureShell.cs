



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
        public SecureShell(string ipAddress)
        {
            serverIpAddress = ipAddress;
        }

        public SecureShell(ref UserInterface ui)
        {
            serverIpAddress = ui.GetIp();
            Run(ref ui);
        }

        public void Run(ref UserInterface ui)
        {
            using (var sshClient = new SshClient(serverIpAddress, ui.GetUsername().ToString(), ui.GetPassword().ToString()))
            {
                sshClient.Connect();

                using (ShellStream shellStream = sshClient.CreateShellStream("xterm", 80, 50, 0, 0, 1024))
                {
                    // do the thing
                    shellStream.WriteLine(ui.GetCommand());
                    shellStream.Close();
                } // shellStream.Dispose();

                sshClient.Disconnect();
            } // sshClient.Dispose();
        }
    }
}
