using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DungeonSim
{
    internal static class NativeScrollbar
    {
        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string? pszSubIdList);
    }

    internal static class EditMessages
    {
        public const int EM_GETFIRSTVISIBLELINE = 0x00CE;
        public const int EM_LINESCROLL = 0x00B6;

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }

    public class DarkScrollTextBox : TextBox
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (IsHandleCreated)
                NativeScrollbar.SetWindowTheme(Handle, "DarkMode_Explorer", null);
        }
    }
    public enum DetailListMode { None, Characters, Hexes }

    public partial class GameWindow : Form
    {
        private CSVDataLoader? csvDataLoader;
        private CharacterCreationSystem? characterCreationSystem;
        private List<Character> createdCharacters = new List<Character>();
        private Character? currentCharacter;
        private HexMap? hexMap;
        private DetailListMode detailListMode = DetailListMode.None;
        private List<Hex>? detailHexList;
        private Hex? hexAwaitingDungeonChoice;
        private SplitContainer? mainSplit;
        private SplitContainer? rightSplit;
        private Panel? panelLeft;
        private Panel? panelMiddle;
        private Panel? panelRight;
        private TextBox? txtDetailInput;
        private HexGridPanel? hexGridPanel;

        public GameWindow()
        {
            InitializeComponent();
            this.Load += GameWindow_Load;
            InitializeGame();
        }

        private void GameWindow_Load(object? sender, EventArgs e)
        {
            // Defer splitter setup until after form and children are laid out (Width/Height are set).
            // Do not set Panel1MinSize/Panel2MinSize - they trigger validation that fails when size is still 0.
            void SetSplitterDistances()
            {
                if (mainSplit != null && mainSplit.Width > 100)
                    mainSplit.SplitterDistance = Math.Clamp(160, 25, mainSplit.Width - 25);
                if (rightSplit != null && rightSplit.Height > 100)
                    rightSplit.SplitterDistance = Math.Clamp(420, 0, rightSplit.Height - 25);
            }
            BeginInvoke(new Action(SetSplitterDistances));
        }

        private void InitializeGame()
        {
            csvDataLoader = new CSVDataLoader();
            csvDataLoader.LoadAllCSVFiles();

            characterCreationSystem = new CharacterCreationSystem();
            characterCreationSystem.csvDataLoader = csvDataLoader;

            DataDrivenTableData.Initialize(csvDataLoader);
            HexGenerationSystem.Initialize(csvDataLoader);

            hexMap = new HexMap();
            hexGridPanel!.HexMap = hexMap;

            AppendToOutput("==========================================" + Environment.NewLine);
            AppendToOutput("         DUNGEON-SIM" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Welcome to the tabletop RPG simulation system!" + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Use the menu on the left. After viewing a character or hex list, type a number in the box at the bottom of the center panel to view full details." + Environment.NewLine);
            AppendToOutput("The right panel shows a hex grid of explored hexes." + Environment.NewLine + Environment.NewLine);
        }



        //----------------------------------------
        // Menu Buttons
        //----------------------------------------
        private void btnCreateCharacter_Click(object? sender, EventArgs e)
        {
            try
            {
                if (characterCreationSystem == null)
                {
                    AppendToOutput("Error: Character creation system not initialized." + Environment.NewLine + Environment.NewLine);
                    return;
                }

                AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
                AppendToOutput("         CREATING NEW CHARACTER" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

                currentCharacter = characterCreationSystem.CreateCharacter();
                createdCharacters.Add(currentCharacter);

                DisplayCharacterSheet(currentCharacter);

                AppendToOutput("Character created successfully! Sheet shown above." + Environment.NewLine);
                AppendToOutput("Use 'Show Party' and enter a number to view any character again." + Environment.NewLine + Environment.NewLine);
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
                AppendToOutput("No characters in the party yet." + Environment.NewLine);
                AppendToOutput("Click 'Create Character' first." + Environment.NewLine + Environment.NewLine);
                return;
            }

            AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
            AppendToOutput("         PARTY" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

            for (int i = 0; i < createdCharacters.Count; i++)
            {
                Character character = createdCharacters[i];
                AppendToOutput($"{i + 1}. {character.characterName} - {character.race} {character.characterClass} (Level {character.level}){Environment.NewLine}");
            }

            AppendToOutput(Environment.NewLine);
            AppendToOutput("Enter a number in the box below to view that character's full sheet." + Environment.NewLine + Environment.NewLine);
            detailListMode = DetailListMode.Characters;
        }

        private void btnShowExploredHexes_Click(object? sender, EventArgs e)
        {
            if (hexMap == null)
            {
                AppendToOutput("Error: Hex map not initialized." + Environment.NewLine + Environment.NewLine);
                return;
            }

            AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
            AppendToOutput("         EXPLORED HEXES" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

            var explored = hexMap.GetAllExploredHexes();
            detailHexList = explored;
            detailListMode = DetailListMode.Hexes;

            if (explored.Count == 0)
            {
                AppendToOutput("No hexes explored yet. Click 'Explore Hex' to explore from the capital." + Environment.NewLine + Environment.NewLine);
                return;
            }

            for (int i = 0; i < explored.Count; i++)
            {
                var hex = explored[i];
                string current = (hex.Coordinate == hexMap.CurrentPartyLocation) ? " [CURRENT]" : "";
                string capital = (hex.Coordinate == hexMap.CapitalLocation) ? " [CAPITAL]" : "";
                AppendToOutput($"{i + 1}. {hex.Coordinate} - {hex.Biome}{current}{capital}{Environment.NewLine}");
            }

            AppendToOutput(Environment.NewLine);
            AppendToOutput($"Enter a number (1–{explored.Count}) in the box below to view that hex's full details." + Environment.NewLine + Environment.NewLine);
            hexGridPanel?.Invalidate();
        }

        private void btnShowCreationLog_Click(object? sender, EventArgs e)
        {
            if (currentCharacter != null && characterCreationSystem != null)
            {
                AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
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
            detailListMode = DetailListMode.None;
            txtDetailInput?.Clear();
            AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
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

                AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
                AppendToOutput("         EXPLORING NEW HEX" + Environment.NewLine);
                AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);

                var adjacentHexes = hexMap.GetAdjacentHexes(hexMap.CurrentPartyLocation);
                var unexplored = new List<HexCoordinate>();
                foreach (var coord in adjacentHexes)
                {
                    Hex hex = hexMap.GetHex(coord);
                    if (!hex.IsExplored)
                        unexplored.Add(coord);
                }

                HexCoordinate targetHex;
                var rng = new Random();
                if (unexplored.Count > 0)
                    targetHex = unexplored[rng.Next(unexplored.Count)];
                else
                    targetHex = adjacentHexes[rng.Next(adjacentHexes.Count)];

                Hex newHex = HexGenerationSystem.GenerateHex(targetHex, hexMap);
                hexMap.SetHex(targetHex, newHex);
                hexMap.MoveParty(targetHex);

                string explorationLog = HexGenerationSystem.GenerateExplorationLog(newHex);
                AppendToOutput(explorationLog);

                detailHexList = hexMap.GetAllExploredHexes();
                detailListMode = DetailListMode.Hexes;
                AppendToOutput("Hex exploration complete! Map updated in the right panel." + Environment.NewLine);
                AppendToOutput($"Enter a number (1–{detailHexList.Count}) below to view that hex's full details." + Environment.NewLine + Environment.NewLine);
                hexGridPanel?.Invalidate();

                if (newHex.ResolvedEncounter is DungeonEncounter dungeonEncounter)
                {
                    hexAwaitingDungeonChoice = newHex;
                    dungeonEncounter.PlayerChoseToEnter = null;
                    AppendToOutput("Enter dungeon or skip? Use [Enter Dungeon] or [Skip Dungeon]." + Environment.NewLine + Environment.NewLine);
                    btnExploreHex!.Enabled = false;
                    btnEnterDungeon!.Visible = true;
                    btnSkipDungeon!.Visible = true;
                }
            }
            catch (Exception ex)
            {
                AppendToOutput($"Error exploring hex: {ex.Message}" + Environment.NewLine + Environment.NewLine);
            }
        }

        private void btnEnterDungeon_Click(object? sender, EventArgs e)
        {
            if (hexAwaitingDungeonChoice?.ResolvedEncounter is not DungeonEncounter dungeon)
                return;
            dungeon.PlayerChoseToEnter = true;
            dungeon.RunAutomatedDungeonCrawl(line => AppendToOutput(line));
            EndDungeonChoice();
        }

        private void btnSkipDungeon_Click(object? sender, EventArgs e)
        {
            if (hexAwaitingDungeonChoice?.ResolvedEncounter is DungeonEncounter dungeon)
                dungeon.PlayerChoseToEnter = false;
            AppendToOutput("Party takes a full rest. Danger roll (1d20): " + DiceSystem.Roll1d20() + Environment.NewLine + Environment.NewLine);
            AppendToOutput("Press Explore Hex to continue." + Environment.NewLine + Environment.NewLine);
            EndDungeonChoice();
        }

        private void EndDungeonChoice()
        {
            hexAwaitingDungeonChoice = null;
            btnEnterDungeon!.Visible = false;
            btnSkipDungeon!.Visible = false;
            btnExploreHex!.Enabled = true;
        }

        private void DisplayCharacterSheet(Character character)
        {
            AppendToOutput("==========================================" + Environment.NewLine, scrollNewToTop: true);
            AppendToOutput("         CHARACTER SHEET" + Environment.NewLine);
            AppendToOutput("==========================================" + Environment.NewLine + Environment.NewLine);
            AppendToOutput(character.GetCharacterSheet());
            AppendToOutput("\n");
        }

        private void AppendToOutput(string text, bool scrollNewToTop = false)
        {
            if (txtOutput?.InvokeRequired == true)
            {
                txtOutput.Invoke(new Action(() => AppendToOutput(text, scrollNewToTop)));
                return;
            }
            if (txtOutput == null) return;

            int startIndex = scrollNewToTop ? txtOutput.TextLength : -1;
            txtOutput.AppendText(text);
            if (scrollNewToTop && startIndex >= 0 && txtOutput.IsHandleCreated)
            {
                int pos = startIndex;
                txtOutput.BeginInvoke(new Action(() => ScrollOutputToShowPosition(pos)));
            }
            else if (!scrollNewToTop)
                txtOutput.ScrollToCaret();
        }

        private void ScrollOutputToShowPosition(int charIndex)
        {
            if (txtOutput == null || !txtOutput.IsHandleCreated) return;
            try
            {
                int len = Math.Min(charIndex, txtOutput.TextLength);
                int targetLine = txtOutput.GetLineFromCharIndex(len);
                int currentFirst = EditMessages.SendMessage(txtOutput.Handle, EditMessages.EM_GETFIRSTVISIBLELINE, IntPtr.Zero, IntPtr.Zero);
                int scrollBy = targetLine - currentFirst;
                if (scrollBy != 0)
                    EditMessages.SendMessage(txtOutput.Handle, EditMessages.EM_LINESCROLL, 0, scrollBy);
            }
            catch
            {
                try
                {
                    txtOutput.SelectionStart = Math.Min(charIndex, txtOutput.TextLength);
                    txtOutput.SelectionLength = 0;
                    txtOutput.ScrollToCaret();
                }
                catch { }
            }
        }

        private void TxtDetailInput_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || txtDetailInput == null) return;
            e.Handled = true;
            e.SuppressKeyPress = true;

            string input = txtDetailInput.Text.Trim();
            txtDetailInput.Clear();

            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int num) || num < 1)
            {
                AppendToOutput("Enter a valid number (1 or higher)." + Environment.NewLine + Environment.NewLine);
                return;
            }

            if (detailListMode == DetailListMode.Characters)
            {
                if (num > createdCharacters.Count)
                {
                    AppendToOutput($"No character #{num}. Choose 1–{createdCharacters.Count}." + Environment.NewLine + Environment.NewLine);
                    return;
                }
                AppendToOutput(Environment.NewLine);
                DisplayCharacterSheet(createdCharacters[num - 1]);
                AppendToOutput(Environment.NewLine);
                return;
            }

            if (detailListMode == DetailListMode.Hexes && detailHexList != null)
            {
                if (num > detailHexList.Count)
                {
                    AppendToOutput($"No hex #{num}. Choose 1–{detailHexList.Count}." + Environment.NewLine + Environment.NewLine);
                    return;
                }
                AppendToOutput(Environment.NewLine);
                AppendToOutput(detailHexList[num - 1].GetDescription());
                AppendToOutput(Environment.NewLine);
            }
        }

        private void InitializeComponent()
        {
            this.txtOutput = new DarkScrollTextBox();
            this.btnCreateCharacter = new System.Windows.Forms.Button();
            this.btnShowCharacterSheet = new System.Windows.Forms.Button();
            this.btnShowCreationLog = new System.Windows.Forms.Button();
            this.btnClearOutput = new System.Windows.Forms.Button();
            this.btnExploreHex = new System.Windows.Forms.Button();
            this.btnShowExploredHexes = new System.Windows.Forms.Button();
            this.btnEnterDungeon = new System.Windows.Forms.Button();
            this.btnSkipDungeon = new System.Windows.Forms.Button();

            this.mainSplit = new SplitContainer();
            this.mainSplit.Dock = DockStyle.Fill;
            this.mainSplit.FixedPanel = FixedPanel.Panel1;

            this.panelLeft = new Panel();
            this.panelLeft.Dock = DockStyle.Fill;
            this.panelLeft.BackColor = Color.Black;
            this.panelLeft.Padding = new Padding(4);

            var leftFlowBottom = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                BackColor = Color.Black,
                Padding = new Padding(4)
            };
            var leftFlowTop = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                BackColor = Color.Black,
                Padding = new Padding(4)
            };

            void AddMenuButton(Button btn, string text, EventHandler? clickHandler, Control parent)
            {
                btn.Text = text;
                btn.Width = 140;
                btn.Height = 32;
                btn.Margin = new Padding(0, 2, 0, 2);
                btn.BackColor = Color.Black;
                btn.FlatStyle = FlatStyle.Flat;
                btn.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
                btn.ForeColor = Color.White;
                if (clickHandler != null)
                    btn.Click += clickHandler;
                parent.Controls.Add(btn);
            }

            AddMenuButton(btnCreateCharacter, "Create Character", btnCreateCharacter_Click, leftFlowTop);
            AddMenuButton(btnShowCharacterSheet, "Show Party", btnShowCharacterSheet_Click, leftFlowTop);
            AddMenuButton(btnExploreHex, "Explore Hex", btnExploreHex_Click, leftFlowTop);
            AddMenuButton(btnEnterDungeon, "Enter Dungeon", btnEnterDungeon_Click, leftFlowTop);
            AddMenuButton(btnSkipDungeon, "Skip Dungeon", btnSkipDungeon_Click, leftFlowTop);
            btnEnterDungeon.Visible = false;
            btnSkipDungeon.Visible = false;
            AddMenuButton(btnShowExploredHexes, "Show Explored Hexes", btnShowExploredHexes_Click, leftFlowTop);
            AddMenuButton(btnShowCreationLog, "Show Creation Log", btnShowCreationLog_Click, leftFlowBottom);
            AddMenuButton(btnClearOutput, "Clear Output", btnClearOutput_Click, leftFlowBottom);

            panelLeft.Controls.Add(leftFlowBottom);
            panelLeft.Controls.Add(leftFlowTop);
            this.mainSplit.Panel1.Controls.Add(panelLeft);

            this.rightSplit = new SplitContainer();
            this.rightSplit.Dock = DockStyle.Fill;
            this.rightSplit.Orientation = Orientation.Vertical;
            this.rightSplit.FixedPanel = FixedPanel.Panel2;

            this.panelMiddle = new Panel();
            this.panelMiddle.Dock = DockStyle.Fill;
            this.panelMiddle.BackColor = Color.Black;

            var bottomBar = new Panel();
            bottomBar.Height = 36;
            bottomBar.Dock = DockStyle.Bottom;
            bottomBar.BackColor = Color.FromArgb(40, 40, 40);
            bottomBar.Padding = new Padding(4, 4, 4, 4);

            var lblPrompt = new Label
            {
                Text = "View details (enter number):",
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Consolas", 9F),
                Location = new Point(6, 10)
            };
            this.txtDetailInput = new TextBox
            {
                Width = 80,
                Height = 22,
                BackColor = Color.Black,
                ForeColor = Color.White,
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.FixedSingle,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            this.txtDetailInput.KeyDown += TxtDetailInput_KeyDown;
            bottomBar.Controls.Add(lblPrompt);
            bottomBar.Controls.Add(txtDetailInput);
            bottomBar.Layout += (s, ev) =>
            {
                if (txtDetailInput != null && bottomBar.ClientSize.Width > txtDetailInput.Width + 12)
                    txtDetailInput.Location = new Point(bottomBar.ClientSize.Width - txtDetailInput.Width - 6, 6);
            };

            this.txtOutput.Dock = DockStyle.Fill;
            this.txtOutput.BackColor = Color.Black;
            this.txtOutput.Font = new Font("Consolas", 9F);
            this.txtOutput.ForeColor = Color.White;
            this.txtOutput.Multiline = true;
            this.txtOutput.ReadOnly = true;
            this.txtOutput.ScrollBars = ScrollBars.Vertical;
            this.txtOutput.BorderStyle = BorderStyle.FixedSingle;
            this.txtOutput.Margin = new Padding(0, 0, 0, 4);

            panelMiddle.Controls.Add(txtOutput);
            panelMiddle.Controls.Add(bottomBar);
            this.rightSplit.Panel1.Controls.Add(panelMiddle);

            this.panelRight = new Panel();
            this.panelRight.Dock = DockStyle.Fill;
            this.panelRight.BackColor = Color.Black;
            this.hexGridPanel = new HexGridPanel { Dock = DockStyle.Fill };
            panelRight.Controls.Add(hexGridPanel);
            this.rightSplit.Panel2.Controls.Add(panelRight);

            this.mainSplit.Panel2.Controls.Add(this.rightSplit);

            this.SuspendLayout();
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.Black;
            this.ClientSize = new Size(1000, 560);
            this.Controls.Add(mainSplit);
            this.Font = new Font("Consolas", 9F);
            this.ForeColor = Color.White;
            this.Name = "GameWindow";
            this.Text = "dungeon sim";
            this.ResumeLayout(false);
        }

        private DarkScrollTextBox? txtOutput;
        private System.Windows.Forms.Button? btnCreateCharacter;
        private System.Windows.Forms.Button? btnShowCharacterSheet;
        private System.Windows.Forms.Button? btnShowCreationLog;
        private System.Windows.Forms.Button? btnClearOutput;
        private System.Windows.Forms.Button? btnExploreHex;
        private System.Windows.Forms.Button? btnShowExploredHexes;
        private System.Windows.Forms.Button? btnEnterDungeon;
        private System.Windows.Forms.Button? btnSkipDungeon;
    }
}
