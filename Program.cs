

class Program
{
    static void Main()
    {
        Particle[] swarm = SetupSwarm(Params.SWARM_SIZE);

        float subtractInertiaVal = Params.INERTIA_CONST / Params.TOTAL_ITTERATIONS * 2;

        for (int i = 0; i < Params.TOTAL_ITTERATIONS; ++i)
        {
            foreach (Particle particle in swarm)
            {
                particle.MoveParticle();
            }
            Params.INERTIA_CONST = Math.Max(Params.INERTIA_CONST - subtractInertiaVal, 0.01f);
            Console.WriteLine($"Best fitness: {Particle.BestParticle.bestFitness}, INERTIA: {Params.INERTIA_CONST}");
        }

        Console.WriteLine($"Best fitness: {Particle.BestParticle.bestFitness}");
        Console.WriteLine($"Alumuniom: {Particle.BestParticle.bestPos.x}, Plastic: {Particle.BestParticle.bestPos.y}");
    }


    static Particle[] SetupSwarm(int size)
    {
        Random random = new();

        Particle[] swarm = new Particle[size];

        for (int i = 0; i < swarm.Length; ++i)
        {
            swarm[i] = new Particle(GenerateRandomPos(random));
        }

        return swarm;
    }

    static private (double x, double y) GenerateRandomPos(Random random)
    {
        float minX = -10;
        float maxX = 10;

        float minY = -10;
        float maxY = 10;

        return (random.NextDouble() * (maxX - minX) + minX, random.NextDouble() * (maxY - minY) + minY);
    }
}