using DI.Services.Scheme.Abstraction;
using DI.Services.Scheme.Attributes;
using EH.StringDistanceProviding.Abstraction;
using System;
namespace EH.StringDistanceProviding;
[DiDescript(Order = 0, Lifetime = EDiServiceLifetime.Singleton)]
public class EhLevenshteinStringDistanceProvider : IEhStringDistanceProvider
{
    public int GetDistance(string from, string to)
    {
        if(string.IsNullOrEmpty(from) && string.IsNullOrEmpty(to)) return 0;
        if(string.IsNullOrEmpty(from)) return to.Length;
        if(string.IsNullOrEmpty(to)) return from.Length;

        int    lengthA   = from.Length;
        int    lengthB   = to.Length;
        int[,] distances = new int[lengthA + 1, lengthB + 1];
        for(int i = 0; i <= lengthA; distances[i, 0] = i++) ;
        for(int j = 0; j <= lengthB; distances[0, j] = j++) ;
        for(int i = 1; i <= lengthA; i++)
        for(int j = 1; j <= lengthB; j++)
            distances[i, j] = Math.Min(Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
            distances[i - 1, j - 1] + (to[j - 1] == from[i - 1] ? 0 : 1));
        return distances[lengthA, lengthB];
    }
}