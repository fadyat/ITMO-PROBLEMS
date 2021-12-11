using System.Collections.Generic;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public interface IStorageExtraMethod : IStorageMethod
    {
        void RemoveRestorePoint(RestorePoint restorePoint);

        IEnumerable<Storage> Merge(RestorePoint lastVersion, RestorePoint newVersion);

        void Move(string from, string too);
    }
}