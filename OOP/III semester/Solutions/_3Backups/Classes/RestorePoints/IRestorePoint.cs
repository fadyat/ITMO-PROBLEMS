using System;
using System.Collections.Generic;
using Backups.Classes.Storages;

namespace Backups.Classes.RestorePoints
{
    public interface IRestorePoint
    {
        string Name { get; }

        string Path { get; }

        DateTime CreationDate { get; }

        string FullPath { get; }

        IEnumerable<Storage> StoragesI { get; }
    }
}