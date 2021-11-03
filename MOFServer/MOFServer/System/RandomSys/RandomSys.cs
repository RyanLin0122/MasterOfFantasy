using System;

public class RandomSys : Singleton<RandomSys>
{
    public void Init()
    {
        random = new Random((int)DateTime.Now.Ticks);
    }
    private Random random;

    public int GetRandomInt(int LowerBound, int UpperBound) //包含Lower，不包含UpperBound
    {
        return random.Next(LowerBound, UpperBound);
    }
}
