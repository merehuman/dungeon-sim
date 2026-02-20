using System;
using System.Collections.Generic;

namespace DungeonSim
{
    public static class HexGenerationSystem
    {
        private static Random random = new Random();
        private static CSVDataLoader? csvLoader;

        public static void Initialize(CSVDataLoader loader)
        {
            csvLoader = loader;
        }

        // Generate a new hex with all its properties
        public static Hex GenerateHex(HexCoordinate coordinate, HexMap map)
        {
            Hex hex = new Hex(coordinate);
            
            // Generate biome
            hex.Biome = RollBiome(map);
            
            // Generate modifier
            hex.Modifier = RollBiomeModifier();
            
            // Generate weather
            hex.Weather = RollWeather();
            
            // Generate encounter
            hex.Encounter = RollEncounter();
            
            // Generate specific content based on encounter
            GenerateEncounterContent(hex);

            // Create resolved encounter instance for integration with encounter/dungeon system
            hex.ResolvedEncounter = Encounter.CreateFromHex(hex);
            if (hex.ResolvedEncounter is DungeonEncounter dungeonEncounter)
                dungeonEncounter.RollThemeAndModifier();
            
            return hex;
        }

        private static BiomeType RollBiome(HexMap map)
        {
            int roll = DiceSystem.Roll1d20();
            
            if (csvLoader == null)
            {
                // Fallback to hardcoded values if CSV not loaded
                return GetBiomeFromRoll(roll, map);
            }
            
            var biomeData = csvLoader.GetRowByRoll("biomes", roll);
            if (biomeData != null && biomeData.ContainsKey("BiomeType"))
            {
                string biomeTypeStr = biomeData["BiomeType"];
                if (biomeTypeStr == "SameAsPrevious")
                {
                    // Same as previous - get the last explored hex's biome
                    var exploredHexes = map.GetAllExploredHexes();
                    if (exploredHexes.Count > 0)
                    {
                        return exploredHexes[exploredHexes.Count - 1].Biome;
                    }
                    return BiomeType.Plains; // Default if no previous hex
                }
                
                if (Enum.TryParse<BiomeType>(biomeTypeStr, out BiomeType biome))
                {
                    return biome;
                }
            }
            
            // Fallback to hardcoded values
            return GetBiomeFromRoll(roll, map);
        }

        private static BiomeType GetBiomeFromRoll(int roll, HexMap map)
        {
            switch (roll)
            {
                case 1:
                case 2:
                    // Same as previous - get the last explored hex's biome
                    var exploredHexes = map.GetAllExploredHexes();
                    if (exploredHexes.Count > 0)
                    {
                        return exploredHexes[exploredHexes.Count - 1].Biome;
                    }
                    return BiomeType.Plains; // Default if no previous hex
                case 3:
                case 4:
                    return BiomeType.Hills;
                case 5:
                case 6:
                    return BiomeType.Plains;
                case 7:
                case 8:
                    return BiomeType.Mountains;
                case 9:
                case 10:
                    return BiomeType.Forest;
                case 11:
                case 12:
                    return BiomeType.Desert;
                case 13:
                case 14:
                    return BiomeType.Tundra;
                case 15:
                case 16:
                    return BiomeType.Canyon;
                case 17:
                case 18:
                    return BiomeType.Lake;
                case 19:
                    return BiomeType.Volcano;
                case 20:
                    return BiomeType.Sinkhole;
                default:
                    return BiomeType.Plains;
            }
        }

        private static BiomeModifier RollBiomeModifier()
        {
            int roll = DiceSystem.Roll1d20();
            
            if (csvLoader == null)
            {
                return GetBiomeModifierFromRoll(roll);
            }
            
            var modifierData = csvLoader.GetRowByRoll("biome_modifiers", roll);
            if (modifierData != null && modifierData.ContainsKey("Modifier"))
            {
                string modifierStr = modifierData["Modifier"];
                if (Enum.TryParse<BiomeModifier>(modifierStr, out BiomeModifier modifier))
                {
                    return modifier;
                }
            }
            
            return GetBiomeModifierFromRoll(roll);
        }

