using System.IO;
using BackupsExtra.Services;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.Serialization
{
    public class JsonSerialization : ISerialize
    {
        public JsonSerialization(string path)
        {
            JsonPath = Path.Combine(path, "backupInfo.json");

            if (!File.Exists(JsonPath))
            {
                File.Create(JsonPath);
            }
        }

        private string JsonPath { get; }

        public void Save(BackupJobServiceExtra backupJobService)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };

            string json = JsonConvert.SerializeObject(backupJobService, settings);

            using var sw = new StreamWriter(JsonPath, false, System.Text.Encoding.Default);
            sw.WriteLine(json);
        }

        public BackupJobServiceExtra Load()
        {
            var fileInfo = new FileInfo(JsonPath);
            if (fileInfo.Length == 0) return null;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };

            string json = File.ReadAllText(JsonPath);
            return JsonConvert.DeserializeObject(json, settings) is not BackupJobServiceExtra service ? null : service;
        }
    }
}