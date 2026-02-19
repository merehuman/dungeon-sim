using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonSim
{
    // Hex coordinate system using axial coordinates
    public struct HexCoordinate
    {
        public int q; // Column (x-axis)
        public int r; // Row (y-axis)

        public HexCoordinate(int q, int r)
        {
            this.q = q;
            this.r = r;
        }

        public override string ToString()
        {
            return $"({q}, {r})";
        }

        public override bool Equals(object? obj)
        {
            if (obj is HexCoordinate other)
            {
                return q == other.q && r == other.r;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(q, r);
        }

        public static bool operator ==(HexCoordinate left, HexCoordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HexCoordinate left, HexCoordinate right)
        {
            return !left.Equals(right);
        }
    }

    // Biome types
    public enum BiomeType
    {
        SameAsPrevious,
        Hills,
        Plains,
        Mountains,
        Forest,
        Desert,
        Tundra,
        Canyon,
        Lake,
        Volcano,
        Sinkhole
    }

    // Biome modifiers
    public enum BiomeModifier
    {
        None,
        Impassable,
        Fertile,
        Desolate,
        Flooded,
        Dangerous,
        SafeHaven,
        Historical,
        Elemental,
        FaeTouched,
        Demonic,
        Everdark,
        Shifting,
        Chaotic
    }

    // Weather types
    public enum WeatherType
    {
        ClearSkies,
        CloudyFoggy,
        Hot,
        Raining,
        SevereStorm,
        ExtremeWeather
    }

    // Encounter types
    public enum EncounterType
    {
        None,
        Creature,
        TwoCreatures,
        NPC,
        Event,
        Landmark,
        DungeonEntrance,
        Settlement,
        BiomeSpecificCreature,
        BiomeSpecificLandmark,
        BiomeSpecificNPC,
        BiomeSpecificEvent
    }

    // Landmark types
    public enum LandmarkType
    {
        None,
        BanditCamp,
        Battlefield,
        Chokepoint,
        Campsite,
        Inn,
        Shrine,
        DruidsGrove,
        TeleportationCircle,
        FleshHill,
        SphericalHill,
        ValleyOfNight,
        MysteriousTubing,
        ForbiddenRiver,
        AbandonedCastle,
        GlitteringMire,
        Pearlhenge,
        OtherworldlyRock,
        TeeteringBoulders,
        ImpossibleBoulder,
        LabyrinthineRidge,
        CrumblingWell,
        BloodyShrine,
        WhisperingGrove,
        InfinitelyTallTree,
        SandPit,
        RestlessDunes,
        SolarConvent,
        BoilingOasis,
        EchoingChapel,
        CrystallineTheater,
        ShiveringTrail,
        RestlessGlacier,
        ShakingGorge,
        ChromaticCliffs,
        ScaledCavern,
        PillarsOfLife,
        SunkenCatacomb,
        PrecariousBridges,
        GuardedBarge,
        WrithingVillage
    }

    // NPC types
    public enum NPCType
    {
        None,
        Thief,
        WanderingMystic,
        Explorer,
        TravelingMerchant,
        SomeoneWhoCouldHelp,
        SomeoneInNeed,
        WanderingHalfling,
        WitchOnHill,
        PolymorphedDruid,
        PriestOfHills,
        CartographerOfFlatlands,
        StrandedComposer,
        ManMole,
        PlainsPirates,
        MountainStalker,
        CliffsideMerchant,
        GhostOfCrags,
        ExiledPrince
    }

    // Event types
    public enum EventType
    {
        None,
        Lost,
        Illness,
        Scuffle,
        Breakthrough,
        Inspired,
        Vision
    }

    public class Hex
    {
        public HexCoordinate Coordinate { get; set; }
        public BiomeType Biome { get; set; }
        public BiomeModifier Modifier { get; set; }
        public WeatherType Weather { get; set; }
        public EncounterType Encounter { get; set; }
        public LandmarkType Landmark { get; set; }
        public NPCType NPC { get; set; }
        public EventType Event { get; set; }
        public bool IsExplored { get; set; }
        public bool IsCapital { get; set; }
        public bool HasDungeon { get; set; }
        public string DungeonType { get; set; } = "";
        public List<string> DiscoveredLandmarks { get; set; } = new List<string>();
        public List<string> Notes { get; set; } = new List<string>();

        public Hex(HexCoordinate coordinate)
        {
            Coordinate = coordinate;
            Biome = BiomeType.Plains;
            Modifier = BiomeModifier.None;
            Weather = WeatherType.ClearSkies;
            Encounter = EncounterType.None;
            Landmark = LandmarkType.None;
            NPC = NPCType.None;
            Event = EventType.None;
            IsExplored = false;
            IsCapital = false;
            HasDungeon = false;
        }

        public string GetDescription()
        {
            string desc = $"=== HEX {Coordinate} ==={Environment.NewLine}";
            desc += $"Biome: {Biome}{Environment.NewLine}";
            desc += $"Modifier: {Modifier}{Environment.NewLine}";
            desc += $"Weather: {Weather}{Environment.NewLine}";
            desc += $"Encounter: {Encounter}{Environment.NewLine}";

            if (Landmark != LandmarkType.None)
                desc += $"Landmark: {Landmark}{Environment.NewLine}";

            if (NPC != NPCType.None)
                desc += $"NPC: {NPC}{Environment.NewLine}";

            if (Event != EventType.None)
                desc += $"Event: {Event}{Environment.NewLine}";

            if (IsCapital)
                desc += $"Special: CAPITAL{Environment.NewLine}";

            if (HasDungeon)
                desc += $"Dungeon: {DungeonType}{Environment.NewLine}";

            if (Notes.Count > 0)
            {
                desc += $"Notes:{Environment.NewLine}";
                foreach (string note in Notes)
                {
                    desc += $"  - {note}{Environment.NewLine}";
                }
            }

            return desc;
        }

        public string GetFullDescription()
        {
            string desc = $"=== HEX {Coordinate} === + Environment.NewLine";
            desc += $"Biome: {Biome} + Environment.NewLine";
            desc += $"Modifier: {Modifier} + Environment.NewLine";
            desc += $"Weather: {Weather} + Environment.NewLine";
            desc += $"Encounter: {Encounter} + Environment.NewLine";
            if (Landmark != LandmarkType.None)
                desc += $"Landmark: {Landmark} + Environment.NewLine";
            if (NPC != NPCType.None)
                desc += $"NPC: {NPC} + Environment.NewLine";
            if (Event != EventType.None)
                desc += $"Event: {Event} + Environment.NewLine";
            if (IsCapital)
                desc += "Special: CAPITAL + Environment.NewLine";
            if (HasDungeon)
                desc += $"Dungeon: {DungeonType} + Environment.NewLine";
            if (Notes.Count > 0)
            {
                desc += "Notes: + Environment.NewLine";
                foreach (string note in Notes)
                    desc += $"  - {note} + Environment.NewLine";
            }
            return desc;
        }
    }

    public class HexMap
    {
        private Dictionary<HexCoordinate, Hex> hexes = new Dictionary<HexCoordinate, Hex>();
        public HexCoordinate CapitalLocation { get; set; }
        public HexCoordinate CurrentPartyLocation { get; set; }
        public List<HexCoordinate> ExploredHexes { get; set; } = new List<HexCoordinate>();

        public HexMap()
        {
            CapitalLocation = new HexCoordinate(0, 0);
            CurrentPartyLocation = CapitalLocation;

            Hex capitalHex = new Hex(CapitalLocation)
            {
                IsCapital = true,
                IsExplored = true,
                Biome = BiomeType.Plains,
                Modifier = BiomeModifier.SafeHaven
            };
            hexes[CapitalLocation] = capitalHex;
            ExploredHexes.Add(CapitalLocation);
        }

        public Hex GetHex(HexCoordinate coordinate)
        {
            if (hexes.ContainsKey(coordinate))
                return hexes[coordinate];

            Hex newHex = new Hex(coordinate);
            hexes[coordinate] = newHex;
            return newHex;
        }

        public void SetHex(HexCoordinate coordinate, Hex hex)
        {
            hexes[coordinate] = hex;
        }

        public List<HexCoordinate> GetAdjacentHexes(HexCoordinate center)
        {
            List<HexCoordinate> adjacent = new List<HexCoordinate>();

            var directions = new[]
            {
                new HexCoordinate(1, 0),
                new HexCoordinate(1, -1),
                new HexCoordinate(0, -1),
                new HexCoordinate(-1, 0),
                new HexCoordinate(-1, 1),
                new HexCoordinate(0, 1)
            };

            foreach (var direction in directions)
            {
                adjacent.Add(new HexCoordinate(
                    center.q + direction.q,
                    center.r + direction.r
                ));
            }

            return adjacent;
        }

        public int GetDistance(HexCoordinate from, HexCoordinate to)
        {
            return (Math.Abs(from.q - to.q) + Math.Abs(from.q + from.r - to.q - to.r) + Math.Abs(from.r - to.r)) / 2;
        }

        public List<HexCoordinate> GetHexesInRange(HexCoordinate center, int range)
        {
            List<HexCoordinate> hexesInRange = new List<HexCoordinate>();

            for (int q = -range; q <= range; q++)
            {
                int r1 = Math.Max(-range, -q - range);
                int r2 = Math.Min(range, -q + range);

                for (int r = r1; r <= r2; r++)
                {
                    HexCoordinate coord = new HexCoordinate(center.q + q, center.r + r);
                    hexesInRange.Add(coord);
                }
            }

            return hexesInRange;
        }

        public void ExploreHex(HexCoordinate coordinate)
        {
            Hex hex = GetHex(coordinate);
            hex.IsExplored = true;

            if (!ExploredHexes.Contains(coordinate))
                ExploredHexes.Add(coordinate);
        }

        public void MoveParty(HexCoordinate newLocation)
        {
            CurrentPartyLocation = newLocation;
            ExploreHex(newLocation);
        }

        public string GetMapDescription(int maxDistance = 3)
        {
            string desc = $"=== HEX MAP ==={Environment.NewLine}{Environment.NewLine}";

            for (int distance = 0; distance <= maxDistance; distance++)
            {
                var hexesInRange = GetHexesInRange(CapitalLocation, distance);

                foreach (var coord in hexesInRange)
                {
                    Hex hex = GetHex(coord);
                    string status = hex.IsExplored ? "EXPLORED" : "UNEXPLORED";
                    string current = (coord == CurrentPartyLocation) ? " [CURRENT]" : "";
                    string capital = (coord == CapitalLocation) ? " [CAPITAL]" : "";

                    desc += $"Distance {distance}: {coord} - {status}{current}{capital}{Environment.NewLine}";

                    if (hex.IsExplored)
                    {
                        desc += $"  {hex.GetDescription()}{Environment.NewLine}";
                    }

                    desc += Environment.NewLine;
                }
            }

            return desc;
        }

        public List<Hex> GetAllExploredHexes()
        {
            return ExploredHexes.Select(coord => GetHex(coord)).ToList();
        }

        public int GetTotalExploredHexes()
        {
            return ExploredHexes.Count;
        }
    }
}
