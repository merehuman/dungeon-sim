using System;

namespace DungeonSim
{
    /// <summary>
    /// Lookups for dungeon tables from Dungeon Tables.md. Used to resolve rolls into readable names and effects.
    /// </summary>
    public static class DungeonTables
    {
        public static (string Name, string Effect) GetTheme(int roll)
        {
            if (roll <= 3) return ("Realm of Undeath", "All enemies are undead.");
            if (roll <= 6) return ("Ruin", "Gold per room doubled; one creature = two of that creature.");
            if (roll <= 9) return ("Monster Den", "All characters poisoned by the smell while inside.");
            if (roll <= 11) return ("Bandit Hideout", "Tenth room: Bandit Leader + 2x loot guaranteed.");
            if (roll <= 13) return ("Wizard's Tower", "Traps +1d6 magical damage; exceptional/ornate/unique items are magical; all rooms manmade.");
            if (roll <= 15) return ("Underdark", "Exits lead up to 3 hexes away; torches diminish at twice the rate.");
            if (roll == 16) return ("Fae-Touched", "All magic loot and creatures are Fae-Touched.");
            if (roll == 17) return ("Demonic", "All magic loot and creatures are Demonic.");
            if (roll == 18) return ("Elemental", "Roll random element; all magic loot and creatures modified by that element.");
            if (roll == 19) return ("Secret Lair", "One room: ELITE creature + one random creature + 2x magical loot.");
            if (roll == 20) return ("The Forecamp", "One room, guarded by a keeper; tunnel to Capital if defeated.");
            return ("Unknown", "");
        }

        public static (string Name, string Effect) GetModifier(int roll)
        {
            if (roll <= 5) return ("None", "No effect.");
            if (roll <= 8) return ("Trapped", "Every second room (from Room 2) contains a trap.");
            if (roll <= 10) return ("Geologically Fantastic", "Enemies made of stone; loot at least Standard but Heavy.");
            if (roll <= 12) return ("Evil", "All healing in dungeon reduced by 1/2.");
            if (roll == 13) return ("Ancient", "Loot Flimsy on 1-13, Unique on 15-17.");
            if (roll == 14) return ("Labyrinthine", "Room Type always corridor; first room ≥3 doors; after 3 rooms exits become Blocked; loot at least Standard.");
            if (roll == 15) return ("Picked-Over", "No Exceptional/Ornate/Unique loot; 6 rooms explored = one Magic item.");
            if (roll == 16) return ("Flooded", "Waist-deep water; 2 turns per room; electric 2x damage; paper loot useless.");
            if (roll == 17) return ("Ultra-Dark", "Only darkvision works; torches do nothing.");
            if (roll == 18) return ("Seething with Enemies", "One creature rolled = two of that creature.");
            if (roll == 19) return ("Bountiful", "Loot rolled = roll twice, take both.");
            if (roll == 20) return ("Roll Twice", "Roll modifier table again and apply both.");
            return ("None", "No effect.");
        }

        public static (string Type, string Shape, string Origin) GetRoomType(int roll3d6)
        {
            if (roll3d6 <= 3) return ("Passage", "Rounded", "Natural");
            return ("Room", "Rectilinear", "Manmade");
        }

        public static (string Name, string Effect, int MaxDoors) GetRoomSize(int roll)
        {
            if (roll == 1) return ("Tiny", "Maximum one door.", 1);
            if (roll >= 2 && roll <= 4) return ("Small", "Maximum two doors.", 2);
            if (roll >= 5 && roll <= 8) return ("Medium", "None.", 99);
            if (roll == 9) return ("Large", "None.", 99);
            if (roll >= 10 && roll <= 11) return ("Huge", "If at least one door rolled, add one extra door.", 99);
            if (roll == 12) return ("Gargantuan", "Before next room: DC 12 WIS check; pass = one piece of loot.", 99);
            return ("Medium", "None.", 99);
        }

