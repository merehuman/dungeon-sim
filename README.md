# dungeon-sim - Console Application

A **pure .NET console application** that simulates a tabletop roleplaying game with comprehensive character creation. Each character is a self-contained object that carries its own stats, equipment, and abilities. The system loads all character creation tables from CSV files, making it easy to modify game data without touching the code.

## Features

### Object-Oriented Character System
- **Character Class**: Each character is an instance of the `Character` class
- **Persistent Data**: Characters maintain all their stats, equipment, and abilities
- **Multiple Characters**: Create and store multiple characters simultaneously
- **Character Sheets**: Generate formatted character sheets for each character

### Data-Driven Design
- **CSV-Based Tables**: All character creation data is loaded from CSV files
- **No Code Changes**: Modify races, classes, equipment, spells, etc. by editing CSV files
- **Flexible Structure**: Easy to add new tables, modify existing ones, or change game balance

### Complete Character Creation Process
The system follows the exact character creation procedure from the rules:

1. **Roll Stats** (3d6 for Strength, Dexterity, Wisdom)
2. **Roll Race** (1d20 with special handling for Elemental/Undead)
3. **Roll Class** (1d4 for Fighter, Rogue, Wizard, Cleric)
4. **Roll Hit Points** (based on race hit dice)
5. **Roll Personality Trait** (1d12)
6. **Roll Class Abilities** (Martial Attacks, Spells, Heal Spells)
7. **Roll Starting Resources** (+2 torches or +2 rations)
8. **Roll Equipment** (Weapon, Item/Tool, Clothing, Armor)
9. **Calculate Armor Value**
10. **Roll Starting Gold** (3d6)
11. **Apply Stat Bonuses** (from race, class, personality)
12. **Generate Character Name** (complex name generation system)

### Dice Rolling System
- **Flexible Dice**: Support for any dice combination (1d4, 1d6, 1d8, 1d10, 1d12, 1d20, 3d6, etc.)
- **Table Rolling**: Roll on weighted tables for various outcomes
- **Random Selection**: Get random elements from arrays and lists

### Equipment System
- **Weapons**: Complete weapon table with damage dice and traits
- **Armor**: Armor table with protection values and special properties
- **Items/Tools**: 20 different items with values and uses
- **Clothing/Accessories**: 12 clothing items with special effects

### Spell System
- **Dynamic Spell Generation**: Create spells using Element + Form + Effect combinations
- **Heal Spells**: Specialized healing spells with different targets and effects
- **Martial Attacks**: Special combat abilities for fighters

## File Structure

```
dungeon-sim/
├── Program.cs                              # Main console application entry point
├── CharacterSystem.cs                      # Character classes and equipment
├── GameSystems.cs                          # Game systems (dice, data loading, creation)
├── DungeonSim.csproj                       # .NET project file
├── run.bat                                 # Windows batch file for easy execution
├── clean.bat                               # Cleanup script for build artifacts
├── README.md                               # This documentation
└── game/
    ├── csv/                                # CSV data files (14 files)
    │   ├── races.csv                       # Race definitions and effects
    │   ├── classes.csv                     # Class definitions and effects
    │   ├── personality_traits.csv          # Personality traits and bonuses
    │   ├── weapons.csv                     # Weapon table with damage and traits
    │   ├── armor.csv                       # Armor table with protection values
    │   ├── items_tools.csv                 # Items and tools with uses
    │   ├── clothing_accessory.csv          # Clothing and accessories
    │   ├── name_generator.csv              # Name generation parts and modifiers
    │   ├── spell_elements.csv              # Spell element types
    │   ├── spell_forms.csv                 # Spell form types
    │   ├── spell_effects.csv               # Spell effect types
    │   ├── spell_formula.csv               # Spell formula patterns
    │   ├── heal_spells.csv                 # Healing spell definitions
    │   └── special_attack_generator.csv    # Martial attack definitions
    ├── Tabletop Materials/                  # Original game design documents
    │   ├── Design Doc.md
    │   ├── Notes.md
    │   ├── Rules/                          # Game rules and mechanics
    │   └── Tables/                         # Original table definitions
    └── project instructions.md             # Original project requirements
```

## Requirements

- **.NET 6.0** or later
- **Windows, macOS, or Linux** (cross-platform)

## Quick Start

### Windows
1. **Run the application**: Double-click `run.bat` or run `dotnet run` in the terminal
2. **Clean build artifacts**: Run `clean.bat` to remove temporary files

### Command Line
```bash
# Build and run
dotnet run

# Build only
dotnet build

# Clean build artifacts
dotnet clean
```

## Usage

When you run the application, it will:
1. Load all CSV data files
2. Create a character using the 12-step process
3. Display the character sheet
4. Show the creation log with all dice rolls and decisions
5. Wait for you to press Enter to create another character

