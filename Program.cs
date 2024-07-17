using System;
using System.Diagnostics;
using ScottPlot;

class Program
{
    static double GetY(double x, double m, double c) => m * x + c;

    static void Main()
    {
        var (carats, prices, cuts) = Utils.LoadFromFile("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/data.csv");

        var (carats_train, carats_test) = Utils.SplitTrainTest<double>(carats);
        var (prices_train, prices_test) = Utils.SplitTrainTest<double>(prices);
        var (cuts_train, cuts_test) = Utils.SplitTrainTest<string>(cuts);


        DecisionNode<string> decisionTree = MLFuncs.GenerateTree([carats_train, prices_train], cuts_train);

        int validCount = 0;
        int invalidCount = 0;
        for (int i = 0; i < cuts_test.Length; ++i)
        {
            string decision = MLFuncs.TakeDecision([carats_test[i], prices_test[i]], decisionTree);
            bool isValidDecision = decision == cuts_test[i];
            if (isValidDecision) ++validCount;
            else ++invalidCount;

            Console.WriteLine($"Predicted: {decision}, Actual: {cuts_test[i]}, -> {isValidDecision}");
        }

        Console.WriteLine($"{validCount}/{invalidCount}, Accuracy: {validCount * 100.0f / (invalidCount + validCount)}%");
    }
}