        private static BiomeModifier GetBiomeModifierFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return BiomeModifier.Impassable;
                case 2:
                case 3:
                case 4:
                    return BiomeModifier.Fertile;
                case 5:
                case 6:
                    return BiomeModifier.Desolate;
                case 7:
                case 8:
                    return BiomeModifier.Flooded;
                case 9:
                case 10:
                    return BiomeModifier.Dangerous;
                case 11:
                case 12:
                    return BiomeModifier.SafeHaven;
                case 13:
                case 14:
                    return BiomeModifier.Historical;
                case 15:
                    return BiomeModifier.Elemental;
                case 16:
                    return BiomeModifier.FaeTouched;
                case 17:
                    return BiomeModifier.Demonic;
                case 18:
                    return BiomeModifier.Everdark;
                case 19:
                    return BiomeModifier.Shifting;
                case 20:
                    return BiomeModifier.Chaotic;
                default:
                    return BiomeModifier.None;
            }
        }

        private static WeatherType RollWeather()
        {
            int roll = DiceSystem.Roll1d8();
            
            if (csvLoader == null)
            {
                return GetWeatherFromRoll(roll);
            }
            
            var weatherData = csvLoader.GetRowByRoll("weather", roll);
            if (weatherData != null && weatherData.ContainsKey("Weather"))
            {
                string weatherStr = weatherData["Weather"];
                if (Enum.TryParse<WeatherType>(weatherStr, out WeatherType weather))
                {
                    return weather;
                }
            }
            
            return GetWeatherFromRoll(roll);
        }

        private static WeatherType GetWeatherFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                case 2:
                case 3:
                    return WeatherType.ClearSkies;
                case 4:
                    return WeatherType.CloudyFoggy;
                case 5:
                    return WeatherType.Hot;
                case 6:
                    return WeatherType.Raining;
                case 7:
                    return WeatherType.SevereStorm;
                case 8:
                    return WeatherType.ExtremeWeather;
                default:
                    return WeatherType.ClearSkies;
            }
        }

        private static EncounterType RollEncounter()
        {
            int roll = DiceSystem.Roll1d20();
            
            if (csvLoader == null)
            {
                return GetEncounterFromRoll(roll);
            }
            
            var encounterData = csvLoader.GetRowByRoll("encounters", roll);
            if (encounterData != null && encounterData.ContainsKey("EncounterType"))
            {
                string encounterStr = encounterData["EncounterType"];
                if (Enum.TryParse<EncounterType>(encounterStr, out EncounterType encounter))
                {
                    return encounter;
                }
            }
            
            return GetEncounterFromRoll(roll);
        }

        private static EncounterType GetEncounterFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return EncounterType.TwoCreatures;
                case 2:
                case 3:
                    return EncounterType.Creature;
                case 4:
                case 5:
                case 6:
                case 7:
                    return EncounterType.None;
                case 8:
                    return EncounterType.NPC;
                case 9:
                    return EncounterType.Event;
                case 10:
                    return EncounterType.Landmark;
                case 11:
                case 12:
                    return EncounterType.DungeonEntrance;
                case 13:
                    return EncounterType.Settlement;
                case 14:
                case 15:
                case 16:
                case 17:
                    return EncounterType.BiomeSpecificCreature;
                case 18:
                    return EncounterType.BiomeSpecificLandmark;
                case 19:
                    return EncounterType.BiomeSpecificNPC;
                case 20:
                    return EncounterType.BiomeSpecificEvent;
                default:
                    return EncounterType.None;
            }
        }

        private static void GenerateEncounterContent(Hex hex)
        {
            switch (hex.Encounter)
            {
                case EncounterType.Landmark:
                    hex.Landmark = RollGeneralLandmark();
                    break;
                case EncounterType.NPC:
                    hex.NPC = RollGeneralNPC();
                    break;
                case EncounterType.Event:
                    hex.Event = RollEvent();
                    break;
                case EncounterType.DungeonEntrance:
                    hex.HasDungeon = true;
                    break;
                case EncounterType.BiomeSpecificLandmark:
                    hex.Landmark = RollBiomeSpecificLandmark(hex.Biome);
                    break;
                case EncounterType.BiomeSpecificNPC:
                    hex.NPC = RollBiomeSpecificNPC(hex.Biome);
                    break;
                case EncounterType.Settlement:
                    hex.Notes.Add("Settlement discovered");
                    break;
                case EncounterType.Creature:
                case EncounterType.TwoCreatures:
                case EncounterType.BiomeSpecificCreature:
                    hex.Notes.Add("Creature encounter");
                    break;
                case EncounterType.BiomeSpecificEvent:
                    hex.Event = RollEvent();
                    break;
            }
        }

        private static LandmarkType RollGeneralLandmark()
        {
            int roll = DiceSystem.Roll1d8();
            
            if (csvLoader == null)
            {
                return GetGeneralLandmarkFromRoll(roll);
            }
            
            var landmarkData = csvLoader.GetRowByRoll("landmarks", roll);
            if (landmarkData != null && landmarkData.ContainsKey("LandmarkType"))
            {
                string landmarkStr = landmarkData["LandmarkType"];
                if (Enum.TryParse<LandmarkType>(landmarkStr, out LandmarkType landmark))
                {
                    return landmark;
                }
            }
            
            return GetGeneralLandmarkFromRoll(roll);
        }

        private static LandmarkType GetGeneralLandmarkFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return LandmarkType.BanditCamp;
                case 2:
                    return LandmarkType.Battlefield;
                case 3:
                    return LandmarkType.Chokepoint;
                case 4:
                    return LandmarkType.Campsite;
                case 5:
                    return LandmarkType.Inn;
                case 6:
                    return LandmarkType.Shrine;
                case 7:
                    return LandmarkType.DruidsGrove;
                case 8:
                    return LandmarkType.TeleportationCircle;
                default:
                    return LandmarkType.None;
            }
        }

        private static NPCType RollGeneralNPC()
        {
            int roll = DiceSystem.Roll1d6();
            
            if (csvLoader == null)
            {
                return GetGeneralNPCFromRoll(roll);
            }
            
            var npcData = csvLoader.GetRowByRoll("npcs", roll);
            if (npcData != null && npcData.ContainsKey("NPCType"))
            {
                string npcStr = npcData["NPCType"];
                if (Enum.TryParse<NPCType>(npcStr, out NPCType npc))
                {
                    return npc;
                }
            }
            
            return GetGeneralNPCFromRoll(roll);
        }

        private static NPCType GetGeneralNPCFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return NPCType.Thief;
                case 2:
                    return NPCType.WanderingMystic;
                case 3:
                    return NPCType.Explorer;
                case 4:
                    return NPCType.TravelingMerchant;
                case 5:
                    return NPCType.SomeoneWhoCouldHelp;
                case 6:
                    return NPCType.SomeoneInNeed;
                default:
                    return NPCType.None;
            }
        }

        private static EventType RollEvent()
        {
            int roll = DiceSystem.Roll1d10();
            
            if (csvLoader == null)
            {
                return GetEventFromRoll(roll);
            }
            
            var eventData = csvLoader.GetRowByRoll("events", roll);
            if (eventData != null && eventData.ContainsKey("EventType"))
            {
                string eventStr = eventData["EventType"];
                if (Enum.TryParse<EventType>(eventStr, out EventType eventType))
                {
                    return eventType;
                }
            }
            
            return GetEventFromRoll(roll);
        }

        private static EventType GetEventFromRoll(int roll)
        {
            switch (roll)
            {
                case 1:
                    return EventType.Lost;
                case 2:
                    return EventType.Illness;
                case 3:
                    return EventType.Scuffle;
                case 4:
                    return EventType.Breakthrough;
                case 5:
                    return EventType.Inspired;
                case 6:
                    return EventType.Vision;
                default:
                    return EventType.None;
            }
        }

        private static LandmarkType RollBiomeSpecificLandmark(BiomeType biome)
        {
            int roll = DiceSystem.Roll1d4();
            
            if (csvLoader == null)
            {
                return GetBiomeSpecificLandmarkFromRoll(biome, roll);
            }
            
            string tableName = $"{biome.ToString().ToLower()}_landmarks";
            var landmarkData = csvLoader.GetRowByRoll(tableName, roll);
            if (landmarkData != null && landmarkData.ContainsKey("LandmarkType"))
            {
                string landmarkStr = landmarkData["LandmarkType"];
                if (Enum.TryParse<LandmarkType>(landmarkStr, out LandmarkType landmark))
                {
                    return landmark;
                }
            }
            
            return GetBiomeSpecificLandmarkFromRoll(biome, roll);
        }

        private static LandmarkType GetBiomeSpecificLandmarkFromRoll(BiomeType biome, int roll)
        {
            switch (biome)
            {
                case BiomeType.Hills:
                    switch (roll)
                    {
                        case 1: return LandmarkType.FleshHill;
                        case 2: return LandmarkType.SphericalHill;
                        case 3: return LandmarkType.ValleyOfNight;
                        case 4: return LandmarkType.MysteriousTubing;
                    }
                    break;
                case BiomeType.Plains:
                    switch (roll)
                    {
                        case 1: return LandmarkType.ForbiddenRiver;
                        case 2: return LandmarkType.AbandonedCastle;
                        case 3: return LandmarkType.GlitteringMire;
                        case 4: return LandmarkType.Pearlhenge;
                    }
                    break;
                case BiomeType.Mountains:
                    switch (roll)
                    {
                        case 1: return LandmarkType.OtherworldlyRock;
                        case 2: return LandmarkType.TeeteringBoulders;
                        case 3: return LandmarkType.ImpossibleBoulder;
                        case 4: return LandmarkType.LabyrinthineRidge;
                    }
                    break;
                case BiomeType.Forest:
                    switch (roll)
                    {
                        case 1: return LandmarkType.CrumblingWell;
                        case 2: return LandmarkType.BloodyShrine;
                        case 3: return LandmarkType.WhisperingGrove;
                        case 4: return LandmarkType.InfinitelyTallTree;
                    }
                    break;
                case BiomeType.Desert:
                    switch (roll)
                    {
                        case 1: return LandmarkType.SandPit;
                        case 2: return LandmarkType.RestlessDunes;
                        case 3: return LandmarkType.SolarConvent;
                        case 4: return LandmarkType.BoilingOasis;
                    }
                    break;
                case BiomeType.Tundra:
                    switch (roll)
                    {
                        case 1: return LandmarkType.EchoingChapel;
                        case 2: return LandmarkType.CrystallineTheater;
                        case 3: return LandmarkType.ShiveringTrail;
                        case 4: return LandmarkType.RestlessGlacier;
                    }
                    break;
                case BiomeType.Canyon:
                    switch (roll)
                    {
                        case 1: return LandmarkType.ShakingGorge;
                        case 2: return LandmarkType.ChromaticCliffs;
                        case 3: return LandmarkType.ScaledCavern;
                        case 4: return LandmarkType.PillarsOfLife;
                    }
                    break;
                case BiomeType.Lake:
                    switch (roll)
                    {
                        case 1: return LandmarkType.SunkenCatacomb;
                        case 2: return LandmarkType.PrecariousBridges;
                        case 3: return LandmarkType.GuardedBarge;
                        case 4: return LandmarkType.WrithingVillage;
                    }
                    break;
            }
            
            return LandmarkType.None;
        }

        private static NPCType RollBiomeSpecificNPC(BiomeType biome)
        {
            int roll = DiceSystem.Roll1d4();
            
            if (csvLoader == null)
            {
                return GetBiomeSpecificNPCFromRoll(biome, roll);
            }
            
            string tableName = $"{biome.ToString().ToLower()}_npcs";
            var npcData = csvLoader.GetRowByRoll(tableName, roll);
            if (npcData != null && npcData.ContainsKey("NPCType"))
            {
                string npcStr = npcData["NPCType"];
                if (Enum.TryParse<NPCType>(npcStr, out NPCType npc))
                {
                    return npc;
                }
            }
            
            return GetBiomeSpecificNPCFromRoll(biome, roll);
        }

        private static NPCType GetBiomeSpecificNPCFromRoll(BiomeType biome, int roll)
        {
            switch (biome)
            {
                case BiomeType.Hills:
                    switch (roll)
                    {
                        case 1: return NPCType.WanderingHalfling;
                        case 2: return NPCType.WitchOnHill;
                        case 3: return NPCType.PolymorphedDruid;
                        case 4: return NPCType.PriestOfHills;
                    }
                    break;
                case BiomeType.Plains:
                    switch (roll)
                    {
                        case 1: return NPCType.CartographerOfFlatlands;
                        case 2: return NPCType.StrandedComposer;
                        case 3: return NPCType.ManMole;
                        case 4: return NPCType.PlainsPirates;
                    }
                    break;
                case BiomeType.Mountains:
                    switch (roll)
                    {
                        case 1: return NPCType.MountainStalker;
                        case 2: return NPCType.CliffsideMerchant;
                        case 3: return NPCType.GhostOfCrags;
                        case 4: return NPCType.ExiledPrince;
                    }
                    break;
            }
            
            return NPCType.None;
        }

        private static string DetermineDungeonType(BiomeType biome)
        {
            switch (biome)
            {
                case BiomeType.Volcano:
                    return "Elemental: Fire";
                case BiomeType.Sinkhole:
                    return "Underdark";
                case BiomeType.Lake:
                    return "Undead, Flooded";
                case BiomeType.Forest:
                    return "Amazing Flora, Flooded, Ruin";
                default:
                    return "Standard";
            }
        }

        // Generate a hex exploration log
        public static string GenerateExplorationLog(Hex hex)
        {
            string log = $"=== HEX EXPLORATION: {hex.Coordinate} ==={Environment.NewLine}{Environment.NewLine}";
            
            log += $"Rolling for hex content...{Environment.NewLine}";
            log += $"Biome: {hex.Biome}{Environment.NewLine}";
            log += $"Modifier: {hex.Modifier}{Environment.NewLine}";
            log += $"Weather: {hex.Weather}{Environment.NewLine}";
            log += $"Encounter: {hex.Encounter}";
            if (hex.ResolvedEncounter != null)
                log += $" — {hex.ResolvedEncounter.GetDescription()}";
            log += Environment.NewLine + Environment.NewLine;
            
            if (hex.Landmark != LandmarkType.None)
            {
                log += $"Landmark Discovered: {hex.Landmark}{Environment.NewLine}";
            }
            
            if (hex.NPC != NPCType.None)
            {
                log += $"NPC Encountered: {hex.NPC}{Environment.NewLine}";
            }
            
            if (hex.Event != EventType.None)
            {
                log += $"Event: {hex.Event}{Environment.NewLine}";
            }
            
            if (hex.HasDungeon)
            {
                log += $"Dungeon Entrance: {hex.DungeonType}{Environment.NewLine}";
            }
            
            log += $"{Environment.NewLine}Hex exploration complete!{Environment.NewLine}";
            
            return log;
        }
    }
} 
