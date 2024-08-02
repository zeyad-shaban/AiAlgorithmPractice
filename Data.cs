using CsvHelper.Configuration.Attributes;

class Data
{
    [Name("Sex")]
    public string SexStr { get; set; }

    public static double minHeight;
    public static double maxHeight;

    public static double minWeight;
    public static double maxWeight;


    public double Height { get; set; }
    public double Weight { get; set; }

    public double HeightScaled { get => (Height - minHeight) / (maxHeight - minHeight); }
    public double WeightScaled { get => (Height - minHeight) / (maxHeight - minHeight); }


    public double Sex { get => SexStr.Equals("female", StringComparison.CurrentCultureIgnoreCase) ? 0 : 1; }

}