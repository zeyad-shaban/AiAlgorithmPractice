
static class Utils
{
    public static (double[] carats, double[] prices, string[] cut) LoadFromFile(string path)
    {
        string[] lines = File.ReadAllLines(path);

        int[] ids = new int[lines.Length - 1];
        double[] carats = new double[lines.Length - 1];
        double[] prices = new double[lines.Length - 1];
        string[] cuts = new string[lines.Length - 1];

        double caratsSum = 0;
        double pricesSum = 0;

        for (int i = 1; i < lines.Length; ++i)
        {
            string[] line = lines[i].Replace("\"", "").Split(",");
            // "","carat","cut","color","clarity","depth","table","price","x","y","z"
            // 0    1      2      3       4          5         6    7      8    9   10
            (ids[i - 1], carats[i - 1], prices[i - 1], cuts[i - 1]) = (int.Parse(line[0]), double.Parse(line[1]), double.Parse(line[7]), line[2]);
            caratsSum += carats[i - 1];
            pricesSum += prices[i - 1];
        }

        return (carats, prices, cuts);
    }

    internal static (T[] data_train, T[] data_test) SplitTrainTest<T>(T[] data)
    {
        int breakPoint = data.Length * 80 / 100;
        return (data[0..breakPoint], data[breakPoint..]);
    }
}