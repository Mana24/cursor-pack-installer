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

            // CHECK THAT THE CURSOR FILES EXIST AND SKIP FILE IF NOT

            ////

            // SET User Scheme Key

            // NOTIFY Windows of changes

            // TELL USER TO PRESS KEY TO EXIT


            //RegistryKey currentUser = Registry.CurrentUser;
            //RegistryKey controlPanel = currentUser.OpenSubKey("Control Panel");
            //RegistryKey cursorsKey = controlPanel.OpenSubKey("Cursors");

            //Console.WriteLine(cursorsKey.GetValueNames().Length);

            //for (int i = 0; i < cursorsKey.ValueCount; i++)
            //{
            //    string valueName = cursorsKey.GetValueNames()[i];

            //    Console.Write(valueName); Console.Write(" ");
            //    Console.WriteLine(cursorsKey.GetValue(valueName));

            //}
            //controlPanel.Close();
            //cursorsKey.Close();

            // SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
        }

        public static void Exit()
        {

        }

        private string[] validKeys =
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

