using Backups.Classes.BackupJobs;
using BackupsExtra.Classes.Selection;

namespace BackupsExtra.Classes.BackupJobsExtra
{
    public interface IBackupJobExtra : IBackupJob
    {
        void Merge(ISelection selection);

        void Clear(ISelection selection);
    }
}