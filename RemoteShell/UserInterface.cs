


using System.Security;

namespace BlueDogeTools.RemoteShell
{
    public class UserInterface
    {
        private string? serverIpAddress;
        private int? serverPort;
        private SecureString? Username;
        private SecureString? Password;
        private string? Command;

        public void DisposeCredentials()
        {
            Username?.Dispose();
            Password?.Dispose();
        }

        public int GetPort()
        {
            // we don't need to check for null, we'll just assume port 22 if null
            return serverPort ?? 22;
        }

        public string GetIp()
        {
            if(serverIpAddress == null)
            {
                Utilities.HardError<NullReferenceException>(this, "Error: invalid ip");
            }

            return serverIpAddress;
        }

        public ref SecureString GetUsername()
        {
            if (Username == null)
            {
                Utilities.HardError<NullReferenceException>(this, "Error: invalid username");
            }

            return ref Username;
        }

        public ref SecureString GetPassword()
        {
            if(Password == null)
            {
                Utilities.HardError<NullReferenceException>(this, "Error: invalid password");
            }

            return ref Password;
        }

        public string GetCommand()
        {
            if(Command == null)
            {
                Utilities.HardError<NullReferenceException>(this, "Error: invalid command");
            }

            return Command;
        }

        private void AskForIpAddress()
        {
            Utilities.WritePrompt("Provide Server IP: ");
            serverIpAddress = Console.ReadLine();
        }

        private void AskForPort()
        {
            Utilities.WritePrompt("Provide SSH Port [22]: ");
            var userData = Console.ReadLine();
            if (userData == null)
            {
                serverPort = 22;
            }
            else
            {
                serverPort = Int32.Parse(userData.Trim() == "" ? "22" : userData);
            }
        }

        private void AskForUsername()
        {
            Utilities.WritePrompt("Provide SSH Username: ");
            Username = Utilities.ReadSecureLine();
        }

        private void AskForPassword()
        {
            Utilities.WritePrompt("Provide SSH Password: ");
            Password = Utilities.ReadSecureLine();
        }

        private void AskForCommand()
        {
            Utilities.WritePrompt("Provide Linux Command: ");
            Command = Console.ReadLine();
            //Console.WriteLine("Raw: {0}\nParsed: {1}", Command, Command.Replace("\\n", "\n"));
        }

        public UserInterface(bool bAutoRun = true)
        {
            if(bAutoRun)
            {
                var result = Run();
                if(result != 0) // always 0 currently, but maybe some use for this
                {
                    Utilities.HardError<Exception>(this, "Error: UI failed!");
                }
            }
        }

        private void ClearScreen()
        {
            Console.Clear();
        }

        public int Run()
        {
            AskForIpAddress();
            AskForPort();
            AskForUsername();
            AskForPassword();
            AskForCommand();
            ClearScreen();

            return 0;
        }
    }
}
