static class MathUtils
{
    public static (double[] x, double x_avg, double[] y, double y_avg) LoadXYFromFile(string path)
    {
        string[] lines = File.ReadAllLines("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/data.csv");

        int[] ids = new int[lines.Length - 1];
        double[] carats = new double[lines.Length - 1];
        double[] prices = new double[lines.Length - 1];

        double caratsSum = 0;
        double pricesSum = 0;

        for (int i = 1; i < lines.Length; ++i)
        {
            string[] line = lines[i].Split(",");
            (ids[i - 1], carats[i - 1], prices[i - 1]) = (int.Parse(line[0]), double.Parse(line[1]), double.Parse(line[2]));
            caratsSum += carats[i - 1];
            pricesSum += prices[i - 1];
        }

        return (carats, caratsSum / carats.Length, prices, pricesSum / prices.Length);
    }

    public static (double m, double c) LinearRegression(double[] xs, double x_avg, double[] ys, double y_avg)
    {
        double a = 0;
        double b = 0;
        for (int i = 0; i < xs.Length; ++i)
        {
            double x = xs[i];
            double y = ys[i];

            a += (x - x_avg) * (y - y_avg);
            b += Math.Pow(x - x_avg, 2);
        }

        double m = a / b;
        return (m, y_avg - m * x_avg);
    }
}