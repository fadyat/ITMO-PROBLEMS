using System.Collections.Generic;
using System.IO;
using Backups.Classes;
using Backups.Classes.StorageAlgorithms;
using Backups.Classes.StorageMethods;
using Backups.Services;

namespace Backups
{
    internal static class Program
    {
        private static void Main()
        {
            // string position = Directory.GetParent(Directory.GetCurrentDirectory())
            //     .Parent?
            //     .Parent?
            //     .FullName;
            //
            // var backupJobService = new BackupJobService(
            //     position,
            //     new FileSystemStorage());
            //
            // BackupJob backupJob = backupJobService.CreateBackup(
            //     new HashSet<string>
            //     {
            //         "/Users/artyomfadeyev/Documents/a.txt",
            //         "/Users/artyomfadeyev/Documents/b.txt",
            //         "/Users/artyomfadeyev/Documents/c.txt",
            //     },
            //     new SplitStorages());
            //
            // backupJob.CreateRestorePoint();
            // backupJob.RemoveFile("/Users/artyomfadeyev/Documents/b.txt");
            // backupJob.CreateRestorePoint();
            // backupJob.AddFile("/Users/artyomfadeyev/Documents/b.txt");
            // backupJob.CreateRestorePoint();
            //
            // BackupJob backupJob2 = backupJobService.CreateBackup(
            //     new HashSet<string>
            //     {
            //         "/Users/artyomfadeyev/Documents/a.txt",
            //         "/Users/artyomfadeyev/Documents/b.txt",
            //         "/Users/artyomfadeyev/Documents/c.txt",
            //     },
            //     new SingleStorage());
            //
            // backupJob2.CreateRestorePoint();
        }
    }
}