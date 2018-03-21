using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Text;

namespace NotPowerShell
{
    internal class Program
    {
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
                DisplayHelp();
            }
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("usage:\r\nnps.exe \"{powershell single command}\"\r\nnps.exe \"& {commands; semi-colon; separated}\"\r\nnps.exe -encodedcommand {base64_encoded_command}\r\nnps.exe -encode \"commands to encode to base64\"\r\nnps.exe -decode {base64_encoded_command}");
        }

        private static void Encode(string[] args)
        {
            if(args.Length == 2)
            {
                var bytes = Encoding.Unicode.GetBytes(args[1]);
                Console.WriteLine(Convert.ToBase64String(bytes));
            }
            else 
            {
                Console.WriteLine("usage: nps.exe -encode \"& commands; separated; by; semicolons;\"");
            }
        }

        private static void Decode(string[] args)
        {
            if (args.Length == 2)
            {
                var cmd = Encoding.Unicode.GetString(Convert.FromBase64String(args[1]));
                Console.WriteLine(cmd);
            }
            else
            {
                Console.WriteLine("usage: nps.exe -decode {base_64_string}");
            }
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
                foreach (var item in output)
                {
                    Console.WriteLine(item.ToString());
                }
            }
        }
    }
}