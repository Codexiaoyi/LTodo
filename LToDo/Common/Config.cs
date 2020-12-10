using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LToDo
{
    public class Config
    {
        public static string DatabasePath = Path.Combine(Path.Combine(GetBaseFolder(), "LTodo.db"));
        public static string ConfigPath = Path.Combine(Path.Combine(GetBaseFolder(), "Config.ini"));

        private const string IS_TASK_TO_BOTTOM = "ISTASKTOBOTTOM";

        public static string GetBaseFolder()
        {
            var baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            if (!Directory.Exists(baseFolder))
            {
                Directory.CreateDirectory(baseFolder);
            }
            return baseFolder;
        }

        public static bool IsTaskToBottom
        {
            get
            {
                return bool.Parse(GetConfig(IS_TASK_TO_BOTTOM) ?? "false");
            }
            set
            {
                SetConfig(IS_TASK_TO_BOTTOM, value.ToString());
            }
        }

        public static void SetConfig(string key, string value)
        {
            if (!File.Exists(ConfigPath))
            {
                File.Create(ConfigPath);
            }
            var lines = File.ReadAllLines(ConfigPath).ToList();
            var result = lines.FirstOrDefault(x => x.StartsWith(key));
            if (!string.IsNullOrEmpty(result))
            {
                lines.RemoveAll(x => x == result || string.IsNullOrWhiteSpace(x));
                lines.Add($"{key}={value}\n");
                File.WriteAllLines(ConfigPath, lines);
            }
            else
            {
                File.AppendAllText(ConfigPath, $"{key}={value}\n");
            }
        }

        public static string GetConfig(string key)
        {
            if (!File.Exists(ConfigPath))
                return null;
            var lines = File.ReadAllLines(ConfigPath);
            var result = lines.FirstOrDefault(x => x.StartsWith(key));
            if (!string.IsNullOrEmpty(result))
            {
                return result.Split('=')[1];
            }
            return null;
        }
    }
}
