using System;
using System.IO;
using DungeonSim.Game;
using System.Collections.Generic;

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
            Console.WriteLine($"Base Stats: Strength: {strength}, Dexterity: {dexterity}, Wisdom: {wisdom}");

            // 2. Roll race
            int raceRoll = DiceRoller.RollDie(20);
            var raceRow = DiceRoller.LookupTable("game/csv/races.csv", raceRoll);
            string raceName = raceRow?["col1"] ?? "Unknown";
            string raceEffects = raceRow?["col2"] ?? "";
            string raceHitDice = raceRow?["col3"] ?? "";
            Console.WriteLine($"Race Roll: {raceRoll} => {raceName} ({raceEffects})");

            // 3. Roll class
            int classRoll = DiceRoller.RollDie(4);
            var classRow = DiceRoller.LookupTable("game/csv/classes.csv", classRoll);
            string className = classRow?["col1"] ?? "Unknown";
            string classEffects = classRow?["col2"] ?? "";
            Console.WriteLine($"Class Roll: {classRoll} => {className} ({classEffects})");

            // 4. Roll HP
            int hp = RollHP(raceHitDice, classEffects, raceEffects);
            Console.WriteLine($"HP: {hp}");

            // 5. Roll personality trait
            int traitRoll = DiceRoller.RollDie(12);
            var traitRow = DiceRoller.LookupTable("game/csv/personality_traits.csv", traitRoll);
            string traitName = traitRow?["col1"] ?? "";
            string traitEffect = traitRow?["col2"] ?? "";
            Console.WriteLine($"Personality Trait Roll: {traitRoll} => {traitName} ({traitEffect})");

            // 6. Attacks/Spells based on class
            string attackSpell = GetAttackSpell(className, classEffects);
            Console.WriteLine($"Attacks/Spells: {attackSpell}");

            // 7. Randomly choose +2 torches or +2 rations
            bool extraTorches = DiceRoller.RollDie(2) == 1;
            Console.WriteLine($"Bonus: +2 {(extraTorches ? "Torches" : "Rations")}");
            Console.WriteLine("All characters also start with 1 torch and 1 ration.");

            // 8. Roll for item/tool, clothing, weapon, armor
            int itemRoll = DiceRoller.RollDie(20);
            var itemRow = DiceRoller.LookupTable("game/csv/items_tools.csv", itemRoll);
            int clothingRoll = DiceRoller.RollDie(12);
            var clothingRow = DiceRoller.LookupTable("game/csv/clothing_accessory.csv", clothingRoll);
            int weaponRoll = (className == "Cleric" || className == "Wizard") ? DiceRoller.RollDie(8) : DiceRoller.RollDie(12);
            var weaponRow = DiceRoller.LookupTable("game/csv/weapons.csv", weaponRoll);
            int armorRoll = DiceRoller.RollDie(12);
            var armorRow = DiceRoller.LookupTable("game/csv/armor.csv", armorRoll);
            Console.WriteLine("Items/Equipment:");
            Console.WriteLine($"- Item/Tool: {itemRow?["col1"]} ({itemRow?["col2"]})");
            Console.WriteLine($"- Clothing: {clothingRow?["col1"]} ({clothingRow?["col2"]})");
            Console.WriteLine($"- Weapon: {weaponRow?["col1"]} ({weaponRow?["col2"]})");
            Console.WriteLine($"- Armor: {armorRow?["col1"]} ({armorRow?["col2"]})");

            // 9. Calculate total armor
            int armorValue = 0;
            int.TryParse(armorRow?["col2"], out armorValue);
            Console.WriteLine($"Total Armor: {armorValue}");

            // 10. Roll 3d6 for starting gold
            int gold = DiceRoller.RollDie(6) + DiceRoller.RollDie(6) + DiceRoller.RollDie(6);
            Console.WriteLine($"Starting Gold: {gold}");

            // 11. Update stats based on outcomes above
            (int finalStr, int finalDex, int finalWis) = ApplyStatModifiers(strength, dexterity, wisdom, raceEffects, classEffects, traitEffect);
            Console.WriteLine($"Final Stats: Strength: {finalStr}, Dexterity: {finalDex}, Wisdom: {finalWis}");

            // 12. Roll to name the character (3d20 for first part, second part, modifier). Apply the modifier to the name.
            int nameFirst = DiceRoller.RollDie(20);
            int nameSecond = DiceRoller.RollDie(20);
            int nameMod = DiceRoller.RollDie(20);
            var nameRow = DiceRoller.LookupTable("game/csv/name_generator.csv", nameFirst);
            var nameRow2 = DiceRoller.LookupTable("game/csv/name_generator.csv", nameSecond);
            var nameRow3 = DiceRoller.LookupTable("game/csv/name_generator.csv", nameMod);
            string name = ApplyNameModifier(nameRow, nameRow2, nameRow3);
            Console.WriteLine($"Name: {name}");
        }

        static int RollStat()
        {
            int total = 0;
            for (int i = 0; i < 3; i++)
                total += DiceRoller.RollDie(6);
            return total;
        }

        static int RollHP(string hitDice, string classEffects, string raceEffects)
        {
            // Default to 1d6 if not found
            int hp = 0;
            if (hitDice != null && hitDice.StartsWith("1d"))
            {
                if (int.TryParse(hitDice.Substring(2), out int die))
                    hp = DiceRoller.RollDie(die);
            }
            // Class bonus HP
            if (classEffects != null && classEffects.Contains("+1hp"))
                hp += 1;
            if (raceEffects != null && raceEffects.Contains("+1hp"))
                hp += 1;
            return hp > 0 ? hp : 1;
        }

        static (int, int, int) ApplyStatModifiers(int str, int dex, int wis, string raceEffects, string classEffects, string traitEffect)
        {
            // Race
            if (raceEffects != null)
            {
                if (raceEffects.Contains("+1 wis")) wis += 1;
                if (raceEffects.Contains("+1 dex")) dex += 1;
                if (raceEffects.Contains("+1 str")) str += 1;
                if (raceEffects.Contains("+1 stat of choice")) str += 1; // Default to STR for now
            }
            // Class
            if (classEffects != null)
            {
                if (classEffects.Contains("+1 Wis")) wis += 1;
                if (classEffects.Contains("+1 Dex")) dex += 1;
                if (classEffects.Contains("+1 Str")) str += 1;
            }
            // Personality Trait
            if (traitEffect != null)
            {
                if (traitEffect.Contains("WIS")) wis += 1;
                if (traitEffect.Contains("DEX")) dex += 1;
                if (traitEffect.Contains("STR")) str += 1;
            }
            return (str, dex, wis);
        }

        static string GetAttackSpell(string className, string classEffects)
        {
            if (className == "Wizard")
                return "2 random spells, 2 spell cast at level 1";
            if (className == "Cleric")
                return "1 random spell, 1 random heal, 1 spell cast at level 1";
            if (className == "Fighter")
                return "Martial Attack";
            if (className == "Rogue")
                return "Advantage Trap Disarm";
            return classEffects;
        }

        static string ApplyNameModifier(Dictionary<string, string> first, Dictionary<string, string> second, Dictionary<string, string> mod)
        {
            // For now, just concatenate and show the modifier
            return $"{first?["col1"]} {second?["col2"]} ({mod?["col3"]})";
        }
    }
} 