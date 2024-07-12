
static class Genetics
{
    internal static int[][] CreatePopulation(int[,] cities, int size)
    {
        Random random = new();
        int[][] population = new int[size][];

        for (int indivIdx = 0; indivIdx < population.Length; ++indivIdx)
        {
            population[indivIdx] = new int[cities.GetLength(0)];
            for (int geneIdx = 0; geneIdx < population[0].Length; ++geneIdx)
            {
                population[indivIdx][geneIdx] = random.Next(0, population[0].Length);
            }
        }

        return population;
    }

    internal static float[] MeasureFitness(int[][] population, int[,] cities)
    {
        float didntVisitAllPenality = 1000;
        float[] fitness = new float[population.Length];

        for (int i = 0; i < fitness.Length; ++i)
        {
            float score = 0;
            int[] individual = population[i];
            bool[] visited = new bool[individual.Length];

            int prev = individual[0];

            for (int geneIdx = 1; geneIdx < population[0].Length; ++geneIdx)
            {
                int next = individual[geneIdx];
                score += cities[prev, next];

                visited[prev] = true;
                visited[next] = true;

                prev = next;
            }

            foreach (bool didVisit in visited)
                if (!didVisit)
                    score += didntVisitAllPenality;

            fitness[i] = score;
        }

        return fitness;
    }

    internal static void Populate(int[][] population, int[,] cities)
    {
        Random random = new();
        int targetKills = (int)(population.Length * Params.KILL_PERCENTAGE);

        for (int offspringIdx = targetKills; offspringIdx < population.Length; ++offspringIdx)
        {
            int[] parentA = population[random.Next(0, targetKills)];
            int[] parentB = population[random.Next(0, targetKills)];

            (int[] offspringA, int[] offspringB) = Crossover(parentA, parentB);

            if (random.NextDouble() < Params.INDIVIDUAL_MUTATION_CHANCE) Mutate(offspringA, cities);
            if (random.NextDouble() < Params.INDIVIDUAL_MUTATION_CHANCE) Mutate(offspringB, cities);

            population[offspringIdx++] = offspringA;
            if (offspringIdx < population.Length) population[offspringIdx] = offspringB;
        }
    }

    private static void Mutate(int[] offspring, int[,] cities)
    {
        Random random = new();
        for (int i = 0; i < offspring.Length; ++i)
        {
            if (random.NextDouble() < Params.GENE_MUTATE_CHANCE) offspring[i] = random.Next(0, cities.GetLength(0));
        }
    }

    private static (int[] offspringA, int[] offspringB) Crossover(int[] parentA, int[] parentB)
    {
        Random random = new();
        int[] offspringA = new int[parentA.Length];
        int[] offspringB = new int[parentA.Length];

        int[] offspring = offspringA;
        int mainMaskSelection = 0;

        for (int i = 0; i < 2; ++i)
        {
            for (int geneIdx = 0; geneIdx < parentA.Length; ++geneIdx)
            {
                offspring[geneIdx] = random.Next(0, 2) == mainMaskSelection ? parentA[geneIdx] : parentB[geneIdx];
            }

            offspring = offspringB;
            mainMaskSelection = 1;
        }

        return (offspringA, offspringB);
    }
}