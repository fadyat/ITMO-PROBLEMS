using Backups.Classes.StorageMethods;
using Newtonsoft.Json;

namespace Backups.Classes.Storages
{
    public class Storage
    {
        public Storage(string storageName, string path)
        {
            Name = storageName + ".zip";
            Path = path;
            FullPath = System.IO.Path.Combine(path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }
    }
}