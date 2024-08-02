using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

class Program
{
    static void Main()
    {
        using var reader = new StreamReader(@"C:\Users\zeyad\Desktop\Workspace\Sandbox\AiAlgorithmPractice\\malefemale.csv");
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        var records = csv.GetRecords<Data>().ToArray();

        Data.minHeight = records.Min(m => m.Height);
        Data.maxHeight = records.Max(m => m.Height);

        Data.minWeight = records.Min(m => m.Weight);
        Data.maxWeight = records.Max(m => m.Weight);

        NeuralNetwork nn = new(2);
        for (int i = 0; i < (int)(records.Length * 0.8); ++i)
        {
            var record = records[i];
            nn.UpdateInputNeurons([record.HeightScaled, record.WeightScaled], record.Sex);

            nn.ForwardPropagation();
            nn.BackPropagation();
        }

        int correct = 0;
        int total = 0;
        for (int i = (int)(records.Length * 0.8); i < records.Length; ++i)
        {
            var record = records[i];
            nn.UpdateInputNeurons([record.HeightScaled, record.WeightScaled], -1);
            nn.ForwardPropagation();

            if (Math.Round(nn.predictedOutput) == record.Sex) ++correct;
            ++total;

            Console.WriteLine($"{correct}/{total}");
        }

    }
}
