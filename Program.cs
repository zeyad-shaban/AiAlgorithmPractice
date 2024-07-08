
static class Params
{
    public static float ANT_COLONY_FACTOR = 0.5f;
    public static float RANDOM_VISIT_PROBABILITY = 0.25f;
    public static float ALPHA = 2;
    public static float BETA = 3;
    public static float EVAPORATION_RATE = 0.5f;
}

class Ant
{
    private static Random random = new();
    public int CurrAttraction;
    public float Distance = 0;
    public float[][] AttractionsDistance;
    public float[][] PheromoneTrails;
    public LinkedList<int> ToVisit = [];
    public LinkedList<int> Visited = [];


    public Ant(int start, float[][] attractionsDistance, float[][] pheromoneTrails)
    {
        CurrAttraction = start;
        AttractionsDistance = attractionsDistance;
        PheromoneTrails = pheromoneTrails;
        for (int i = 0; i < AttractionsDistance.Length; ++i) if (i != CurrAttraction) ToVisit.AddLast(i);
        Visited.AddLast(CurrAttraction);
    }

    public void CompleteTour()
    {
        while (ToVisit.Count > 0)
        {
            Visit();
        }

    }

    public void Visit()
    {
        int destinationIdx = random.NextDouble() < Params.RANDOM_VISIT_PROBABILITY ? random.Next(0, ToVisit.Count) : ProbabalisticVisit();
        int destinationVal = ToVisit.ElementAt(destinationIdx);

        Distance += AttractionsDistance[CurrAttraction][destinationVal];
        CurrAttraction = destinationVal;
        ToVisit.Remove(destinationVal);
        Visited.AddLast(CurrAttraction);
    }

    public void Reset()
    {
        ToVisit = [];
        Visited = [];
        CurrAttraction = random.Next(0, AttractionsDistance.Length);
        Distance = 0;

        for (int i = 0; i < AttractionsDistance.Length; ++i) if (i != CurrAttraction) ToVisit.AddLast(i);
        Visited.AddLast(CurrAttraction);
    }

    private int ProbabalisticVisit()
    {
        float total = 0;
        float[] values = new float[ToVisit.Count];
        int i = -1;
        foreach (int node in ToVisit)
        {
            values[++i] = (float)(Math.Pow(PheromoneTrails[CurrAttraction][node], Params.ALPHA) * Math.Pow(1 / AttractionsDistance[CurrAttraction][node], Params.BETA));
            total += values[i];
        }

        float[] slices = new float[values.Length];
        for (i = 0; i < values.Length; ++i)
        {
            slices[i] = (i == 0 ? 0 : slices[i - 1]) + values[i] / total;
        }

        // spin wheel and return result
        float randomSpin = (float)(random.NextDouble() * slices[^1]);

        int randomIdx = Array.BinarySearch(slices, randomSpin);
        if (randomIdx < 0) randomIdx = ~randomIdx;

        return randomIdx;
    }
}

class Program
{
    static void Main()
    {
        float[][] attractions = [
                [0, 569, 476, 894, 784],
                [569, 0, 408, 736, 1154],
                [476, 408, 0, 434, 774],
                [894, 736, 434, 0, 852],
                [784, 1154, 774, 852, 0],
            ];

        float[][] pheromoneTrails = SetupPhermones(attractions);

        Ant[] colony = SetupColony(attractions, pheromoneTrails, Params.ANT_COLONY_FACTOR);

        float bestDistance = float.MaxValue;
        LinkedList<int> bestPath = null;

        int generations = -1;
        while (bestDistance != 2195)
        {
            // complete tour
            foreach (Ant ant in colony) ant.CompleteTour();

            // Evaborate
            for (int x = 0; x < attractions.Length; ++x)
            {
                for (int y = 0; y < attractions[0].Length; ++y)
                {
                    pheromoneTrails[x][y] *= Params.EVAPORATION_RATE;
                }
            }

            // add phermones
            foreach (Ant ant in colony)
            {
                if (ant.Distance < bestDistance)
                {
                    bestDistance = ant.Distance;
                    bestPath = new LinkedList<int>(ant.ToVisit);
                }
                int start = -1;
                int end = 0;
                while (end + 1 < ant.Visited.Count)
                {
                    pheromoneTrails[++start][++end] += 1 / ant.Distance;
                    ant.Reset();
                }
            }
            Console.WriteLine($"Generation: {++generations}, Best Ant: {bestDistance}");
        }

        Console.WriteLine($"Done, best ant of distance: {bestDistance}");
    }

    private static Ant[] SetupColony(float[][] attractions, float[][] pheromoneTrails, float antFactor)
    {
        Random random = new();
        Ant[] colony = new Ant[(int)(attractions.Length * antFactor)];

        for (int i = 0; i < colony.Length; ++i)
        {
            colony[i] = new Ant(random.Next(0, attractions.Length), attractions, pheromoneTrails);
        }

        return colony;
    }

    private static float[][] SetupPhermones(float[][] attractions)
    {
        float[][] pheromoneTrails = new float[attractions.Length][];
        for (int x = 0; x < attractions.Length; ++x)
        {
            pheromoneTrails[x] = new float[attractions[0].Length];
            for (int y = 0; y < attractions[0].Length; ++y)
            {
                pheromoneTrails[x][y] = 1;
            }
        }

        return pheromoneTrails;
    }
}