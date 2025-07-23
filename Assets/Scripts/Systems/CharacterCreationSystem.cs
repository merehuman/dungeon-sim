using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationSystem : MonoBehaviour
{
    [Header("Character Creation Settings")]
    public bool printRolls = true;
    public bool printDescriptions = true;
    
    [Header("Data Loading")]
    public CSVDataLoader csvDataLoader;
    
    private List<string> creationLog = new List<string>();
    
    // Main character creation method
    public Character CreateCharacter()
    {
        // Initialize data-driven system if needed
        if (!DataDrivenTableData.IsInitialized() && csvDataLoader != null)
        {
            DataDrivenTableData.Initialize(csvDataLoader);
        }
        
        creationLog.Clear();
        LogMessage("=== STARTING CHARACTER CREATION ===");
        
        Character character = new Character();
        
        // Step 1: Roll stats (3d6 x 3)
        RollStats(character);
        
        // Step 2: Roll race
        RollRace(character);
        
        // Step 3: Roll class
        RollClass(character);
        
        // Step 4: Roll HP
        RollHitPoints(character);
        
        // Step 5: Roll personality trait
        RollPersonalityTrait(character);
        
        // Step 6: Roll attacks or spells based on class
        RollClassAbilities(character);
        
        // Step 7: Randomly choose +2 extra torches or +2 extra rations
        RollStartingResources(character);
        
        // Step 8: Roll equipment
        RollEquipment(character);
        
        // Step 9: Calculate total armor value
        character.CalculateArmorValue();
        
        // Step 10: Roll 3d6 to determine starting amount of Gold
        RollStartingGold(character);
        
        // Step 11: Update stats based on outcomes
        character.ApplyStatBonuses();
        
        // Step 12: Roll to name the character
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
        
        // Handle special cases first
        if (roll == 19) // Elemental
        {
            LogMessage("Rolled Elemental - rerolling with elemental modifications");
            roll = DiceSystem.Roll1d20();
            LogRoll("Elemental Race Reroll", "1d20", roll);
            character.race = GetRaceFromRoll(roll);
            character.racialAbilities.Add("Elemental nature - special modifications apply");
        }
        else if (roll == 20) // Undead
        {
            LogMessage("Rolled Undead - rerolling with undead modifications");
            roll = DiceSystem.Roll1d20();
            LogRoll("Undead Race Reroll", "1d20", roll);
            character.race = GetRaceFromRoll(roll);
            character.racialAbilities.Add("Undead nature - special modifications apply");
        }
        else
        {
            character.race = GetRaceFromRoll(roll);
        }
        
        LogMessage($"Race determined: {character.race}");
    }
    
    private CharacterRace GetRaceFromRoll(int roll)
    {
        return DataDrivenTableData.GetRaceFromRoll(roll);
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
        
        // Fallback to hardcoded values
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
                return 6; // Default
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
        
        // Handle roll twice special case
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
        
        // Roll weapon
        RollWeapon(character);
        
        // Roll item/tool
        RollItem(character);
        
        // Roll clothing
        RollClothing(character);
        
        // Roll armor
        RollArmor(character);
    }
    
    private void RollWeapon(Character character)
    {
        int maxRoll = 12;
        
        // Apply class restrictions
        if (character.characterClass == CharacterClass.Cleric || 
            character.characterClass == CharacterClass.Wizard || 
            character.characterClass == CharacterClass.Rogue)
        {
            maxRoll = 8; // 1d8 for these classes
        }
        
        int roll = DiceSystem.RollDie(maxRoll);
        LogRoll("Weapon", $"1d{maxRoll}", roll);
        
        var weapon = DataDrivenTableData.GetWeaponFromRoll(roll);
        if (weapon != null)
        {
            // Apply class restrictions
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
            Debug.Log(message);
        }
        creationLog.Add(message);
    }
    
    public List<string> GetCreationLog()
    {
        return new List<string>(creationLog);
    }
    
    public void PrintCharacterSheet(Character character)
    {
        if (printDescriptions)
        {
            Debug.Log(character.GetCharacterSheet());
        }
    }
} 