using System.Collections.Generic;
using System.Linq;
using Backups.Classes.RestorePoints;

namespace BackupsExtra.Classes.Selection
{
    public class HybridSelection : ISelection
    {
        private readonly List<ISelection> _clearAlgorithms;
        private readonly bool _unite;

        /// <summary>
        /// Initializes a new instance of the <see cref="HybridSelection"/> class.
        /// </summary>
        /// <param name="clearAlgorithms"> List of clear algorithms that will used. </param>
        /// <param name="unite"> Need to delete restore point if she incorrect in all algorithms. </param>
        public HybridSelection(List<ISelection> clearAlgorithms, bool unite)
        {
            _clearAlgorithms = clearAlgorithms;
            _unite = unite;
        }

        public IEnumerable<IRestorePoint> Clear(LinkedList<IRestorePoint> restorePoints)
        {
            var answer = new HashSet<IRestorePoint>(restorePoints);
            foreach (IEnumerable<IRestorePoint> result in _clearAlgorithms
                .Select(clearAlgo => clearAlgo.Clear(restorePoints)))
            {
                switch (_unite)
                {
                    case true:
                        answer.IntersectWith(result);
                        break;
                    default:
                        answer.UnionWith(result);
                        break;
                }
            }

            return answer;
        }
    }
}