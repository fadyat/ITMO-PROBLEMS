using System;
using System.IO;
using BackupsExtra.Services;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.Serialization
{
    public class StorageMethodExtraJson /*: IStorageMethodExtra*/
    {
        public StorageMethodExtraJson(string path)
        {
            JsonPath = Path.Combine(path, "backupInfo.json");

            if (File.Exists(JsonPath)) return;

            Console.WriteLine(JsonPath);
            File.Create(JsonPath);
        }

        private string JsonPath { get; }

        public void Save(BackupExtraJobService backupJobService)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            string json = JsonConvert.SerializeObject(backupJobService, Formatting.Indented, settings);

            using var sw = new StreamWriter(JsonPath, false, System.Text.Encoding.Default);
            sw.WriteLine(json);
        }

        public BackupExtraJobService Load()
        {
            var fileInfo = new FileInfo(JsonPath);
            if (fileInfo.Length == 0) return null;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            string json = File.ReadAllText(JsonPath);
            return JsonConvert.DeserializeObject(json, settings) as BackupExtraJobService;
        }
    }
}