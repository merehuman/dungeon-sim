using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DungeonSim
{
    public class CSVDataLoader
    {
        private Dictionary<string, CSVTableData> tableCache = new Dictionary<string, CSVTableData>();
        public List<CSVTableData> loadedTables = new List<CSVTableData>();

        public void LoadAllCSVFiles()
        {
            // Resolve game/csv relative to exe (e.g. bin\Debug\net6.0-windows) so it works from any working directory
            string baseDir = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string csvFolderPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "game", "csv"));
            if (!Directory.Exists(csvFolderPath))
                csvFolderPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "game", "csv"));
            if (!Directory.Exists(csvFolderPath))
                csvFolderPath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "game", "csv"));

            string[] csvFiles = {
                "races.csv", "classes.csv", "personality_traits.csv", "weapons.csv",
                "armor.csv", "items_tools.csv", "clothing_accessory.csv", "name_generator.csv",
                "spell_elements.csv", "spell_forms.csv", "spell_effects.csv", "spell_formula.csv",
                "heal_spells.csv", "special_attack_generator.csv",
                "biomes.csv", "biome_modifiers.csv", "weather.csv", "encounters.csv",
                "landmarks.csv", "npcs.csv", "events.csv", "hills_landmarks.csv",
                "plains_landmarks.csv", "mountains_landmarks.csv", "forest_landmarks.csv",
                "desert_landmarks.csv", "tundra_landmarks.csv", "canyon_landmarks.csv",
                "lake_landmarks.csv", "hills_npcs.csv", "plains_npcs.csv", "mountains_npcs.csv"
            };

            foreach (string fileName in csvFiles)
            {
                LoadCSVFile(fileName, csvFolderPath);
            }
        }

        private void LoadCSVFile(string fileName, string csvFolderPath)
        {
            string filePath = Path.Combine(csvFolderPath, fileName);

            if (!File.Exists(filePath))
            {
                return;
            }

            string csvContent = File.ReadAllText(filePath);
            CSVTableData? tableData = ParseCSV(csvContent, fileName);

            if (tableData != null)
            {
                loadedTables.Add(tableData);
                tableCache[fileName] = tableData;
            }
        }

        private CSVTableData? ParseCSV(string csvContent, string fileName)
        {
            CSVTableData tableData = new CSVTableData();
            tableData.tableName = Path.GetFileNameWithoutExtension(fileName);

            string[] lines = csvContent.Split('\n');
            if (lines.Length < 2)
            {
                return null;
            }

            string[] headers = ParseCSVLine(lines[0]);

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

        public CSVTableData? GetTable(string tableName)
        {
            string fileName = $"{tableName}.csv";
            if (tableCache.ContainsKey(fileName))
            {
                return tableCache[fileName];
            }
            return null;
        }

        public Dictionary<string, string>? GetRowByRoll(string tableName, int roll)
        {
            CSVTableData? table = GetTable(tableName);
            if (table == null) return null;

            foreach (var row in table.rows)
            {
                if (row.ContainsKey("Roll (1d20)") || row.ContainsKey("Roll (1d12)") ||
                    row.ContainsKey("Roll (1d4)") || row.ContainsKey("Roll (1d6)"))
                {
                    string? rollKey = row.Keys.FirstOrDefault(k => k.StartsWith("Roll"));
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

            if (int.TryParse(rollRange, out int singleRoll))
            {
                return roll == singleRoll;
            }

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
            CSVTableData? table = GetTable(tableName);

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
            CSVTableData? table = GetTable(tableName);
            if (table != null)
            {
                return new List<Dictionary<string, string>>(table.rows);
            }
            return new List<Dictionary<string, string>>();
        }
    }

    public class CSVTableData
    {
        public string tableName = "";
        public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
    }
}
