using System.Collections.Generic;
using Backups.Classes.RestorePoints;
using Backups.Classes.StorageMethods;
using Backups.Classes.Storages;

namespace BackupsExtra.Classes.StorageMethodsExtra
{
    public interface IStorageMethodExtra : IStorageMethod
    {
        void RemoveRestorePoint(RestorePoint restorePoint);

        IEnumerable<Storage> Merge(RestorePoint lastVersion, RestorePoint newVersion);

        void Move(string from, string too);

        void Recover(string from, string too);

        void RemoveDirectory(string path);

        void RemoveFile(string path);
    }
}