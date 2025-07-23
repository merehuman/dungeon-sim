# Dungeon Simulator - Character Creation System

This Unity project implements a comprehensive **data-driven, object-oriented** character creation system for a tabletop roleplaying game simulation. Each character is a self-contained object that carries its own stats, equipment, and abilities. The system loads all character creation tables from CSV files, making it easy to modify game data without touching the code.

## Features

### Object-Oriented Character System
- **Character Class**: Each character is an instance of the `Character` class
- **Persistent Data**: Characters maintain all their stats, equipment, and abilities
- **Multiple Characters**: Create and store multiple characters simultaneously
- **Character Sheets**: Generate formatted character sheets for each character

### Data-Driven Design
- **CSV-Based Tables**: All character creation data is loaded from CSV files
- **No Code Changes**: Modify races, classes, equipment, spells, etc. by editing CSV files
- **Hot Reloading**: Reload CSV data during development without restarting
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
├── Assets/
│   └── Scripts/
│       ├── Character/
│       │   ├── Character.cs              # Main character class
│       │   ├── CharacterEnums.cs         # Enums for race, class, personality
│       │   └── Equipment.cs              # Equipment classes (Weapon, Armor, Item, etc.)
│       ├── Systems/
│       │   ├── DiceSystem.cs             # Dice rolling utilities
│       │   ├── CSVDataLoader.cs          # Loads and parses CSV files
│       │   ├── DataDrivenTableData.cs    # Data-driven table access system
│       │   └── CharacterCreationSystem.cs # Main character creation logic
│       ├── Setup/
│       │   └── GameSetup.cs              # Automatic game setup and configuration
│       ├── Test/
│       │   └── CharacterCreationTest.cs  # Test script for character creation
│       └── UI/
│           └── TerminalInterface.cs      # Terminal-style user interface
├── game/
│   ├── csv/                              # CSV data files (14 files)
│   │   ├── races.csv                      # Race definitions and effects
│   │   ├── classes.csv                    # Class definitions and effects
│   │   ├── personality_traits.csv         # Personality traits and bonuses
│   │   ├── weapons.csv                    # Weapon table with damage and traits
│   │   ├── armor.csv                      # Armor table with protection values
│   │   ├── items_tools.csv                # Items and tools with uses
│   │   ├── clothing_accessory.csv         # Clothing and accessories
│   │   ├── name_generator.csv             # Name generation parts and modifiers
│   │   ├── spell_elements.csv             # Spell element types
│   │   ├── spell_forms.csv                # Spell form types
│   │   ├── spell_effects.csv              # Spell effect types
│   │   ├── spell_formula.csv              # Spell formula patterns
│   │   ├── heal_spells.csv                # Healing spell definitions
│   │   └── special_attack_generator.csv   # Martial attack definitions
│   ├── Tabletop Materials/                # Original game design documents
│   │   ├── Design Doc.md
│   │   ├── Notes.md
│   │   ├── Rules/                         # Game rules and mechanics
│   │   └── Tables/                        # Original table definitions
│   └── project instructions.md            # Original project requirements
├── TestConsole/                           # Standalone console test application
│   ├── Program.cs                         # Main console application
│   ├── CharacterSystem.cs                 # Character classes (standalone version)
│   ├── GameSystems.cs                     # Game systems (standalone version)
│   ├── TestConsole.csproj                 # .NET project file
│   ├── run_test.bat                       # Windows batch file for easy execution
│   └── README.md                          # Console test documentation
└── README.md                              # This documentation
```

## Usage

### Basic Character Creation

```csharp
// Get the character creation system
CharacterCreationSystem creationSystem = FindObjectOfType<CharacterCreationCreationSystem>();

// Create a new character
Character newCharacter = creationSystem.CreateCharacter();

// Access character properties
Debug.Log($"Name: {newCharacter.characterName}");
Debug.Log($"Race: {newCharacter.race}");
Debug.Log($"Class: {newCharacter.characterClass}");
Debug.Log($"Stats: STR {newCharacter.strength}, DEX {newCharacter.dexterity}, WIS {newCharacter.wisdom}");

// Print character sheet
Debug.Log(newCharacter.GetCharacterSheet());
```

### Creating Multiple Characters

```csharp
List<Character> party = new List<Character>();

for (int i = 0; i < 4; i++)
{
    Character character = creationSystem.CreateCharacter();
    party.Add(character);
}

// Access any character
Character fighter = party[0];
Character wizard = party[1];
```

### Using the Terminal Interface

1. **Setup**: Add the `TerminalInterface` component to a GameObject in your scene
2. **UI Setup**: Connect the UI elements (Text for output, InputField for commands)
3. **Commands**:
   - `start` or `create` - Create a new character
   - `list` or `characters` - Show all created characters
   - `sheet` - Display character sheet(s)
   - `help` - Show available commands
   - `clear` - Clear terminal output
   - `quit` or `exit` - Exit the application

### Testing Without Unity

You can test the character creation system without Unity using the standalone console application:

```bash
cd TestConsole
dotnet run
```

Or use the provided batch file on Windows:
```bash
TestConsole/run_test.bat
```

This will:
- Load all CSV data files
- Create characters using the same data-driven system
- Display complete character sheets
- Show detailed creation logs with all dice rolls
- Allow you to create multiple characters interactively

**Benefits:**
✅ **No Unity Required** - Test the system without Unity installation  
✅ **Fast Testing** - Quick iteration and testing of character creation  
✅ **Data Validation** - Verify CSV files are loaded correctly  
✅ **Debugging** - See exactly what's happening during character creation  
✅ **Portable** - Can run on any system with .NET 6.0+  

### Using the Test System

```csharp
// Add CharacterCreationTest to a GameObject
CharacterCreationTest test = gameObject.AddComponent<CharacterCreationTest>();

