using System.Collections.Generic;
using System.IO;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public class AbstractFileSystemStorageExtra : AbstractFileSystemStorage, IStorageExtraMethod
    {
        public void RemoveRestorePoint(IRestorePoint restorePoint)
        {
            foreach (Storage storages in restorePoint.StoragesI)
            {
                PathFiles.Remove(storages.FullPath);
            }

            PathDirectories.Remove(restorePoint.FullPath);
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
                if (!newVersionNames.Contains(lastStorage.Name))
                {
                    Move(lastStorage.FullPath, Path.Combine(newVersion.Path, lastStorage.Name));
                }
            }

            RemoveRestorePoint(lastVersion);
        }

        public void Move(string from, string too)
        {
            PathFiles.Remove(from);
            PathFiles.Add(too);
        }
    }
}