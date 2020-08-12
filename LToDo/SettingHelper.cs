using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace LToDo
{
    public class SettingHelper
    {
        public static bool IsRegister(string key)
        {
            RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//检索指定的子项
            var getApp = registry.GetValue(key);
            return getApp == null ? false : true;
        }

        public static bool RegisterAutoRun()
        {
            string appName = AppDomain.CurrentDomain.BaseDirectory + "LTodo.exe";//获取要自动运行的应用程序名
            if (!System.IO.File.Exists(appName))//判断要自动运行的应用程序文件是否存在
                return false;
            RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true
                );//检索指定的子项
            if (registry == null)//若指定的子项不存在
                registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");//则创建指定的子项
            registry.SetValue("ltodo.exe", appName);//设置该子项的新的“键值对”
            return IsRegister("ltodo.exe") ? true : false;
        }

        public static bool UnregisterAutoRun()
        {
            string appName = AppDomain.CurrentDomain.BaseDirectory + "LTodo.exe";//获取要自动运行的应用程序名
            if (!System.IO.File.Exists(appName))//判断要自动运行的应用程序文件是否存在
                return false;
            RegistryKey registry = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);//检索指定的子项
            if (registry == null)//若指定的子项不存在
                registry = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");//则创建指定的子项
            registry.DeleteValue("ltodo.exe");//设置该子项的新的“键值对”
            return IsRegister("ltodo.exe") ? true : false;
        }
    }
}