// Create characters programmatically
test.CreateTestCharacters();

// Get all created characters
List<Character> characters = test.GetCreatedCharacters();

// Get a specific character
Character character = test.GetCharacter(0);
```

## Character Object Properties

### Basic Information
- `characterName`: Generated character name
- `race`: Character race (Human, Dwarf, Elf, etc.)
- `characterClass`: Character class (Fighter, Rogue, Wizard, Cleric)
- `personalityTrait`: Personality trait with stat bonuses

### Core Stats
- `strength`: Physical strength (3-18)
- `dexterity`: Agility and reflexes (3-18)
- `wisdom`: Mental acuity and perception (3-18)

### Combat Stats
- `maxHealth`: Maximum hit points
- `currentHealth`: Current hit points
- `armorValue`: Total armor protection
- `level`: Character level (starts at 1)
- `experience`: Experience points

### Resources
- `gold`: Starting gold pieces
- `torches`: Number of torches (starts with 1 + bonus)
- `rations`: Number of rations (starts with 1 + bonus)

### Equipment
- `equippedWeapon`: Primary weapon
- `equippedArmor`: List of worn armor pieces
- `inventory`: List of carried items

### Abilities
- `spells`: List of known spells (Wizards/Clerics)
- `martialAttacks`: Special combat abilities (Fighters)
- `healSpells`: Healing abilities (Clerics)

### Special Abilities
- `specialAbilities`: General special abilities
- `racialAbilities`: Race-specific abilities
- `classAbilities`: Class-specific abilities

## Extending the System

### Modifying Game Data (No Code Changes!)
Simply edit the CSV files to modify the game:

**Adding/Modifying Races:**
- Edit `races.csv` - add new rows or modify existing ones
- The system automatically loads the new data

**Adding/Modifying Classes:**
- Edit `classes.csv` - add new classes or change effects
- No code changes needed

**Adding/Modifying Equipment:**
- Edit `weapons.csv`, `armor.csv`, `items_tools.csv`, `clothing_accessory.csv`
- Add new items, change damage values, modify traits

**Adding/Modifying Spells:**
- Edit `spell_elements.csv`, `spell_forms.csv`, `spell_effects.csv`
- Add new spell components or modify existing ones

### Adding New CSV Tables
1. Create a new CSV file in the `game/csv/` folder
2. Add the filename to the `csvFiles` array in `CSVDataLoader.cs`
3. Add access methods in `DataDrivenTableData.cs`

### Code Extensions
If you need to add new functionality:

**Adding New Races:**
1. Add the race to `CharacterRace` enum
2. Add racial bonuses in `Character.ApplyRacialBonuses()`

**Adding New Classes:**
1. Add the class to `CharacterClass` enum
2. Add class bonuses in `Character.ApplyStatBonuses()`
3. Add class abilities in `CharacterCreationSystem.RollClassAbilities()`

**Adding New Equipment Types:**
1. Create new equipment classes in `Equipment.cs`
2. Add loading methods in `DataDrivenTableData.cs`
3. Update the character creation system to use them

## Unity Setup

### Quick Setup (Recommended)
1. **Create a new Unity project** (2022.3 LTS or later recommended)
2. **Import the scripts** into your Assets/Scripts folder
3. **Copy CSV files** from `game/csv/` to `Assets/game/csv/` (or create a Resources folder)
4. **Add GameSetup component** to any GameObject in your scene
5. **Press Play** - the system will auto-setup everything!
6. **Type "start"** in the console to create your first character

### Manual Setup
1. **Create a new Unity project** (2022.3 LTS or later recommended)
2. **Import the scripts** into your Assets/Scripts folder
3. **Copy CSV files** from `game/csv/` to `Assets/game/csv/`
4. **Create a scene** with the following GameObjects:
   - CSVDataLoader (add the component)
   - CharacterCreationSystem (add the component)
   - TerminalInterface (add the component and UI elements)
   - CharacterCreationTest (optional, for testing)
5. **Set up UI** (if using terminal interface):
   - Canvas with Text component for output
   - InputField for commands
   - ScrollRect for scrolling
6. **Connect references** in the inspector
7. **Press Play** and type "start" to create your first character!

### CSV File Setup
The system looks for CSV files in the following locations:
- **Editor**: `Assets/game/csv/` (relative to project root)
- **Built Game**: `Resources/game/csv/` (for distribution)

To use your existing CSV files:
1. Copy them to `Assets/game/csv/` folder
2. Or create a `Resources` folder and place them in `Resources/game/csv/`

## Example Output

```
=== STARTING CHARACTER CREATION ===

--- ROLLING STATS ---
Rolled 3d6 for Strength: 14
Rolled 3d6 for Dexterity: 12
Rolled 3d6 for Wisdom: 16

--- ROLLING RACE ---
Rolled 1d20 for Race: 7
Race determined: Elf

--- ROLLING CLASS ---
Rolled 1d4 for Class: 3
Class determined: Wizard

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
```

This system provides a solid foundation for a tabletop RPG simulation, with each character being a complete, self-contained object that can be easily extended and modified. 