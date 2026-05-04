namespace GameSimulation
{
    public class AdventureGame : Game, ITransmittable
    {
        public bool CanStream => true;

        public AdventureGame(string name, int size, Hardware req)
            : base(name, size, req)
        {
        }

        public void StreamToDevice(string deviceName)
        {
            if (!IsRunning)
            {
                OnStatusChanged("помилка: гра не запущена");
                throw new InvalidOperationException("Гра не запущена");
            }

            OnStatusChanged($"трансляція розпочалась на {deviceName}");
        }
    }
}