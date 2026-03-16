using System;
using System.Collections.Generic;

namespace FileManagerLab5
{
    public class CompositeFileSystemAdapter : IFileSystem
    {
        private readonly FileSystemItem root;

        public CompositeFileSystemAdapter(FileSystemItem root)
        {
            this.root = root;
        }

        private FileSystemItem? FindItem(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || path == "/") return root;

            var parts = path.Trim('/').Split('/');
            FileSystemItem current = root;

            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;

                if (current is not Folder folder) return null;

                FileSystemItem? found = null;
                int i = 0;
                while (true)
                {
                    var child = folder.GetChild(i);
                    if (child == null) break;
                    if (child.Name == part)
                    {
                        found = child;
                        break;
                    }
                    i++;
                }

                if (found == null) return null;
                current = found;
            }

            return current;
        }

        public List<string> ListItems(string path)
        {
            var item = FindItem(path);
            if (item is Folder f)
            {
                var list = new List<string>();
                int i = 0;
                while (true)
                {
                    var c = f.GetChild(i);
                    if (c == null) break;
                    list.Add(c.Name);
                    i++;
                }
                return list;
            }
            return new List<string>();
        }

        public byte[] ReadFile(string path)
        {
            var item = FindItem(path);
            if (item is File f) return new byte[f.GetSize()];
            throw new InvalidOperationException($"Не найден файл: {path}");
        }

        public void WriteFile(string path, byte[] data)
        {
            Console.WriteLine($"[Write] {path} ({data.Length} байт)");
        }

        public void DeleteItem(string path)
        {
            Console.WriteLine($"[Delete] {path}");
        }
    }
}