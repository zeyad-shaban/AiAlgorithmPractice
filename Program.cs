class Program
{
    static void Main()
    {
        int[,] cities = new int[,]
        {
            {int.MaxValue, 29, 20, 21, 16, 31, 100, 12, 4, 31 },
            { 29,int.MaxValue, 15, 29, 28, 40, 72, 21, 29, 41 },
            { 20, 15,int.MaxValue, 15, 14, 25, 81, 9, 23, 27 },
            { 21, 29, 15,int.MaxValue, 4, 12, 92, 12, 25, 13 },
            { 16, 28, 14, 4,int.MaxValue, 16, 94, 9, 20, 16 },
            { 31, 40, 25, 12, 16,int.MaxValue, 95, 24, 36, 3 },
            { 100, 72, 81, 92, 94, 95,int.MaxValue, 90, 101, 99 },
            { 12, 21, 9, 12, 9, 24, 90,int.MaxValue, 15, 25 },
            { 4, 29, 23, 25, 20, 36, 101, 15,int.MaxValue, 35 },
            { 31, 41, 27, 13, 16, 3, 99, 25, 35,int.MaxValue }
        };




        int[][] population = Genetics.CreatePopulation(cities, Params.POPULATION_SIZE);

        for (int gen = 0; gen < Params.TOTAL_GENERATIONS; ++gen)
        {
            float[] fitness = Genetics.MeasureFitness(population, cities);
            Array.Sort(fitness, population);

            Genetics.Populate(population, cities);
            Console.WriteLine($"Generation {gen}, best {fitness[0]} {string.Join("->", population[0])}");
        }

        Console.WriteLine(string.Join("->", population[0]));

    }
}