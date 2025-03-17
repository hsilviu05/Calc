using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calc.Models;
using System.Xml.Serialization;

namespace Calc.Helpers
{
    public static class SettingsManager
    {
        private static Settings _settings;
        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "CalculatorApp",
            "settings.xml");

        public static Settings GetSettings()
        {
            if (_settings == null)
            {
                LoadSettings();
            }

            return _settings;
        }

        public static void SetSettings(Settings settings)
        {
            _settings = settings;
            SaveSettings();
        }

        public static void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    using (FileStream fs = new FileStream(SettingsPath, FileMode.Open))
                    {
                        _settings = (Settings)serializer.Deserialize(fs);
                    }
                }
                else
                {
                    _settings = new Settings();
                }
            }
            catch (Exception)
            {
                _settings = new Settings();
            }
        }

        public static void SaveSettings()
        {
            try
            {
                string directory = Path.GetDirectoryName(SettingsPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                using (FileStream fs = new FileStream(SettingsPath, FileMode.Create))
                {
                    serializer.Serialize(fs, _settings);
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
