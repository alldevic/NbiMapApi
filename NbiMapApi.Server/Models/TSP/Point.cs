using System;

namespace NbiMapApi.Server.Models.TSP
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double DistanceTo(Point c) => Math.Sqrt(Math.Pow(c.X - X, 2) + Math.Pow(c.Y - Y, 2));


        public static Point Random()
        {
            var rnd = new Random();
            return new Point(rnd.NextDouble(), rnd.NextDouble());
        }
    }
}