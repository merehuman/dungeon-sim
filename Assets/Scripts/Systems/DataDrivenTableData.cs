using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataDrivenTableData
{
    private static CSVDataLoader csvLoader;
    
    // Initialize the system with a CSV loader
    public static void Initialize(CSVDataLoader loader)
    {
        csvLoader = loader;
    }
    
    // Get race from roll
    public static CharacterRace GetRaceFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("races", roll);
        if (row != null && row.ContainsKey("Race"))
        {
            string raceName = row["Race"];
            if (Enum.TryParse(raceName, out CharacterRace race))
            {
                return race;
            }
        }
        return CharacterRace.Human; // Default fallback
    }
    
    // Get class from roll
    public static CharacterClass GetClassFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("classes", roll);
        if (row != null && row.ContainsKey("Class"))
        {
            string className = row["Class"];
            if (Enum.TryParse(className, out CharacterClass characterClass))
            {
                return characterClass;
            }
        }
        return CharacterClass.Fighter; // Default fallback
    }
    
    // Get personality trait from roll
    public static PersonalityTrait GetPersonalityFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("personality_traits", roll);
        if (row != null && row.ContainsKey("Personality Trait"))
        {
            string traitName = row["Personality Trait"];
            if (Enum.TryParse(traitName, out PersonalityTrait trait))
            {
                return trait;
            }
        }
        return PersonalityTrait.Friendly; // Default fallback
    }
    
    // Get weapon from roll
    public static Weapon GetWeaponFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("weapons", roll);
        if (row != null)
        {
            string weaponName = row.ContainsKey("Weapon") ? row["Weapon"] : "Unknown Weapon";
            string damageDie = row.ContainsKey("Damage Die") ? row["Damage Die"] : "1d4";
            
            List<string> traits = new List<string>();
            if (row.ContainsKey("Additional Traits"))
            {
                string traitsString = row["Additional Traits"];
                if (!string.IsNullOrEmpty(traitsString))
                {
                    traits = traitsString.Split(';').Select(t => t.Trim()).ToList();
                }
            }
            
            return new Weapon(weaponName, damageDie, traits);
        }
        return new Weapon("Dagger", "1d4", new List<string> { "1h" }); // Default fallback
    }
    
    // Get armor from roll
    public static Armor GetArmorFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("armor", roll);
        if (row != null)
        {
            string armorName = row.ContainsKey("Armor Type") ? row["Armor Type"] : "Unknown Armor";
            int armorValue = 1;
            int value = 15;
            
            if (row.ContainsKey("Armor Value") && int.TryParse(row["Armor Value"], out int parsedArmor))
            {
                armorValue = parsedArmor;
            }
            
            List<string> traits = new List<string>();
            if (row.ContainsKey("Additional Traits"))
            {
                string traitsString = row["Additional Traits"];
                if (!string.IsNullOrEmpty(traitsString))
                {
                    traits = traitsString.Split(';').Select(t => t.Trim()).ToList();
                }
            }
            
            return new Armor(armorName, armorValue, value, traits);
        }
        return new Armor("Leather Armor", 1, 15, new List<string>()); // Default fallback
    }
    
    // Get item from roll
    public static Item GetItemFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("items_tools", roll);
        if (row != null)
        {
            string itemName = row.ContainsKey("Tool") ? row["Tool"] : "Unknown Item";
            int value = 5;
            string description = row.ContainsKey("Uses") ? row["Uses"] : "Unknown use";
            
            if (row.ContainsKey("Value") && int.TryParse(row["Value"], out int parsedValue))
            {
                value = parsedValue;
            }
            
            List<string> uses = new List<string>();
            if (row.ContainsKey("Uses"))
            {
                string usesString = row["Uses"];
                if (!string.IsNullOrEmpty(usesString))
                {
                    uses = usesString.Split(';').Select(u => u.Trim()).ToList();
                }
            }
            
            return new Item(itemName, value, description, uses);
        }
        return new Item("Torch", 5, "Provides light", new List<string> { "Light source" }); // Default fallback
    }
    
    // Get clothing from roll
    public static Item GetClothingFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("clothing_accessory", roll);
        if (row != null)
        {
            string clothingName = row.ContainsKey("Clothing/Accessory") ? row["Clothing/Accessory"] : "Unknown Clothing";
            int value = 10;
            string description = row.ContainsKey("Uses") ? row["Uses"] : "Unknown use";
            
            if (row.ContainsKey("Value") && int.TryParse(row["Value"], out int parsedValue))
            {
                value = parsedValue;
            }
            
            List<string> uses = new List<string>();
            if (row.ContainsKey("Uses"))
            {
                string usesString = row["Uses"];
                if (!string.IsNullOrEmpty(usesString))
                {
                    uses = usesString.Split(';').Select(u => u.Trim()).ToList();
                }
            }
            
            return new Item(clothingName, value, description, uses);
        }
        return new Item("Shirt", 12, "Basic clothing", new List<string> { "Appearance" }); // Default fallback
    }
    
    // Get heal spell from roll
    public static HealSpell GetHealSpellFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("heal_spells", roll);
        if (row != null)
        {
            string spellName = row.ContainsKey("Spell Name") ? row["Spell Name"] : "Unknown Heal";
            string healValue = row.ContainsKey("Value") ? row["Value"] : "1d4";
            string target = row.ContainsKey("Target") ? row["Target"] : "Self";
            int castsPerDay = 1;
            string effects = row.ContainsKey("Other Effects") ? row["Other Effects"] : "";
            
            return new HealSpell(spellName, healValue, target, castsPerDay, effects);
        }
        return new HealSpell("Minor Heal", "1d4", "Self", 1, "None"); // Default fallback
    }
    
    // Get martial attack from roll
    public static MartialAttack GetMartialAttackFromRoll(int roll)
    {
        var row = csvLoader.GetRowByRoll("special_attack_generator", roll);
        if (row != null)
        {
            string attackName = row.ContainsKey("Type of Attack") ? row["Type of Attack"] : "Unknown Attack";
            string damageDie = row.ContainsKey("Damage") ? row["Damage"] : "1d4";
            string target = row.ContainsKey("Target") ? row["Target"] : "Body";
            string description = $"A {attackName.ToLower()} attack targeting the {target.ToLower()}";
            
            return new MartialAttack(attackName, damageDie, target, description);
        }
        return new MartialAttack("Sweep", "1d4", "Body", "A sweeping attack to the body"); // Default fallback
    }
    
    // Get name parts
    public static string[] GetNameFirstParts()
    {
        return csvLoader.GetColumn("name_generator", "First Part").ToArray();
    }
    
    public static string[] GetNameSecondParts()
    {
        return csvLoader.GetColumn("name_generator", "Second Part").ToArray();
    }
    
    public static string[] GetNameModifiers()
    {
        return csvLoader.GetColumn("name_generator", "Modifier").ToArray();
    }
    
    // Get spell elements
    public static string[] GetSpellElements()
    {
        return csvLoader.GetColumn("spell_elements", "Element").ToArray();
    }
    
    // Get spell forms
    public static string[] GetSpellForms()
    {
        return csvLoader.GetColumn("spell_forms", "Form").ToArray();
    }
    
    // Get spell effects
    public static string[] GetSpellEffects()
    {
        return csvLoader.GetColumn("spell_effects", "Effect").ToArray();
    }
    
    // Get spell formula
    public static string GetSpellFormula(int roll)
    {
        var row = csvLoader.GetRowByRoll("spell_formula", roll);
        if (row != null && row.ContainsKey("Formula"))
        {
            return row["Formula"];
        }
        return "[Element] [Form]"; // Default fallback
    }
    
    // Get race effects
    public static string GetRaceEffects(CharacterRace race)
    {
        var rows = csvLoader.GetAllRows("races");
        foreach (var row in rows)
        {
            if (row.ContainsKey("Race") && row["Race"] == race.ToString())
            {
                return row.ContainsKey("Effects") ? row["Effects"] : "";
            }
        }
        return "";
    }
    
    // Get race hit dice
    public static string GetRaceHitDice(CharacterRace race)
    {
        var rows = csvLoader.GetAllRows("races");
        foreach (var row in rows)
        {
            if (row.ContainsKey("Race") && row["Race"] == race.ToString())
            {
                return row.ContainsKey("Hit Dice") ? row["Hit Dice"] : "1d6";
            }
        }
        return "1d6"; // Default fallback
    }
    
    // Get class effects
    public static string GetClassEffects(CharacterClass characterClass)
    {
        var rows = csvLoader.GetAllRows("classes");
        foreach (var row in rows)
        {
            if (row.ContainsKey("Class") && row["Class"] == characterClass.ToString())
            {
                return row.ContainsKey("Effects") ? row["Effects"] : "";
            }
        }
        return "";
    }
    
    // Get personality trait effects
    public static string GetPersonalityEffects(PersonalityTrait trait)
    {
        var rows = csvLoader.GetAllRows("personality_traits");
        foreach (var row in rows)
        {
            if (row.ContainsKey("Personality Trait") && row["Personality Trait"] == trait.ToString())
            {
                return row.ContainsKey("Effect") ? row["Effect"] : "";
            }
        }
        return "";
    }
    
    // Check if CSV loader is available
    public static bool IsInitialized()
    {
        return csvLoader != null;
    }
} 