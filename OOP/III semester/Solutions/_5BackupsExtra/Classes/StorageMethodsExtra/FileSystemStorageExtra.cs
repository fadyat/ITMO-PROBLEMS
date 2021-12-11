using System.Collections.Generic;
using System.IO;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public class FileSystemStorageExtra : FileSystemStorage, IStorageExtraMethod
    {
        public void RemoveRestorePoint(RestorePoint restorePoint)
        {
            foreach (Storage storages in restorePoint.PublicStorages)
            {
                File.Delete(storages.FullPath);
            }

            Directory.Delete(restorePoint.FullPath);
        }

        public IEnumerable<Storage> Merge(RestorePoint lastVersion, RestorePoint newVersion)
        {
            var newVersionNames = new HashSet<string>();
            foreach (Storage storage in newVersion.PublicStorages)
            {
                newVersionNames.Add(storage.Name);
            }

            var addedStorages = new LinkedList<Storage>();
            foreach (Storage lastStorage in lastVersion.PublicStorages)
            {
                if (newVersionNames.Contains(lastStorage.Name)) continue;

                var updatedStorage = new Storage(lastStorage.Name, newVersion.FullPath, lastStorage.JobObjects);
                Move(lastStorage.FullPath, updatedStorage.FullPath);
                addedStorages.AddFirst(updatedStorage);
            }

            RemoveRestorePoint(lastVersion);
            return addedStorages;
        }

        public void Move(string from, string too)
        {
            File.Move(from, too);
        }
    }
}