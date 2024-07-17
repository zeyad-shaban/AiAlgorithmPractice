static class MLFuncs
{
    public static DecisionNode<string> GenerateTree(double[][] features, string[] y, int depth = 10)
    {
        double gini = CalculateGini(y);
        if (gini == 0)
        {
            return new DecisionNode<string>(y[0]);
        }
        else if (depth <= 0)
        {
            Dictionary<string, int> groups = [];
            string mostFrequent = y[0];

            foreach (string group in y)
            {
                if (!groups.ContainsKey(group)) groups[group] = 0;
                ++groups[group];
                if (groups[group] > groups[mostFrequent]) mostFrequent = group;
            }
        }

        double bestGiniGain = -1;
        int bestFeature = -1; // keeping those two for debugging reasons
        double bestQuestion = -1;

        string[] bestFalseBranch = [];
        string[] bestTrueBranch = [];

        for (int featureIdx = 0; featureIdx < features.Length; ++featureIdx)
        {
            double[] feature = features[featureIdx];

            for (int exIdx = 0; exIdx < features[featureIdx].Length; ++exIdx)
            {
                double example = feature[exIdx];

                List<string> falseBranch = [];
                List<string> trueBranch = [];

                for (int yIdx = 0; yIdx < y.Length; ++yIdx)
                {
                    if (features[featureIdx][yIdx] >= example)
                    {
                        trueBranch.Add(y[yIdx]);
                    }
                    else
                    {
                        falseBranch.Add(y[yIdx]);
                    }
                }

                double falseGini = CalculateGini([.. falseBranch]);
                double trueGini = CalculateGini([.. trueBranch]);

                double newGini = trueBranch.Count * trueGini / y.Length + falseBranch.Count * falseGini / y.Length;
                double giniGain = gini - newGini;

                if (giniGain > bestGiniGain)
                {
                    bestGiniGain = giniGain;
                    bestFeature = featureIdx;
                    bestQuestion = example;

                    bestFalseBranch = [.. falseBranch];
                    bestTrueBranch = [.. trueBranch];
                }
            }
        }


        return new DecisionNode<string>("", bestFeature, bestQuestion, GenerateTree(features, bestFalseBranch, depth - 1), GenerateTree(features, bestTrueBranch, depth - 1));
    }

    public static double CalculateGini(string[] ys)
    {
        double gini = 0;

        Dictionary<string, int> groups = [];

        foreach (string y in ys)
        {
            if (!groups.ContainsKey(y)) groups[y] = 0;
            ++groups[y];
        }


        foreach (KeyValuePair<string, int> group in groups)
        {
            gini += Math.Pow(group.Value / ys.Length, 2);
        }

        return 1 - gini;
    }

    public static string TakeDecision(double[] features, DecisionNode<string> node)
    {
        if (node.IsLeaf)
        {
            return node.decision;
        }

        bool satisfiesQuestion = features[node.featureIdx] >= node.question;

        return TakeDecision(features, satisfiesQuestion ? node.trueBranch : node.falseBranch);
    }
}