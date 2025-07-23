using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Weapon
{
    public string weaponName;
    public string damageDie;
    public int value = 15;
    public List<string> traits = new List<string>();
    
    public Weapon(string name, string damage, List<string> weaponTraits = null)
    {
        weaponName = name;
        damageDie = damage;
        if (weaponTraits != null)
            traits = weaponTraits;
    }
}

[System.Serializable]
public class Armor
{
    public string armorName;
    public int armorValue;
    public int value;
    public List<string> traits = new List<string>();
    
    public Armor(string name, int armor, int armorValue, List<string> armorTraits = null)
    {
        armorName = name;
        armorValue = armor;
        value = armorValue;
        if (armorTraits != null)
            traits = armorTraits;
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public int value;
    public string description;
    public List<string> uses = new List<string>();
    
    public Item(string name, int itemValue, string desc, List<string> itemUses = null)
    {
        itemName = name;
        value = itemValue;
        description = desc;
        if (itemUses != null)
            uses = itemUses;
    }
}

[System.Serializable]
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

[System.Serializable]
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

[System.Serializable]
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