using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace DungeonSim.Game
{
    public class DiceRoller
    {
        private static Random rng = new Random();

        // Rolls a die with the given number of sides
        public static int RollDie(int sides)
        {
            return rng.Next(1, sides + 1);
        }

        // Loads a CSV file and returns the row that matches the roll
        // Handles ranges like "1-3" in the first column
        public static Dictionary<string, string> LookupTable(string csvPath, int roll)
        {
            var lines = File.ReadAllLines(csvPath).Skip(1); // skip header
            foreach (var line in lines)
            {
                var columns = line.Split(',');
                var range = columns[0].Trim();
                if (IsRollInRange(range, roll))
                {
                    var result = new Dictionary<string, string>();
                    for (int i = 0; i < columns.Length; i++)
                    {
                        result[$"col{i}"] = columns[i].Trim();
                    }
                    return result;
                }
            }
            return null;
        }

        // Checks if a roll is within a range string like "1-3" or matches a single number
        private static bool IsRollInRange(string range, int roll)
        {
            if (range.Contains("-"))
            {
                var parts = range.Split('-');
                int start = int.Parse(parts[0]);
                int end = int.Parse(parts[1]);
                return roll >= start && roll <= end;
            }
            else
            {
                return int.TryParse(range, out int value) && value == roll;
            }
        }
    }
} 