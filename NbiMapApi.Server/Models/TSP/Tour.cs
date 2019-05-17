using System;
using System.Collections.Generic;
using System.Linq;


namespace NbiMapApi.Server.Models.TSP
{
    public class Tour
    {
        // Member variables
        public List<Point> Points { get; private set; }
        public double Distance { get; private set; }
        public double Fitness { get; private set; }

        private Random _rnd = new Random();

        // ctor
        public Tour(List<Point> points)
        {
            Points = points;
            Distance = CalcDist();
            Fitness = CalcFit();
        }

        // Functionality
        public static Tour Random(int n)
        {
            var t = new List<Point>();

            for (var i = 0; i < n; ++i)
            {
                t.Add(Point.Random());
            }

            return new Tour(t);
        }

        public Tour Shuffle()
        {
            var tmp = new List<Point>(Points);
            var n = tmp.Count;
            
            while (n > 1)
            {
                n--;
                var k = _rnd.Next(n + 1);
                var v = tmp[k];
                tmp[k] = tmp[n];
                tmp[n] = v;
            }

            return new Tour(tmp);
        }

        public Tour Crossover(Tour m)
        {
            var i = _rnd.Next(0, m.Points.Count);
            var j = _rnd.Next(i, m.Points.Count);
            var s = Points.GetRange(i, j - i + 1);
            var ms = m.Points.Except(s).ToList();
            var c = ms.Take(i)
                .Concat(s)
                .Concat(ms.Skip(i))
                .ToList();
            return new Tour(c);
        }

        public Tour Mutate()
        {
            var tmp = new List<Point>(Points);

            if (!(_rnd.NextDouble() < TspSettings.MutRate))
            {
                return new Tour(tmp);
            }

            var i = _rnd.Next(0, Points.Count);
            var j = _rnd.Next(0, Points.Count);
            var v = tmp[i];
            tmp[i] = tmp[j];
            tmp[j] = v;

            return new Tour(tmp);
        }

        private double CalcDist()
        {
            double total = 0;
            for (var i = 0; i < Points.Count; ++i)
            {
                total += Points[i].DistanceTo(Points[(i + 1) % Points.Count]);
            }

            return total;

            // Execution time is doubled by using linq in this case
            //return this.t.Sum( c => c.distanceTo(this.t[ (this.t.IndexOf(c) + 1) % this.t.Count] ) );
        }

        private double CalcFit() => 1 / Distance;
    }
}