class Item(string name, int weight, int price) {
    public string Name { get; set; } = name;
    public int Price { get; set; } = price;
    public int Weight { get; set; } = weight;
}


class Program {

    static void Main(string[] args) {
        const int MAX_CAPACITY = 6404180;
        const float OFFSPRINGS_PERCENTAGE = 1.1f;
        const float KILL_ANSESTORS_PERCENTAGE = 1;
        const float MUTATION_PERCENTAGE = 0.25f;
        const int INITIAL_SIZE = 10000;

        Item[] items = [
            new("Axe", 32252, 68674),
    new("Bronze coin", 225790, 471010),
    new("Crown", 468164, 944620),
    new("Diamond statue", 489494, 962094),
    new("Emerald belt", 35384, 78344),
    new("Fossil", 265590, 579152),
    new("Gold coin", 497911, 902698),
    new("Helmet", 800493, 1686515),
    new("Ink", 823576, 1688691),
    new("Jewel box", 552202, 1056157),
    new("Knife", 323618, 677562),
    new("Long sword", 382846, 833132),
    new("Mask", 44676, 99192),
    new("Necklace", 169738, 376418),
    new("Opal badge", 610876, 1253986),
    new("Pearls", 854190, 1853562),
    new("Quiver", 671123, 1320297),
    new("Ruby ring", 698180, 1301637),
    new("Silver bracelet", 446517, 859835),
    new("Timepiece", 909620, 1677534),
    new("Uniform", 904818, 1910501),
    new("Venom potion", 730061, 1528646),
    new("Wool scarf", 931932, 1827477),
    new("Crossbow", 952360, 2068204),
    new("Yesteryear book", 926023, 1746556),
    new("Zinc cup", 978724, 2100851)
        ];

        // 1. Create population [0, 1, 0, 1, 1, 1, 0]
        int[][] population = CreatePopulation(INITIAL_SIZE, items);

        int i = -1;
        while (true) {
            // 2. Measure fitness
            (int[] fitness, float totalFitness, int strongest, int strongestIdx) = MeasureFitness(population, MAX_CAPACITY, items);
            Console.WriteLine($"GENERATION {i} - STRONGEST {strongest} LENGTH {population.Length}");

            // 3. Select parents to reproduce: Roulette wheel
            float[] strongestSlices = CreateSlices(fitness, totalFitness);
            float[] weakestSlices = CreateSlices(fitness, totalFitness, true, strongest);

            int[][] offsprings = CreateOffsprings(population, strongestSlices, population.Length, OFFSPRINGS_PERCENTAGE, MUTATION_PERCENTAGE);

            population = KillAndReplace(population, weakestSlices, KILL_ANSESTORS_PERCENTAGE, offsprings);
            ++i;
            if (strongest == 13692887) {
                break;
            }
        }
    }

    private static int[][] KillAndReplace(int[][] population, float[] slices, float targetKillPercentage, int[][] offsprings) {
        int targetKills = (int)(targetKillPercentage * population.Length);

        int offSpringIdx = 0;
        for (int killed = 0; killed < targetKills && killed < offsprings.Length; ++killed) {
            int toKillIdx = PickRandomIndividualIdx(slices);
            population[toKillIdx] = offsprings[offSpringIdx++];
        }

        return [.. population, .. offsprings[offSpringIdx..]];
    }

    private static int[][] CreateOffsprings(int[][] population, float[] slices, int POPULATION_SIZE, float newPercentage, float mutationPercentage) {
        int[][] offsprings = new int[(int)(POPULATION_SIZE * newPercentage)][];

        int i = -1;
        while (i < offsprings.Length - 1) {
            int[] parentA = PickRandomIndividual(population, slices);
            int[] parentB = PickRandomIndividual(population, slices);

            if (i + 2 < offsprings.Length)
                (offsprings[++i], offsprings[++i]) = Crossover(parentA, parentB, mutationPercentage);
            else
                (offsprings[++i], _) = Crossover(parentA, parentB, mutationPercentage);
        }

        return offsprings;
    }


    private static (int[], int[]) Crossover(int[] parentA, int[] parentB, float mutationPercentage) {
        Random random = new();
        int[][] offSprings = new int[2][];
        int maskChoice = 0;
        for (int i = 0; i < offSprings.Length; ++i) {
            offSprings[i] = new int[parentA.Length];

            bool shouldMutate = random.Next(0, 10) < mutationPercentage * 10;
            for (int geneIdx = 0; geneIdx < offSprings[i].Length; ++geneIdx) {
                offSprings[i][geneIdx] = random.Next(0, 2) == maskChoice ? parentB[geneIdx] : parentA[geneIdx];
                if (shouldMutate) offSprings[i][geneIdx] ^= 1;
            }

            maskChoice = 1;
        }

        return (offSprings[0], offSprings[1]);
    }

    private static int PickRandomIndividualIdx(float[] slices) {
        Random random = new();
        float prob = (float)random.NextDouble() * slices[^1];
        int min = 0;
        int max = slices.Length - 1;
        int mid = -1;

        while (min <= max) {
            mid = (max - min) / 2 + min;

            if (prob <= slices[mid] && (mid == 0 || prob > slices[mid - 1]))
                break;

            if (prob < slices[mid]) max = mid - 1;
            else min = mid + 1;
        }

        return mid;
    }

    private static int[] PickRandomIndividual(int[][] population, float[] slices) => population[PickRandomIndividualIdx(slices)];

    private static float[] CreateSlices(int[] fitness, float totalFitness, bool invert = false, int strongest = -1) {
        float[] slices = new float[fitness.Length];
        slices[0] = invert ? (strongest - fitness[0]) / totalFitness : fitness[0] / totalFitness;

        for (int i = 1; i < slices.Length; ++i)
            slices[i] = slices[i - 1] + (invert ? (strongest - fitness[i]) / totalFitness : fitness[i] / totalFitness);

        return slices;
    }

    static int[][] CreatePopulation(int POPULATION_SIZE, Item[] items) {
        Random random = new();
        int[][] population = new int[POPULATION_SIZE][];
        for (int i = 0; i < POPULATION_SIZE; ++i) {
            population[i] = new int[items.Length];
            for (int j = 0; j < items.Length; ++j) {
                population[i][j] = random.Next(0, 2);
            }
        }

        return population;
    }

    static (int[], float, int, int) MeasureFitness(int[][] population, int MAX_CAPACITY, Item[] items) {
        int[] popFitness = new int[population.Length];
        float totalFitness = 0;
        int strongest = -1;
        int strongestIdx = -1;



        for (int individualIdx = 0; individualIdx < population.Length; ++individualIdx) {
            int[] individual = population[individualIdx];

            int fitness = 0;
            int weight = 0;

            for (int geneIdx = 0; geneIdx < individual.Length; ++geneIdx) {
                int gene = individual[geneIdx];

                fitness += items[geneIdx].Price * gene;
                totalFitness += fitness;
                weight += items[geneIdx].Weight * gene;
            }
            popFitness[individualIdx] = weight > MAX_CAPACITY ? 0 : fitness;
            if (popFitness[individualIdx] > strongest) {
                strongest = popFitness[individualIdx];
                strongestIdx = individualIdx;
            }
            strongest = Math.Max(strongest, popFitness[individualIdx]);
        }

        return (popFitness, totalFitness, strongest, strongestIdx);
    }
}