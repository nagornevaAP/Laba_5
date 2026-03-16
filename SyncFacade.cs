using System;

namespace FileManagerLab5
{
    public class SyncFacade
    {
        private readonly IFileSystem sourceFS;
        private readonly IFileSystem targetFS;

        public SyncFacade(IFileSystem source, IFileSystem target)
        {
            sourceFS = source;
            targetFS = target;
        }

        public void SyncFolder(string sourcePath, string targetPath)
        {
            var items = sourceFS.ListItems(sourcePath);

            foreach (var item in items)
            {
                string srcFull = Combine(sourcePath, item);
                string tgtFull = Combine(targetPath, item);

                try
                {
                    byte[] data = sourceFS.ReadFile(srcFull);
                    targetFS.WriteFile(tgtFull, data);
                    Console.WriteLine($" Скопирован файл: {item}");
                }
                catch
                {
                    SyncFolder(srcFull, tgtFull);
                }
            }

            Console.WriteLine("Синхронизация завершена.");
        }

        public void Backup(string sourcePath, string backupPath)
        {
            try
            {
                SyncFolder(sourcePath, backupPath);
                Console.WriteLine("Резервное копирование завершено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка бэкапа: {ex.Message}");
            }
        }

        private string Combine(string basePath, string name)
        {
            basePath = basePath.TrimEnd('/');
            name = name.TrimStart('/');
            return string.IsNullOrEmpty(basePath) ? $"/{name}" : $"{basePath}/{name}";
        }
    }
}
