

using System.Security;

namespace BlueDogeTools.RemoteShell
{
    public static class Utilities
    {
        public static void WritePrompt(string Message, int embeddedDepth = 1, char embeddedPadding = '>')
        {
            // PadLeft with the Length of the message + the depth of the embedding. Starting with 1
            Console.Write(Message.PadLeft(embeddedDepth + Message.Length, embeddedPadding));
        }

        public static void WriteLog(string Tag, string Message)
        {
            Console.WriteLine("[{0}] {1}", Tag, Message);
        }

        // is valid keyboard entry
        public static bool IsValidChar(char test)
        {
            return test != '\u0000';
        }

        // is comprised of valid keyboard entries
        public static bool IsValidString(string test)
        {
            string ValidKeyboardCharacters = "abcdefghijklmnopqrstuvwxyz1234567890-=!@#$%^&*()_+[]\\{}|;':\",.<>/?";
            // we don't care about case sensitive checking, so lower it and only check for one type of each alphabet char
            return ValidateString(test.ToLower(), ValidKeyboardCharacters.ToCharArray());
        }

        // this is case sensitive, be careful
        public static bool ValidateString(string test, char[] validChars)
        {
            // if there's an issue, this'll find it, surely!
            foreach(char c in test.ToCharArray())
            {
                if(!validChars.Contains(c))
                {
                    return false;
                }
            }
            // otherise true!
            return true;
        }

        public static SecureString ReadSecureLine(bool bOverride = true, bool bAddChars = true, char charToAdd = '*')
        {
            SecureString secureLine = new SecureString();
            var keyData = Console.ReadKey(bOverride);
            while (keyData.Key != ConsoleKey.Enter)
            {
                bool bIsValidChar = IsValidChar(keyData.KeyChar);
                if (keyData.Key == ConsoleKey.Backspace)
                {
                    secureLine.RemoveAt(secureLine.Length - 1);
                    // if we're not overriding, or we are and adding characters...
                    // we should remove the character added
                    if (!bOverride || (bOverride && bAddChars))
                    {
                        Console.Write("\b \b");
                    }
                }
                if (bIsValidChar)
                {
                    if (bAddChars && bOverride)
                    {
                        Console.Write(charToAdd); // would add *'s if defaulted
                    }
                    secureLine.AppendChar(keyData.KeyChar);
                }
                keyData = Console.ReadKey(bOverride);
            }
            // the new line was consumed.
            Console.Write('\n');

            return secureLine;
        }

        public static void HardError<T>(object origin, string message) where T : Exception
        {
            Console.Error.WriteLine(message);
            string exceptionMessage = String.Format("Name: {0}\nMessage: {1}", origin.GetType().FullName, message).ToString();
            throw (T)Activator.CreateInstance(typeof(T), new object[] { exceptionMessage }) ?? new Exception(exceptionMessage);
        }
    }
}
