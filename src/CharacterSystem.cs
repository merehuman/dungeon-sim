using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonSim
{
    // Enums
    public enum CharacterRace
    {
        Human, Dwarf, Elf, Halfling, Fey, Giant, Elemental, Undead
    }

    public enum CharacterClass
    {
        Fighter, Rogue, Wizard, Cleric
    }

    public enum PersonalityTrait
    {
        Friendly, Mean, Judgmental, OpenMinded, Greedy, Giving, Sympathetic, ColdBlooded, Depressed, Happy, Anxious, Relaxed
    }

    // Character class
    public class Character
    {
        public string characterName = "";
        public CharacterRace race;
        public CharacterClass characterClass;
        public PersonalityTrait personalityTrait;

        public int strength;
        public int dexterity;
        public int wisdom;

        public int maxHealth;
        public int currentHealth;
        public int armorValue;
        public int level = 1;
        public int experience = 0;

        public int gold;
        public int torches;
        public int rations;

        public Weapon? equippedWeapon;
        public List<Armor> equippedArmor = new List<Armor>();
        public List<Item> inventory = new List<Item>();

        public List<Spell> spells = new List<Spell>();
        public List<MartialAttack> martialAttacks = new List<MartialAttack>();
        public List<HealSpell> healSpells = new List<HealSpell>();

        public List<string> specialAbilities = new List<string>();
        public List<string> racialAbilities = new List<string>();
        public List<string> classAbilities = new List<string>();

        public Character()
        {
            torches = 1;
            rations = 1;
            currentHealth = maxHealth;
        }

        public void CalculateArmorValue()
        {
            armorValue = 0;
            foreach (var armor in equippedArmor)
            {
                armorValue += armor.armorValue;
            }
        }

        public void ApplyStatBonuses()
        {
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

            ApplyRacialBonuses();
            currentHealth = maxHealth;
        }

        private void ApplyRacialBonuses()
        {
            switch (race)
            {
                case CharacterRace.Dwarf:
                    maxHealth += 1;
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
                    break;
                case CharacterRace.Giant:
                    specialAbilities.Add("+1 to all damage rolls");
                    break;
            }
        }

        public string GetCharacterSheet()
        {
            string sheet = $"=== CHARACTER SHEET ==={Environment.NewLine}";
            sheet += $"Name: {characterName}{Environment.NewLine}";
            sheet += $"Race: {race}{Environment.NewLine}";
            sheet += $"Class: {characterClass}{Environment.NewLine}";
            sheet += $"Level: {level}{Environment.NewLine}";
            sheet += $"Personality: {personalityTrait}{Environment.NewLine}{Environment.NewLine}";

            sheet += $"=== STATS ==={Environment.NewLine}";
            sheet += $"Strength: {strength}{Environment.NewLine}";
            sheet += $"Dexterity: {dexterity}{Environment.NewLine}";
            sheet += $"Wisdom: {wisdom}{Environment.NewLine}{Environment.NewLine}";

            sheet += $"=== COMBAT ==={Environment.NewLine}";
            sheet += $"Health: {currentHealth}/{maxHealth}{Environment.NewLine}";
            sheet += $"Armor: {armorValue}{Environment.NewLine}{Environment.NewLine}";

            sheet += $"=== EQUIPMENT ==={Environment.NewLine}";
            if (equippedWeapon != null)
                sheet += $"Weapon: {equippedWeapon.weaponName} ({equippedWeapon.damageDie}){Environment.NewLine}";

            foreach (var armor in equippedArmor)
            {
                sheet += $"Armor: {armor.armorName} (+{armor.armorValue}){Environment.NewLine}";
            }

            sheet += $"{Environment.NewLine}=== INVENTORY ==={Environment.NewLine}";
            foreach (var item in inventory)
            {
                sheet += $"- {item.itemName} (Value: {item.value}g){Environment.NewLine}";
            }

            sheet += $"{Environment.NewLine}=== RESOURCES ==={Environment.NewLine}";
            sheet += $"Gold: {gold}g{Environment.NewLine}";
            sheet += $"Torches: {torches}{Environment.NewLine}";
            sheet += $"Rations: {rations}{Environment.NewLine}{Environment.NewLine}";

            if (spells.Count > 0)
            {
                sheet += $"=== SPELLS ==={Environment.NewLine}";
                foreach (var spell in spells)
                {
                    sheet += $"- {spell.spellName} ({spell.castsPerDay} casts/day){Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            if (martialAttacks.Count > 0)
            {
                sheet += $"=== MARTIAL ATTACKS ==={Environment.NewLine}";
                foreach (var attack in martialAttacks)
                {
                    sheet += $"- {attack.attackName}: {attack.damageDie} damage to {attack.target}{Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            if (healSpells.Count > 0)
            {
                sheet += $"=== HEAL SPELLS ==={Environment.NewLine}";
                foreach (var heal in healSpells)
                {
                    sheet += $"- {heal.spellName}: {heal.healValue} to {heal.target} ({heal.castsPerDay} casts/day){Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            if (specialAbilities.Count > 0)
            {
                sheet += $"=== SPECIAL ABILITIES ==={Environment.NewLine}";
                foreach (var ability in specialAbilities)
                {
                    sheet += $"- {ability}{Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            if (racialAbilities.Count > 0)
            {
                sheet += $"=== RACIAL ABILITIES ==={Environment.NewLine}";
                foreach (var ability in racialAbilities)
                {
                    sheet += $"- {ability}{Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            if (classAbilities.Count > 0)
            {
                sheet += $"=== CLASS ABILITIES ==={Environment.NewLine}";
                foreach (var ability in classAbilities)
                {
                    sheet += $"- {ability}{Environment.NewLine}";
                }
                sheet += Environment.NewLine;
            }

            return sheet;
        }
    }

    // Equipment classes
    public class Weapon
    {
        public string weaponName;
        public string damageDie;
        public int value = 15;
        public List<string> traits = new List<string>();

        public Weapon(string name, string damage, List<string>? weaponTraits = null)
        {
            weaponName = name;
            damageDie = damage;
            if (weaponTraits != null)
                traits = weaponTraits;
        }
    }

    public class Armor
    {
        public string armorName;
        public int armorValue;
        public int value;
        public List<string> traits = new List<string>();

        public Armor(string name, int armor, int armorValue, List<string>? armorTraits = null)
        {
            armorName = name;
            armorValue = armor;
            value = armorValue;
            if (armorTraits != null)
                traits = armorTraits;
        }
    }

    public class Item
    {
        public string itemName;
        public int value;
        public string description;
        public List<string> uses = new List<string>();

        public Item(string name, int itemValue, string desc, List<string>? itemUses = null)
        {
            itemName = name;
            value = itemValue;
            description = desc;
            if (itemUses != null)
                uses = itemUses;
        }
    }

    public class Spell
    {
        public string spellName;
        public string element;
        public string form;
        public string effect;
        public int castsPerDay;

        public Spell(string name, string spellElement, string spellForm, string spellEffect, int casts)
        {
            spellName = name;
            element = spellElement;
            form = spellForm;
            effect = spellEffect;
            castsPerDay = casts;
        }
    }

    public class MartialAttack
    {
        public string attackName;
        public string damageDie;
        public string target;
        public string description;

        public MartialAttack(string name, string damage, string attackTarget, string desc)
        {
            attackName = name;
            damageDie = damage;
            target = attackTarget;
            description = desc;
        }
    }

    public class HealSpell
    {
        public string spellName;
        public string healValue;
        public string target;
        public int castsPerDay;
        public string additionalEffects;

        public HealSpell(string name, string heal, string healTarget, int casts, string effects = "")
        {
            spellName = name;
            healValue = heal;
            target = healTarget;
            castsPerDay = casts;
            additionalEffects = effects;
        }
    }
}
