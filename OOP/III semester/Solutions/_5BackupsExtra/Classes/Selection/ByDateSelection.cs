using System;
using System.Collections.Generic;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public class ByDateSelection : ISelection
    {
        private readonly DateTime _oldestDate;

        public ByDateSelection(DateTime oldestDate)
        {
            _oldestDate = oldestDate;
        }

        public IEnumerable<IRestorePoint> Clear(LinkedList<IRestorePoint> restorePoints)
        {
            while (restorePoints.First?.Value.CreationDate <= _oldestDate)
            {
                restorePoints.RemoveFirst();
            }

            return restorePoints;
        }
    }
}