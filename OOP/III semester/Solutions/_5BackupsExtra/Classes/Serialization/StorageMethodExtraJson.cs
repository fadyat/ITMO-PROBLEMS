using System;
using System.IO;
using System.Linq;
using Backups.Classes.RestorePoints;
using Backups.Exceptions;
using BackupsExtra.Classes.BackupJobsExtra;
using BackupsExtra.Services;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.Serialization
{
    public class StorageMethodExtraJson : ISerialize
    {
        public StorageMethodExtraJson(string path)
        {
            JsonPath = Path.Combine(path, "backupInfo.json");

            if (!File.Exists(JsonPath))
            {
                File.Create(JsonPath);
            }
        }

        private string JsonPath { get; }

        public void Save(BackupExtraJobService backupJobService)
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

        public BackupExtraJobService Load()
        {
            var fileInfo = new FileInfo(JsonPath);
            if (fileInfo.Length == 0) return null;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented,
            };

            string json = File.ReadAllText(JsonPath);

            return JsonConvert.DeserializeObject(json, settings) is not BackupExtraJobService service ? null : service;
        }
    }
}