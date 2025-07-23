using System;
using System.Collections.Generic;
using UnityEngine;

public static class DiceSystem
{
    private static System.Random random = new System.Random();
    
    // Roll a single die
    public static int RollDie(int sides)
    {
        return random.Next(1, sides + 1);
    }
    
    // Roll multiple dice (e.g., 3d6)
    public static int RollDice(int numberOfDice, int sides)
    {
        int total = 0;
        for (int i = 0; i < numberOfDice; i++)
        {
            total += RollDie(sides);
        }
        return total;
    }
    
    // Roll 3d6 (standard stat rolling)
    public static int Roll3d6()
    {
        return RollDice(3, 6);
    }
    
    // Roll 1d20 (common for tables)
    public static int Roll1d20()
    {
        return RollDie(20);
    }
    
    // Roll 1d12 (for some tables)
    public static int Roll1d12()
    {
        return RollDie(12);
    }
    
    // Roll 1d10 (for gold and some other things)
    public static int Roll1d10()
    {
        return RollDie(10);
    }
    
    // Roll 1d8 (for some weapon restrictions)
    public static int Roll1d8()
    {
        return RollDie(8);
    }
    
    // Roll 1d6 (for many things)
    public static int Roll1d6()
    {
        return RollDie(6);
    }
    
    // Roll 1d4 (for some damage and other things)
    public static int Roll1d4()
    {
        return RollDie(4);
    }
    
    // Get a random element from a list
    public static T GetRandomElement<T>(List<T> list)
    {
        if (list == null || list.Count == 0)
            return default(T);
        
        int index = random.Next(0, list.Count);
        return list[index];
    }
    
    // Get a random element from an array
    public static T GetRandomElement<T>(T[] array)
    {
        if (array == null || array.Length == 0)
            return default(T);
        
        int index = random.Next(0, array.Length);
        return array[index];
    }
    
    // Roll on a table with weighted results
    public static int RollOnTable(Dictionary<int, int> table)
    {
        int roll = Roll1d20();
        
        foreach (var entry in table)
        {
            if (roll <= entry.Key)
                return entry.Value;
        }
        
        return table[20]; // Default to last entry
    }
} 