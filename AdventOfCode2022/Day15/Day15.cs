using AdventOfCode2022.Common;

namespace AdventOfCode2022.Day15;

public sealed class Day15 : Solver
{
    public Day15() 
        : base(nameof(Day15).Substring(3, 2))
    {
    }

    protected override object Solve1Internal(StreamReader input)
    {
        var sensors = GetSensors(input).ToArray();

        var minX = sensors.Select(x => x.Position.X - x.Radius).Min();
        var maxX = sensors.Select(x => x.Position.X + x.Radius).Max();

        var taskLine = 2000000;
        var result = 0;
        for (var j = minX; j <= maxX; j++)
        {
            var point = new Point(j, taskLine);
            var isPointCounted = false;
            
            foreach (var sensor in sensors)
            {
                var distanceToPoint = sensor.Position.GetManhattanDistanceTo(point);
                if (distanceToPoint <= sensor.Radius && point != sensor.BeaconPosition)
                {
                    if (isPointCounted)
                    {
                        continue;
                    }
                    
                    result++;
                    isPointCounted = true;
                }
            }
        }

        return result;
    }

    protected override object Solve2Internal(StreamReader input)
    {
        var sensors = GetSensors(input).ToArray();

        var distressBeacon = Tuple.Create(-1, -1);
        for (var i = 0; i < sensors.Length; i++)
        {
            var sensor1 = sensors[i];
            for (var j = i + 1; j < sensors.Length; j++)
            {
                var sensor2 = sensors[j];
                
                var distanceBetweenSensors = sensor1.Position.GetManhattanDistanceTo(sensor2.Position);
                // if distress beacon is somewhere near this two sensors 
                if (distanceBetweenSensors == sensor1.Radius + sensor2.Radius + 2)
                {
                    // just for getting lesser outer perimeter
                    if (sensor1.Radius > sensor2.Radius)
                    {
                        (sensor1, sensor2) = (sensor2, sensor1);
                    }

                    var perimeter = sensor1.GetOuterPerimeter().ToList();

                    // check outer perimeter within all sensors
                    foreach (var point in perimeter)
                    {
                        if (CheckIfPointIsDistressBeacon(point, sensor2, sensors))
                        {
                            return (long)point.X * 4000000 + point.Y;
                        }
                    }
                }
            }
        }

        return (long)distressBeacon.Item1 * 4000000 + distressBeacon.Item2;
    }

    private static IEnumerable<Sensor> GetSensors(StreamReader input)
    {
        while (!input.EndOfStream)
        {
            var line = input.ReadLine()!;

            var split = line.Split(new[] { ' ', ':', '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            var sensorX = int.Parse(split[3]);
            var sensorY = int.Parse(split[5]);
            
            var beaconX = int.Parse(split[11]);
            var beaconY = int.Parse(split[13]);
            
            var distance = CountDistance(sensorX, sensorY, beaconX, beaconY);

            var sensor = new Sensor
            {
                Position = new Point(sensorX, sensorY),
                BeaconPosition = new Point(beaconX, beaconY),
                Radius = distance
            };

            yield return sensor;
        }
    }

    private static int CountDistance(int firstX, int firstY, int secondX, int secondY)
    {
        return Math.Abs(firstX - secondX) + Math.Abs(firstY - secondY);
    }

    private static bool CheckIfPointIsDistressBeacon(Point point, Sensor sensor2, IEnumerable<Sensor> sensors)
    {
        if (sensor2.Position.GetManhattanDistanceTo(point.X, point.Y) != sensor2.Radius + 1)
        {
            return false;
        }
        
        var canBeDistressBeacon = true;
        foreach (var sensor in sensors)
        {
            if (sensor.Position.GetManhattanDistanceTo(point.X, point.Y) <= sensor.Radius)
            {
                canBeDistressBeacon = false;
            }
        }

        return canBeDistressBeacon;
    }

    private sealed class Sensor
    {
        public Point Position { get; set; }
        
        public Point BeaconPosition { get; set; }
        
        public int Radius { get; set; }
        
        public IEnumerable<Point> GetOuterPerimeter()
        {
            var outerRadius = this.Radius + 1;
            
            yield return new Point(this.Position.X, this.Position.Y - outerRadius);
            yield return new Point(this.Position.X, this.Position.Y + outerRadius);
            
            for (var i = this.Position.Y - outerRadius + 1; i < this.Position.Y + outerRadius; i++)
            {
                var j = this.Position.X - outerRadius + Math.Abs(this.Position.Y - i);
                yield return new Point(j, i);

                j = outerRadius - Math.Abs(this.Position.Y - i) + this.Position.X;
                yield return new Point(j, i);
            }
        }
    }
}