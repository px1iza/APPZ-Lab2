namespace GameSimulation
{
    public class RPGGame : Game, IMultiplayer, ITransmittable
    {
        private int _connectedControllers = 0;
        public bool IsMultiplayerAvailable { get; private set; } = false;
        public bool CanStream => true;

        public RPGGame(string name, int size, Hardware req)
            : base(name, size, req)
        {
        }

        public void EnableMultiplayer(int controllerCount)
        {
            if (controllerCount < 2)
            {
                OnStatusChanged("помилка: мультиплеєр потребує мінімум 2 маніпулятора");
                throw new InvalidOperationException("Мультиплеєр потребує мінімум 2 маніпулятора");
            }

            _connectedControllers = controllerCount;
            IsMultiplayerAvailable = true;
            OnStatusChanged($"мультиплеєр увімкнено для {controllerCount} гравців");
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

        public override void Stop()
        {
            IsMultiplayerAvailable = false;
            _connectedControllers = 0;
            base.Stop();
        }
    }
}