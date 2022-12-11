namespace AdventOfCode2022.Day11;

public sealed class Day11 : Solver
{
    public Day11()
        : base(nameof(Day11).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var monkeys = GetMonkeys(input);

        for (var i = 0; i < 20; i++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    var newConcernValue = monkey.Operation(item);
                    newConcernValue = (long)((double)newConcernValue / 3);
                    
                    var newMonkeyIndex = monkey.Throw(monkey.Test(newConcernValue));
                    monkeys[newMonkeyIndex].Items.Add(newConcernValue);

                    monkey.InspectedItems++;
                }

                monkey.Items.Clear();
            }
        }

        monkeys.Sort(Comparison);
        return monkeys[0].InspectedItems * monkeys[1].InspectedItems;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var monkeys = GetMonkeys(input);

        var leastCommonMultiple = 1;
        foreach (var monkey in monkeys)
        {
            leastCommonMultiple *= monkey.TestValue;
        }

        for (var i = 0; i < 10000; i++)
        {
            foreach (var monkey in monkeys)
            {
                foreach (var item in monkey.Items)
                {
                    var newValue = monkey.Operation(item);
                    var newMonkey = monkey.Throw(monkey.Test(newValue));
                    monkeys[newMonkey].Items.Add(newValue % leastCommonMultiple);

                    monkey.InspectedItems++;
                }

                monkey.Items.Clear();
            }
        }

        monkeys.Sort(Comparison);

        monkeys.Sort(Comparison);
        return (long)monkeys[0].InspectedItems * monkeys[1].InspectedItems;
    }

    private static List<Monkey> GetMonkeys(StreamReader input)
    {
        var monkeys = new List<Monkey>();
        while (!input.EndOfStream)
        {
            var monkey = new Monkey();
            var title = input.ReadLine();

            var itemsLine = input.ReadLine()!;
            monkey.FillItems(itemsLine);

            var operationLine = input.ReadLine()!;
            monkey.FillOperation(operationLine);

            var testLine = input.ReadLine()!;
            var trueLine = input.ReadLine()!;
            var falseLine = input.ReadLine()!;
            monkey.FillTest(testLine, trueLine, falseLine);

            input.ReadLine();

            monkeys.Add(monkey);
        }

        return monkeys;
    }

    private static int Comparison(Monkey x, Monkey y)
    {
        return -x.InspectedItems.CompareTo(y.InspectedItems);
    }

    private class Monkey
    {
        public List<long> Items { get; } = new();

        public Func<long, long> Operation { get; private set; } = _ => 0;

        public Func<long, bool> Test { get; set; } = _ => false;

        public Func<bool, int> Throw { get; set; } = _ => 0;

        public int InspectedItems { get; set; }
        
        public int TestValue { get; set; }

        public void FillItems(string input)
        {
            var itemLine = input[(input.IndexOf(':') + 1)..];
            var itemValues = itemLine.Split(", ", StringSplitOptions.RemoveEmptyEntries);

            foreach (var itemValue in itemValues)
            {
                var item = long.Parse(itemValue);
                this.Items.Add(item);
            }
        }

        public void FillOperation(string input)
        {
            var operationLine = input[(input.IndexOf("d ", StringComparison.InvariantCultureIgnoreCase) + 2)..];
            var operationType = operationLine[0];
            var operationValue = operationLine[(operationLine.IndexOf(' ') + 1)..];
            
            if (string.Compare(operationValue, "old", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                var value = long.Parse(operationValue);
                
                this.Operation = operationType switch
                {
                    '+' => old => old + value,
                    '*' => old => old * value,
                    _ => this.Operation
                };
            }
            else
            {
                this.Operation = operationType switch
                {
                    '+' => old => old + old,
                    '*' => old => old * old,
                    _ => this.Operation
                };
            }
        }

        public void FillTest(string testLine, string trueLine, string falseLine)
        {
            var monkeyIndex = testLine[(testLine.IndexOf("by", StringComparison.InvariantCultureIgnoreCase) + 2)..];
            var testValue = int.Parse(monkeyIndex);
            
            this.Test = item => item % testValue == 0;
            this.TestValue = testValue;

            monkeyIndex = trueLine[(trueLine.IndexOf("monkey", StringComparison.InvariantCultureIgnoreCase) + 6)..];
            var monkeyIfTrue = int.Parse(monkeyIndex);

            monkeyIndex = falseLine[(falseLine.IndexOf("monkey", StringComparison.InvariantCultureIgnoreCase) + 6)..];
            var ifFalse = int.Parse(monkeyIndex);
            
            this.Throw = testResult => testResult ? monkeyIfTrue : ifFalse;
        }
    }
}