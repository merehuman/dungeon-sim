using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonSim
{
    /// <summary>
    /// Encounter representing a dungeon entrance found during hex exploration.
    /// Dungeon identity is Theme + Modifier (d20 each), independent of the hex.
    /// Once the player chooses to enter or not, the rest of the crawl is automated until exit; simulation then pauses until the player presses Explore Hex.
    /// </summary>
    public class DungeonEncounter : Encounter
    {
        /// <summary>Theme roll (d20) made when the dungeon is first encountered. Null until rolled.</summary>
        public int? ThemeRoll { get; private set; }

        /// <summary>Modifier roll (d20) made when the dungeon is first encountered. Null until rolled.</summary>
        public int? ModifierRoll { get; private set; }

        /// <summary>Whether the player has chosen to enter the dungeon. Null = not yet chosen.</summary>
        public bool? PlayerChoseToEnter { get; set; }

        public DungeonEncounter(Hex hex) : base(EncounterType.DungeonEntrance, hex) { }

        /// <summary>
        /// Per Dungeon Rules STEP 1: Roll Theme (d20) and Modifier (d20) when the party encounters the dungeon. Independent of the hex.
        /// </summary>
        public void RollThemeAndModifier()
        {
            ThemeRoll = DiceSystem.Roll1d20();
            ModifierRoll = DiceSystem.Roll1d20();
        }

        public override string GetDisplayName() => "Dungeon Entrance";

        public override string GetDescription()
        {
            var lines = new List<string> { "Dungeon Entrance" };
            if (ThemeRoll.HasValue)
                lines.Add($"Theme (d20): {ThemeRoll.Value}");
            if (ModifierRoll.HasValue)
                lines.Add($"Modifier (d20): {ModifierRoll.Value}");
            if (PlayerChoseToEnter.HasValue)
                lines.Add(PlayerChoseToEnter.Value ? "Party chose to enter." : "Party chose not to enter (or left).");
            return string.Join(" — ", lines);
        }

        /// <summary>
        /// Gold per room (Dungeon Rules: 10 gold per room).
        /// </summary>
        public const int GoldPerRoom = 10;

        /// <summary>
        /// Turns per hour for torch duration (Dungeon Rules: 6 turns = 1 hour).
        /// </summary>
        public const int TurnsPerHour = 6;

        /// <summary>
        /// Maximum rooms per crawl to avoid runaway loops.
        /// </summary>
        private const int MaxRoomsPerCrawl = 200;

        /// <summary>
        /// Dungeon Door Choice table: roll for current room with N doors. Returns 0 = Exit dungeon, 1..N = go through that door (1-based).
        /// </summary>
        private static int RollDoorChoice(int numDoors)
        {
            if (numDoors <= 0) return 0;
            int roll;
            int result;
            switch (numDoors)
            {
                case 1: roll = DiceSystem.Roll1d4(); result = roll == 1 ? 0 : 1; break;
                case 2: roll = DiceSystem.Roll1d6(); result = roll == 1 ? 0 : (roll <= 3 ? 1 : 2); break;
                case 3: roll = DiceSystem.Roll1d8(); result = roll == 1 ? 0 : (roll <= 3 ? 1 : (roll <= 5 ? 2 : 3)); break;
                case 4: roll = DiceSystem.Roll1d6(); result = roll == 1 ? 0 : (roll <= 4 ? roll - 1 : 4); break;
                default: roll = DiceSystem.RollDie(numDoors + 1); result = roll == 1 ? 0 : (roll - 1); break;
            }
            return result;
        }

        /// <summary>
        /// Backtrack table: when continuing from a dead end, pick which of K unused doors. Returns 1-based index 1..K.
        /// </summary>
        private static int RollWhichUnusedDoor(int unusedCount)
        {
            if (unusedCount <= 0) return 1;
            if (unusedCount == 1) return 1;
            return DiceSystem.RollDie(unusedCount);
        }

        /// <summary>
        /// Runs the full dungeon crawl: short rest, then room loop. Only a door-choice of EXIT or dead-end + leave (1d10 1-4) ends the crawl. At a dead end, 5-10 = backtrack through a previously unused door. Appends each line via <paramref name="appendLine"/>.
        /// </summary>
        public void RunAutomatedDungeonCrawl(Action<string> appendLine)
        {
            appendLine("Party takes a short rest, then enters the dungeon." + Environment.NewLine);

            int turn = 0;
            int totalGold = 0;
            var history = new List<RoomState>();
            var rng = new Random();
            bool exited = false;

            while (!exited && history.Count < MaxRoomsPerCrawl)
            {
                int roomType = DiceSystem.Roll3d6();
                int roomSize = DiceSystem.Roll1d12();
                int doors = DiceSystem.Roll1d12();
                int roomDetail = DiceSystem.Roll1d12();
                turn++;
                int roomNumber = history.Count + 1;

                appendLine($"--- Room {roomNumber} (Turn {turn}) ---");
                appendLine($"  Room Type (3d6): {roomType}, Size (d12): {roomSize}, Doors (d12): {doors}, Detail (d12): {roomDetail}");

                for (int d = 0; d < doors; d++)
                    appendLine($"  Door {d + 1} type (d20): {DiceSystem.Roll1d20()}");

                int encounterRoll = DiceSystem.Roll1d20();
                appendLine($"  Dungeon encounter (d20): {encounterRoll} — resolved.");
                totalGold += GoldPerRoom;
                appendLine($"  +{GoldPerRoom} gold (total this run: {totalGold})");

                if (doors == 0)
                {
                    appendLine("  Dead end (no exits).");
                    int deadEndRoll = DiceSystem.Roll1d10();
                    appendLine($"  Dead end roll (1d10): {deadEndRoll} — ");
                    if (deadEndRoll <= 4)
                    {
                        appendLine("  Leave dungeon." + Environment.NewLine);
                        exited = true;
                        break;
                    }
                    appendLine("  Continue exploring (use a previously unused door).");
                    var withUnused = history.Where(r => r.UsedDoors.Count < r.NumDoors).ToList();
                    if (withUnused.Count == 0)
                    {
                        appendLine("  No rooms with unused doors; party leaves." + Environment.NewLine);
                        exited = true;
                        break;
                    }
                    var roomToBacktrack = withUnused[rng.Next(withUnused.Count)];
                    var unusedIndices = Enumerable.Range(1, roomToBacktrack.NumDoors).Where(i => !roomToBacktrack.UsedDoors.Contains(i)).ToList();
                    int whichUnused = RollWhichUnusedDoor(unusedIndices.Count);
                    int doorToTake = unusedIndices[whichUnused - 1];
                    roomToBacktrack.UsedDoors.Add(doorToTake);
                    appendLine($"  Backtrack to room {roomToBacktrack.RoomNumber}, take unused door {doorToTake} (roll 1d{unusedIndices.Count} = {whichUnused})." + Environment.NewLine);
                    continue;
                }

                history.Add(new RoomState(roomNumber, doors, new HashSet<int>()));
                int choice = RollDoorChoice(doors);
                int sides = doors == 1 ? 4 : (doors == 2 ? 6 : (doors == 3 ? 8 : (doors == 4 ? 6 : doors + 1)));
                appendLine($"  Door choice (1d{sides}): ");
                if (choice == 0)
                {
                    appendLine("  Exit — party leaves the dungeon." + Environment.NewLine);
                    exited = true;
                    break;
                }
                history[history.Count - 1].UsedDoors.Add(choice);
                appendLine($"  Door {choice} — proceed to next room." + Environment.NewLine);
            }

            if (history.Count >= MaxRoomsPerCrawl && !exited)
                appendLine("(Max rooms reached; party leaves.)" + Environment.NewLine);

            appendLine("Party takes a full rest. Danger roll (1d20): " + DiceSystem.Roll1d20());
            appendLine("");
            appendLine("Exited dungeon. Press Explore Hex to continue." + Environment.NewLine);
        }

        private sealed class RoomState
        {
            public int RoomNumber { get; }
            public int NumDoors { get; }
            public HashSet<int> UsedDoors { get; } = new HashSet<int>();

            public RoomState(int roomNumber, int numDoors, HashSet<int> usedDoors)
            {
                RoomNumber = roomNumber;
                NumDoors = numDoors;
                if (usedDoors != null)
                    foreach (var d in usedDoors) UsedDoors.Add(d);
            }
        }
    }
}
