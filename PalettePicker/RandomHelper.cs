using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class RandomHelper
{
    private static Random rand = new Random();
    public static double nextDouble()
    {
        return rand.NextDouble();
    }
    public static int nextInt(int maxValue)
    {
        return rand.Next(maxValue);
    }
}

