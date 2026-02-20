using System;

namespace DungeonSim
{
    /// <summary>
    /// Base class for all hex encounters. When a player rolls for a hex encounter,
    /// the result is represented by an Encounter (or a derived type such as DungeonEncounter).
    /// </summary>
    public abstract class Encounter
    {
        public EncounterType EncounterType { get; protected set; }
        public Hex? SourceHex { get; protected set; }

        protected Encounter(EncounterType encounterType, Hex? sourceHex = null)
        {
            EncounterType = encounterType;
            SourceHex = sourceHex;
        }

        /// <summary>
        /// Short display name for this encounter (e.g. "Dungeon Entrance", "Creature").
        /// </summary>
        public abstract string GetDisplayName();

        /// <summary>
        /// Full description for logs and UI. Override in derived classes for encounter-specific details.
        /// </summary>
        public virtual string GetDescription()
        {
            return GetDisplayName();
        }

        /// <summary>
        /// Creates the appropriate Encounter instance from a hex's rolled encounter.
        /// </summary>
        public static Encounter CreateFromHex(Hex hex)
        {
            return hex.Encounter switch
            {
                EncounterType.None => new NoneEncounter(hex),
                EncounterType.Creature => new CreatureEncounter(hex),
                EncounterType.TwoCreatures => new TwoCreaturesEncounter(hex),
                EncounterType.NPC => new NPCEncounter(hex),
                EncounterType.Event => new EventEncounter(hex),
                EncounterType.Landmark => new LandmarkEncounter(hex),
                EncounterType.DungeonEntrance => new DungeonEncounter(hex),
                EncounterType.Settlement => new SettlementEncounter(hex),
                EncounterType.BiomeSpecificCreature => new BiomeSpecificCreatureEncounter(hex),
                EncounterType.BiomeSpecificLandmark => new BiomeSpecificLandmarkEncounter(hex),
                EncounterType.BiomeSpecificNPC => new BiomeSpecificNPCEncounter(hex),
                EncounterType.BiomeSpecificEvent => new BiomeSpecificEventEncounter(hex),
                _ => new NoneEncounter(hex)
            };
        }
    }

    // --- Stub encounter types (to be fleshed out later) ---

    public class NoneEncounter : Encounter
    {
        public NoneEncounter(Hex? hex = null) : base(EncounterType.None, hex) { }
        public override string GetDisplayName() => "None";
    }

    public class CreatureEncounter : Encounter
    {
        public CreatureEncounter(Hex hex) : base(EncounterType.Creature, hex) { }
        public override string GetDisplayName() => "Creature";
        public override string GetDescription() => "A creature encounter. (To be fleshed out.)";
    }

    public class TwoCreaturesEncounter : Encounter
    {
        public TwoCreaturesEncounter(Hex hex) : base(EncounterType.TwoCreatures, hex) { }
        public override string GetDisplayName() => "2x Creature";
        public override string GetDescription() => "Two creatures encountered. (To be fleshed out.)";
    }

    public class NPCEncounter : Encounter
    {
        public NPCEncounter(Hex hex) : base(EncounterType.NPC, hex) { }
        public override string GetDisplayName() => "NPC";
        public override string GetDescription() => $"NPC: {SourceHex?.NPC ?? NPCType.None}. (To be fleshed out.)";
    }

    public class EventEncounter : Encounter
    {
        public EventEncounter(Hex hex) : base(EncounterType.Event, hex) { }
        public override string GetDisplayName() => "Event";
        public override string GetDescription() => $"Event: {SourceHex?.Event ?? EventType.None}. (To be fleshed out.)";
    }

    public class LandmarkEncounter : Encounter
    {
        public LandmarkEncounter(Hex hex) : base(EncounterType.Landmark, hex) { }
        public override string GetDisplayName() => "Landmark";
        public override string GetDescription() => $"Landmark: {SourceHex?.Landmark ?? LandmarkType.None}. (To be fleshed out.)";
    }

    public class SettlementEncounter : Encounter
    {
        public SettlementEncounter(Hex hex) : base(EncounterType.Settlement, hex) { }
        public override string GetDisplayName() => "Settlement";
        public override string GetDescription() => "A settlement discovered. (To be fleshed out.)";
    }

    public class BiomeSpecificCreatureEncounter : Encounter
    {
        public BiomeSpecificCreatureEncounter(Hex hex) : base(EncounterType.BiomeSpecificCreature, hex) { }
        public override string GetDisplayName() => "Biome Specific Creature";
        public override string GetDescription() => "Biome-specific creature encounter. (To be fleshed out.)";
    }

    public class BiomeSpecificLandmarkEncounter : Encounter
    {
        public BiomeSpecificLandmarkEncounter(Hex hex) : base(EncounterType.BiomeSpecificLandmark, hex) { }
        public override string GetDisplayName() => "Biome Specific Landmark";
        public override string GetDescription() => $"Biome-specific landmark in {SourceHex?.Biome}. (To be fleshed out.)";
    }

    public class BiomeSpecificNPCEncounter : Encounter
    {
        public BiomeSpecificNPCEncounter(Hex hex) : base(EncounterType.BiomeSpecificNPC, hex) { }
        public override string GetDisplayName() => "Biome Specific NPC";
        public override string GetDescription() => $"Biome-specific NPC in {SourceHex?.Biome}. (To be fleshed out.)";
    }

    public class BiomeSpecificEventEncounter : Encounter
    {
        public BiomeSpecificEventEncounter(Hex hex) : base(EncounterType.BiomeSpecificEvent, hex) { }
        public override string GetDisplayName() => "Biome Specific Event";
        public override string GetDescription() => "Biome-specific event. (To be fleshed out.)";
    }
}
