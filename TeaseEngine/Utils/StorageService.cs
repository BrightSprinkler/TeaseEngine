using System;
using System.IO;

namespace TeaseEngine.Utils
{
    public class StorageService
    {
        private PathManager PathManager { get; } = new PathManager();
        private Logger Logger { get; } = App.Logging.GetLogger<StorageService>();

        public void Write(string name, object value)
        {
            File.WriteAllText(Path.Combine(PathManager.StorageDirectory, name + ".json"), Newtonsoft.Json.JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented));
        }

        public T Read<T>(string name, T defaultValue)
        {
            try
            {
                string file = File.ReadAllText(Path.Combine(PathManager.StorageDirectory, name + ".json"));

                file = file.Replace("@ROOT", PathManager.EscapedRootDirectory);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(file);
            }
            catch (Exception ex)
            {
                Logger.Warn($"Failed to read storage. Returning default value. {ex}");
                return defaultValue;
            }
        }

        public void Delete(string name)
        {
            string path = Path.Combine(PathManager.StorageDirectory, name + ".json");
            if (File.Exists(path)) File.Delete(path);
        }
    }
}
