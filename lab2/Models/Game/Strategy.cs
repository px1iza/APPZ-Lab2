namespace GameSimulation
{
    public class StrategyGame : Game
    {
        public StrategyGame(string name, int size, Hardware req)
            : base(name, size, req)
        {
        }

        public override void Install(Device device)
        {
            if (device.OS != OperatingSystemType.Windows)
                throw new Exception("Стратегія тільки на Windows");

            base.Install(device);
        }
    }
}