using System;

namespace JahresprojektNeu.Classes
{
    public class GameManager
    {
        private static GameManager instance;
        public decimal Balance { get; private set; }

        private GameManager()
        {
            Balance = 1000.00m; // Startguthaben
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameManager();
                }
                return instance;
            }
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
        }
    }
}
