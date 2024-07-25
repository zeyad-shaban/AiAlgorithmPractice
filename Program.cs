using System;
using System.Globalization;
using CsvHelper;

public class Data
{
    public float Height;
    public float Weight;
    public string? Sex;
}

class Program
{
    static void Main()
    {
        var reader = new StreamReader("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/dataset.csv");
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        IEnumerable<Data> records = csv.GetRecords<Data>().ToArray();
    }
}