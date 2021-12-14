using System.IO;
using Backups.Classes.JobObjects;
using Backups.Classes.RestorePoints;
using Backups.Classes.Storages;
using BackupsExtra.Classes.StorageMethodsExtra;

namespace BackupsExtra.Classes.Recovery
{
    public class OriginRecover : IRecovery
    {
        public void Restore(IStorageExtraMethod storageExtraMethod, RestorePoint restorePoint)
        {
            foreach (Storage storage in restorePoint.PublicStorages)
            {
                string toDirectory = Path.Combine(Directory.GetCurrentDirectory(), "tmp");
                storageExtraMethod.MakeDirectory(toDirectory);
                storageExtraMethod.Recover(storage.FullPath, toDirectory);
                foreach (IJobObject jobObject in storage.JobObjects)
                {
                    string tmpLocation = Path.Combine(toDirectory, Path.GetFileName(jobObject.Path) !);
                    storageExtraMethod.Move(tmpLocation, jobObject.Path);
                }

                storageExtraMethod.RemoveDirectory(toDirectory);
            }
        }
    }
}