using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;

class DataPoint
{
    public int Id { get; set; }
    public float Carat { get; set; }
    public string Cut { get; set; }
    public char Color { get; set; }
    public string Clarity { get; set; }
    public float Depth { get; set; }
    public float Table { get; set; }
    public int Price { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public float[] Features
    {
        get
        {
            return [this.Carat, this.Price, this.Depth, this.Table, this.X, this.Y, this.Z]; // will test this later cause i heard that KNN is dimensional cursed
            // return [this.Carat, this.Price];
        }
    }
    public string Label { get { return this.Cut; } }
}

class Program
{
    static DataPoint[] FindKNN(DataPoint[] dataPoints, DataPoint targetPoint, int k)
    {
        PriorityQueue<DataPoint, double> neighbours = new();

        foreach (var dataPoint in dataPoints)
        {
            neighbours.Enqueue(dataPoint, MeasureDistance(targetPoint, dataPoint));
        }

        DataPoint[] knn = new DataPoint[k];
        for (int i = 0; i < knn.Length; ++i) knn[i] = neighbours.Dequeue();
        return knn;
    }

    static double MeasureDistance(DataPoint A, DataPoint B)
    {
        if (A.Features.Length != B.Features.Length) throw new Exception("Dimensions Mismatch");
        double distance = 0;

        for (int i = 0; i < A.Features.Length; ++i)
        {
            distance += Math.Pow(A.Features[i] - B.Features[i], 2);
        }

        return Math.Sqrt(distance);
    }

    static void Main()
    {
        const int K = 5;

        var csv = new CsvReader(new StreamReader("C:/Users/zeyad/Desktop/Workspace/Sandbox/AiAlgorithmPractice/diamonds.csv"), CultureInfo.InvariantCulture);
        DataPoint[] records = csv.GetRecords<DataPoint>().ToArray();

        (DataPoint[] train_records, DataPoint[] test_records) = SplitTrainTest(records);

        int correct = 0;
        int total = 0;
        for (int i = 0; i < test_records.Length; ++i)
        {
            DataPoint[] knn = FindKNN(train_records, test_records[i], K);
            string classification = GetMostFrequestNeighbour(knn);
            if (classification == test_records[i].Label) ++correct;
            ++total;
        }

        Console.WriteLine($"Accuracy: {correct * 100.0 / total}%");
    }

    private static string GetMostFrequestNeighbour(DataPoint[] knn)
    {
        Dictionary<string, int> labelsDict = new();

        foreach (DataPoint neighbour in knn)
        {
            if (!labelsDict.ContainsKey(neighbour.Label)) labelsDict[neighbour.Label] = 0;
            ++labelsDict[neighbour.Label];
        }

        int mostFreq = -1;
        string mostFreqLabel = "";

        foreach (var pair in labelsDict)
        {
            if (labelsDict[pair.Key] > mostFreq)
            {
                mostFreq = labelsDict[pair.Key];
                mostFreqLabel = pair.Key;
            }
        }

        return mostFreqLabel;
    }

    private static (DataPoint[] train_records, DataPoint[] test_records) SplitTrainTest(DataPoint[] records)
    {
        int splitPnt = (int)(records.Length * 80.0 / 100);

        return (records[0..splitPnt], records[splitPnt..]);
    }
}