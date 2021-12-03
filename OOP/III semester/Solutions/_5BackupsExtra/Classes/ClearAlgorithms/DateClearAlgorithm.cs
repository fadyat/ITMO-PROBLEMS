using System;
using System.Collections.Generic;
using System.Diagnostics;
using Backups.Classes;
using Backups.Exceptions;

namespace BackupsExtra.Classes.ClearAlgorithms
{
    public class DateClearAlgorithm : IClearAlgorithm
    {
        private readonly DateTime _oldestDate;

        public DateClearAlgorithm(DateTime oldestDate)
        {
            _oldestDate = oldestDate;
        }

        public IEnumerable<RestorePoint> Clear(LinkedList<RestorePoint> restorePoints)
        {
            while (restorePoints.First?.Value.CreationDate <= _oldestDate)
            {
                restorePoints.RemoveFirst();
            }

            if (restorePoints.Count <= 0)
            {
                throw new BackupException("Clear algorithm can't remove all points!");
            }

            return restorePoints;
        }
    }
}