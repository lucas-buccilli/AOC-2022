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

            var folder = getAllSubFolders(root);
            folder.Add(root);

            return folder
                .Select(x => new { name = x.name, size = x.getSize() })
                .Select(x => x.size)
                .Where(x => x <= 100000)
                .Sum()
                .ToString();
        }



        public override string part2(string input)
        {
            Folder root = generateFolderStructure(input);

            var folder = getAllSubFolders(root);
            folder.Add(root);

            var folderSize = folder
                .Select(x => new { name = x.name, size = x.getSize() })
                .ToList();

            var freeSpace = 70000000 - root.getSize();

            return folderSize
                .Where(x => freeSpace + x.size >= 30000000)
                .OrderBy(x => x.size)
                .First()
                .size
                .ToString();
        }

        private static Folder generateFolderStructure(string input)
        {
            var lines = input.Split("\n");
            Folder? currentOperatingFolder = null;
            foreach (var line in lines)
            {
                switch (line)
                {
                    case var commandString when Regex.IsMatch(line, @"^\$.+$"):
                        var commandMatch = Regex.Match(commandString, @"^\$\s([a-zA-Z]+)(\s(.+))?$");
                        var command = commandMatch.Groups[1].Value;
                        if (command == "cd")
                        {
                            var directory = commandMatch.Groups[3].Value;
                            if (directory == "/" && currentOperatingFolder == null)
                            {
                                currentOperatingFolder = new Folder("/");
                            }
                            else if (directory == "..")
                            {
                                currentOperatingFolder = currentOperatingFolder.parent;
                            }
                            else
                            {
                                currentOperatingFolder = currentOperatingFolder.children.OfType<Folder>().Single(x => x.name == directory);
                            }
                        }

                        break;
                    case var folderString when Regex.IsMatch(line, @"^dir.+$"):
                        var folderMatch = Regex.Match(folderString, @"^dir\s(.+)$");
                        var folderName = folderMatch.Groups[1].Value;

                        currentOperatingFolder.children.Add(new Folder(folderName, currentOperatingFolder));
                        break;
                    case var fileString when Regex.IsMatch(line, @"^[0-9]+ .+$"):
                        var fileMatch = Regex.Match(fileString, @"^([0-9]+)\s(.+)$");
                        var fileSize = int.Parse(fileMatch.Groups[1].Value);
                        var fileName = fileMatch.Groups[2].Value;

                        currentOperatingFolder.children.Add(new File(fileName, currentOperatingFolder, fileSize));
                        break;
                    default:
                        break;
                }
            }


            while (currentOperatingFolder.parent != null)
            {
                currentOperatingFolder = currentOperatingFolder.parent;
            }

            return currentOperatingFolder;
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
            public List<FileSystemObject> children = new List<FileSystemObject>();

            public Folder(String name, Folder parent) : base(name, parent) { }

            public Folder(String name) : base(name) { }

            public override int getSize()
            {
                return children.Sum(x => x.getSize());
            }
        }

        private class File : FileSystemObject
        {
            public int size;

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
            protected FileSystemObject(String name, Folder parent)
            {
                this.name = name;
                this.parent = parent;
            }

            protected FileSystemObject(String name)
            {
                this.name = name;
            }

            public String name;
            public Folder? parent;

            public abstract int getSize();
        }
    }

}

