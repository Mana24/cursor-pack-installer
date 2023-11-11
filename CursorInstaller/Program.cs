using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace CursorInstaller
{
    internal class Program
    {

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);

        const int SPI_SETCURSORS = 0x0057;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        const int USER_SCHEME = 0x02;

        private const string ConfigFileName = "Config.json";

        static void Main(string[] args)
        {
            // GET VALUE NAMES AND VALUES FROM CONFIG
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            Config config;
            try
            {
                config = JsonFileReader.Read<Config>(Path.Join(assemblyPath, ConfigFileName));
            }
            // TELL USER IF THE CONFIG FILE IS MISSING AND ABORT
            catch (FileNotFoundException e)
            {
                Console.WriteLine("Config file missing. Make sure it's in the same directory as this executable");
                Exit();
                return;
            }
            catch (JsonException e)
            {
                Console.WriteLine("Bad json syntax in config file. Make sure it hasn't been modified");
                Exit();
                return;
            }

            if (config == null)
            {
                Console.WriteLine("Invalid config file");
                Exit();
                return;
            }

            // SET Registry Keys 

            RegistryKey currentUser = Registry.CurrentUser;
            RegistryKey controlPanel = currentUser.OpenSubKey("Control Panel");
            RegistryKey cursorsKey = controlPanel.OpenSubKey("Cursors");

            // CHECK THAT THE CURSOR FILES EXIST AND SKIP FILE IF NOT

            foreach (KeyValuePair<string, string> keyValuePair in config.CursorNameToFilePath)
            {
                if (!validKeys.Contains(keyValuePair.Key))
                {
                    Console.WriteLine($"'{keyValuePair.Key}' is not a valid key so we're skipping it..");
                    continue;
                }
                
                string filePath = Path.Join(assemblyPath, keyValuePair.Value);

                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"No file exists at ${filePath} so we're skipping it..");
                    continue;
                }

                cursorsKey.SetValue(keyValuePair.Key, keyValuePair.Value, 
                    keyValuePair.Key == "" ? RegistryValueKind.String : RegistryValueKind.ExpandString);
                Console.WriteLine($"Set Key {keyValuePair.Key} to ${keyValuePair.Value} successfully");
            }

            ////

            // SET User Scheme Key
            cursorsKey.SetValue("Scheme Source", USER_SCHEME, RegistryValueKind.DWord);
            Console.WriteLine("Set System scheme to User Scheme");

            // NOTIFY Windows of changes
            SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_SENDCHANGE | SPIF_UPDATEINIFILE);
            Console.WriteLine("Notified windows of changes");

            // TELL USER TO PRESS KEY TO EXIT
            Exit();
        }

        public static void Exit()
        {
            Console.Write("\nPress any key to exit...");
            Console.ReadKey();
        }

        public static string[] validKeys =
        {
            "",
            "AppStarting",
            "Arrow",
            "Crosshair",
            "Hand",
            "Help",
            "IBeam",
            "No",
            "NWPen",
            "Person",
            "Pin",
            "SizeAll",
            "SizeNESW",
            "SizeNS",
            "SizeNWSE",
            "SizeWE",
            "UpArrow",
            "Wait"
        };
    }

    public static class JsonFileReader
    {
        public static T? Read<T>(string filePath)
        {
            string text = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(text);
        }
    }
}

