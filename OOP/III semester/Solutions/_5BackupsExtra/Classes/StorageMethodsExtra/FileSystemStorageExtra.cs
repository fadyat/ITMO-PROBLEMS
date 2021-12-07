using System.Collections.Generic;
using System.IO;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public class FileSystemStorageExtra : FileSystemStorage, IStorageExtraMethod
    {
        public void RemoveRestorePoint(IRestorePoint restorePoint)
        {
            foreach (Storage storages in restorePoint.StoragesI)
            {
                File.Delete(storages.FullPath);
            }

            Directory.Delete(restorePoint.FullPath);
        }

        public void Merge(IRestorePoint lastVersion, IRestorePoint newVersion)
        {
            var newVersionNames = new HashSet<string>();
            foreach (Storage storage in newVersion.StoragesI)
            {
                newVersionNames.Add(storage.Name);
            }

            foreach (Storage lastStorage in lastVersion.StoragesI)
            {
                if (newVersionNames.Contains(lastStorage.Name)) continue;

                var updatedStorage = new Storage(lastStorage.Name, newVersion.FullPath, lastStorage.JobObjects);
                Move(lastStorage.FullPath, updatedStorage.FullPath);
                newVersion.AddStorage(updatedStorage);
            }

            RemoveRestorePoint(lastVersion);
        }

        public void Move(string from, string too)
        {
            File.Move(from, too);
        }
    }
}