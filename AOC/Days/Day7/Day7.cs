using System.Linq;
using System.Text.RegularExpressions;
using Days;

namespace Day7
{
    public class Executor : Day
    {
        public Executor()
        {
        }

        public override string part1(string input)
        {
            Folder root = generateFolderStructure(input);

            var directories = getAllSubFolders(root);
            directories.Add(root);

            return directories
                .Select(x => x.getSize())
                .Where(x => x <= 100000)
                .Sum()
                .ToString();
        }



        public override string part2(string input)
        {
            Folder root = generateFolderStructure(input);
            var freeSpace = 70000000 - root.getSize();

            var directories = getAllSubFolders(root);
            directories.Add(root);

            return directories
                .Select(x => x.getSize())
                .Where(x => freeSpace + x >= 30000000)
                .Order()
                .First()
                .ToString();
        }

        private static Folder generateFolderStructure(string input)
        {
            var lines = input.Split("\n");
            Folder root = new Folder("/");
            Folder currentOperatingFolder = root;
            foreach (var line in lines.Skip(1))
            {
                switch (line)
                {
                    case var commandString when Regex.IsMatch(line, @"^\$\scd.+$"):
                        var commandMatch = Regex.Match(commandString, @"^\$\scd\s(.+)$");
                        var directory = commandMatch.Groups[1].Value;
                        if (directory == "..")
                        {
                            currentOperatingFolder = currentOperatingFolder.getParent();
                        }
                        else
                        {
                            currentOperatingFolder = currentOperatingFolder.children.OfType<Folder>().Single(x => x.name == directory);
                        }

                        break;
                    case var folderString when Regex.IsMatch(line, @"^dir.+$"):
                        var folderMatch = Regex.Match(folderString, @"^dir\s(.+)$");
                        var folderName = folderMatch.Groups[1].Value;

                        currentOperatingFolder.children.Add(new Folder(folderName, currentOperatingFolder));
                        break;
                    case var fileString when Regex.IsMatch(line, @"^[0-9]+\s.+$"):
                        var fileMatch = Regex.Match(fileString, @"^([0-9]+)\s(.+)$");
                        var fileSize = int.Parse(fileMatch.Groups[1].Value);
                        var fileName = fileMatch.Groups[2].Value;

                        currentOperatingFolder.children.Add(new File(fileName, currentOperatingFolder, fileSize));
                        break;
                    default:
                        break;
                }
            }


            return root;
        }

        private List<Folder> getAllSubFolders(Folder root)
        {
            List<Folder> subFolders = root.children.OfType<Folder>().ToList();
            subFolders.AddRange(
                subFolders
                    .Select(folder => getAllSubFolders(folder))
                    .SelectMany(x => x)
                    .ToList()
                );
            return subFolders;

        }

        private class Folder : FileSystemObject
        {
            public List<FileSystemObject> children { get; } = new List<FileSystemObject>();

            public Folder(String name, Folder parent) : base(name, parent) { }

            public Folder(String name) : base(name) { }

            public override int getSize()
            {
                return children.Sum(x => x.getSize());
            }
        }

        private class File : FileSystemObject
        {
            public int size { get; set; }

            public File(String name, Folder parent, int size) : base(name, parent)
            {
                this.size = size;
            }

            public override int getSize()
            {
                return size;
            }
        }

        abstract class FileSystemObject
        {

            public String name { get; set; }
            private readonly Folder? parent;

            protected FileSystemObject(String name, Folder parent)
            {
                this.name = name;
                this.parent = parent;
            }

            protected FileSystemObject(String name)
            {
                this.name = name;
            }

            public abstract int getSize();

            public Folder getParent()
            {
                return parent != null ? parent : throw new ArgumentException();
            }
        }
    }

}

