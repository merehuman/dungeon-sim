using System;
using System.IO;
using DungeonSim.Game;

namespace DungeonSim
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Character Creation ---");

            // 1. Roll stats (3d6 x 3)
            int strength = RollStat();
            int dexterity = RollStat();
            int wisdom = RollStat();
            Console.WriteLine($"Strength: {strength}, Dexterity: {dexterity}, Wisdom: {wisdom}");

            // 2. Roll race
            int raceRoll = DiceRoller.RollDie(20);
            var raceRow = DiceRoller.LookupTable("game/csv/races.csv", raceRoll);
            Console.WriteLine($"Race Roll: {raceRoll} => {raceRow?["col1"]} ({raceRow?["col2"]})");

            // 3. Roll HP (stub)
            Console.WriteLine("HP: (to be determined by class/race)");

            // 4. Roll class
            int classRoll = DiceRoller.RollDie(4);
            var classRow = DiceRoller.LookupTable("game/csv/classes.csv", classRoll);
            Console.WriteLine($"Class Roll: {classRoll} => {classRow?["col1"]} ({classRow?["col2"]})");

            // 5. Roll personality trait
            int traitRoll = DiceRoller.RollDie(12);
            var traitRow = DiceRoller.LookupTable("game/csv/personality_traits.csv", traitRoll);
            Console.WriteLine($"Personality Trait Roll: {traitRoll} => {traitRow?["col1"]} ({traitRow?["col2"]})");

            // 6. Roll necessary attacks/spells (stub)
            Console.WriteLine("Attacks/Spells: (to be implemented)");

            // ... stub out remaining steps ...
        }

        static int RollStat()
        {
            int total = 0;
            for (int i = 0; i < 3; i++)
                total += DiceRoller.RollDie(6);
            return total;
        }
    }
} 