## 5. Code analysis

> Используя инструменты dotTrace, dotMemory, всё-что-угодно-хоть-windbg, проанализировать работу написанного кода для бекапов. Необходимо написать сценарий, когда в цикле будет выполняться много запусков, будут создаваться и удаляться точки. Проверить два сценария: с реальной работой с файловой системой и без неё. В отчёте неоходимо проанализировать полученные результаты, сделать вывод о написанном коде. Опционально: предложить варианты по модернизации или написать альтернативную имплементацию.

### C#

- Create C# project
- Import `BackupsExtra` package
```C#
using Backups.Classes.JobObjects;
using Backups.Classes.StorageAlgorithms;
using Backups.Services;
using BackupsExtra.Classes.BackupLogs;
using BackupsExtra.Classes.Selection;
using BackupsExtra.Classes.StorageMethodsExtra;
using BackupsExtra.Services;


namespace BackupsExtra
{
    internal static class Program
    {
        private static void Main()
        {
            var dataStorage = Path.Combine(
                Path.Combine(
                    Directory.GetParent(Directory.GetCurrentDirectory())?
                        .Parent?
                        .FullName!),
                "Data");

            var service = new BackupJobServiceExtra(
                new BackupJobService(dataStorage, new FileSystemStorageExtra()),
                new FileSystemStorageExtra());

            var b1 = service.CreateBackup(new List<IJobObject>
                {
                    new JobObject("/Users/artyomfadeyev/Documents/files/1.png"),
                    new JobObject("/Users/artyomfadeyev/Documents/files/2.png"),
                    new JobObject("/Users/artyomfadeyev/Documents/files/3.png"),
                },
                new SplitStorages(),
                new FileLogger(dataStorage),
                "backup");

            while (true)
            {
                b1.CreateRestorePoint();
                b1.CreateRestorePoint();
                b1.CreateRestorePoint();
                b1.CreateRestorePoint();
                b1.Clear(new ByNumberSelection(4));
            }
        }
    }
}
```
- Build project
- Install dotMemory for console
- Write command: `~/Downloads/dotMemory/dotMemory start-net-core --trigger-timer=30s BackupsExtra-Dotmemory.dll`
- Open received file `*.dmw` file at dotMemory Windows Application
- [Abstract file system](https://i.ibb.co/yYr0csD/2.png)

![](https://i.ibb.co/yYr0csD/2.png)

- [Local file system](https://i.ibb.co/Kwjvv5x/1.png)

![](https://i.ibb.co/Kwjvv5x/1.png)