        /// <summary>Returns number of doors from d12 roll (0-3).</summary>
        public static int GetDoorCount(int roll)
        {
            if (roll <= 2) return 0;
            if (roll <= 8) return 1;
            if (roll <= 11) return 2;
            return 3;
        }

        public static (string Name, string Effect) GetDoorType(int roll)
        {
            if (roll <= 7) return ("Open", "None.");
            if (roll <= 11) return ("Locked", "Must be picked to open.");
            if (roll <= 13) return ("Blocked", "DC 12 STR to unblock, then normal open door.");
            if (roll <= 15) return ("Trapped (Open)", "Resolve trap; then becomes open door.");
            if (roll == 16) return ("Trapped (Locked)", "Resolve trap; then becomes locked door.");
            if (roll == 17) return ("Secret", "DC 12 WIS to notice; if noticed = two open doors.");
            if (roll == 18) return ("Trapped (Secret)", "DC 12 WIS to notice; resolve trap then two open doors.");
            if (roll >= 19) return ("Dungeon Exit", "Leads back to hex exploration.");
            return ("Open", "None.");
        }

        public static (string Name, string Effect) GetRoomDetail(int roll)
        {
            if (roll <= 3) return ("None", "None.");
            if (roll <= 6) return ("Brightly lit", "No torches needed in this room.");
            if (roll <= 8) return ("Dead adventurer", "Loot for one random item/tool.");
            if (roll <= 10) return ("Small harmless creatures", "+1 Rations.");
            if (roll == 11) return ("Portal", "WIS check: teleport to dungeon entrance.");
            if (roll == 12) return ("Puzzle", "WIS check: success = random ornate+ item.");
            return ("None", "None.");
        }

        public static (string Name, bool HasTrap, bool HasLoot, bool IsThemeEncounter) GetDungeonEncounter(int roll)
        {
            if (roll == 1) return ("2 Creatures", false, false, false);
            if (roll == 2) return ("2 Creatures + 1 Loot", false, true, false);
            if (roll <= 4) return ("1 Creature + 1 Loot", false, true, false);
            if (roll <= 7) return ("1 Creature", false, false, false);
            if (roll <= 9) return ("1 Trap", true, false, false);
            if (roll <= 12) return ("1 Trap + 1 Loot", true, true, false);
            if (roll <= 16) return ("1 Loot", false, true, false);
            return ("Theme Specific Encounter", false, false, true);
        }

        public static (string Name, int Dc, string Stat, string Damage, string Effects) GetTrap(int roll)
        {
            if (roll <= 4) return ("Poison Darts", 8, "Dex", "1d4", "Target is poisoned.");
            if (roll <= 8) return ("Walls Close In", 8, "Str", "1d4", "None.");
            if (roll <= 11) return ("Hot Oil", 8, "Dex", "1d6", "If torch lit, +1d4 extra damage.");
            if (roll <= 13) return ("Swinging Blade", 10, "Dex", "1d6", "None.");
            if (roll <= 15) return ("Snake Pit", 10, "Dex", "1d6", "None.");
            if (roll <= 17) return ("Magical Glyph", 10, "Wis", "1d8", "None.");
            if (roll == 18) return ("Cage Trap", 12, "Dex", "1d8", "Victim trapped; one turn to get them out.");
            if (roll == 19) return ("Rolling Ball", 12, "Dex", "1d10", "None.");
            if (roll == 20) return ("Poison Gas", 12, "Wis", "1d8", "Effects whole party.");
            return ("Unknown Trap", 10, "Dex", "1d6", "None.");
        }

