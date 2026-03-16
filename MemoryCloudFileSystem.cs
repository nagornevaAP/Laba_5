using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManagerLab5
{
    public class MemoryCloudFileSystem : IFileSystem
    {
        private readonly Dictionary<string, byte[]> _storage = new Dictionary<string, byte[]>();

        private string NormalizePath(string path)
        {
            path = path.Trim().Replace('\\', '/');
            if (!path.StartsWith("/")) path = "/" + path;
            if (path.EndsWith("/")) path = path.TrimEnd('/');
            return path;
        }

        public List<string> ListItems(string path)
        {
            path = NormalizePath(path);
            var result = new HashSet<string>();

            foreach (var key in _storage.Keys)
            {
                if (key.StartsWith(path + "/") || key == path)
                {
                    string relative = key.Substring(path.Length).TrimStart('/');
                    if (!string.IsNullOrEmpty(relative))
                    {
                        string name = relative.Split('/')[0];
                        result.Add(name);
                    }
                }
            }

            return result.ToList();
        }

        public byte[] ReadFile(string path)
        {
            string key = NormalizePath(path);
            if (_storage.TryGetValue(key, out byte[] data))
                return data;
            throw new InvalidOperationException($"Файл не найден: {path}");
        }

        public void WriteFile(string path, byte[] data)
        {
            string key = NormalizePath(path);
            _storage[key] = (byte[])data.Clone(); 
            Console.WriteLine($" [Облако] Сохранён файл: {path} (размер {data.Length} байт)");
        }

        public void DeleteItem(string path)
        {
            string key = NormalizePath(path);
            _storage.Remove(key);
        }
    }
}
