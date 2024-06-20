class Item(string name, int price, int weight) {
    public string Name { get; set; } = name;
    public int Price { get; set; } = price;
    public int Weight { get; set; } = weight;
}


class Program {
    static int MeasureFitness(int[] individual, Item[] items, int capacity) {
        int value = 0;
        int weight = 0;

        for (int i = 0; i < individual.Length; ++i) {
            value += individual[i] * items[i].Price;
            weight += individual[i] * items[i].Weight;
            if (weight > capacity) return 0;
        }

        return value;
    }

    static int[][] CreatePopulation(int popSize, int genesSize) {
        Random random = new(42);
        int[][] population = new int[popSize][];

        for (int i = 0; i < popSize; ++i) {
            population[i] = new int[genesSize];
            for (int j = 0; j < genesSize; ++j)
                population[i][j] = random.Next(0, 2);
        }

        return population;
    }

    static float[] CreateWheel(int[][] population, Item[] items, int capacity) {
        // create fitness model
        int[] fitness = new int[population.Length];
        float totalFitness = 0;

        for (int i = 0; i < population.Length; ++i) {
            fitness[i] = MeasureFitness(population[i], items, capacity);
            totalFitness += fitness[i];
        }

        // create slices
        float[] slices = new float[fitness.Length];

        slices[0] = fitness[0] / totalFitness;
        for (int i = 1; i < fitness.Length; ++i)
            slices[i] = slices[i - 1] + fitness[i] / totalFitness;

        return slices;
    }

    static int BinSearchInBetween(float[] slices, float randProbability) {
        int min = 0;
        int max = slices.Length;
        int individualIdx = -1;
        bool found = false;
        while (min < max) {
            individualIdx = (max - min) / 2 + min;
            if (randProbability <= slices[individualIdx] && (individualIdx == 0 || randProbability > slices[individualIdx - 1])) {
                found = true;
                break;
            };

            if (slices[individualIdx] < randProbability)
                min = individualIdx;
            else
                max = individualIdx;
        }

        return found ? individualIdx : -1;
    }

    static int[][] SingePointCrossOver(int[] parentA, int[] parentB, int xoverPoint) {
        return [[.. parentA[0..xoverPoint], .. parentB[xoverPoint..]], [.. parentB[0..xoverPoint], .. parentA[xoverPoint..]]];
    }
    static int[][] UniformCrossover(int[] parentA, int[] parentB) {
        int[][] children = new int[2][];

        for (int i = 0; i < children.Length; ++i) {
            children[i] = new int[parentA.Length];
            for (int gene = 0; gene < parentA.Length; ++gene)
                children[i][gene] = parentB[gene] == 0 ? parentA[gene] : parentB[gene];
        }

        return children;
    }

    static int[] FlipBitMutation(int[] individual) {
        for (int i = 0; i < individual.Length; ++i)
            individual[i] ^= 1;
        return individual;
    }

    static void Main(string[] args) {
        int capacity = 9;
        int populationSize = 100;
        Item[] items = [
            new("Ring", 4,3),
            new("Gold", 7, 7),
            new("Crown", 5,4),
            new("Coin", 1,1),
            new("Axe", 4, 5),
            new("Sword", 3,4),
            new("Ring", 5, 2),
            new("Cup", 1, 3),
    ];

        int[][] population = CreatePopulation(populationSize, items.Length);

        float[] slices = CreateWheel(population, items, capacity);

        Random random = new(42);
        float randProbability = (float)random.NextDouble() * slices[^1];

        int individual = BinSearchInBetween(slices, randProbability);

        Console.WriteLine(individual);
    }
}