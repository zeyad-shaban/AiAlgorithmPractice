public static class Globals
{
    public static int POPULATION_SIZE = 10000;
    public static float MUTATION_RATE = 0.25f;
    public static float REPLACE_PERCENTAGE = 0.4f;
    public static int MAX_LEN = 100;
    public static string CORRECT_WORD = "I'm an ai developer, IM AN AI DEVELOPER, I AM THE AI DEVELOPER, NOTHING CAN STOP ME WITH THE POWER OF AI, N O T H I N G SIGN UP AND START HAVING FUN! asfdsafjsadk wqqpiehwer rqwiopr2jrio 4j3j231980u23194j 123k4 231l;j 321l 3l2jasdffkasdl'f sa'dff dsafdsF dsafj sadf";
};


class Program
{
    static void Main()
    {
        char[][] population = CreatePopulation(Globals.POPULATION_SIZE);

        int generation = -1;
        int strongestIdx = 0;
        while (new string(population[strongestIdx]) != Globals.CORRECT_WORD)
        {
            (int[] fitness, int totalFitness, strongestIdx) = MeasureFitness(population, Globals.CORRECT_WORD);

            float[] slices = CreateSlices(fitness, totalFitness);
            float[] weakSlices = CreateSlices(fitness, totalFitness, true, fitness[strongestIdx]);

            char[][] offsprings = SelectAndReproduce(slices, population);

            Populate(population, offsprings, weakSlices);
            Console.WriteLine($"Generation: {++generation}, strongest: {new string(population[strongestIdx])}");
        }

        Console.WriteLine($"Word found: \"{new string(population[strongestIdx])}\" \n Generations taken: {generation}");
    }

    private static float[] CreateSlices(int[] fitness, float totalFitness, bool shouldInverse = false, int strongest = -1)
    {
        float[] slices = new float[fitness.Length];

        slices[0] = (shouldInverse ? strongest - fitness[0] : fitness[0]) / totalFitness;
        for (int i = 1; i < fitness.Length; ++i)
        {
            slices[i] = slices[i - 1] + (shouldInverse ? strongest - fitness[i] : fitness[i]) / totalFitness;
        }
        return slices;
    }

    private static void Populate(char[][] population, char[][] offsprings, float[] weakSlices)
    {
        Random random = new();
        int targetKills = (int)(Globals.REPLACE_PERCENTAGE * population.Length);
        int[] killedIndices = new int[targetKills];

        for (int killed = 0; killed < targetKills; ++killed)
        {
            int killIdx = BinSearch(weakSlices, (float)random.NextDouble() * weakSlices[^1]);
            if (population[killIdx] != null)
            {
                killedIndices[killed] = killIdx;
                population[killIdx] = null;
            }
            else --killIdx;
        }

        int offspringIdx = -1;
        foreach (int idx in killedIndices)
        {
            population[idx] = offsprings[++offspringIdx];
        }
    }

    private static char[][] SelectAndReproduce(float[] slices, char[][] population)
    {
        Random random = new();
        int targetOffsprings = (int)(Globals.REPLACE_PERCENTAGE * population.Length);

        char[][] offsprings = new char[targetOffsprings][];


        for (int i = 0; i < targetOffsprings; ++i)
        {
            char[] parentA = population[BinSearch(slices, (float)random.NextDouble() * slices[^1])];
            char[] parentB = population[BinSearch(slices, (float)random.NextDouble() * slices[^1])];

            (char[] offspring1, char[] offspring2) = CrossOver(parentA, parentB);
            offsprings[i] = offspring1;
            if (i + 1 < targetOffsprings) offsprings[++i] = offspring2;
        }

        return offsprings;
    }

    private static int BinSearch(float[] slices, float target)
    {
        int min = 0;
        int max = slices.Length - 1;
        int idx = -1;

        while (min <= max)
        {
            idx = (max - min) / 2 + min;
            if ((idx == 0 || slices[idx - 1] <= target) && target <= slices[idx])
            {
                break;
            }

            if (target < slices[idx]) max = idx - 1;
            else if (target > slices[idx]) min = idx + 1;
        }

        return idx;
    }

    private static (char[] offspring1, char[] offspring2) CrossOver(char[] parentA, char[] parentB)
    {
        Random random = new();
        char[] offspring1 = new char[parentA.Length];
        char[] offspring2 = new char[parentA.Length];

        for (int geneIdx = 0; geneIdx < offspring1.Length; ++geneIdx)
        {
            int pick = random.Next(0, 2);
            if (pick == 0)
            {
                offspring1[geneIdx] = geneIdx >= parentA.Length ? GetRandomLetter() : parentA[geneIdx];
                offspring2[geneIdx] = geneIdx >= parentB.Length ? GetRandomLetter() : parentB[geneIdx];
            }
            else
            {
                offspring1[geneIdx] = geneIdx >= parentB.Length ? GetRandomLetter() : parentB[geneIdx];
                offspring2[geneIdx] = geneIdx >= parentA.Length ? GetRandomLetter() : parentA[geneIdx];
            }
        }

        bool shouldMutate1 = random.NextDouble() < Globals.MUTATION_RATE;
        bool shouldMutate2 = random.NextDouble() < Globals.MUTATION_RATE;

        if (shouldMutate1) Mutate(offspring1);
        if (shouldMutate2) Mutate(offspring2);

        return (offspring1, offspring2);
    }

    private static void Mutate(char[] offspring)
    {
        Random random = new();
        for (int i = 0; i < offspring.Length; ++i)
        {
            if (random.Next(0, 10) < 5) offspring[i] = (char)random.Next(0, Globals.MAX_LEN);
        }
    }

    private static (int[] fitness, int totalFitness, int strongestIdx) MeasureFitness(char[][] population, string correctWord)
    {
        int totalFitness = 0;
        int[] fitness = new int[population.Length];
        int strongestIdx = -1;
        int strongestVal = -1;
        for (int indivIdx = 0; indivIdx < population.Length; ++indivIdx)
        {
            int score = 0;
            char[] individual = population[indivIdx];

            for (int letterIdx = 0; letterIdx < correctWord.Length; ++letterIdx)
            {
                if (individual[letterIdx] == correctWord[letterIdx]) ++score;
            }

            fitness[indivIdx] = score;

            totalFitness += score;

            if (score > strongestVal)
            {
                strongestVal = score;
                strongestIdx = indivIdx;
            }
        }

        return (fitness, totalFitness, strongestIdx);
    }

    private static char[][] CreatePopulation(int POPULATION_SIZE)
    {
        Random random = new();
        char[][] population = new char[POPULATION_SIZE][];
        for (int indivIdx = 0; indivIdx < POPULATION_SIZE; ++indivIdx)
        {
            population[indivIdx] = new char[Globals.CORRECT_WORD.Length];
            for (int geneIdx = 0; geneIdx < Globals.CORRECT_WORD.Length; ++geneIdx)
            {
                population[indivIdx][geneIdx] = GetRandomLetter(random);
            }
        }
        return population;
    }

    private static char GetRandomLetter(Random? random = null) => (char)(random ?? new Random()).Next(' ', '~');
}