using System;
using System.Collections.Generic;
using System.Linq;

namespace NbiMapApi.Server.Models.TSP
{
  public class Population
    {
        // Member variables
        public List<Tour> Paths { get; private set; }
        public double MaxFit { get; private set; }
        
        private Random _rnd = new Random();

        // ctor
        public Population(List<Tour> l)
        {
            Paths = l;
            MaxFit = CalcMaxFit();
        }

        // Functionality
        public static Population Randomized(Tour t, int n)
        {
            var tmp = new List<Tour>();

            for (var i = 0; i < n; ++i)
            {
                tmp.Add(t.Shuffle());
            }

            return new Population(tmp);
        }

        private double CalcMaxFit() => Paths.Max(t => t.Fitness);

        public Tour Select()
        {
            while (true)
            {
                var i = _rnd.Next(0, TspSettings.PopSize);

                if (_rnd.NextDouble() < Paths[i].Fitness / MaxFit)
                {
                    return new Tour(Paths[i].Points);
                }
            }
        }

        public Population GenNewPop(int n)
        {
            var p = new List<Tour>();

            for (var i = 0; i < n; ++i)
            {
                var t = Select().Crossover(Select());

                foreach (var c in t.Points)
                {
                    t = t.Mutate();
                }

                p.Add(t);
            }

            return new Population(p);
        }

        public Population Elite(int n)
        {
            var best = new List<Tour>();
            var tmp = new Population(Paths);

            for (var i = 0; i < n; ++i)
            {
                best.Add(tmp.FindBest());
                tmp = new Population(tmp.Paths.Except(best).ToList());
            }

            return new Population(best);
        }

        public Tour FindBest()
        {
            foreach (var t in Paths)
            {
                if (Math.Abs(t.Fitness - MaxFit) < double.Epsilon)
                {
                    return t;
                }
            }

            // Should never happen, it's here to shut up the compiler
            return null;
        }

        public Population Evolve()
        {
            var best = Elite(TspSettings.Elitism);
            var np = GenNewPop(TspSettings.PopSize - TspSettings.Elitism);

            return new Population(best.Paths.Concat(np.Paths).ToList());
        }
    }
}