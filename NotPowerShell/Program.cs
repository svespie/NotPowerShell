using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;

namespace NotPowerShell
{
    internal class Program
    {
        private const string prompt = "> ";

        private static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                var firstArg = args[0].ToLower();

                if( firstArg == "-help")
                {
                    DisplayHelp();
                }   
                else if (firstArg == "-encode")
                {
                    Encode(args);
                }
                else if (firstArg == "-decode")
                {
                    Decode(args);
                }
                else 
                {
                    ExecuteCommand(args);
                }                
            }
            else
            {
                DisplayShell();
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("usage:\r\nnps.exe \"{powershell single command}\"\r\nnps.exe \"& {commands; semi-colon; separated}\"\r\nnps.exe -encodedcommand {base64_encoded_command}\r\nnps.exe -encode \"commands to encode to base64\"\r\nnps.exe -decode {base64_encoded_command}");
        }

        private static void Encode(string[] args)
        {
            Console.WriteLine("");

            if(args.Length == 2)
            {
                Console.WriteLine(EncodeBase64(args[1]));
            }
            else 
            {
                Console.WriteLine("usage: nps.exe -encode \"& commands; separated; by; semicolons;\"");
            }

            Console.WriteLine("");
        }

        private static string EncodeBase64(string value)
        {
            var bytes = Encoding.Unicode.GetBytes(value);
            
            return Convert.ToBase64String(bytes);
        }

        private static void Decode(string[] args)
        {
            Console.WriteLine("");

            if (args.Length == 2)
            {
                Console.WriteLine(DecodeBase64(args[1]));
            }
            else
            {
                Console.WriteLine("usage: nps.exe -decode {base_64_string}");
            }

            Console.WriteLine("");
        }

        private static string DecodeBase64(string value)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(value));
        }

        private static void ExecuteCommand(string[] args)
        {
            using(var ps = PowerShell.Create())
            {
                AddScript(ps, args);
                Invoke(ps);
            }
        }

        private static void AddScript(PowerShell ps, string[] args)
        {
            var script = "";
            var firstArg = args[0].ToLower();
            var encoded = firstArg == "-encodedcommand" || firstArg == "-enc";

            for (var index = encoded ? 1 : 0; index < args.Length; index++)
            {
                script += encoded ? Encoding.Unicode.GetString(Convert.FromBase64String(args[index])) : args[index];
            }

            if(script.Length > 0 && script[0] != '&')
            {
                script = "& " + script;
            }

            ps.AddScript(script);
        }

        private static void Invoke(PowerShell ps)
        {
            Collection<PSObject> output = null;

            try
            {
                output = ps.Invoke();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error while executing the script.\r\n" + e.Message.ToString());
            }

            if (output != null)
            {
                Console.WriteLine("");

                foreach (var item in output)
                {
                    
                    Console.WriteLine(item.ToString());
                    
                }

                if(output.Count > 0)
                {
                    Console.WriteLine("");
                }
            }
        }

        private static void DisplayShell()
        {
            Console.WriteLine("");
            Console.WriteLine("NotPowerShell");
            Console.WriteLine("");
            Console.WriteLine("Type 'exit' to exit. Otherwise type commands like you normally would in a PowerShell instance and hit <enter>.");
            Console.WriteLine("");

            while(true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine();

                if(string.IsNullOrEmpty(input))
                {
                    continue;
                }

                if(input.ToLower() == "exit")
                {
                    break;
                }

                var encodedInput = EncodeBase64(input);
                var command = new string[] {"-encodedcommand", encodedInput};

                ExecuteCommand(command);
            }
        }
    }
}