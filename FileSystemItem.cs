using System;
using System.Collections.Generic;

namespace FileManagerLab5
{
    public abstract class FileSystemItem
    {
        public string Name { get; protected set; }

        protected FileSystemItem(string name)
        {
            Name = name;
        }

        public abstract long GetSize();
        public abstract void Add(FileSystemItem item);
        public abstract void Remove(FileSystemItem item);
        public abstract FileSystemItem? GetChild(int index);
    }

    public class File : FileSystemItem
    {
        private readonly long size;

        public File(string name, long size) : base(name)
        {
            this.size = size;
        }

        public override long GetSize() => size;

        public override void Add(FileSystemItem item) => throw new InvalidOperationException();
        public override void Remove(FileSystemItem item) => throw new InvalidOperationException();
        public override FileSystemItem? GetChild(int index) => null;
    }

    public class Folder : FileSystemItem
    {
        private readonly List<FileSystemItem> children = new();

        public Folder(string name) : base(name) { }

        public override long GetSize()
        {
            long total = 0;
            foreach (var child in children)
                total += child.GetSize();
            return total;
        }

        public override void Add(FileSystemItem item) => children.Add(item);
        public override void Remove(FileSystemItem item) => children.Remove(item);

        public override FileSystemItem? GetChild(int index)
        {
            if (index >= 0 && index < children.Count)
                return children[index];
            return null;
        }
    }
}