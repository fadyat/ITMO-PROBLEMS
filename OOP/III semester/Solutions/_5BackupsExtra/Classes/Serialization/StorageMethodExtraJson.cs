using System;
using System.IO;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.RestorePoints;
using Backups.Services;
using Newtonsoft.Json;

namespace BackupsExtra.Classes.Serialization
{
    public class StorageMethodExtraJson : IStorageMethodExtra
    {
        public StorageMethodExtraJson(string path)
        {
            JsonPath = Path.Combine(path, "backupInfo.json");

            if (File.Exists(JsonPath)) return;

            Console.WriteLine(JsonPath);
            File.Create(JsonPath);
        }

        private string JsonPath { get; }

        public void Save(IBackupJobService backupJobService)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            string json = JsonConvert.SerializeObject(backupJobService, Formatting.Indented, settings);

            using var sw = new StreamWriter(JsonPath, false, System.Text.Encoding.Default);
            sw.WriteLine(json);
        }

        public IBackupJobService Load()
        {
            var fileInfo = new FileInfo(JsonPath);
            if (fileInfo.Length == 0) return null;

            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };

            string json = File.ReadAllText(JsonPath);

            if (JsonConvert.DeserializeObject(json, settings) is not IBackupJobService service) return null;

            Console.WriteLine("$" + service.Backups.Count());
            foreach (var bckp in service.Backups.ToList())
            {
                Console.WriteLine(bckp.RestorePoints.Count());
            }

            foreach (IBackupJob backup in service.Backups.ToList())
            {
                service.CreateBackup(backup);
                foreach (IRestorePoint restorePoint in backup.RestorePoints.ToList())
                {
                    backup.CreateRestorePoint(restorePoint);
                }
            }

            Console.WriteLine("$" + service.Backups.Count());
            foreach (var bckp in service.Backups.ToList())
            {
                Console.WriteLine(bckp.RestorePoints.Count());
            }

            return service;
        }
    }
}