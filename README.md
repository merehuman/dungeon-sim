# dungeon-sim - Windows Forms Application

A **pure .NET Windows Forms application** that simulates a tabletop roleplaying game with comprehensive character creation, character management, and hex-based exploration. Each character is a self-contained object that carries its own stats, equipment, and abilities. The system loads all game data from CSV files, making it easy to modify game data without touching the code.

## Features

### Object-Oriented Character System
- **Character Class**: Each character is an instance of the `Character` class
- **Persistent Data**: Characters maintain all their stats, equipment, and abilities
- **Multiple Characters**: Create and store multiple characters simultaneously
- **Character Management**: View all characters, compare sheets, and track current character
- **Character Sheets**: Generate formatted character sheets with proper line breaks

### Character Management System
- **Character List**: View all created characters with names, races, classes, and levels
- **Current Character Tracking**: See which character is currently active
- **All Character Display**: View all character sheets at once for easy comparison
- **Character Selection**: Browse character list and see current character status

### Hex-Based Exploration System
- **Hex Map**: Object-oriented hex coordinate system with axial coordinates
- **Dynamic Generation**: Hexes are generated using data-driven tables
- **Exploration Log**: Detailed logs of hex exploration with dice rolls and outcomes
- **Map Display**: View current map with explored and unexplored hexes

### Data-Driven Design
- **CSV-Based Tables**: All character creation and hex data is loaded from CSV files
- **No Code Changes**: Modify races, classes, equipment, spells, biomes, landmarks, etc. by editing CSV files
- **Flexible Structure**: Easy to add new tables, modify existing ones, or change game balance

### User Interface
- **Windows Forms**: Clean, modern interface with black background and lime green text
- **Button-Based Navigation**: Easy-to-use buttons for all major functions
- **Proper Line Breaks**: All text displays with correct line breaks using `Environment.NewLine`
- **Thread-Safe Updates**: UI updates are properly handled for multi-threading

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

### Hex Generation System
- **Biome Types**: 10 different biomes (Hills, Plains, Mountains, Forest, Desert, Tundra, Canyon, Lake, Volcano, Sinkhole)
- **Biome Modifiers**: 13 modifiers (Fertile, Dangerous, Historical, Elemental, etc.)
- **Weather System**: 6 weather types with different effects
- **Encounter Types**: 12 encounter types including creatures, NPCs, landmarks, and events
- **Landmarks**: General landmarks and biome-specific landmarks
- **NPCs**: General NPCs and biome-specific NPCs

## File Structure

```
dungeon-sim/
├── Program.cs                              # Main Windows Forms application entry point
├── CharacterSystem.cs                      # Character classes and equipment
├── GameSystems.cs                          # Game systems (dice, data loading, creation)
├── HexSystem.cs                            # Hex coordinate system and map management
├── HexGenerationSystem.cs                  # Hex generation using data-driven tables
├── DungeonSim.csproj                       # .NET project file
├── run.bat                                 # Windows batch file for easy execution
├── clean.bat                               # Cleanup script for build artifacts
├── README.md                               # This documentation
└── game/
    ├── csv/                                # CSV data files (35+ files)
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
    │   ├── special_attack_generator.csv    # Martial attack definitions
    │   ├── biomes.csv                      # Biome types and effects
    │   ├── biome_modifiers.csv             # Biome modifiers
    │   ├── weather.csv                     # Weather conditions
    │   ├── encounters.csv                  # Encounter types
    │   ├── landmarks.csv                   # General landmarks
    │   ├── npcs.csv                        # General NPCs
    │   ├── events.csv                      # Random events
    │   ├── hills_landmarks.csv             # Hills-specific landmarks
    │   ├── plains_landmarks.csv            # Plains-specific landmarks
    │   ├── mountains_landmarks.csv         # Mountains-specific landmarks
    │   ├── forest_landmarks.csv            # Forest-specific landmarks
    │   ├── desert_landmarks.csv            # Desert-specific landmarks
    │   ├── tundra_landmarks.csv            # Tundra-specific landmarks
    │   ├── canyon_landmarks.csv            # Canyon-specific landmarks
    │   ├── lake_landmarks.csv              # Lake-specific landmarks
    │   ├── hills_npcs.csv                  # Hills-specific NPCs
    │   ├── plains_npcs.csv                 # Plains-specific NPCs
    │   └── mountains_npcs.csv              # Mountains-specific NPCs
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
2. Display a Windows Forms interface with buttons for different actions
3. Allow you to create characters, manage character lists, explore hexes, and view maps
4. Show detailed logs of all dice rolls and decisions

### Available Actions
- **Create Character**: Generate a new character using the 12-step process
- **Show All Characters**: Display all created character sheets in sequence
- **Select Character**: View the character list and current character status
- **Show Creation Log**: View the detailed creation process with all dice rolls
- **Explore Hex**: Generate a new hex using data-driven tables
- **Show Map**: Display the current hex map with exploration status
- **Clear Output**: Clear the text display area

### Character Management
The application supports multiple characters with the following features:

#### Character List Display
```
==========================================
         CHARACTER LIST
