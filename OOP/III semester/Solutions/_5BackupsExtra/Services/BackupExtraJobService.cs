// using System.Collections.Generic;
// using Backups.Classes;
// using Backups.Classes.StorageAlgorithms;
// using Backups.Classes.StorageMethods;
// using Backups.Services;
//
// namespace BackupsExtra.Services
// {
//     public class BackupExtraJobService : IBackupJobService
//     {
//         private readonly BackupJobService _backupJobService;
//
//         public BackupExtraJobService(string path, IStorageMethod storageMethod, string name = "Repository")
//         {
//             _backupJobService = new BackupJobService(path, storageMethod, name);
//         }
//
//         public IEnumerable<BackupJob> Backups => _backupJobService.Backups;
//
//         public IStorageMethod StorageMethod => _backupJobService.StorageMethod;
//
//         public string Path => _backupJobService.Path;
//
//         public string FullPath => _backupJobService.FullPath;
//
//         public int IssuedBackupId => _backupJobService.IssuedBackupId;
//
//         public string Name => _backupJobService.Name;
//
//         public BackupJob CreateBackup(
//             HashSet<JobObject> objects, IStorageAlgorithm storageAlgorithm, string name = "backupJob_")
//         {
//             return _backupJobService.CreateBackup(objects, storageAlgorithm, name);
//         }
//
//         public BackupJob GetBackup(int id)
//         {
//             return _backupJobService.GetBackup(id);
//         }
//     }
// }