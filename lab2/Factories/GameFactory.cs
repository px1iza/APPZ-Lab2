namespace GameSimulation
{
    public static class GameFactory
    {
        public static Game CreateGame(string type, string name, int size, Hardware requirements)
        {
            return type switch
            {
                "1" => new RPGGame(name, size, requirements),
                "2" => new StrategyGame(name, size, requirements),
                "3" => new AdventureGame(name, size, requirements),
                _ => new RPGGame(name, size, requirements)
            };
        }
    }
}