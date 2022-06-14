


using System.Security;

namespace BlueDogeTools.RemoteShell
{
    public class UserInterface
    {
        private string? serverIpAddress;
        private SecureString? Username;
        private SecureString? Password;
        private string? Command;

        public string GetIp()
        {
            if(serverIpAddress == null)
            {
                throw new NullReferenceException("Error: invalid ip");
            }

            return serverIpAddress;
        }

        public ref SecureString GetUsername()
        {
            if (Username == null)
            {
                throw new NullReferenceException("Error: invalid username");
            }

            return ref Username;
        }

        public ref SecureString GetPassword()
        {
            if(Password == null)
            {
                throw new NullReferenceException("Error: invalid password");
            }

            return ref Password;
        }

        public string GetCommand()
        {
            if(Command == null)
            {
                throw new NullReferenceException("Error: invalid command");
            }

            return Command;
        }

        private void AskForIpAddress()
        {
            Utilities.WritePrompt("Provide Server IP: ");
            serverIpAddress = Console.ReadLine();
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
                    throw new Exception("Error: UI failed!");
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
            AskForUsername();
            AskForPassword();
            AskForCommand();
            //ClearScreen();
            Console.WriteLine("Would usually clear screen here...");
            return 0;
        }
    }
}
