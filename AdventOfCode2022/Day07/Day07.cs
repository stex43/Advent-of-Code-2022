namespace AdventOfCode2022.Day07;

public sealed class Day07 : Solver
{
    public Day07()
        : base(nameof(Day07).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var dir = InitializeFilesystem(input);

        long sum = 0;
        long size = GetSize(dir);

        dir.Size = size;

        if (size < 100000)
        {
            sum += size;
        }


        return t;
    }

    private long t = 0;

    private long GetSize(Directory directory)
    {
        long size = 0;

        foreach (var file in directory.Files)
        {
            size += file.Size;
        }

        foreach (var dir in directory.Directories.Values)
        {
            if (dir.Size == -1)
            {
                dir.Size = GetSize(dir);

                if (dir.Size < 100000)
                {
                    t += dir.Size;
                }
            }

            size += dir.Size;
        }

        return size;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var dir = InitializeFilesystem(input);


        long sum = 0;
        long size = GetSize(dir);

        dir.Size = size;

        if (size < 100000)
        {
            sum += size;
        }

        var root = dir;
        long need = 30000000 - (70000000 - root.Size);


        Ccc(root, need);

        return this.min;
    }
    
    
    private static Directory InitializeFilesystem(StreamReader input)
    {
        var root = new Directory
        {
            Name = "/",
            Parent = null
        };
        
        var currentDir = root;

        var state = State.WaitingForBeginning;

        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;

            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            var tokens = line.Split();

            foreach (var token in tokens)
            {
                var tokenType = GetTokenType(token);

                var newState = Grammar[state][tokenType];

                if (newState == State.EndCd)
                {
                    switch (token)
                    {
                        case "/":
                        {
                            currentDir = root;
                            break;
                        }
                        case "..":
                        {
                            currentDir = currentDir!.Parent;
                            break;
                        }
                        default:
                        {
                            
                        }
                    }
                }
            }
        }

        return root;
    }

    private static Dictionary<State, Dictionary<TokenType, State>> Grammar = new()
    {
        {
            State.WaitingForBeginning, new Dictionary<TokenType, State>
            {
                { TokenType.CommandStart, State.WaitingForCommand }
            }
        },
        {
            State.WaitingForCommand, new Dictionary<TokenType, State>
            {
                { TokenType.Cd, State.WaitingForCdArgument },
                { TokenType.Ls, State.WaitingForLsResult }
            }
        },
        {
            State.WaitingForCdArgument, new Dictionary<TokenType, State>
            {
                { TokenType.Root, State.WaitingForBeginning },
                { TokenType.Up, State.WaitingForBeginning },
                { TokenType.String, State.WaitingForBeginning }
            }
        },
        {
            State.EndCd, new Dictionary<TokenType, State>
            {
                { TokenType.CommandStart, State.WaitingForCommand }
            }
        },
        {
            State.WaitingForLsResult, new Dictionary<TokenType, State>
            {
                { TokenType.Dir, State.WaitingForDirName },
                { TokenType.Int, State.WaitingForFileName }
            }
        },
        {
            State.WaitingForDirName, new Dictionary<TokenType, State>
            {
                { TokenType.String, State.WaitingForLsResult },
            }
        },
        {
            State.WaitingForFileName, new Dictionary<TokenType, State>
            {
                { TokenType.String, State.WaitingForLsResult },
            }
        }
    };

    private enum State
    {
        WaitingForBeginning,
        WaitingForCommand,
        WaitingForCdArgument,
        EndCd,
        WaitingForLsResult,
        WaitingForDirName,
        WaitingForFileName
    }

    private enum TokenType
    {
        CommandStart = 0,
        Cd = 1,
        Ls = 2,
        Root = 3,
        Up = 4,
        Dir = 5,
        String = 6,
        Int = 7
    }

    private static TokenType GetTokenType(string token)
    {
        if (string.Equals(token, "$"))
        {
            return TokenType.CommandStart;
        }

        return token switch
        {
            "$" => TokenType.CommandStart,
            "cd" => TokenType.Cd,
            "ls" => TokenType.Ls,
            "/" => TokenType.Root,
            ".." => TokenType.Up,
            "dir" => TokenType.Dir,
            _ => token.All(char.IsDigit) ? TokenType.Int : TokenType.String
        };
    }

    private static Directory InitializeFilesystem1(StreamReader input)
    {
        var root = new Directory
        {
            Name = "/",
            Parent = null
        };
        var currentDir = root;

        var line = input.ReadLine()!;
        while (!input.EndOfStream)
        {
            
            var split = line.Split(' ');

            if (line[0] == '$')
            {
                var command = split[1];

                if (command == "cd")
                {
                    var argDirName = split[2];

                    if (argDirName == "/")
                    {
                        currentDir = root;
                    }
                    else if (argDirName == "..")
                    {
                        currentDir = currentDir.Parent;
                    }
                    else
                    {
                        if (!currentDir.Directories.TryGetValue(argDirName, out var argDir))
                        {
                            argDir = new Directory
                            {
                                Name = argDirName,
                                Parent = currentDir
                            };

                            currentDir.Directories[argDirName] = argDir;
                        }

                        currentDir = argDir;
                    }

                    line = input.ReadLine();
                }
                else if (command == "ls")
                {
                    line = input.ReadLine();
                    while (!input.EndOfStream)
                    {
                        if (line[0] == '$')
                        {
                            break;
                        }

                        split = line.Split();
                        var argDirNameOrSize = split[0];

                        if (argDirNameOrSize == "dir")
                        {
                            var argDirName = split[1];

                            if (!currentDir.Directories.TryGetValue(argDirName, out var argDir))
                            {
                                argDir = new Directory
                                {
                                    Name = argDirName,
                                    Parent = currentDir
                                };

                                currentDir.Directories[argDirName] = argDir;
                            }

                            argDir.Parent = currentDir;
                        }
                        else
                        {
                            var size = long.Parse(split[0]);
                            var name = split[1];

                            var file = new File
                            {
                                Name = name,
                                Size = size
                            };

                            currentDir.Files.Add(file);
                        }

                        line = input.ReadLine();
                    }
                }
            }
        }

        return root;
    }

    private long min = long.MaxValue;

    private void Ccc(Directory directory, long need)
    {
        if (directory.Size < need)
        {
            return;
        }

        this.min = Math.Min(this.min, directory.Size);

        foreach (var value in directory.Directories.Values)
        {
            Ccc(value, need);
        }
    }

    private class File
    {
        public string Name { get; set; }

        public long Size { get; set; }

        protected bool Equals(File other)
        {
            return this.Name == other.Name;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((File)obj);
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }

    private class Directory
    {
        public string Name { get; set; }

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
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
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