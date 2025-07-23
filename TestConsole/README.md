# Dungeon Simulator - Console Test

This is a standalone console application that tests the character creation system without requiring Unity.

## What It Does

- Loads all CSV data files from `game/csv/`
- Creates characters using the same data-driven system as Unity
- Displays complete character sheets
- Shows detailed creation logs with all dice rolls
- Allows you to create multiple characters interactively

## Requirements

- .NET 6.0 or later
- CSV files in `game/csv/` folder (relative to the test console)

## How to Run

### Option 1: Using the batch file (Windows)
```bash
run_test.bat
```

### Option 2: Using dotnet commands
```bash
dotnet build
dotnet run
```

### Option 3: Using Visual Studio
1. Open `TestConsole.csproj` in Visual Studio
2. Press F5 to run

## Usage

1. **Start the application** - It will load all CSV files and show what was loaded
2. **Press any key** to create a character
3. **View the character sheet** - Complete character information will be displayed
4. **View creation log** - See all dice rolls and decisions made during creation
5. **Press any key again** to create another character
6. **Press 'q'** to quit

## Example Output

```
=== DUNGEON SIMULATOR - CHARACTER CREATION TEST ===
Testing the data-driven character creation system...

Loaded races.csv with 8 rows
Loaded classes.csv with 4 rows
Loaded personality_traits.csv with 12 rows
...
Loaded 14 CSV tables

Press any key to create a character, or 'q' to quit...

=== CREATING NEW CHARACTER ===

=== STARTING CHARACTER CREATION ===

--- ROLLING STATS ---
Rolled 3d6 for Strength: 14
Rolled 3d6 for Dexterity: 12
Rolled 3d6 for Wisdom: 16

--- ROLLING RACE ---
Rolled 1d20 for Race: 7
Race determined: Elf

=== CHARACTER SHEET ===
Name: GishBinzo
Race: Elf
Class: Wizard
Level: 1
Personality: OpenMinded

=== STATS ===
Strength: 14
Dexterity: 12
Wisdom: 18

=== COMBAT ===
Health: 6/6
Armor: 1

=== EQUIPMENT ===
Weapon: Staff (1d6)
Armor: Bracers (+1)

=== INVENTORY ===
- Lantern (Value: 50g)
- Cloak/Cape (Value: 20g)

=== RESOURCES ===
Gold: 12g
Torches: 1
Rations: 3

=== SPELLS ===
- Crystal Bolt (2 casts/day)
- Draining Wall (2 casts/day)

=== RACIAL ABILITIES ===
- Cannot get tired

=== CLASS ABILITIES ===
- 2 spell casts at level 1
```

## Files

- `Program.cs` - Main console application
- `CharacterSystem.cs` - Character classes and enums (standalone version)
- `GameSystems.cs` - Dice system, data-driven tables, and character creation (standalone version)
- `TestConsole.csproj` - .NET project file
- `run_test.bat` - Windows batch file for easy execution

## Benefits

✅ **No Unity Required** - Test the system without Unity installation  
✅ **Fast Testing** - Quick iteration and testing of character creation  
✅ **Data Validation** - Verify CSV files are loaded correctly  
✅ **Debugging** - See exactly what's happening during character creation  
✅ **Portable** - Can run on any system with .NET 6.0+  

## Troubleshooting

**"Could not find CSV file" errors:**
- Make sure the `game/csv/` folder exists relative to the TestConsole folder
- Check that all 14 CSV files are present

**Build errors:**
- Ensure you have .NET 6.0 or later installed
- Run `dotnet --version` to check your .NET version

**Runtime errors:**
- Check that CSV files are properly formatted
- Look for missing or malformed data in the CSV files 