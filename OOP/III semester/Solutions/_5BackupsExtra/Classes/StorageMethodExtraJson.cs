using System;
using System.IO;
using System.Text.Json;
using BackupsExtra.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BackupsExtra.Classes
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
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
            };

            string json = JsonSerializer.Serialize(backupJobService, options);

            using var sw = new StreamWriter(JsonPath, false, System.Text.Encoding.Default);
            sw.WriteLine(json);
        }

        public BackupExtraJobService Loading()
        {
            var fileInfo = new FileInfo(JsonPath);
            if (fileInfo.Length == 0)
            {
                return null;
            }

            string json = File.ReadAllText(JsonPath);
            return JsonSerializer.Deserialize<BackupExtraJobService>(json);
        }
    }
}