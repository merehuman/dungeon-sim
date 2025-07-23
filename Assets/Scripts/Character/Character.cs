using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [Header("Basic Information")]
    public string characterName;
    public CharacterRace race;
    public CharacterClass characterClass;
    public PersonalityTrait personalityTrait;
    
    [Header("Core Stats")]
    public int strength;
    public int dexterity;
    public int wisdom;
    
    [Header("Combat Stats")]
    public int maxHealth;
    public int currentHealth;
    public int armorValue;
    public int level = 1;
    public int experience = 0;
    
    [Header("Resources")]
    public int gold;
    public int torches;
    public int rations;
    
    [Header("Equipment")]
    public Weapon equippedWeapon;
    public List<Armor> equippedArmor = new List<Armor>();
    public List<Item> inventory = new List<Item>();
    
    [Header("Abilities")]
    public List<Spell> spells = new List<Spell>();
    public List<MartialAttack> martialAttacks = new List<MartialAttack>();
    public List<HealSpell> healSpells = new List<HealSpell>();
    
    [Header("Special Abilities")]
    public List<string> specialAbilities = new List<string>();
    public List<string> racialAbilities = new List<string>();
    public List<string> classAbilities = new List<string>();
    
    // Constructor
    public Character()
    {
        // Initialize with starting equipment
        torches = 1;
        rations = 1;
        currentHealth = maxHealth;
    }
    
    // Calculate total armor value from all equipped armor
    public void CalculateArmorValue()
    {
        armorValue = 0;
        foreach (var armor in equippedArmor)
        {
            armorValue += armor.armorValue;
        }
    }
    
    // Apply stat bonuses from race, class, and personality
    public void ApplyStatBonuses()
    {
        // Apply class bonuses
        switch (characterClass)
        {
            case CharacterClass.Fighter:
                strength += 1;
                maxHealth += 1;
                armorValue += 1;
                break;
            case CharacterClass.Rogue:
                dexterity += 1;
                break;
            case CharacterClass.Wizard:
                wisdom += 1;
                break;
            case CharacterClass.Cleric:
                wisdom += 1;
                break;
        }
        
        // Apply personality trait bonuses
        switch (personalityTrait)
        {
            case PersonalityTrait.Friendly:
            case PersonalityTrait.OpenMinded:
            case PersonalityTrait.Sympathetic:
            case PersonalityTrait.Relaxed:
                wisdom += 1;
                break;
            case PersonalityTrait.Mean:
            case PersonalityTrait.Greedy:
            case PersonalityTrait.ColdBlooded:
            case PersonalityTrait.Happy:
                strength += 1;
                break;
            case PersonalityTrait.Judgmental:
            case PersonalityTrait.Giving:
            case PersonalityTrait.Depressed:
            case PersonalityTrait.Anxious:
                dexterity += 1;
                break;
        }
        
        // Apply racial bonuses
        ApplyRacialBonuses();
        
        // Update current health to match max health
        currentHealth = maxHealth;
    }
    
    private void ApplyRacialBonuses()
    {
        switch (race)
        {
            case CharacterRace.Human:
                // +1 stat of choice - will be handled during character creation
                break;
            case CharacterRace.Dwarf:
                maxHealth += 1; // +1hp/level
                racialAbilities.Add("Immune to poison");
                break;
            case CharacterRace.Elf:
                wisdom += 1;
                racialAbilities.Add("Cannot get tired");
                break;
            case CharacterRace.Halfling:
                dexterity += 1;
                racialAbilities.Add("Lucky (1 reroll/day)");
                break;
            case CharacterRace.Fey:
                wisdom += 1;
                // +1 random spell will be handled during character creation
                break;
            case CharacterRace.Giant:
                specialAbilities.Add("+1 to all damage rolls");
                break;
        }
    }
    
    // Get character sheet as formatted string
    public string GetCharacterSheet()
    {
        string sheet = $"=== CHARACTER SHEET ===\n";
        sheet += $"Name: {characterName}\n";
        sheet += $"Race: {race}\n";
        sheet += $"Class: {characterClass}\n";
        sheet += $"Level: {level}\n";
        sheet += $"Personality: {personalityTrait}\n\n";
        
        sheet += $"=== STATS ===\n";
        sheet += $"Strength: {strength}\n";
        sheet += $"Dexterity: {dexterity}\n";
        sheet += $"Wisdom: {wisdom}\n\n";
        
        sheet += $"=== COMBAT ===\n";
        sheet += $"Health: {currentHealth}/{maxHealth}\n";
        sheet += $"Armor: {armorValue}\n\n";
        
        sheet += $"=== EQUIPMENT ===\n";
        if (equippedWeapon != null)
            sheet += $"Weapon: {equippedWeapon.weaponName} ({equippedWeapon.damageDie})\n";
        
        foreach (var armor in equippedArmor)
        {
            sheet += $"Armor: {armor.armorName} (+{armor.armorValue})\n";
        }
        
        sheet += $"\n=== INVENTORY ===\n";
        foreach (var item in inventory)
        {
            sheet += $"- {item.itemName} (Value: {item.value}g)\n";
        }
        
        sheet += $"\n=== RESOURCES ===\n";
        sheet += $"Gold: {gold}g\n";
        sheet += $"Torches: {torches}\n";
        sheet += $"Rations: {rations}\n\n";
        
        if (spells.Count > 0)
        {
            sheet += $"=== SPELLS ===\n";
            foreach (var spell in spells)
            {
                sheet += $"- {spell.spellName} ({spell.castsPerDay} casts/day)\n";
            }
            sheet += "\n";
        }
        
        if (martialAttacks.Count > 0)
        {
            sheet += $"=== MARTIAL ATTACKS ===\n";
            foreach (var attack in martialAttacks)
            {
                sheet += $"- {attack.attackName}: {attack.damageDie} damage to {attack.target}\n";
            }
            sheet += "\n";
        }
        
        if (healSpells.Count > 0)
        {
            sheet += $"=== HEAL SPELLS ===\n";
            foreach (var heal in healSpells)
            {
                sheet += $"- {heal.spellName}: {heal.healValue} to {heal.target} ({heal.castsPerDay} casts/day)\n";
            }
            sheet += "\n";
        }
        
        if (specialAbilities.Count > 0)
        {
            sheet += $"=== SPECIAL ABILITIES ===\n";
            foreach (var ability in specialAbilities)
            {
                sheet += $"- {ability}\n";
            }
            sheet += "\n";
        }
        
        if (racialAbilities.Count > 0)
        {
            sheet += $"=== RACIAL ABILITIES ===\n";
            foreach (var ability in racialAbilities)
            {
                sheet += $"- {ability}\n";
            }
            sheet += "\n";
        }
        
        if (classAbilities.Count > 0)
        {
            sheet += $"=== CLASS ABILITIES ===\n";
            foreach (var ability in classAbilities)
            {
                sheet += $"- {ability}\n";
            }
            sheet += "\n";
        }
        
        return sheet;
    }
} 