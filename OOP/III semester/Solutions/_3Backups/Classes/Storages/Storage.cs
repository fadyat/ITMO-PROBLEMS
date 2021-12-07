using System.Collections.Generic;
using Backups.Classes.JobObjects;
using Newtonsoft.Json;

namespace Backups.Classes.Storages
{
    public class Storage
    {
        public Storage(string storageName, string path, IEnumerable<IJobObject> jobObjects)
        {
            JobObjects = jobObjects;
            Name = storageName + ".zip";
            Path = path;
            FullPath = System.IO.Path.Combine(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public IEnumerable<IJobObject> JobObjects { get; }
    }
}