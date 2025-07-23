using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class CSVTableData
{
    public string tableName;
    public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
}

public class CSVDataLoader : MonoBehaviour
{
    [Header("CSV File Paths")]
    public string csvFolderPath = "game/csv";
    
    [Header("Loaded Data")]
    public List<CSVTableData> loadedTables = new List<CSVTableData>();
    
    // Cached data for quick access
    private Dictionary<string, CSVTableData> tableCache = new Dictionary<string, CSVTableData>();
    
    void Awake()
    {
        LoadAllCSVFiles();
    }
    
    public void LoadAllCSVFiles()
    {
        tableCache.Clear();
        loadedTables.Clear();
        
        // Define the CSV files to load
        string[] csvFiles = {
            "races.csv",
            "classes.csv", 
            "personality_traits.csv",
            "weapons.csv",
            "armor.csv",
            "items_tools.csv",
            "clothing_accessory.csv",
            "name_generator.csv",
            "spell_elements.csv",
            "spell_forms.csv",
            "spell_effects.csv",
            "spell_formula.csv",
            "heal_spells.csv",
            "special_attack_generator.csv"
        };
        
        foreach (string fileName in csvFiles)
        {
            LoadCSVFile(fileName);
        }
        
        Debug.Log($"Loaded {loadedTables.Count} CSV tables");
    }
    
    private void LoadCSVFile(string fileName)
    {
        string filePath = Path.Combine(csvFolderPath, fileName);
        
        // Try to load from Resources first (for built game)
        TextAsset textAsset = Resources.Load<TextAsset>(filePath);
        string csvContent = "";
        
        if (textAsset != null)
        {
            csvContent = textAsset.text;
        }
        else
        {
            // Try to load from file system (for editor)
            string fullPath = Path.Combine(Application.dataPath, filePath);
            if (File.Exists(fullPath))
            {
                csvContent = File.ReadAllText(fullPath);
            }
            else
            {
                Debug.LogWarning($"Could not load CSV file: {filePath}");
                return;
            }
        }
        
        CSVTableData tableData = ParseCSV(csvContent, fileName);
        if (tableData != null)
        {
            loadedTables.Add(tableData);
            tableCache[fileName] = tableData;
            Debug.Log($"Loaded {fileName} with {tableData.rows.Count} rows");
        }
    }
    
    private CSVTableData ParseCSV(string csvContent, string fileName)
    {
        CSVTableData tableData = new CSVTableData();
        tableData.tableName = Path.GetFileNameWithoutExtension(fileName);
        
        string[] lines = csvContent.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError($"Invalid CSV file {fileName}: Not enough lines");
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
    
    // Get a specific table by name
    public CSVTableData GetTable(string tableName)
    {
        string fileName = $"{tableName}.csv";
        if (tableCache.ContainsKey(fileName))
        {
            return tableCache[fileName];
        }
        return null;
    }
    
    // Get a specific row from a table by roll value
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
    
    // Parse roll ranges like "1-3", "4-6", etc.
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
    
    // Get all rows from a table
    public List<Dictionary<string, string>> GetAllRows(string tableName)
    {
        CSVTableData table = GetTable(tableName);
        return table?.rows ?? new List<Dictionary<string, string>>();
    }
    
    // Get a specific column from a table
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
    
    // Reload all CSV files (useful for hot reloading in editor)
    [ContextMenu("Reload CSV Files")]
    public void ReloadCSVFiles()
    {
        LoadAllCSVFiles();
    }
    
    // Get a random row from a table
    public Dictionary<string, string> GetRandomRow(string tableName)
    {
        CSVTableData table = GetTable(tableName);
        if (table == null || table.rows.Count == 0) return null;
        
        int randomIndex = UnityEngine.Random.Range(0, table.rows.Count);
        return table.rows[randomIndex];
    }
} 