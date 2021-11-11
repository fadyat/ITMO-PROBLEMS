using System;
using System.Collections.Generic;
using System.IO;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            string position = Directory.GetParent(Directory.GetCurrentDirectory())
                .Parent?
                .Parent?
                .FullName;

            var backupJobService = new BackupJobService("test", position);
            BackupJob backupJob = backupJobService.CreateBackup(new HashSet<string>
            {
                "/Users/artyomfadeyev/Documents/a.txt",
                "/Users/artyomfadeyev/Documents/b.txt",
                "/Users/artyomfadeyev/Documents/c.txt",
            });

            var restorePointService = new RestorePointService();
            restorePointService.CreateRestorePoint(backupJob, new SplitStorages());
            backupJob.RemoveFile("/Users/artyomfadeyev/Documents/b.txt");
            backupJob.AddFile("/Users/artyomfadeyev/Documents/b.txt");
            backupJob.AddFile("/Users/artyomfadeyev/Documents/b.txt");
            restorePointService.CreateRestorePoint(backupJob, new SplitStorages());
            restorePointService.CreateRestorePoint(backupJob, new SplitStorages());
            restorePointService.CreateRestorePoint(backupJob, new SingleStorage());
        }
    }
}
