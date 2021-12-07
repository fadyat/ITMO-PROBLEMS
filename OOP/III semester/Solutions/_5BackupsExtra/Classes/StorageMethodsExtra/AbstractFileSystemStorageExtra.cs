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
    }
}