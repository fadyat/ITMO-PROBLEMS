// using System.Collections.Generic;
// using System.Linq;
// using Backups.Classes;
// using Backups.Exceptions;
//
// namespace BackupsExtra.Classes.ClearAlgorithms
// {
//     public class HybridClearAlgorithm : IClearAlgorithm
//     {
//         private readonly List<IClearAlgorithm> _clearAlgorithms;
//         private readonly bool _deleteIfIncorrectInAll;
//
//         /// <summary>
//         /// Initializes a new instance of the <see cref="HybridClearAlgorithm"/> class.
//         /// </summary>
//         /// <param name="clearAlgorithms"> List of clear algorithms that will used. </param>
//         /// <param name="deleteIfIncorrectInAll"> Need to delete restore point if she incorrect in all algorithms. </param>
//         public HybridClearAlgorithm(List<IClearAlgorithm> clearAlgorithms, bool deleteIfIncorrectInAll)
//         {
//             _clearAlgorithms = clearAlgorithms;
//             _deleteIfIncorrectInAll = deleteIfIncorrectInAll;
//         }
//
//         public IEnumerable<RestorePoint> Clear(LinkedList<RestorePoint> restorePoints)
//         {
//             var pointCorrectness = restorePoints.ToDictionary(rp => rp, _ => 0);
//
//             foreach (RestorePoint rp in _clearAlgorithms
//                 .Select(algo => algo.Clear(restorePoints))
//                 .SelectMany(cps => cps))
//             {
//                 pointCorrectness[rp] += 1;
//             }
//
//             var correctPoints = restorePoints.Where(point =>
//                     (_deleteIfIncorrectInAll && pointCorrectness[point] != 0) ||
//                     (!_deleteIfIncorrectInAll && pointCorrectness[point] == _clearAlgorithms.Count))
//                 .ToList();
//
//             if (correctPoints.Count <= 0)
//             {
//                 throw new BackupException("Clear algorithm can't remove all points!");
//             }
//
//             return correctPoints;
//         }
//     }
// }