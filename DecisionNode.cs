class DecisionNode<T>
{
    public T decision;
    public DecisionNode<T>? falseBranch;
    public DecisionNode<T>? trueBranch;
    public int featureIdx;
    public double question;

    public bool IsLeaf { get { return falseBranch == null && trueBranch == null; } }

    public DecisionNode(T decision, int featureIdx = -1, double question = -1, DecisionNode<T>? falseBranch = null, DecisionNode<T>? trueBranch = null)
    {
        this.decision = decision;
        this.featureIdx = featureIdx;
        this.question = question;

        this.falseBranch = falseBranch;
        this.trueBranch = trueBranch;
    }
}