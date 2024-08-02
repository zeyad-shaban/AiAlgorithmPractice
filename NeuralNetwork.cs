using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Double;

class NeuralNetwork
{
    public static double learningRate = 1;

    public double[] features;
    public double[] hiddenLayer1 = new double[3];
    public Matrix[] weights;

    public double predictedOutput;
    public double actualOutput;

    public NeuralNetwork(int featuresLength)
    {
        int hiddenLayerCount = 1;

        weights = new Matrix[hiddenLayerCount + 1];

        weights[0] = DenseMatrix.CreateRandom(featuresLength, hiddenLayer1.Length, new ContinuousUniform(0, 1));
        weights[1] = DenseMatrix.CreateRandom(hiddenLayer1.Length, 1, new ContinuousUniform(0, 1));
    }

    public void UpdateInputNeurons(double[] features, double actualOutput)
    {
        this.features = features;
        this.actualOutput = actualOutput;
    }

    public void ForwardPropagation()
    {
        // Input to hidden
        Matrix weight = weights[0];
        double sum;

        for (int hiddenIdx = 0; hiddenIdx < hiddenLayer1.Length; ++hiddenIdx)
        {
            sum = 0;
            for (int featureIdx = 0; featureIdx < features.Length; ++featureIdx)
            {
                sum += weight[featureIdx, hiddenIdx] * features[featureIdx];
            }
            hiddenLayer1[hiddenIdx] = Utils.Sigmoid(sum);
        }

        // Hidden to output
        weight = weights[1];

        sum = 0;
        for (int i = 0; i < hiddenLayer1.Length; ++i)
        {
            sum += hiddenLayer1[i] * weight[i, 0];
        }
        predictedOutput = Utils.Sigmoid(sum);
    }

    public void BackPropagation()
    {
        // output to hidden layer 1
        double cost = Math.Pow(predictedOutput - actualOutput, 2);

        for (int i = 0; i < hiddenLayer1.Length; ++i)
        {
            weights[1][i, 0] += hiddenLayer1[i] * 2 * cost * Utils.Sigmoid_d(predictedOutput);
        }

        // input to hidden layer 1
        for (int featureIdx = 0; featureIdx < features.Length; ++featureIdx)
        {
            for (int hiddenIdx = 0; hiddenIdx < hiddenLayer1.Length; ++hiddenIdx)
            {
                weights[0][featureIdx, hiddenIdx] += features[featureIdx] * weights[0][featureIdx, hiddenIdx] * 2 * cost * Utils.Sigmoid_d(predictedOutput) * Utils.Sigmoid_d(hiddenLayer1[hiddenIdx]);
            }
        }
    }
}