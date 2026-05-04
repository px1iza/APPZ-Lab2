namespace GameSimulation
{
    public class GameObserver
    {
        public void OnGameStatusChanged(object? sender, GameEventArgs evt)
        {
            if (sender is Game game)
            {
                Console.WriteLine($"{game.Name} -> {evt.Message}");
            }
        }
    }
}