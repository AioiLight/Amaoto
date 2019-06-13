using System.Text;
using Newtonsoft.Json;

namespace Amaoto
{
    /// <summary>
    /// 設定ファイル入出力クラス。
    /// </summary>
    public static class ConfigManager
    {
        /// <summary>
        /// 設定ファイルの読み込みを行います。
        /// </summary>
        /// <typeparam name="T">シリアライズしたクラス。</typeparam>
        /// <param name="filePath">ファイル名。</param>
        /// <returns>デシリアライズ結果。</returns>
        public static T GetConfig<T>(string filePath) where T : new()
        {
            var json = "";
            if (!System.IO.File.Exists(filePath))
            {
                return new T();
            }
            using (var stream = new System.IO.StreamReader(filePath, Encoding.UTF8))
            {
                json = stream.ReadToEnd();
            }
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings() { ObjectCreationHandling = ObjectCreationHandling.Reuse, DefaultValueHandling  = DefaultValueHandling.Populate});
        }

        /// <summary>
        /// 設定ファイルの書き込みを行います。
        /// </summary>
        /// <param name="obj">シリアライズするインスタンス。</param>
        /// <param name="filePath">ファイル名。</param>
        public static void SaveConfig(object obj, string filePath)
        {
            using (var stream = new System.IO.StreamWriter(filePath, false, Encoding.UTF8))
            {
                stream.Write(JsonConvert.SerializeObject(obj, Formatting.Indented));
            }
        }
    }
}
