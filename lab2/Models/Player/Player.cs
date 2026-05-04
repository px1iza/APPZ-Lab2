namespace GameSimulation
{
    public class Player
    {
        public string Name { get; set; }
        public List<Game> GameLibrary { get; set; } = new();
        public Game? CurrentGame { get; set; }

        private Dictionary<Device, List<Game>> _installedGames = new();

        public Player(string name)
        {
            Name = name;
        }

        public void AddGame(Game game)
        {
            if (game != null)
                GameLibrary.Add(game);
        }

        public int GetGameCount() => GameLibrary.Count;

        public int GetInstalledGameCount(Device device)
        {
            return GetInstalledGamesForDevice(device).Count;
        }

        public bool IsGameInstalledOnDevice(Game game, Device device)
        {
            if (!_installedGames.ContainsKey(device))
                return false;
            return _installedGames[device].Contains(game);
        }

        public void AddGameToDevice(Game game, Device device)
        {
            if (!_installedGames.ContainsKey(device))
                _installedGames[device] = new List<Game>();

            if (!_installedGames[device].Contains(game))
                _installedGames[device].Add(game);
        }

        public void RemoveGameFromDevice(Game game, Device device)
        {
            if (_installedGames.ContainsKey(device))
                _installedGames[device].Remove(game);
        }

        public List<Game> GetInstalledGamesForDevice(Device device)
        {
            if (!_installedGames.ContainsKey(device))
                _installedGames[device] = new List<Game>();
            return _installedGames[device];
        }

        public void StartGame(Game game)
        {
            CurrentGame = game;
        }

        public void StopCurrentGame()
        {
            if (CurrentGame != null)
            {
                if (CurrentGame.IsRunning)
                    CurrentGame.Stop();
                CurrentGame = null;
            }
        }
    }
}