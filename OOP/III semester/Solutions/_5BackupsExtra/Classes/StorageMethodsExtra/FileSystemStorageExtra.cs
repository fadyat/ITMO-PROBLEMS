using System;
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
    }
}