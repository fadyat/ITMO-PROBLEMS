using System.Collections.Generic;

namespace Backups.Classes.StorageMethods
{
    public interface IStorageMethod
    {
        string ConstructPath(string path, string name);

        void MakeDirectory(string path);

        void Archive(IEnumerable<JobObject> from, string where);

        bool ExistsDirectory(string path);

        bool ExistsFile(string path);
    }
}