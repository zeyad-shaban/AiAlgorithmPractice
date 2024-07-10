public static class Utils
{
    public static Random random = new();
    public static int SpinWheel(double[] probabilities, double total)
    {
        double[] slices = new double[probabilities.Length];

        for (int i = 0; i < slices.Length; ++i)
        {
            slices[i] += (i == 0 ? 0 : slices[i - 1]) + probabilities[i] / total;
        }

        int idx = Array.BinarySearch(slices, random.NextDouble() * slices[^1]);
        if (idx < 0) idx = ~idx;

        return idx;
    }
}