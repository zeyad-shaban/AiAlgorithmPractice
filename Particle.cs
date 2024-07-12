class Particle
{
    private static readonly Random random = new();
    public static Particle? BestParticle;
    public double velocity = 0;

    public (double x, double y) pos;
    public (double x, double y) bestPos;

    public double fitness;
    public double bestFitness = double.MaxValue;

    public Particle((double x, double y) pos)
    {
        this.pos = pos;
        UpdateFitness();
    }

    public void MoveParticle()
    {
        UpdateVelocity();
        UpdatePosition();
        UpdateFitness();
    }

    private void UpdateFitness()
    {
        fitness = Math.Pow(pos.x + 2 * pos.y - 7, 2) + Math.Pow(2 * pos.x + pos.y - 5, 2);

        if (BestParticle == null || fitness < BestParticle.bestFitness) BestParticle = this;
        if (fitness < bestFitness)
        {
            bestPos = pos;
            bestFitness = fitness;
        }
    }

    private void UpdateVelocity()
    {
        if (BestParticle == null)
        {
            throw new Exception("bruh how is there no best particle");
        }
        double inertia = Params.INERTIA_CONST * velocity;
        double cognitive = Params.COGNITIVE_CONST * random.NextDouble() * Math.Sqrt(Math.Pow(bestPos.x - pos.x, 2) + Math.Pow(bestPos.y - pos.y, 2));
        double social = Params.SOCIAL_CONST * random.NextDouble() * Math.Sqrt(Math.Pow(BestParticle.bestPos.x - pos.x, 2) + Math.Pow(BestParticle.bestPos.y - pos.y, 2));

        velocity = inertia + cognitive + social;
    }

    private void UpdatePosition()
    {
        pos = (pos.x + velocity, pos.y + velocity);
    }
}