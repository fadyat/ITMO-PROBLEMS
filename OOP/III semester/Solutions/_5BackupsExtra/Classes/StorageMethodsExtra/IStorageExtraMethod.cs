using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public interface IStorageExtraMethod : IStorageMethod
    {
        void RemoveRestorePoint(IRestorePoint restorePoint);

        void Merge(IRestorePoint lastVersion, IRestorePoint newVersion);

        void Move(string from, string too);
    }
}