using System;
using System.Collections.Generic;
using Backups.Classes.Storages;

namespace Backups.Classes.RestorePoints
{
    public class RestorePoint
    {
        private readonly List<Storage> _storages;

        public RestorePoint(string path, List<Storage> storages, string name, DateTime dateTime)
        {
            _storages = storages;
            Path = path;
            Name = name;
            CreationDate = dateTime;
            FullPath = System.IO.Path.Combine(Path, Name);
        }

        public string Name { get; }

        public string Path { get; }

        public string FullPath { get; }

        public DateTime CreationDate { get; }

        public IEnumerable<Storage> StoragesI => _storages;
    }
}