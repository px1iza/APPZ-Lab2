namespace GameSimulation
{
    public abstract class Game : IInstallable, IPlayable, ISaveable
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Hardware Requirements { get; set; }

        public bool IsInstalled { get; protected set; }
        public bool IsRunning { get; protected set; }
        public bool IsLoggedIn { get; protected set; }

        public List<string> SavedStates { get; protected set; } = new();

        public event EventHandler<GameEventArgs>? StatusChanged;

        protected Game(string name, int size, Hardware requirements)
        {
            Name = name;
            Size = size;
            Requirements = requirements;
        }

        protected void OnStatusChanged(string message)
        {
            StatusChanged?.Invoke(this, new GameEventArgs(message));
        }

        public virtual void Install(Device device)
        {
            if (IsInstalled)
            {
                OnStatusChanged("вже встановлена");
                return;
            }

            if (device.FreeHdd < Size)
            {
                throw new Exception($"Недостатньо місця. Потрібно: {Size}GB, Доступно: {device.FreeHdd}GB");
            }

            device.FreeHdd -= Size;
            IsInstalled = true;
            OnStatusChanged("встановлена успішно");
        }

        public virtual void Uninstall(Device device)
        {
            if (!IsInstalled)
            {
                OnStatusChanged("не встановлена");
                return;
            }

            if (IsRunning)
            {
                Stop();
            }

            device.FreeHdd += Size;
            IsInstalled = false;
            IsLoggedIn = false;
            SavedStates.Clear();
            OnStatusChanged("видалена");
        }

        // ===== LOGIN =====
        public virtual void Login(string username)
        {
            if (!IsInstalled)
            {
                throw new Exception("Гра не встановлена");
            }

            IsLoggedIn = true;
            OnStatusChanged($"користувач '{username}' увійшов");
        }

        public virtual void Logout()
        {
            if (IsRunning)
            {
                Stop();
            }

            IsLoggedIn = false;
            OnStatusChanged("користувач вийшов");
        }

        // ===== PLAY =====
        public virtual void Play()
        {
            if (!IsInstalled)
            {
                throw new Exception("Гра не встановлена");
            }

            if (!IsLoggedIn)
            {
                throw new Exception("Необхідно увійти в акаунт");
            }

            IsRunning = true;
            OnStatusChanged("запущена");
        }

        public virtual void Stop()
        {
            if (!IsRunning)
            {
                throw new Exception("Гра не запущена");
            }

            IsRunning = false;
            OnStatusChanged("зупинена");
        }

        public virtual void SaveState(string slotName)
        {
            if (!IsRunning)
            {
                throw new Exception("Гра не запущена");
            }

            if (!SavedStates.Contains(slotName))
            {
                SavedStates.Add(slotName);
            }

            OnStatusChanged($"стан збережено в слот '{slotName}'");
        }

        public virtual void LoadState(string slotName)
        {
            if (!IsInstalled)
            {
                throw new Exception("Гра не встановлена");
            }

            if (!SavedStates.Contains(slotName))
            {
                throw new Exception($"Слот '{slotName}' не існує");
            }

            OnStatusChanged($"стан завантажено з слота '{slotName}'");
        }
        public bool HasSavedStates => SavedStates.Count > 0;
    }
}