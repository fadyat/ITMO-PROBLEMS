using System;
using System.Collections.Generic;
using Backups.Classes.Storages;
using Newtonsoft.Json;

namespace Backups.Classes.RestorePoints
{
    public interface IRestorePoint
    {
        public string Name { get; }

        public string Path { get; }

        [JsonIgnore]
        public string FullPath { get; }

        public DateTime CreationDate { get; }

        public IEnumerable<Storage> StoragesI { get; }

        void AddStorages(List<Storage> storages);
    }
}