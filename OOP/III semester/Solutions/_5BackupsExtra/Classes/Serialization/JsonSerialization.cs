using System.IO;
using System.Linq;
using Backups.Classes.RestorePoints;
using BackupsExtra.Classes.BackupJobsExtra;
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
            if (JsonConvert.DeserializeObject(json, settings) is not BackupJobServiceExtra service) return null;

            foreach (BackupJobExtra backup in service.BackupsI.ToList())
            {
                service.StorageMethod.MakeDirectory(backup.FullPath);
                foreach (RestorePoint restorePoint in backup.RestorePoints.ToList())
                {
                    backup.StorageMethod.MakeDirectory(restorePoint.FullPath);
                    var allRestorePointObjects =
                        restorePoint.PublicStorages.SelectMany(storage => storage.JobObjects).ToHashSet();

                    backup.StorageAlgorithm.CreateStorages(
                        restorePoint.FullPath,
                        allRestorePointObjects,
                        backup.StorageMethod);
                }
            }

            return service;
        }
    }
}