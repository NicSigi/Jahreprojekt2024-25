namespace JahresprojektNeu.Classes
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            ApplicationConfiguration.Initialize();
            Application.Run(new Login());
        }
    }
}