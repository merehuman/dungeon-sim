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

            // Display welcome message
            AppendToOutput("=== DUNGEON SIMULATOR ===\n");
            AppendToOutput("Welcome to the tabletop RPG character creation system!\n\n");
            AppendToOutput("Click 'Create Character' to generate a new character.\n");
            AppendToOutput("Click 'Show Character Sheet' to view the current character.\n");
            AppendToOutput("Click 'Clear Output' to clear the display.\n\n");
        }

        private void btnCreateCharacter_Click(object sender, EventArgs e)
        {
            try
            {
                AppendToOutput("=== CREATING NEW CHARACTER ===\n");
                
                // Create character
                currentCharacter = characterCreationSystem.CreateCharacter();
                createdCharacters.Add(currentCharacter);

                // Display character sheet
                DisplayCharacterSheet(currentCharacter);

                AppendToOutput("\nCharacter created successfully! Click 'Show Character Sheet' to view details.\n");
                AppendToOutput("Click 'Create Character' to create another character.\n\n");
            }
            catch (Exception ex)
            {
                AppendToOutput($"Error creating character: {ex.Message}\n");
            }
        }

        private void btnShowCharacterSheet_Click(object sender, EventArgs e)
        {
            if (currentCharacter != null)
            {
                DisplayCharacterSheet(currentCharacter);
            }
            else
            {
                AppendToOutput("No character has been created yet. Click 'Create Character' first.\n");
            }
        }

        private void btnShowCreationLog_Click(object sender, EventArgs e)
        {
            if (currentCharacter != null)
            {
                AppendToOutput("=== CREATION LOG ===\n");
                foreach (string logEntry in characterCreationSystem.GetCreationLog())
                {
                    AppendToOutput(logEntry + "\n");
                }
                AppendToOutput("\n");
            }
            else
            {
                AppendToOutput("No character has been created yet. Click 'Create Character' first.\n");
            }
        }

        private void btnClearOutput_Click(object sender, EventArgs e)
        {
            txtOutput.Clear();
            AppendToOutput("=== DUNGEON SIMULATOR ===\n");
            AppendToOutput("Output cleared. Ready for new commands.\n\n");
        }

        private void DisplayCharacterSheet(Character character)
        {
            AppendToOutput("=== CHARACTER SHEET ===\n");
            AppendToOutput(character.GetCharacterSheet());
            AppendToOutput("\n");
        }

        private void AppendToOutput(string text)
        {
            if (txtOutput.InvokeRequired)
            {
                txtOutput.Invoke(new Action(() => AppendToOutput(text)));
            }
            else
            {
                txtOutput.AppendText(text);
                txtOutput.ScrollToCaret();
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
            // 
            // btnCreateCharacter
            // 
            this.btnCreateCharacter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateCharacter.BackColor = System.Drawing.Color.DarkGreen;
            this.btnCreateCharacter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreateCharacter.ForeColor = System.Drawing.Color.White;
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
            this.btnShowCharacterSheet.BackColor = System.Drawing.Color.DarkBlue;
            this.btnShowCharacterSheet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCharacterSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowCharacterSheet.ForeColor = System.Drawing.Color.White;
            this.btnShowCharacterSheet.Location = new System.Drawing.Point(138, 418);
            this.btnShowCharacterSheet.Name = "btnShowCharacterSheet";
            this.btnShowCharacterSheet.Size = new System.Drawing.Size(140, 30);
            this.btnShowCharacterSheet.TabIndex = 2;
            this.btnShowCharacterSheet.Text = "Show Character Sheet";
            this.btnShowCharacterSheet.UseVisualStyleBackColor = false;
            this.btnShowCharacterSheet.Click += new System.EventHandler(this.btnShowCharacterSheet_Click);
            // 
            // btnShowCreationLog
            // 
            this.btnShowCreationLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowCreationLog.BackColor = System.Drawing.Color.DarkOrange;
            this.btnShowCreationLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowCreationLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShowCreationLog.ForeColor = System.Drawing.Color.White;
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
            this.btnClearOutput.BackColor = System.Drawing.Color.DarkRed;
            this.btnClearOutput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClearOutput.ForeColor = System.Drawing.Color.White;
            this.btnClearOutput.Location = new System.Drawing.Point(668, 418);
            this.btnClearOutput.Name = "btnClearOutput";
            this.btnClearOutput.Size = new System.Drawing.Size(120, 30);
            this.btnClearOutput.TabIndex = 4;
            this.btnClearOutput.Text = "Clear Output";
            this.btnClearOutput.UseVisualStyleBackColor = false;
            this.btnClearOutput.Click += new System.EventHandler(this.btnClearOutput_Click);
            // 
            // GameWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(800, 460);
            this.Controls.Add(this.btnClearOutput);
            this.Controls.Add(this.btnShowCreationLog);
            this.Controls.Add(this.btnShowCharacterSheet);
            this.Controls.Add(this.btnCreateCharacter);
            this.Controls.Add(this.txtOutput);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "GameWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dungeon Simulator - Character Creation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnCreateCharacter;
        private System.Windows.Forms.Button btnShowCharacterSheet;
        private System.Windows.Forms.Button btnShowCreationLog;
        private System.Windows.Forms.Button btnClearOutput;
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
                "heal_spells.csv", "special_attack_generator.csv"
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