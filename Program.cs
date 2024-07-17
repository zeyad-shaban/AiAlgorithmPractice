using System;
using System.Diagnostics;
using ScottPlot;

class Program
{
    static double GetY(double x, double m, double c) => m * x + c;

    static void Main()
    {
        var (x, x_avg, y, y_avg) = MathUtils.LoadXYFromFile("./data.csv");
        (double m, double c) = MathUtils.LinearRegression(x, x_avg, y, y_avg);

        Plot plt = new();

        plt.Add.Scatter(x, y);

        double xmin = x.Min();
        double xmax = x.Max();

        double ymin = GetY(xmin, m, c);
        double ymax = GetY(xmax, m, c);

        plt.Add.Line(xmin, ymin, xmax, ymax); // Red dashed line


        // now how to plot the slope i have from m, c
        plt.SavePng("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/quickstart.png", 400, 300);
    }
}