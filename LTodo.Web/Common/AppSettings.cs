using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LTodo.Web.Common
{
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; }

        public AppSettings(string contentPath)
        {
            string Path = "appsettings.json";
            //这样的话，可以直接读目录里的json文件，而不是 bin 文件夹下的，所以不用修改复制属性
            Configuration = new ConfigurationBuilder()
               .SetBasePath(contentPath)
               .Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true })
               .Build();
        }

        /// <summary>
        /// 得到appsettings.json中的字符串
        /// </summary>
        /// <param name="sections">Json对象名，按照顺序传</param>
        /// <returns>得到字符串</returns>
        public static string GetJsonString(params string[] sections)
        {
            var values = string.Empty;
            for (int i = 0; i < sections.Length; i++)
            {
                values += sections[i] + ":";
            }
            return Configuration[values.TrimEnd(':')];//返回得到的字符串
        }
    }
}
