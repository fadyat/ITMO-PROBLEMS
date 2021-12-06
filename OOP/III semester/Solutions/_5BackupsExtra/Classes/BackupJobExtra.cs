// using System.Collections.Generic;
// using Backups.Classes;
// using Backups.Classes.StorageAlgorithms;
// using Backups.Classes.StorageMethods;
// using BackupsExtra.Classes.ClearAlgorithms;
//
// namespace BackupsExtra.Classes
// {
//     public class BackupJobExtra : BackupJob
//     {
//         public BackupJobExtra(
//             int id,
//             string path,
//             HashSet<JobObject> objects,
//             string name,
//             IStorageAlgorithm storageAlgorithm,
//             IStorageMethod storageMethod)
//             : base(id, path, objects, name, storageAlgorithm, storageMethod)
//         {
//         }
//
//         public void ClearOldRestorePoints(IClearAlgorithm clearAlgorithm)
//         {
//             LinkedRestorePoints = clearAlgorithm.Clear(LinkedRestorePoints) as LinkedList<RestorePoint>;
//         }
//     }
// }