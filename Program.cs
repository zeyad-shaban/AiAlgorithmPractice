using System;
using System.Globalization;
using CsvHelper;

public class Data
{
    public float age { get; set; }
    public string? dead { get; set; }
}

class Program
{
    static public void Predict(Data point, double threshold)
    {
        point.dead = point.age < threshold ? "no" : "yes";
    }

    static public double FindThreshold(Data[] data)
    {
        double maxNo = double.MinValue;
        double minYes = double.MaxValue;

        foreach (Data d in data)
        {
            if (d.dead == "no" && d.age > maxNo) maxNo = d.age;
            else if (d.dead == "yes" && d.age < minYes) minYes = d.age;
        }

        return (minYes - maxNo) / 2 + maxNo;
    }

    static void Main()
    {
        var reader = new StreamReader("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/age.csv");
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        Data[] records = csv.GetRecords<Data>().ToArray();

        double threshold = FindThreshold(records);

        Data testPnt = new Data();
        testPnt.age = 200;
        Predict(testPnt, threshold);
        Console.WriteLine(testPnt.dead);
    }
}