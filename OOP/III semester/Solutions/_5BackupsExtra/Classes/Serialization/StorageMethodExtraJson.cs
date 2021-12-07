using System;
using System.IO;
using System.Linq;
using Backups.Classes.BackupJobs;
using Backups.Classes.RestorePoints;
using Backups.Exceptions;
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

            foreach (IBackupJob backup in service.Backups.ToList())
            {
                service.StorageMethod.MakeDirectory(backup.FullPath);
                foreach (IRestorePoint restorePoint in backup.RestorePoints.ToList())
                {
                    if (!backup.StorageMethod.ExistsDirectory(backup.FullPath))
                        throw new BackupException("This backup wasn't registered!");

                    if (Equals(backup.Objects.ToList().Count, 0))
                        throw new BackupException("No files for restore point!");

                    backup.StorageMethod.MakeDirectory(restorePoint.FullPath);
                    var allRestorePointObjects =
                        restorePoint.StoragesI.SelectMany(storage => storage.JobObjects).ToHashSet();

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