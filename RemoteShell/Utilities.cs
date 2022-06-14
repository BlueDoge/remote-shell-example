

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

        public static bool IsValidChar(char test)
        {
            return IsValidString(""+test);
        }

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

        // un-overescapes newline characters (i.e. "\\n" => "\n")
        public static string ReadRawLine()
        {
            string line = "";
            var keyData = Console.ReadKey(true);
            ConsoleKeyInfo? last = null;
            while (keyData.Key != ConsoleKey.Enter)
            {
                bool bIsValidChar = IsValidChar(keyData.KeyChar);
                if (keyData.Key == ConsoleKey.Backspace) // write this in, but also remove from the str buffer
                {
                    line = line.Substring(0, line.Length - 1);
                    Console.Write("\b \b");
                }

                if (bIsValidChar || keyData.Key == ConsoleKey.Spacebar)
                {
                    Console.Write(keyData.KeyChar);
                    // if entering a \n sequence, inject the new line
                    //if (last != null && last?.KeyChar == '\\' && keyData.KeyChar == 'n')
                    //{
                    //    line = line.Substring(0, line.Length - 1);
                    //    line += "\n";
                    //}
                    //else
                    {
                        line += keyData.KeyChar;
                    }
                }

                last = keyData;
                keyData = Console.ReadKey(true);
            }
            // the new line was consumed.
            Console.Write('\n');

            return line;
        }
    }
}
