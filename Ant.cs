class Ant
{
    private static Random random = new();
    public int[][] routes;
    public float[][] pheromones;

    public LinkedList<int> visited = [];
    public LinkedList<int> toVisit = [];

    public int curr;
    public int fitness = 0;
    public int targetNode;

    public Ant(int[][] routes, float[][] pheromones, int start, int targetNode)
    {
        this.routes = routes;
        this.pheromones = pheromones;
        this.targetNode = targetNode;
        curr = start;

        visited.AddLast(curr);

        for (int i = 0; i < routes.Length; ++i)
        {
            if (i != curr) toVisit.AddLast(i);
        }
    }

    public void CompleteTour()
    {
        while (toVisit.Count > 0 && curr != targetNode)
        {
            int target = random.NextDouble() < Params.RANDOM_VISIT_CHANCE ? RandomVisit() : ProbabalisticVisit();

            fitness += routes[curr][target];
            toVisit.Remove(target);
            visited.AddLast(target);
            curr = target;
        }
    }

    private int RandomVisit()
    {
        int randomChoice;

        do
        {
            randomChoice = toVisit.ElementAt(random.Next(0, toVisit.Count));
        } while (routes[curr][randomChoice] == 0);

        return randomChoice;
    }

    private int ProbabalisticVisit()
    {
        double total = 0;
        double[] probabilities = new double[toVisit.Count];

        int i = 0;
        foreach (int attraction in toVisit)
        {
            double value = Math.Pow(pheromones[curr][attraction], Params.ALPHA) * (routes[curr][attraction] == 0 ? 0 : 1 / Math.Pow(routes[curr][attraction], Params.BETA));
            total += value;
            probabilities[i++] = value;
        }

        // Create Slices
        int chosenIdx = Utils.SpinWheel(probabilities, total);

        return toVisit.ElementAt(chosenIdx);
    }
}