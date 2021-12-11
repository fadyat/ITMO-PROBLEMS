using System.Collections.Generic;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public interface ISelection
    {
        IEnumerable<RestorePoint> Fetch(LinkedList<RestorePoint> restorePoints);
    }
}