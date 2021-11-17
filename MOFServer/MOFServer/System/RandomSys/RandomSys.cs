using System;

public class RandomSys : Singleton<RandomSys>
{
    public void Init()
    {
        random = new Random(Guid.NewGuid().GetHashCode());
    }
    public Random random;

    public int GetRandomInt(int LowerBound, int UpperBound) //包含Lower，不包含UpperBound
    {
        random = new Random(Guid.NewGuid().GetHashCode());
        return random.Next(LowerBound, UpperBound);
    }

    public double NextDouble()
    {
        return random.NextDouble();
    }
}
