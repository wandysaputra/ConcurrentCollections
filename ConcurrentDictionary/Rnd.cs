﻿namespace ConcurrentDictionary;

public class Rnd
{
    private static Random _generator = new Random();
    public static int NextInt(int max) => _generator.Next(max);
    public static bool TrueWithProb(double probOfTrue)
        => _generator.NextDouble() < probOfTrue;
}