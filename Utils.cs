static class Utils
{
    static public double Sigmoid(double x) => 1 / (1 + Math.Exp(-x));
    static public double Sigmoid_d(double x) => Sigmoid(x) * (1 - Sigmoid(x));
}