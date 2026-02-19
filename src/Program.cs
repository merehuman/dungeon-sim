using System;
using System.IO;
using System.Windows.Forms;

namespace DungeonSim
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new GameWindow());
            }
            catch (Exception ex)
            {
                string message = $"{ex.GetType().Name}: {ex.Message}\n\n{ex.StackTrace}";
                MessageBox.Show(message, "Dungeon-Sim Startup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { File.WriteAllText("startup_error.txt", message); } catch { }
                throw;
            }
        }
    }
}