        /// <summary>Returns theme-specific encounter name/description for the given theme roll (1-20) and d6 roll (1-6).</summary>
        public static string GetThemeEncounter(int themeRoll, int d6Roll)
        {
            d6Roll = Math.Clamp(d6Roll, 1, 6);
            if (themeRoll <= 3) return GetRealmOfUndeathEncounter(d6Roll);
            if (themeRoll <= 6) return GetRuinEncounter(d6Roll);
            if (themeRoll <= 9) return GetMonsterDenEncounter(d6Roll);
            if (themeRoll <= 11) return GetBanditHideoutEncounter(d6Roll);
            if (themeRoll <= 13) return GetWizardsTowerEncounter(d6Roll);
            if (themeRoll <= 15) return GetUnderdarkEncounter(d6Roll);
            if (themeRoll == 16) return GetFaeTouchedEncounter(d6Roll);
            if (themeRoll == 17) return GetDemonicEncounter(d6Roll);
            if (themeRoll == 18) return GetElementalEncounter(d6Roll);
            return "Theme encounter (see tables).";
        }

        private static string GetRealmOfUndeathEncounter(int r)
        {
            switch (r) {
                case 1: return "Flesh Golem (20 HP, 1d10) — revives after 1d6 turns, will not pursue.";
                case 2: return "Undead Mound (15 HP, 1d6) — 3+ damage (after reduction) sucks target in; death if mound not killed next turn.";
                case 3: return "1d4 Inside Out Dog (5 HP each, 1d4) — if all killed, lowest WIS save or panicked until rest.";
                case 4: return "Cloud of Undeath — lowest WIS save or turned undead until leave dungeon; 1d6 each new room they may attack.";
                case 5: return "Necromancer's Retinue: 2d6 skeletons (2 HP each, 1d4); one has Necromantic Amulet (force or trickery).";
                case 6: return "Fountain of Blood — drink to heal party to full; Fae/cautious may refuse.";
                default: return "Realm of Undeath encounter.";
            }
        }

        private static string GetRuinEncounter(int r)
        {
            switch (r) {
                case 1: return "Living Stone (50 HP, 1d4) — 1/2 damage from non-magical weapons; on death roll loot +10, heavy.";
                case 2: return "Ancient Guardian (20 HP, 1d6) — Undead; chases if party does not fight.";
                case 3: return "1d6 Treasure Goblins (5 HP each) — flee after one round; on kill: 1-3 = 1d100 gold, 4-6 = loot +14.";
                case 4: return "Buzzing Core — each spellcaster reroll one spell; WIS <10 take 1d4 damage.";
                case 5: return "Preserved Library: 2d6 animated books (one random spell/turn); success = two random spell scrolls.";
                case 6: return "Ancient Forge — choose one weapon, increase quality by 1.";
                default: return "Ruin encounter.";
            }
        }

        private static string GetMonsterDenEncounter(int r)
        {
            switch (r) {
                case 1: return "Manticore (30 HP, 1d8) — poison on crit; +1d4 baby manticores (5 HP, 1d4).";
                case 2: return "Giant Spider (20 HP, 1d8) — stun on crit; +1d4 baby giant spiders (5 HP, 1d4).";
                case 3: return "1d4 Troglodytes (8 HP each, 1d6) — if all killed, +1 torch per creature.";
                case 4: return "Dung — lowest DEX steps in it; all party diseased until that member reaches settlement or leaves.";
                case 5: return "Mutation — random member grotesque transformation; invert two stats permanently.";
                case 6: return "Eggs — unhatched beast egg; 2d6 days hatch into baby manticore (1-3) or giant spider (4-6); joins party.";
                default: return "Monster Den encounter.";
            }
        }

        private static string GetBanditHideoutEncounter(int r)
        {
            switch (r) {
                case 1: return "Bandit King (30 HP, 1d10) + 1d6 bandits; if king killed first, bandits flee.";
                case 2: return "The Berserker (30 HP, 2d6) — damage distributed evenly among party.";
                case 3: return "Trained Beasts (5 HP each, 1d6) — WIS check: success = beasts flee; next creature encounter = room with creature dead.";
                case 4: return "Captured — random member captured; 1d6 rooms later find them; fight 1d6 bandits.";
                case 5: return "Bandit King's Horde — 3× loot +15, 1d100 gold; taking it triggers Bandit King next room.";
                case 6: return "The Armory — roll one weapon or one weapon +15 quality.";
                default: return "Bandit Hideout encounter.";
            }
        }

