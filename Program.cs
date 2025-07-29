using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DungeonSim
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameWindow());
        }
    }

    public partial class GameWindow : Form
    {
        private CSVDataLoader? csvDataLoader;
        private CharacterCreationSystem? characterCreationSystem;
        private List<Character> createdCharacters = new List<Character>();
        private Character? currentCharacter;
        private HexMap? hexMap; // Added hex map

        public GameWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // Initialize CSV data loader
            csvDataLoader = new CSVDataLoader();
            csvDataLoader.LoadAllCSVFiles();

            // Initialize character creation system
            characterCreationSystem = new CharacterCreationSystem();
            characterCreationSystem.csvDataLoader = csvDataLoader;

            // Initialize data-driven table system
            DataDrivenTableData.Initialize(csvDataLoader);

            // Initialize hex generation system
            HexGenerationSystem.Initialize(csvDataLoader);

            // Initialize hex map
            hexMap = new HexMap();

            // Display welcome message
            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         DUNGEON-SIM" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Welcome to the tabletop RPG simulation system!" + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Click 'Create Character' to generate a new character." + Environment.NewLine);
            AppendToOutput("Click 'Show All Characters' to view all character sheets." + Environment.NewLine);
            AppendToOutput("Click 'Select Character' to see the character list." + Environment.NewLine);
            AppendToOutput("Click 'Show Creation Log' to see the creation process." + Environment.NewLine);
            AppendToOutput("Click 'Explore Hex' to explore a new hex." + Environment.NewLine);
            AppendToOutput("Click 'Show Map' to view the current map." + Environment.NewLine);
            AppendToOutput("Click 'Clear Output' to clear the display." + Environment.NewLine + Environment.NewLine);
        }

        private void btnCreateCharacter_Click(object? sender, EventArgs e)
        {
            try
            {
                if (characterCreationSystem == null)
                {
                    AppendToOutput("Error: Character creation system not initialized." + Environment.NewLine + Environment.NewLine);
                    return;
                }

                AppendToOutput("==========================================" + Environment.NewLine);
                AppendToOutput("         CREATING NEW CHARACTER" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
                
                // Create character
                currentCharacter = characterCreationSystem.CreateCharacter();
                createdCharacters.Add(currentCharacter);

                // Display character sheet
                DisplayCharacterSheet(currentCharacter);

                AppendToOutput("Character created successfully!" + Environment.NewLine);
                AppendToOutput("Click 'Show Character Sheet' to view details." + Environment.NewLine);
                AppendToOutput("Click 'Create Character' to create another character." + Environment.NewLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                AppendToOutput($"Error creating character: {ex.Message}" + Environment.NewLine + Environment.NewLine);
            }
        }

        private void btnShowCharacterSheet_Click(object? sender, EventArgs e)
        {
            if (createdCharacters.Count == 0)
            {
                AppendToOutput("No characters have been created yet." + Environment.NewLine);
                AppendToOutput("Click 'Create Character' first." + Environment.NewLine + Environment.NewLine);
                return;
            }

            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         CHARACTER LIST" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

            // Display list of characters
            for (int i = 0; i < createdCharacters.Count; i++)
            {
                Character character = createdCharacters[i];
                string currentIndicator = (character == currentCharacter) ? " [CURRENT]" : "";
                AppendToOutput($"{i + 1}. {character.characterName} - {character.race} {character.characterClass} (Level {character.level}){currentIndicator}{Environment.NewLine}");
            }

            AppendToOutput(Environment.NewLine);
            AppendToOutput("Showing all character sheets:" + Environment.NewLine + Environment.NewLine);

            // Show all character sheets
            for (int i = 0; i < createdCharacters.Count; i++)
            {
                Character character = createdCharacters[i];
                AppendToOutput($"--- CHARACTER {i + 1} ---{Environment.NewLine}");
                DisplayCharacterSheet(character);
                AppendToOutput(Environment.NewLine);
            }
        }

        private void btnSelectCharacter_Click(object? sender, EventArgs e)
        {
            if (createdCharacters.Count == 0)
            {
                AppendToOutput("No characters have been created yet." + Environment.NewLine);
                AppendToOutput("Click 'Create Character' first." + Environment.NewLine + Environment.NewLine);
                return;
            }

            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         SELECT CHARACTER" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

            // Display list of characters
            for (int i = 0; i < createdCharacters.Count; i++)
            {
                Character character = createdCharacters[i];
                string currentIndicator = (character == currentCharacter) ? " [CURRENT]" : "";
                AppendToOutput($"{i + 1}. {character.characterName} - {character.race} {character.characterClass} (Level {character.level}){currentIndicator}{Environment.NewLine}");
            }

            AppendToOutput(Environment.NewLine);
            AppendToOutput("Current character is: " + (currentCharacter?.characterName ?? "None") + Environment.NewLine);
            AppendToOutput("(To change current character, create a new one or use the 'Create Character' button)" + Environment.NewLine + Environment.NewLine);
        }

        private void btnShowCreationLog_Click(object? sender, EventArgs e)
        {
            if (currentCharacter != null && characterCreationSystem != null)
            {
                AppendToOutput("==========================================" + Environment.NewLine);
                AppendToOutput("         CREATION LOG" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
                foreach (string logEntry in characterCreationSystem.GetCreationLog())
                {
                    AppendToOutput(logEntry);
                }
                AppendToOutput(Environment.NewLine);
            }
            else
            {
                AppendToOutput("No character has been created yet." + Environment.NewLine);
                AppendToOutput("Click 'Create Character' first." + Environment.NewLine + Environment.NewLine);
            }
        }

        private void btnClearOutput_Click(object? sender, EventArgs e)
        {
            txtOutput?.Clear();
            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         DUNGEON-SIM" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Output cleared. Ready for new commands." + Environment.NewLine + Environment.NewLine);
        }

        private void btnExploreHex_Click(object? sender, EventArgs e)
        {
            try
            {
                if (hexMap == null)
                {
                    AppendToOutput("Error: Hex map not initialized." + Environment.NewLine + Environment.NewLine);
                    return;
                }

                AppendToOutput("==========================================" + Environment.NewLine);
                AppendToOutput("         EXPLORING NEW HEX" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

                // Get adjacent hexes to current location
                var adjacentHexes = hexMap.GetAdjacentHexes(hexMap.CurrentPartyLocation);
                
                // Find an unexplored adjacent hex
                HexCoordinate? targetHex = null;
                foreach (var coord in adjacentHexes)
                {
                    Hex hex = hexMap.GetHex(coord);
                    if (!hex.IsExplored)
                    {
                        targetHex = coord;
                        break;
                    }
                }

                if (targetHex == null)
                {
                    // If no adjacent unexplored hex, create a new one in a random direction
                    var randomDirection = adjacentHexes[new Random().Next(adjacentHexes.Count)];
                    targetHex = randomDirection;
                }

                // Generate the hex content
                Hex newHex = HexGenerationSystem.GenerateHex(targetHex.Value, hexMap);
                hexMap.SetHex(targetHex.Value, newHex);
                hexMap.MoveParty(targetHex.Value);

                // Display exploration log
                string explorationLog = HexGenerationSystem.GenerateExplorationLog(newHex);
                AppendToOutput(explorationLog);

                AppendToOutput("Hex exploration complete!" + Environment.NewLine);
                AppendToOutput("Click 'Show Map' to view the updated map." + Environment.NewLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                AppendToOutput($"Error exploring hex: {ex.Message}" + Environment.NewLine + Environment.NewLine);
            }
        }

        private void btnShowMap_Click(object? sender, EventArgs e)
        {
            if (hexMap != null)
            {
                AppendToOutput("==========================================" + Environment.NewLine);
                AppendToOutput("         HEX MAP" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
                
                string mapDescription = hexMap.GetMapDescription(3);
                AppendToOutput(mapDescription);
                
                AppendToOutput($"Total Explored Hexes: {hexMap.GetTotalExploredHexes()}" + Environment.NewLine);
                AppendToOutput($"Current Location: {hexMap.CurrentPartyLocation}" + Environment.NewLine);
                AppendToOutput($"Capital Location: {hexMap.CapitalLocation}" + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                AppendToOutput("Error: Hex map not initialized." + Environment.NewLine + Environment.NewLine);
            }
        }

        private void DisplayCharacterSheet(Character character)
        {
            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         CHARACTER SHEET" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
            AppendToOutput(character.GetCharacterSheet());
            AppendToOutput("\n");
        }

        private void AppendToOutput(string text)
        {
            if (txtOutput?.InvokeRequired == true)
            {
                txtOutput.Invoke(new Action(() => AppendToOutput(text)));
            }
            else
            {
                txtOutput?.AppendText(text);
                txtOutput?.ScrollToCaret();
            }
        }

        // Windows Forms Designer generated code
        private void InitializeComponent()
        {
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnCreateCharacter = new System.Windows.Forms.Button();
            this.btnShowCharacterSheet = new System.Windows.Forms.Button();
            this.btnShowCreationLog = new System.Windows.Forms.Button();
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.btnExploreHex = new System.Windows.Forms.Button();
            this.btnShowMap = new System.Windows.Forms.Button();
            this.btnSelectCharacter = new System.Windows.Forms.Button(); // Added new button
            this.SuspendLayout();
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.BackColor = System.Drawing.Color.Black;
            this.txtOutput.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtOutput.ForeColor = System.Drawing.Color.Lime;
            this.txtOutput.Location = new System.Drawing.Point(12, 12);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOutput.Size = new System.Drawing.Size(776, 400);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // btnCreateCharacter
            // 
            this.btnCreateCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateCharacter.BackColor = System.Drawing.Color.Black;
            this.btnCreateCharacter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreateCharacter.ForeColor = System.Drawing.Color.Lime;
            this.btnCreateCharacter.Location = new System.Drawing.Point(12, 418);
            this.btnCreateCharacter.Name = "btnCreateCharacter";
            this.btnCreateCharacter.Size = new System.Drawing.Size(120, 30);
            this.btnCreateCharacter.TabIndex = 1;
            this.btnCreateCharacter.Text = "Create Character";
            this.btnCreateCharacter.UseVisualStyleBackColor = false;
            this.btnCreateCharacter.Click += new System.EventHandler(this.btnCreateCharacter_Click);
            // 
            // btnShowCharacterSheet
            // 
            this.btnShowCharacterSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowCharacterSheet.BackColor = System.Drawing.Color.Black;
            this.btnShowCharacterSheet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCharacterSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowCharacterSheet.ForeColor = System.Drawing.Color.Lime;
            this.btnShowCharacterSheet.Location = new System.Drawing.Point(138, 418);
            this.btnShowCharacterSheet.Name = "btnShowCharacterSheet";
            this.btnShowCharacterSheet.Size = new System.Drawing.Size(140, 30);
            this.btnShowCharacterSheet.TabIndex = 2;
            this.btnShowCharacterSheet.Text = "Show All Characters";
            this.btnShowCharacterSheet.UseVisualStyleBackColor = false;
            this.btnShowCharacterSheet.Click += new System.EventHandler(this.btnShowCharacterSheet_Click);
            // 
            // btnShowCreationLog
            // 
            this.btnShowCreationLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowCreationLog.BackColor = System.Drawing.Color.Black;
            this.btnShowCreationLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCreationLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowCreationLog.ForeColor = System.Drawing.Color.Lime;
            this.btnShowCreationLog.Location = new System.Drawing.Point(284, 418);
            this.btnShowCreationLog.Name = "btnShowCreationLog";
            this.btnShowCreationLog.Size = new System.Drawing.Size(120, 30);
            this.btnShowCreationLog.TabIndex = 3;
            this.btnShowCreationLog.Text = "Show Creation Log";
            this.btnShowCreationLog.UseVisualStyleBackColor = false;
            this.btnShowCreationLog.Click += new System.EventHandler(this.btnShowCreationLog_Click);
            // 
            // btnClearOutput
            // 
            this.btnClearOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearOutput.BackColor = System.Drawing.Color.Black;
            this.btnClearOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClearOutput.ForeColor = System.Drawing.Color.Lime;
            this.btnClearOutput.Location = new System.Drawing.Point(668, 454);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(120, 30);
            this.btnClearOutput.TabIndex = 4;
            this.btnClearOutput.Text = "Clear Output";
            this.btnClearOutput.UseVisualStyleBackColor = false;
            this.btnClearOutput.Click += new System.EventHandler(this.btnClearOutput_Click);
            // 
            // btnExploreHex
            // 
            this.btnExploreHex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExploreHex.BackColor = System.Drawing.Color.Black;
            this.btnExploreHex.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExploreHex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExploreHex.ForeColor = System.Drawing.Color.Lime;
            this.btnExploreHex.Location = new System.Drawing.Point(410, 418);
            this.btnExploreHex.Name = "btnExploreHex";
            this.btnExploreHex.Size = new System.Drawing.Size(120, 30);
            this.btnExploreHex.TabIndex = 5;
            this.btnExploreHex.Text = "Explore Hex";
            this.btnExploreHex.UseVisualStyleBackColor = false;
            this.btnExploreHex.Click += new System.EventHandler(this.btnExploreHex_Click);
            // 
            // btnShowMap
            // 
            this.btnShowMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowMap.BackColor = System.Drawing.Color.Black;
            this.btnShowMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowMap.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowMap.ForeColor = System.Drawing.Color.Lime;
            this.btnShowMap.Location = new System.Drawing.Point(536, 418);
            this.btnShowMap.Name = "btnShowMap";
            this.btnShowMap.Size = new System.Drawing.Size(120, 30);
            this.btnShowMap.TabIndex = 6;
            this.btnShowMap.Text = "Show Map";
            this.btnShowMap.UseVisualStyleBackColor = false;
            this.btnShowMap.Click += new System.EventHandler(this.btnShowMap_Click);
            // 
            // btnSelectCharacter
            // 
            this.btnSelectCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSelectCharacter.BackColor = System.Drawing.Color.Black;
            this.btnSelectCharacter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSelectCharacter.ForeColor = System.Drawing.Color.Lime;
            this.btnSelectCharacter.Location = new System.Drawing.Point(662, 418);
            this.btnSelectCharacter.Name = "btnSelectCharacter";
            this.btnSelectCharacter.Size = new System.Drawing.Size(120, 30);
            this.btnSelectCharacter.TabIndex = 7;
            this.btnSelectCharacter.Text = "Select Character";
            this.btnSelectCharacter.UseVisualStyleBackColor = false;
            this.btnSelectCharacter.Click += new System.EventHandler(this.btnSelectCharacter_Click);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.btnSelectCharacter);
            this.Controls.Add(this.btnShowMap);
            this.Controls.Add(this.btnExploreHex);
            this.Controls.Add(this.btnClearOutput);
            this.Controls.Add(this.btnShowCreationLog);
            this.Controls.Add(this.btnShowCharacterSheet);
            this.Controls.Add(this.btnCreateCharacter);
            this.Controls.Add(this.txtOutput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.Lime;
            this.Name = "GameWindow";
            this.Text = "DUNGEON-SIM - Character Creation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox? txtOutput;
        private System.Windows.Forms.Button? btnCreateCharacter;
        private System.Windows.Forms.Button? btnShowCharacterSheet;
        private System.Windows.Forms.Button? btnShowCreationLog;
        private System.Windows.Forms.Button? btnClearOutput;
        private System.Windows.Forms.Button? btnExploreHex;
        private System.Windows.Forms.Button? btnShowMap;
        private System.Windows.Forms.Button? btnSelectCharacter; // Added new button
    }

    // CSV Data Loader (simplified for Windows Forms)
    public class CSVDataLoader
    {
        private Dictionary<string, CSVTableData> tableCache = new Dictionary<string, CSVTableData>();
        public List<CSVTableData> loadedTables = new List<CSVTableData>();

        public void LoadAllCSVFiles()
        {
            string csvFolderPath = "game/csv";
            
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

    // Simplified CSV Table Data for Windows Forms
    public class CSVTableData
    {
        public string tableName = "";
        public List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();
    }
} 