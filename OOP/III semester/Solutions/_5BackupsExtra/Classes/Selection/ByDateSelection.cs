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

        public IEnumerable<IRestorePoint> Fetch(LinkedList<IRestorePoint> restorePoints)
        {
            var result = new LinkedList<IRestorePoint>(restorePoints);
            while (result.First?.Value.CreationDate <= _oldestDate)
            {
                result.RemoveFirst();
            }

            return result;
        }
    }
}