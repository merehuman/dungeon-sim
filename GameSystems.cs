using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonSim
{
    // Standalone dice system
    public static class DiceSystem
    {
        private static Random random = new Random();
        
        public static int RollDie(int sides)
        {
            return random.Next(1, sides + 1);
        }
        
        public static int RollDice(int numberOfDice, int sides)
        {
            int total = 0;
            for (int i = 0; i < numberOfDice; i++)
            {
                total += RollDie(sides);
            }
            return total;
        }
        
        public static int Roll3d6()
        {
            return RollDice(3, 6);
        }
        
        public static int Roll1d20()
        {
            return RollDie(20);
        }
        
        public static int Roll1d12()
        {
            return RollDie(12);
        }
        
        public static int Roll1d10()
        {
            return RollDie(10);
        }
        
        public static int Roll1d8()
        {
            return RollDie(8);
        }
        
        public static int Roll1d6()
        {
            return RollDie(6);
        }
        
        public static int Roll1d4()
        {
            return RollDie(4);
        }
        
        public static int Roll1d2()
        {
            return RollDie(2);
        }
    }

    // Standalone data-driven table data
    public static class DataDrivenTableData
    {
        private static CSVDataLoader? csvLoader;
        
        public static void Initialize(CSVDataLoader loader)
        {
            csvLoader = loader;
        }
        
        public static CharacterRace GetRaceFromRoll(int roll)
        {
            if (csvLoader == null) return CharacterRace.Human;
            
            var row = csvLoader.GetRowByRoll("races", roll);
            if (row != null && row.ContainsKey("Race"))
            {
                string raceName = row["Race"];
                if (Enum.TryParse(raceName, out CharacterRace race))
                {
                    return race;
                }
            }
            return CharacterRace.Human;
        }
        
        public static CharacterClass GetClassFromRoll(int roll)
        {
            if (csvLoader == null) return CharacterClass.Fighter;
            
            var row = csvLoader.GetRowByRoll("classes", roll);
            if (row != null && row.ContainsKey("Class"))
            {
                string className = row["Class"];
                if (Enum.TryParse(className, out CharacterClass characterClass))
                {
                    return characterClass;
                }
            }
            return CharacterClass.Fighter;
        }
        
        public static PersonalityTrait GetPersonalityFromRoll(int roll)
        {
            if (csvLoader == null) return PersonalityTrait.Friendly;
            
            var row = csvLoader.GetRowByRoll("personality_traits", roll);
            if (row != null && row.ContainsKey("Personality Trait"))
            {
                string traitName = row["Personality Trait"];
                if (Enum.TryParse(traitName, out PersonalityTrait trait))
                {
                    return trait;
                }
            }
            return PersonalityTrait.Friendly;
        }
        
        public static Weapon GetWeaponFromRoll(int roll)
        {
            if (csvLoader == null) return new Weapon("Dagger", "1d4", new List<string> { "1h" });
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
            return new Weapon("Dagger", "1d4", new List<string> { "1h" });
        }
        
        public static Armor GetArmorFromRoll(int roll)
        {
            if (csvLoader == null) return new Armor("Leather Armor", 1, 15, new List<string>());
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
            return new Armor("Leather Armor", 1, 15, new List<string>());
        }
        
        public static Item GetItemFromRoll(int roll)
        {
            if (csvLoader == null) return new Item("Torch", 5, "Provides light", new List<string> { "Light source" });
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
            return new Item("Torch", 5, "Provides light", new List<string> { "Light source" });
        }
        
        public static Item GetClothingFromRoll(int roll)
        {
            if (csvLoader == null) return new Item("Shirt", 12, "Basic clothing", new List<string> { "Appearance" });
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
            return new Item("Shirt", 12, "Basic clothing", new List<string> { "Appearance" });
        }
        
        public static HealSpell GetHealSpellFromRoll(int roll)
        {
            if (csvLoader == null) return new HealSpell("Minor Heal", "1d4", "Self", 1, "None");
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
            return new HealSpell("Minor Heal", "1d4", "Self", 1, "None");
        }
        
        public static MartialAttack GetMartialAttackFromRoll(int roll)
        {
            if (csvLoader == null) return new MartialAttack("Sweep", "1d4", "Body", "A sweeping attack to the body");
            var row = csvLoader.GetRowByRoll("special_attack_generator", roll);
            if (row != null)
            {
                string attackName = row.ContainsKey("Type of Attack") ? row["Type of Attack"] : "Unknown Attack";
                string damageDie = row.ContainsKey("Damage") ? row["Damage"] : "1d4";
                string target = row.ContainsKey("Target") ? row["Target"] : "Body";
                string description = $"A {attackName.ToLower()} attack targeting the {target.ToLower()}";
                
                return new MartialAttack(attackName, damageDie, target, description);
            }
            return new MartialAttack("Sweep", "1d4", "Body", "A sweeping attack to the body");
        }
        
        public static string[] GetNameFirstParts()
        {
            if (csvLoader == null) return new string[] { "Gar", "Thorne", "Wirl" };
            return csvLoader.GetColumn("name_generator", "First Part").ToArray();
        }
        
        public static string[] GetNameSecondParts()
        {
            if (csvLoader == null) return new string[] { "Binzo", "Shadowstep", "Flit" };
            return csvLoader.GetColumn("name_generator", "Second Part").ToArray();
        }
        
        public static string[] GetNameModifiers()
        {
            if (csvLoader == null) return new string[] { "", "the Brave", "the Wise" };
            return csvLoader.GetColumn("name_generator", "Modifier").ToArray();
        }
        
        public static string[] GetSpellElements()
        {
            if (csvLoader == null) return new string[] { "Fire", "Ice", "Lightning" };
            return csvLoader.GetColumn("spell_elements", "Element").ToArray();
        }
        
        public static string[] GetSpellForms()
        {
            if (csvLoader == null) return new string[] { "Bolt", "Wall", "Burst" };
            return csvLoader.GetColumn("spell_forms", "Form").ToArray();
        }
        
        public static string[] GetSpellEffects()
        {
            if (csvLoader == null) return new string[] { "Damage", "Drain", "Stun" };
            return csvLoader.GetColumn("spell_effects", "Effect").ToArray();
        }
        
        public static string GetSpellFormula(int roll)
        {
            if (csvLoader == null) return "[Element] [Form]";
            var row = csvLoader.GetRowByRoll("spell_formula", roll);
            if (row != null && row.ContainsKey("Formula"))
            {
                return row["Formula"];
            }
            return "[Element] [Form]";
        }
        
        public static string GetRaceHitDice(CharacterRace race)
        {
            if (csvLoader == null) return "1d6";
            var rows = csvLoader.GetAllRows("races");
            foreach (var row in rows)
            {
                if (row.ContainsKey("Race") && row["Race"] == race.ToString())
                {
                    return row.ContainsKey("Hit Dice") ? row["Hit Dice"] : "1d6";
                }
            }
            return "1d6";
        }
        
        public static bool IsInitialized()
        {
            return csvLoader != null;
        }
    }

    // Standalone character creation system
    public class CharacterCreationSystem
    {
        public bool printRolls = true;
        public bool printDescriptions = true;
        public CSVDataLoader? csvDataLoader;
        
        private List<string> creationLog = new List<string>();
        
        public Character CreateCharacter()
        {
            if (!DataDrivenTableData.IsInitialized() && csvDataLoader != null)
            {
                DataDrivenTableData.Initialize(csvDataLoader);
            }
            
            creationLog.Clear();
            LogMessage("=== STARTING CHARACTER CREATION ===");
            
            Character character = new Character();
            
            RollStats(character);
            RollRace(character);
            RollClass(character);
            RollHitPoints(character);
            RollPersonalityTrait(character);
            RollClassAbilities(character);
            RollStartingResources(character);
            RollEquipment(character);
            character.CalculateArmorValue();
            RollStartingGold(character);
            character.ApplyStatBonuses();
            GenerateCharacterName(character);
            
            LogMessage("=== CHARACTER CREATION COMPLETE ===");
            
            return character;
        }
        
        private void RollStats(Character character)
        {
            LogMessage("\n--- ROLLING STATS ---");
            
            character.strength = DiceSystem.Roll3d6();
            LogRoll("Strength", "3d6", character.strength);
            
            character.dexterity = DiceSystem.Roll3d6();
            LogRoll("Dexterity", "3d6", character.dexterity);
            
            character.wisdom = DiceSystem.Roll3d6();
            LogRoll("Wisdom", "3d6", character.wisdom);
        }
        
        private void RollRace(Character character)
        {
            LogMessage("\n--- ROLLING RACE ---");
            
            int roll = DiceSystem.Roll1d20();
            LogRoll("Race", "1d20", roll);
            
            if (roll == 19)
            {
                LogMessage("Rolled Elemental - rerolling with elemental modifications");
                roll = DiceSystem.Roll1d20();
                LogRoll("Elemental Race Reroll", "1d20", roll);
                character.race = DataDrivenTableData.GetRaceFromRoll(roll);
                character.racialAbilities.Add("Elemental nature - special modifications apply");
            }
            else if (roll == 20)
            {
                LogMessage("Rolled Undead - rerolling with undead modifications");
                roll = DiceSystem.Roll1d20();
                LogRoll("Undead Race Reroll", "1d20", roll);
                character.race = DataDrivenTableData.GetRaceFromRoll(roll);
                character.racialAbilities.Add("Undead nature - special modifications apply");
            }
            else
            {
                character.race = DataDrivenTableData.GetRaceFromRoll(roll);
            }
            
            LogMessage($"Race determined: {character.race}");
        }
        
        private void RollClass(Character character)
        {
            LogMessage("\n--- ROLLING CLASS ---");
            
            int roll = DiceSystem.Roll1d4();
            LogRoll("Class", "1d4", roll);
            
            character.characterClass = DataDrivenTableData.GetClassFromRoll(roll);
            LogMessage($"Class determined: {character.characterClass}");
        }
        
        private void RollHitPoints(Character character)
        {
            LogMessage("\n--- ROLLING HIT POINTS ---");
            
            int hitDice = GetHitDiceForRace(character.race);
            int roll = DiceSystem.RollDie(hitDice);
            LogRoll("Hit Points", $"1d{hitDice}", roll);
            
            character.maxHealth = roll;
            character.currentHealth = roll;
            LogMessage($"Hit Points: {character.maxHealth}");
        }
        
        private int GetHitDiceForRace(CharacterRace race)
        {
            if (DataDrivenTableData.IsInitialized())
            {
                string hitDiceString = DataDrivenTableData.GetRaceHitDice(race);
                if (hitDiceString.StartsWith("1d"))
                {
                    if (int.TryParse(hitDiceString.Substring(2), out int dice))
                    {
                        return dice;
                    }
                }
            }
            
            switch (race)
            {
                case CharacterRace.Halfling:
                case CharacterRace.Fey:
                    return 4;
                case CharacterRace.Human:
                case CharacterRace.Elf:
                    return 6;
                case CharacterRace.Dwarf:
                    return 8;
                case CharacterRace.Giant:
                    return 10;
                default:
                    return 6;
            }
        }
        
        private void RollPersonalityTrait(Character character)
        {
            LogMessage("\n--- ROLLING PERSONALITY TRAIT ---");
            
            int roll = DiceSystem.Roll1d12();
            LogRoll("Personality Trait", "1d12", roll);
            
            character.personalityTrait = DataDrivenTableData.GetPersonalityFromRoll(roll);
            LogMessage($"Personality Trait: {character.personalityTrait}");
        }
        
        private void RollClassAbilities(Character character)
        {
            LogMessage("\n--- ROLLING CLASS ABILITIES ---");
            
            switch (character.characterClass)
            {
                case CharacterClass.Fighter:
                    RollMartialAttack(character);
                    character.classAbilities.Add("Martial Attack");
                    character.classAbilities.Add("+1 armor at start of game");
                    break;
                    
                case CharacterClass.Rogue:
                    character.classAbilities.Add("Advantage Trap Disarm");
                    break;
                    
                case CharacterClass.Wizard:
                    RollSpell(character);
                    RollSpell(character);
                    character.classAbilities.Add("2 spell casts at level 1");
                    break;
                    
                case CharacterClass.Cleric:
                    RollSpell(character);
                    RollHealSpell(character);
                    character.classAbilities.Add("1 spell cast at level 1");
                    break;
            }
        }
        
        private void RollMartialAttack(Character character)
        {
            int roll = DiceSystem.RollDice(3, 12);
            LogRoll("Martial Attack", "3d12", roll);
            
            if (roll == 12)
            {
                LogMessage("Rolled 'Roll Twice' - generating two martial attacks");
                RollMartialAttack(character);
                RollMartialAttack(character);
                return;
            }
            
            var attack = DataDrivenTableData.GetMartialAttackFromRoll(roll);
            if (attack != null)
            {
                character.martialAttacks.Add(attack);
                LogMessage($"Martial Attack: {attack.attackName} - {attack.damageDie} damage to {attack.target}");
            }
        }
        
        private void RollSpell(Character character)
        {
            LogMessage("--- Rolling Spell ---");
            
            int formulaRoll = DiceSystem.Roll1d6();
            string formula = DataDrivenTableData.GetSpellFormula(formulaRoll);
            LogRoll("Spell Formula", "1d6", formulaRoll);
            
            int elementRoll = DiceSystem.Roll1d12();
            string[] elements = DataDrivenTableData.GetSpellElements();
            string element = elements.Length > 0 ? elements[elementRoll - 1] : "Elemental";
            LogRoll("Spell Element", "1d12", elementRoll);
            
            int formRoll = DiceSystem.Roll1d12();
            string[] forms = DataDrivenTableData.GetSpellForms();
            string form = forms.Length > 0 ? forms[formRoll - 1] : "Bolt";
            LogRoll("Spell Form", "1d12", formRoll);
            
            int effectRoll = DiceSystem.Roll1d12();
            string[] effects = DataDrivenTableData.GetSpellEffects();
            string effect = effects.Length > 0 ? effects[effectRoll - 1] : "Charming";
            LogRoll("Spell Effect", "1d12", effectRoll);
            
            string spellName = GenerateSpellName(formula, element, form, effect);
            int castsPerDay = (character.characterClass == CharacterClass.Wizard) ? 2 : 1;
            
            var spell = new Spell(spellName, element, form, effect, castsPerDay);
            character.spells.Add(spell);
            
            LogMessage($"Spell Created: {spellName} ({castsPerDay} casts/day)");
        }
        
        private string GenerateSpellName(string formula, string element, string form, string effect)
        {
            switch (formula)
            {
                case "[Element] [Form]":
                    return $"{element} {form}";
                case "[Effect] [Form]":
                    return $"{effect} {form}";
                case "[Effect] [Element]":
                    return $"{effect} {element}";
                default:
                    return $"{element} {form}";
            }
        }
        
        private void RollHealSpell(Character character)
        {
            int roll = DiceSystem.Roll1d12();
            LogRoll("Heal Spell", "1d12", roll);
            
            var healSpell = DataDrivenTableData.GetHealSpellFromRoll(roll);
            if (healSpell != null)
            {
                character.healSpells.Add(healSpell);
                LogMessage($"Heal Spell: {healSpell.spellName} - {healSpell.healValue} to {healSpell.target}");
            }
        }
        
        private void RollStartingResources(Character character)
        {
            LogMessage("\n--- ROLLING STARTING RESOURCES ---");
            
            int roll = DiceSystem.Roll1d2();
            LogRoll("Extra Resources", "1d2", roll);
            
            if (roll == 1)
            {
                character.torches += 2;
                LogMessage("Chose +2 extra torches");
            }
            else
            {
                character.rations += 2;
                LogMessage("Chose +2 extra rations");
            }
        }
        
        private void RollEquipment(Character character)
        {
            LogMessage("\n--- ROLLING EQUIPMENT ---");
            
            RollWeapon(character);
            RollItem(character);
            RollClothing(character);
            RollArmor(character);
        }
        
        private void RollWeapon(Character character)
        {
            int maxRoll = 12;
            
            if (character.characterClass == CharacterClass.Cleric || 
                character.characterClass == CharacterClass.Wizard || 
                character.characterClass == CharacterClass.Rogue)
            {
                maxRoll = 8;
            }
            
            int roll = DiceSystem.RollDie(maxRoll);
            LogRoll("Weapon", $"1d{maxRoll}", roll);
            
            var weapon = DataDrivenTableData.GetWeaponFromRoll(roll);
            if (weapon != null)
            {
                if (character.characterClass == CharacterClass.Rogue)
                {
                    if (!weapon.traits.Contains("1h") || weapon.traits.Contains("2h"))
                    {
                        LogMessage("Rogue restriction: Rerolling for one-handed weapon");
                        RollWeapon(character);
                        return;
                    }
                }
                
                character.equippedWeapon = weapon;
                LogMessage($"Weapon: {weapon.weaponName} ({weapon.damageDie})");
            }
        }
        
        private void RollItem(Character character)
        {
            int roll = DiceSystem.Roll1d20();
            LogRoll("Item/Tool", "1d20", roll);
            
            var item = DataDrivenTableData.GetItemFromRoll(roll);
            if (item != null)
            {
                character.inventory.Add(item);
                LogMessage($"Item: {item.itemName} - {item.description}");
            }
        }
        
        private void RollClothing(Character character)
        {
            int roll = DiceSystem.Roll1d12();
            LogRoll("Clothing/Accessory", "1d12", roll);
            
            var clothing = DataDrivenTableData.GetClothingFromRoll(roll);
            if (clothing != null)
            {
                character.inventory.Add(clothing);
                LogMessage($"Clothing: {clothing.itemName} - {clothing.description}");
            }
        }
        
        private void RollArmor(Character character)
        {
            int roll = DiceSystem.Roll1d12();
            LogRoll("Armor", "1d12", roll);
            
            var armor = DataDrivenTableData.GetArmorFromRoll(roll);
            if (armor != null)
            {
                character.equippedArmor.Add(armor);
                LogMessage($"Armor: {armor.armorName} (+{armor.armorValue})");
            }
        }
        
        private void RollStartingGold(Character character)
        {
            LogMessage("\n--- ROLLING STARTING GOLD ---");
            
            int roll = DiceSystem.Roll3d6();
            LogRoll("Starting Gold", "3d6", roll);
            
            character.gold = roll;
            LogMessage($"Starting Gold: {character.gold}g");
        }
        
        private void GenerateCharacterName(Character character)
        {
            LogMessage("\n--- GENERATING CHARACTER NAME ---");
            
            int firstPartRoll = DiceSystem.Roll1d20();
            int secondPartRoll = DiceSystem.Roll1d20();
            int modifierRoll = DiceSystem.Roll1d20();
            
            LogRoll("Name First Part", "1d20", firstPartRoll);
            LogRoll("Name Second Part", "1d20", secondPartRoll);
            LogRoll("Name Modifier", "1d20", modifierRoll);
            
            string[] firstParts = DataDrivenTableData.GetNameFirstParts();
            string[] secondParts = DataDrivenTableData.GetNameSecondParts();
            string[] modifiers = DataDrivenTableData.GetNameModifiers();
            
            string firstPart = firstParts.Length > 0 ? firstParts[firstPartRoll - 1] : "Gar";
            string secondPart = secondParts.Length > 0 ? secondParts[secondPartRoll - 1] : "Binzo";
            string modifier = modifiers.Length > 0 ? modifiers[modifierRoll - 1] : "None";
            
            string finalName = ApplyNameModifier(firstPart, secondPart, modifier);
            character.characterName = finalName;
            
            LogMessage($"Character Name: {finalName}");
        }
        
        private string ApplyNameModifier(string firstPart, string secondPart, string modifier)
        {
            switch (modifier)
            {
                case "Invert Part Order":
                    return secondPart + firstPart;
                case "Add Prefix (reroll, use first 2 letters of first part column)":
                    string prefix1 = firstPart.Substring(0, Math.Min(2, firstPart.Length));
                    return prefix1 + firstPart + secondPart;
                case "Add Prefix (reroll, use first 2 letters of second part column)":
                    string prefix2 = secondPart.Substring(0, Math.Min(2, secondPart.Length));
                    return prefix2 + firstPart + secondPart;
                case "Cut last Letter of first part":
                    string cutFirst = firstPart.Length > 1 ? firstPart.Substring(0, firstPart.Length - 1) : firstPart;
                    return cutFirst + secondPart;
                case "Cut last letter of second part":
                    string cutSecond = secondPart.Length > 1 ? secondPart.Substring(0, secondPart.Length - 1) : secondPart;
                    return firstPart + cutSecond;
                case "Cut first letter of first part":
                    string cutFirstFirst = firstPart.Length > 1 ? firstPart.Substring(1) : firstPart;
                    return cutFirstFirst + secondPart;
                case "Cut first letter of second part":
                    string cutSecondFirst = secondPart.Length > 1 ? secondPart.Substring(1) : secondPart;
                    return firstPart + cutSecondFirst;
                case "Repeat First Part":
                    return firstPart + firstPart + secondPart;
                case "Repeat Second Part":
                    return firstPart + secondPart + secondPart;
                case "First + Second + First":
                    return firstPart + secondPart + firstPart;
                case "Second + First + Second":
                    return secondPart + firstPart + secondPart;
                case "First Part Only":
                    return firstPart;
                case "Second Part Only":
                    return secondPart;
                default:
                    return firstPart + secondPart;
            }
        }
        
        private void LogRoll(string description, string dice, int result)
        {
            string message = $"Rolled {dice} for {description}: {result}";
            LogMessage(message);
        }
        
        private void LogMessage(string message)
        {
            if (printRolls)
            {
                Console.WriteLine(message);
            }
            creationLog.Add(message);
        }
        
        public List<string> GetCreationLog()
        {
            return new List<string>(creationLog);
        }
    }
} 