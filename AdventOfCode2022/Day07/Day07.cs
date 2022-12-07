namespace AdventOfCode2022.Day07;

public sealed class Day07 : Solver
{
    public Day07()
        : base(nameof(Day07).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var root = InitializeFilesystem(input);

        var dirsLesserThan = GetDirsLesserThan(root, 100000);
        return dirsLesserThan.Select(x => x.Size).Sum();
    }

    private static IEnumerable<Directory> GetDirsLesserThan(Directory root, long value)
    {
        if (root.Size <= value)
        {
            yield return root;
        }

        foreach (var subDir in root.Directories.Values)
        {
            foreach (var dirsLesserThan in GetDirsLesserThan(subDir, value))
            {
                yield return dirsLesserThan;
            }
        }
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var dir = InitializeFilesystem(input);

        var root = dir;
        long need = 30000000 - (70000000 - root.Size);


        this.GetMinBiggerThan(root, need);

        return this.GetMinBiggerThan(root, need);
    }

    private static Directory InitializeFilesystem(StreamReader input)
    {
        var root = new Directory
        {
            Name = "/",
            Parent = null
        };

        var currentDir = root;

        while (true)
        {
            var line = input.ReadLine()!;

            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var split = line!.Split(' ');

            if (line[0] != '$')
            {
                split = line.Split();

                ProcessLsResultLine(currentDir, split[0], split[1]);

                continue;
            }

            var command = split[1];

            if (command == "ls")
            {
                continue;
            }

            var argDirName = split[2];

            currentDir = ProcessCd(currentDir, argDirName, root);
        }
        
        CountDirectoriesSize(root);

        return root;
    }

    private static Directory ProcessCd(Directory currentDir, string argDirName, Directory root)
    {
        switch (argDirName)
        {
            case "/":
                currentDir = root;
                break;

            case "..":
                currentDir = currentDir.Parent!;
                break;

            default:
            {
                currentDir = CreateNewDir(currentDir, argDirName);
                break;
            }
        }

        return currentDir!;
    }

    private static void ProcessLsResultLine(Directory currentDir, string argFileSizeOrDir, string argFileNameOrDirName)
    {
        if (argFileSizeOrDir == "dir")
        {
            CreateNewDir(currentDir, argFileNameOrDirName);
        }
        else
        {
            var size = long.Parse(argFileSizeOrDir);

            var file = new File
            {
                Size = size
            };

            currentDir.Files.Add(file);
        }
    }

    private static Directory CreateNewDir(Directory currentDir, string newDirName)
    {
        var newDir = new Directory
        {
            Name = newDirName,
            Parent = currentDir
        };

        currentDir.Directories[newDirName] = newDir;

        newDir.Parent = currentDir;

        return newDir;
    }

    private static long CountDirectoriesSize(Directory root)
    {
        long size = 0;

        foreach (var file in root.Files)
        {
            size += file.Size;
        }

        foreach (var dir in root.Directories.Values)
        {
            if (dir.Size == -1)
            {
                dir.Size = CountDirectoriesSize(dir);
            }

            size += dir.Size;
        }

        root.Size = size;

        return size;
    }

    private long GetMinBiggerThan(Directory root, long value)
    {
        if (root.Size < value)
        {
            return long.MaxValue;
        }

        var min = root.Size;

        foreach (var dir in root.Directories.Values)
        {
            min = Math.Min(min, this.GetMinBiggerThan(dir, value));
        }

        return min;
    }

    private class File
    {
        public long Size { get; init; }
    }

    private class Directory
    {
        public string Name { get; init; } = null!;

        public Dictionary<string, Directory> Directories { get; } = new();

        public HashSet<File> Files { get; } = new();

        public Directory? Parent { get; set; }

        public long Size { get; set; } = -1;

        protected bool Equals(Directory other)
        {
            return this.Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            
            return Equals((Directory)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}