using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


public class RandomSys
{
    private static RandomSys instance = null;
    public static RandomSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new RandomSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        random = new Random((int)DateTime.Now.Ticks);
    }
    private Random random;

    public int GetRandomInt(int LowerBound, int UpperBound) //包含Lower，不包含UpperBound
    {
        random = new Random((int)DateTime.Now.Ticks);
        return random.Next(LowerBound, UpperBound);
    }
}
