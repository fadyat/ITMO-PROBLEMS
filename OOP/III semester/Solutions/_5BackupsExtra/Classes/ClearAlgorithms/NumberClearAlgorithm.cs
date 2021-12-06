// using System.Collections.Generic;
// using Backups.Classes;
// using Backups.Exceptions;
//
// namespace BackupsExtra.Classes.ClearAlgorithms
// {
//     public class NumberClearAlgorithm : IClearAlgorithm
//     {
//         private readonly int _numberToSave;
//
//         public NumberClearAlgorithm(int numberToSave)
//         {
//             _numberToSave = numberToSave;
//         }
//
//         public IEnumerable<RestorePoint> Clear(LinkedList<RestorePoint> restorePoints)
//         {
//             int toSave = _numberToSave;
//             while (toSave > 0)
//             {
//                 restorePoints.RemoveFirst();
//                 toSave--;
//             }
//
//             if (restorePoints.Count <= 0)
//             {
//                 throw new BackupException("Clear algorithm can't remove all points!");
//             }
//
//             return restorePoints;
//         }
//     }
// }