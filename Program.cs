using System;

namespace FileManagerLab5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Лабораторная 5 – Composite + Adapter + Facade\n");

            Folder root = new Folder("Root");
            Folder docs = new Folder("Документы");
            File report = new File("Отчет.docx", 180_000);
            File image = new File("photo.jpg", 2_500_000);

            docs.Add(report);
            root.Add(docs);
            root.Add(image);

            Console.WriteLine($"Размер корневой папки: {root.GetSize():N0} байт");

            root.Remove(image);
            Console.WriteLine($"После удаления фото: {root.GetSize():N0} байт\n");

            // Локальная ФС
            IFileSystem local = new CompositeFileSystemAdapter(root);

            // Облако
            IFileSystem cloud = new MemoryCloudFileSystem();

            // Фасад
            var facade = new SyncFacade(local, cloud);

            Console.WriteLine("Запуск синхронизации локальной папки → облако...");
            facade.SyncFolder("/", "/backup");

            Console.WriteLine("\n=== Содержимое в облаке после синхронизации (/backup) ===");
            var cloudItems = cloud.ListItems("/backup");
            if (cloudItems.Count == 0)
            {
                Console.WriteLine(" (пусто)");
            }
            else
            {
                foreach (var item in cloudItems)
                {
                    Console.WriteLine(" " + item);
                }
            }

            Console.WriteLine("\nЗапуск резервного копирования...");
            facade.Backup("/", "/backup2");


            Console.WriteLine("\n=== Содержимое в /backup2 после резервного копирования ===");
            var backupItems = cloud.ListItems("/backup2");
            foreach (var item in backupItems)
            {
                Console.WriteLine(" " + item);
            }

            Console.WriteLine("\nГотово. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}