        private static string GetWizardsTowerEncounter(int r)
        {
            switch (r) {
                case 1: return "The Wizard (30 HP, 3 random spells) — drops 1 random spellbook.";
                case 2: return "The Wizard's Homunculus (20 HP, 2 random spells) — drops 2 random spell scrolls.";
                case 3: return "The Wizard's Projection (10 HP, 1 random spell) — drops 1 random spell scroll.";
                case 4: return "The Wizard's Denial — party teleported outside; dungeon entrance disappears; crawl over.";
                case 5: return "The Wizard's Indifference — tower empty until this table rolled again; no encounters.";
                case 6: return "The Wizard's Embrace — 100g: each spellcaster +1 spell and +1 cast/day.";
                default: return "Wizard's Tower encounter.";
            }
        }

        private static string GetUnderdarkEncounter(int r)
        {
            switch (r) {
                case 1: return "Myconid Destroyer (30 HP, 1d10) — on death spawn 1d6 Myconid.";
                case 2: return "Bulette (25 HP, 1d8) — carapace = one random +15 armor.";
                case 3: return "1d4 Drow Scouts (5 HP each, 1d8) — last one can join as level 1 rogue (sunlight disadvantage, dungeon advantage).";
                case 4: return "Lost in the Depths — lose 3 torches; wind up at dungeon entrance.";
                case 5: return "Hallucinatory Spores — d6: 1-3 reroll personality (lowest WIS); 4-5 reroll all traits; 6 = reroll all + spellcasters gain spell.";
                case 6: return "Mycelial Network — WIS check: teleport to any region within 3 hexes; always usable.";
                default: return "Underdark encounter.";
            }
        }

        private static string GetFaeTouchedEncounter(int r)
        {
            switch (r) {
                case 1: return "Doppelgangers — one per party member; 1/2 HP each, same damage die; exact visual copy.";
                case 2: return "Hag (20 HP, 1d6) — one random spell/turn; on death 1d4 healing potions.";
                case 3: return "2d6 Fairies (1 HP each, 1d4) — will not attack if party has Fae member.";
                case 4: return "House of Loss — highest WIS pledges to deity: +3 spells, HP permanently reduced to 3.";
                case 5: return "Fae Chaos — roll on fae chaos table.";
                case 6: return "Groggle (10 HP, 1d8) — joins for rest of dungeon; −15 gold per room he's in.";
                default: return "Fae-Touched encounter.";
            }
        }

        private static string GetDemonicEncounter(int r)
        {
            switch (r) {
                case 1: return "Buzzing Host (15 HP, 3d6) — only damaged by spells/magical; damage spread evenly.";
                case 2: return "The Spoiled Egg (20 HP, 1d6) — two lowest WIS panic until slain or escape.";
                case 3: return "1d6 Imps (3 HP each, 1d4) — if one remains at turn end, call random demonic creature.";
                case 4: return "Demonic Possession — lowest STR turns on party; defeat to wake (1 HP, permanently depressed).";
                case 5: return "Demonic Incursion — last settlement (not capital) destroyed; may enter ruins dungeon from that hex.";
                case 6: return "Demonic Pact — highest WIS: +1 spell, −2 STR.";
                default: return "Demonic encounter.";
            }
        }

        private static string GetElementalEncounter(int r)
        {
            switch (r) {
                case 1: return "Interplanar Being (30 HP, 1d10) — two elemental affinities.";
                case 2: return "Living Gem (20 HP, 2d6) — only damage that can hurt stone; damage spread evenly.";
                case 3: return "1d6 Spitlings (3 HP each, 1d4) — on death 1d4 elemental damage to killer.";
                case 4: return "Elemental Devastation — lowest DEX: 1d6 damage, fear of element (double damage from it).";
                case 5: return "Elemental Revolution — reroll dungeon element.";
                case 6: return "Elemental Transfiguration — highest DEX gains elemental property.";
                default: return "Elemental encounter.";
            }
        }
    }
}
