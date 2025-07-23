using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DungeonSimTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DUNGEON SIMULATOR - CHARACTER CREATION TEST ===");
            Console.WriteLine("Testing the data-driven character creation system...\n");

            // Initialize the CSV data loader
            var csvLoader = new CSVDataLoader();
            csvLoader.LoadAllCSVFiles();

            // Initialize the data-driven system
            DataDrivenTableData.Initialize(csvLoader);

            // Create character creation system
            var characterCreationSystem = new CharacterCreationSystem();
            characterCreationSystem.csvDataLoader = csvLoader;

            Console.WriteLine("Press any key to create a character, or 'q' to quit...");
            
            while (true)
            {
                var key = Console.ReadKey(true);
                if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                    break;

                Console.Clear();
                Console.WriteLine("=== CREATING NEW CHARACTER ===\n");

                try
                {
                    // Create a character
                    Character character = characterCreationSystem.CreateCharacter();

                    // Display the character sheet
                    Console.WriteLine(character.GetCharacterSheet());

                    // Display creation log
                    Console.WriteLine("\n=== CREATION LOG ===");
                    List<string> creationLog = characterCreationSystem.GetCreationLog();
                    foreach (string log in creationLog)
                    {
                        Console.WriteLine(log);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating character: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }

                Console.WriteLine("\nPress any key to create another character, or 'q' to quit...");
            }

            Console.WriteLine("Test completed!");
        }
    }

    // Simplified CSV Data Loader for console testing
    public class CSVDataLoader
    {
        private Dictionary<string, CSVTableData> tableCache = new Dictionary<string, CSVTableData>();
        public List<CSVTableData> loadedTables = new List<CSVTableData>();

        public void LoadAllCSVFiles()
        {
            string csvFolderPath = "../game/csv";
            
            string[] csvFiles = {
                "races.csv", "classes.csv", "personality_traits.csv", "weapons.csv",
                "armor.csv", "items_tools.csv", "clothing_accessory.csv", "name_generator.csv",
                "spell_elements.csv", "spell_forms.csv", "spell_effects.csv", "spell_formula.csv",
                "heal_spells.csv", "special_attack_generator.csv"
            };

            foreach (string fileName in csvFiles)
            {
                LoadCSVFile(fileName, csvFolderPath);
            }

            Console.WriteLine($"Loaded {loadedTables.Count} CSV tables");
        }

        private void LoadCSVFile(string fileName, string csvFolderPath)
        {
            string filePath = Path.Combine(csvFolderPath, fileName);
            
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Warning: Could not find CSV file: {filePath}");
                return;
            }

            string csvContent = File.ReadAllText(filePath);
            CSVTableData tableData = ParseCSV(csvContent, fileName);
            
            if (tableData != null)
            {
                loadedTables.Add(tableData);
                tableCache[fileName] = tableData;
                Console.WriteLine($"Loaded {fileName} with {tableData.rows.Count} rows");
            }
        }

        private CSVTableData ParseCSV(string csvContent, string fileName)
        {
            CSVTableData tableData = new CSVTableData();
            tableData.tableName = Path.GetFileNameWithoutExtension(fileName);

            string[] lines = csvContent.Split('\n');
            if (lines.Length < 2)
            {
                Console.WriteLine($"Error: Invalid CSV file {fileName}: Not enough lines");
                return null;
            }

            // Parse header
            string[] headers = ParseCSVLine(lines[0]);

            // Parse data rows
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;

                string[] values = ParseCSVLine(lines[i]);
                Dictionary<string, string> row = new Dictionary<string, string>();

                for (int j = 0; j < Math.Min(headers.Length, values.Length); j++)
                {
                    row[headers[j]] = values[j];
                }

                tableData.rows.Add(row);
            }

            return tableData;
        }

        private string[] ParseCSVLine(string line)
        {
            List<string> values = new List<string>();
            bool inQuotes = false;
            string currentValue = "";

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.Trim());
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
            }

            values.Add(currentValue.Trim());
            return values.ToArray();
        }

        public CSVTableData GetTable(string tableName)
        {
            string fileName = $"{tableName}.csv";
            if (tableCache.ContainsKey(fileName))
            {
                return tableCache[fileName];
            }
            return null;
        }

        public Dictionary<string, string> GetRowByRoll(string tableName, int roll)
        {
            CSVTableData table = GetTable(tableName);
            if (table == null) return null;

            foreach (var row in table.rows)
            {
                if (row.ContainsKey("Roll (1d20)") || row.ContainsKey("Roll (1d12)") ||
                    row.ContainsKey("Roll (1d4)") || row.ContainsKey("Roll (1d6)"))
                {
                    string rollKey = row.Keys.FirstOrDefault(k => k.StartsWith("Roll"));
                    if (rollKey != null && ParseRollRange(row[rollKey], roll))
                    {
                        return row;
                    }
                }
            }

            return null;
        }

        private bool ParseRollRange(string rollRange, int roll)
        {
            if (string.IsNullOrEmpty(rollRange)) return false;

            // Handle single numbers
            if (int.TryParse(rollRange, out int singleRoll))
            {
                return roll == singleRoll;
            }

            // Handle ranges like "1-3"
            if (rollRange.Contains("-"))
            {
                string[] parts = rollRange.Split('-');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int min) &&
                    int.TryParse(parts[1], out int max))
                {
                    return roll >= min && roll <= max;
                }
            }

            return false;
        }

        public List<string> GetColumn(string tableName, string columnName)
        {
            List<string> column = new List<string>();
            CSVTableData table = GetTable(tableName);

            if (table != null)
            {
                foreach (var row in table.rows)
                {
                    if (row.ContainsKey(columnName))
                    {
                        column.Add(row[columnName]);
                    }
                }
            }

            return column;
        }
        
        public List<Dictionary<string, string>> GetAllRows(string tableName)
        {
            CSVTableData table = GetTable(tableName);
            if (table != null)
            {
                return new List<Dictionary<string, string>>(table.rows);
            }
            return new List<Dictionary<string, string>>();
        }
    }

    // Simplified CSV Table Data for console testing
    public class CSVTableData
    {
        public string tableName = "";
        public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
    }
} 