### Example Output
```
=== DUNGEON SIMULATOR - CHARACTER CREATION TEST ===
Testing the data-driven character creation system...

Loaded races.csv with 8 rows
Loaded classes.csv with 4 rows
Loaded personality_traits.csv with 12 rows
Loaded weapons.csv with 12 rows
Loaded armor.csv with 12 rows
Loaded items_tools.csv with 20 rows
Loaded clothing_accessory.csv with 12 rows
Loaded name_generator.csv with 20 rows
Loaded spell_elements.csv with 12 rows
=== CREATING NEW CHARACTER ===

=== STARTING CHARACTER CREATION ===

--- ROLLING STATS ---
Rolled 3d6 for Strength: 13
Rolled 3d6 for Dexterity: 7
Rolled 3d6 for Wisdom: 8

--- ROLLING RACE ---
Rolled 1d20 for Race: 4
Race determined: Dwarf

--- ROLLING CLASS ---
Rolled 1d4 for Class: 2
Class determined: Fighter

--- ROLLING HIT POINTS ---
Rolled 1d8 for Hit Points: 7
Hit Points: 7

--- ROLLING PERSONALITY TRAIT ---
Rolled 1d12 for Personality Trait: 1
Personality Trait: Friendly

--- ROLLING CLASS ABILITIES ---
Rolled 3d12 for Martial Attack: 14
Martial Attack: Sweep - 1d4 damage to Body

--- ROLLING STARTING RESOURCES ---
Rolled 1d2 for Extra Resources: 1
Chose +2 extra torches

--- ROLLING EQUIPMENT ---
Rolled 1d12 for Weapon: 9
Weapon: Great-sword (1d8)
Rolled 1d20 for Item/Tool: 9
Item: Net - Restrain enemies
Rolled 1d12 for Clothing/Accessory: 8
Clothing: Cloak/Cape - Hides identity
Rolled 1d12 for Armor: 8
Armor: Shield (+0)

--- ROLLING STARTING GOLD ---
Rolled 3d6 for Starting Gold: 12
Starting Gold: 12g

--- GENERATING CHARACTER NAME ---
Rolled 1d20 for Name First Part: 19
Rolled 1d20 for Name Second Part: 14
Rolled 1d20 for Name Modifier: 20
Character Name: Tyte

=== CHARACTER CREATION COMPLETE ===

=== CHARACTER SHEET ===
Name: Tyte
Race: Dwarf
Class: Fighter
Level: 1
Personality: Friendly

=== STATS ===
Strength: 14
Dexterity: 7
Wisdom: 9

=== COMBAT ===
Health: 9/9
Armor: 1

=== EQUIPMENT ===
Weapon: Great-sword (1d8)
Armor: Shield (+0)

=== INVENTORY ===
- Net (Value: 5g)
- Cloak/Cape (Value: 10g)

=== RESOURCES ===
Gold: 12g
Torches: 3
Rations: 1

=== MARTIAL ATTACKS ===
- Sweep: 1d4 damage to Body

=== RACIAL ABILITIES ===
- Immune to poison

=== CLASS ABILITIES ===
- Martial Attack
- +1 armor at start of game

=== CREATION LOG ===
[... detailed creation log with all dice rolls and decisions ...]

Press any key to create another character, or 'q' to quit...
```

## Data-Driven Customization

### Modifying Game Data
All game data is stored in CSV files in the `game/csv/` folder. You can modify these files without changing any code:

- **`races.csv`**: Add new races, modify stat bonuses, change hit dice
- **`classes.csv`**: Add new classes, modify abilities, change starting equipment
- **`weapons.csv`**: Add new weapons, modify damage dice, change special traits
- **`spell_*.csv`**: Add new spell elements, forms, or effects
- **`name_generator.csv`**: Add new name parts for character generation

### CSV File Format
Each CSV file follows a consistent format:
- **Headers**: Column names (e.g., "Roll", "Name", "Description", "Effect")
- **Data Rows**: Game data with roll ranges and outcomes
- **Roll Ranges**: Use "1-5", "6-10", "11-15" format for weighted tables

### Example CSV Structure
```csv
Roll,Name,Description,Effect
1-5,Human,Adaptable and balanced,+1 to all stats
6-10,Elf,Graceful and magical,+2 Dexterity, +1 Wisdom
11-15,Dwarf,Sturdy and strong,+2 Strength, +1 Constitution
```

## Development

### Project Structure
- **`Program.cs`**: Main application entry point and user interface
- **`CharacterSystem.cs`**: Character class, enums, and equipment definitions
- **`GameSystems.cs`**: Dice system, data loading, and character creation logic

### Adding New Features
1. **New Equipment Types**: Add classes to `CharacterSystem.cs`
2. **New Dice Rolls**: Add methods to `DiceSystem` in `GameSystems.cs`
3. **New Tables**: Create CSV files and add loading logic to `CSVDataLoader`
4. **New Character Properties**: Extend the `Character` class in `CharacterSystem.cs`

### Building and Testing
```bash
# Build the project
dotnet build

# Run with debugging
dotnet run

# Clean build artifacts
dotnet clean
```

## Troubleshooting

### Common Issues
1. **CSV files not found**: Ensure `game/csv/` folder exists with all CSV files
2. **Build errors**: Run `dotnet clean` then `dotnet build`
3. **Runtime errors**: Check that all CSV files have proper headers and data

### Performance
- The application loads all CSV data at startup
- Character creation is fast and efficient
- Memory usage is minimal for typical usage

## License

This project is for educational and personal use. The game mechanics and rules are based on the original tabletop materials in the `game/Tabletop Materials/` folder. 