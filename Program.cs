class Program
{
    static void Main()
    {
        int targetNode = 5;
        int[][] routes =
        [
            [0, 3, 10, 0, 0, 0],
            [0, 0, 2, 7, 0, 0 ],
            [0, 0, 0, 1, 2, 0 ],
            [0, 0, 0, 0, 3, 5 ],
            [0, 0, 0, 0, 0, 1 ],
            [0, 0, 0, 0, 0, 0 ],
        ];

        float[][] pheromones = SetupPheromones(routes);
        Ant[] colony = SetupColony(routes, pheromones, targetNode);


        LinkedList<int> bestPath = [];
        int bestFitness = int.MaxValue;

        for (int i = 0; i < Params.ITTERATIONS; ++i)
        {
            foreach (Ant ant in colony)
            {
                ant.CompleteTour();

                if (ant.fitness < bestFitness)
                {
                    bestFitness = ant.fitness;
                    bestPath = new LinkedList<int>(ant.visited);
                }
            }

            for (int x = 0; x < pheromones.Length; ++x)
            {
                for (int y = 0; y < pheromones[0].Length; ++y)
                {
                    pheromones[x][y] *= Params.EVAPROATION_RATE;
                }
            }

            foreach (Ant ant in colony)
            {
                int[] arr = [.. ant.visited];
                for (int x = 0; x < arr.Length; ++x)
                {
                    for (int y = x + 1; y < arr.Length; ++y)
                    {
                        pheromones[x][y] += 1 / ant.fitness;
                    }
                }

            }
        }
        Console.WriteLine($"BestPath Value: {bestFitness}");
        foreach (int node in bestPath)
            Console.Write($"{node + 1}->");
    }

    private static Ant[] SetupColony(int[][] routes, float[][] pheromones, int targetNode)
    {
        Ant[] colony = new Ant[(int)(routes.Length * Params.ANT_FACTOR)];

        for (int i = 0; i < colony.Length; ++i)
        {
            colony[i] = new Ant(routes, pheromones, 0, targetNode);
        }

        return colony;
    }

    public static float[][] SetupPheromones(int[][] routes)
    {
        float[][] pheromones = new float[routes.Length][];

        for (int x = 0; x < routes.Length; ++x)
        {
            pheromones[x] = new float[routes[0].Length];
            for (int y = 0; y < routes.Length; ++y)
            {
                pheromones[x][y] = 1;
            }
        }

        return pheromones;
    }
}