==========================================

1. GarBinzo - Human Fighter (Level 1) [CURRENT]
2. Tyte - Dwarf Fighter (Level 1)
3. Elara - Elf Wizard (Level 1)

Showing all character sheets:

--- CHARACTER 1 ---
=== CHARACTER SHEET ===
Name: GarBinzo
Race: Human
Class: Fighter
Level: 1
Personality: Friendly

=== STATS ===
Strength: 14
Dexterity: 12
Wisdom: 10

[... full character sheet with proper line breaks ...]
```

#### Character Selection
- View numbered list of all created characters
- See current character indicator
- Track character status and information

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

### Hex Exploration Examples

#### Example 1: Plains Hex
```
=== HEX EXPLORATION: (1, 0) ===

Rolling for hex content...
Biome: Plains
Modifier: Fertile
Weather: ClearSkies
Encounter: Landmark

Landmark Discovered: Campsite

Hex exploration complete!
```

#### Example 2: Mountain Hex with Dungeon
```
=== HEX EXPLORATION: (0, 1) ===

Rolling for hex content...
Biome: Mountains
Modifier: Dangerous
Weather: CloudyFoggy
Encounter: DungeonEntrance

Dungeon Entrance: Standard

Hex exploration complete!
```

#### Example 3: Forest Hex with Biome-Specific Content
```
=== HEX EXPLORATION: (-1, 0) ===

Rolling for hex content...
Biome: Forest
Modifier: SafeHaven
Weather: Raining
Encounter: BiomeSpecificLandmark

Landmark Discovered: WhisperingGrove

Hex exploration complete!
```

### Map Display Example
```
=== HEX MAP ===

Distance 0: (0, 0) - EXPLORED [CURRENT] [CAPITAL]
  Hex (0, 0): Plains (SafeHaven) [CAPITAL]

Distance 1: (1, 0) - EXPLORED
  Hex (1, 0): Plains (Fertile) - ClearSkies [LANDMARK: Campsite]

Distance 1: (0, 1) - EXPLORED
  Hex (0, 1): Mountains (Dangerous) - CloudyFoggy [DUNGEON: Standard]

Distance 1: (-1, 0) - EXPLORED
  Hex (-1, 0): Forest (SafeHaven) - Raining [LANDMARK: WhisperingGrove]

Distance 1: (0, -1) - UNEXPLORED

Distance 1: (1, -1) - UNEXPLORED

Distance 1: (-1, 1) - UNEXPLORED

Total Explored Hexes: 4
Current Location: (0, 0)
Capital Location: (0, 0)
```

## Data-Driven Customization

### Modifying Game Data
All game data is stored in CSV files in the `game/csv/` folder. You can modify these files without changing any code:

- **`races.csv`**: Add new races, modify stat bonuses, change hit dice
- **`classes.csv`**: Add new classes, modify abilities, change starting equipment
- **`weapons.csv`**: Add new weapons, modify damage dice, change special traits
- **`spell_*.csv`**: Add new spell elements, forms, or effects
- **`name_generator.csv`**: Add new name parts for character generation
- **`biomes.csv`**: Add new biomes, modify biome effects
- **`biome_modifiers.csv`**: Add new modifiers, change modifier effects
- **`weather.csv`**: Add new weather types, modify weather effects
- **`encounters.csv`**: Add new encounter types
- **`landmarks.csv`**: Add new general landmarks
- **`npcs.csv`**: Add new general NPCs
- **`events.csv`**: Add new random events
- **`*_landmarks.csv`**: Add biome-specific landmarks
- **`*_npcs.csv`**: Add biome-specific NPCs

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
- **`HexSystem.cs`**: Hex coordinate system and map management
- **`HexGenerationSystem.cs`**: Hex generation using data-driven tables

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