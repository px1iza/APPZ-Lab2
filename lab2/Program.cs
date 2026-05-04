namespace GameSimulation
{
    class Program
    {
        static Player? _player;
        static GameObserver _observer = new();
        static Device? _device;
        static Device? _pcDevice;
        static Device? _mobileDevice;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            _player = new Player("liza");
            Console.WriteLine($"Привіт, {_player.Name}!");

            InitializeDevices();
            AddGames();

            MainMenu();
        }

        static void InitializeDevices()
        {
            Console.WriteLine("\nПристрій:");
            Console.WriteLine("1 - PC (Windows)");
            Console.WriteLine("2 - Mobile (Android)");
            Console.Write("Вибір: ");

            string choice = Console.ReadLine() ?? "1";

            _pcDevice = new Device(
                "My PC",
                Platform.PC,
                OperatingSystemType.Windows,
                new Hardware(4, 8, 5, 512),
                512
            );

            _mobileDevice = new Device(
                "My Mobile",
                Platform.Mobile,
                OperatingSystemType.Android,
                new Hardware(2, 4, 3, 128),
                128
            );

            _device = choice == "2" ? _mobileDevice : _pcDevice;
        }

        static void AddGames()
        {
            Game s = GameFactory.CreateGame("2", "Civilization VI", 50, new Hardware(3, 4, 3, 50));
            Game a = GameFactory.CreateGame("3", "Zelda", 30, new Hardware(2, 4, 2, 30));
            Game r = GameFactory.CreateGame("1", "Witcher 3", 80, new Hardware(4, 8, 5, 80));

            s.StatusChanged += _observer.OnGameStatusChanged;
            a.StatusChanged += _observer.OnGameStatusChanged;
            r.StatusChanged += _observer.OnGameStatusChanged;

            _player.AddGame(s);
            _player.AddGame(a);
            _player.AddGame(r);
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== МЕНЮ ===");
                Console.WriteLine($"Пристрій: {_device.Name} ({_device.OS}) | Ігри: {_player.GetGameCount()} | Встановлено: {_player.GetInstalledGameCount(_device)}");
                Console.WriteLine($"Вільне місце: {_device.FreeHdd}ГБ\n");

                Console.WriteLine("1 - Мої ігри");
                Console.WriteLine("2 - Запустити");
                Console.WriteLine("3 - Змінити пристрій");
                Console.WriteLine("0 - Вихід");
                Console.Write("Вибір: ");

                switch (Console.ReadLine())
                {
                    case "1": MyGames(); break;
                    case "2": PlayGame(); break;
                    case "3": ChangeDevice(); break;
                    case "0": return;
                }
            }
        }

        static void ChangeDevice()
        {
            Console.Clear();
            Console.WriteLine("=== ЗМІНИТИ ПРИСТРІЙ ===\n");

            Console.WriteLine($"1. {_pcDevice.Name} ({_pcDevice.OS}) - Вільно: {_pcDevice.FreeHdd}ГБ");
            Console.WriteLine($"2. {_mobileDevice.Name} ({_mobileDevice.OS}) - Вільно: {_mobileDevice.FreeHdd}ГБ");

            Console.Write("\nВибір: ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1")
            {
                if (_device == _pcDevice)
                {
                    Console.WriteLine("Ви вже на PC. Можете змінити на мобільний");
                }
                else
                {
                    _device = _pcDevice;
                    Console.WriteLine("Переключено на PC");
                }
            }
            else if (choice == "2")
            {
                if (_device == _mobileDevice)
                {
                    Console.WriteLine(" Ви вже на мобільному. Можете змінити на PC");
                }
                else
                {
                    _device = _mobileDevice;
                    Console.WriteLine("Переключено на мобільний пристрій");
                }
            }
            else
            {
                Console.WriteLine("Невірний вибір");
            }

            Console.ReadKey();
        }

        static void MyGames()
        {
            Console.Clear();
            Console.WriteLine("=== МОЇ ІГИ ===\n");

            for (int i = 0; i < _player.GameLibrary.Count; i++)
            {
                Game g = _player.GameLibrary[i];
                bool installed = _player.IsGameInstalledOnDevice(g, _device);
                Console.WriteLine($"{i + 1}. {g.Name} {(installed ? "✅" : "❌")} ({g.Size}ГБ)");
            }

            Console.WriteLine("\n1 - Встановити");
            Console.WriteLine("2 - Видалити");
            Console.WriteLine("0 - Назад");
            Console.Write("Вибір: ");

            string choice = Console.ReadLine() ?? "";

            if (choice == "0")
                return;

            Console.Write("Номер гри: ");

            if (int.TryParse(Console.ReadLine(), out int idx) && idx > 0 && idx <= _player.GameLibrary.Count)
            {
                Game game = _player.GameLibrary[idx - 1];

                if (choice == "1")
                    InstallInfo(game);
                else if (choice == "2")
                    UninstallGame(game);
            }

            Console.ReadKey();
        }

        static void InstallInfo(Game game)
        {
            Console.Clear();
            Console.WriteLine($"=== {game.Name} ===\n");
            Console.WriteLine($"Пристрій: {_device.Name} ({_device.OS})");
            Console.WriteLine($"Вільне місце: {_device.FreeHdd}ГБ");
            Console.WriteLine($"Розмір гри: {game.Size}ГБ\n");

            Console.WriteLine("Вимоги гри:");
            Console.WriteLine($"  CPU: {game.Requirements.CPU}ГГц");
            Console.WriteLine($"  RAM: {game.Requirements.RAM}ГБ");
            Console.WriteLine($"  GPU: {game.Requirements.GPU}");
            Console.WriteLine($"  HDD: {game.Requirements.HDD}ГБ\n");

            Console.WriteLine("Ваш пристрій:");
            Console.WriteLine($"  CPU: {_device.Hardware.CPU}ГГц");
            Console.WriteLine($"  RAM: {_device.Hardware.RAM}ГБ");
            Console.WriteLine($"  GPU: {_device.Hardware.GPU}");
            Console.WriteLine($"  HDD: {_device.Hardware.HDD}ГБ\n");

            if (_player.IsGameInstalledOnDevice(game, _device))
            {
                Console.WriteLine("Гра вже встановлена на цьому пристрої!");
            }
            else if (_device.FreeHdd < game.Size)
            {
                Console.WriteLine($"Недостатньо місця! Потрібно {game.Size}ГБ, доступно {_device.FreeHdd}ГБ");
            }
            else if (!game.Requirements.CanRunOn(_device.Hardware))
            {
                Console.WriteLine("Апаратне забезпечення не відповідає вимогам!");
            }
            else
            {
                Console.WriteLine("Можна встановити!");
                Console.Write("\nПідтвердити встановлення? (y/n): ");
                string answer = Console.ReadLine()?.ToLower() ?? "n";

                if (answer == "y")
                {
                    try
                    {
                        game.Install(_device);
                        _player.AddGameToDevice(game, _device);
                        Console.WriteLine("Встановлено!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("Встановлення скасовано!");
                }
            }
        }

        static void UninstallGame(Game game)
        {
            if (!_player.IsGameInstalledOnDevice(game, _device))
            {
                Console.WriteLine("Ви не можете видалити цю гру, бо вона не встановлена на цьому пристрої!");
                return;
            }

            try
            {
                game.Uninstall(_device);
                _player.RemoveGameFromDevice(game, _device);
                Console.WriteLine("Видалено!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
            }
        }

        static void PlayGame()
        {
            if (_player.CurrentGame != null)
            {
                RunningGame();
                return;
            }

            var installed = _player.GetInstalledGamesForDevice(_device);

            if (installed.Count == 0)
            {
                Console.Clear();
                Console.WriteLine("Немає встановлених ігор на цьому пристрої");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine("=== ВСТАНОВЛЕНІ ІГИ ===\n");

            for (int i = 0; i < installed.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {installed[i].Name}");
            }

            Console.Write("\nВибір: ");
            if (!int.TryParse(Console.ReadLine(), out int idx) || idx < 1 || idx > installed.Count)
            {
                return;
            }

            Game game = installed[idx - 1];

            if (game is StrategyGame && _device.Platform != Platform.PC)
            {
                Console.WriteLine("Стратегія тільки на PC!");
                Console.ReadKey();
                return;
            }

            if (!game.Requirements.CanRunOn(_device.Hardware))
            {
                Console.WriteLine("Недостатньо потужності!");
                Console.ReadKey();
                return;
            }

            try
            {
                game.Login("user");
                game.Play();
                _player.StartGame(game);
                RunningGame();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                Console.ReadKey();
            }
        }

        static void RunningGame()
        {
            Game game = _player.CurrentGame;

            while (_player.CurrentGame != null)
            {
                Console.Clear();
                Console.WriteLine($"{game.Name}");

                Console.WriteLine("1 - Зберегти");
                Console.WriteLine("2 - Завантажити");
                Console.WriteLine("3 - Показати збереження");
                Console.WriteLine("4 - Мультиплеєр");
                Console.WriteLine("5 - Стрім");
                Console.WriteLine("6 - Зупинити гру");
                Console.WriteLine("0 - Вихід в меню");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        Console.Write("Назва слота: ");
                        try
                        {
                            game.SaveState(Console.ReadLine() ?? "save");
                            Console.WriteLine("Збережено!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{ex.Message}");
                        }
                        break;

                    case "2":
                        if (game.HasSavedStates)
                        {
                            Console.WriteLine("\nЗбереження:");
                            for (int i = 0; i < game.SavedStates.Count; i++)
                                Console.WriteLine($"{i + 1}. {game.SavedStates[i]}");

                            bool validChoice = false;
                            while (!validChoice)
                            {
                                Console.Write("\nОберіть завантаження (номер): ");
                                if (int.TryParse(Console.ReadLine(), out int n) && n > 0 && n <= game.SavedStates.Count)
                                {
                                    try
                                    {
                                        game.LoadState(game.SavedStates[n - 1]);
                                        Console.WriteLine("Завантажено!");
                                        validChoice = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{ex.Message}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Невірна цифра! Оберіть завантаження з наявних.");
                                }
                            }
                        }
                        else Console.WriteLine("Немає збережень");
                        break;

                    case "3":
                        if (game.HasSavedStates)
                        {
                            Console.WriteLine("\nВаші збереження:");
                            foreach (var s in game.SavedStates)
                                Console.WriteLine($"  {s}");
                        }
                        else Console.WriteLine("Немає збережень");
                        break;

                    case "4":
                        if (game is RPGGame rpg)
                        {
                            Console.Write("Маніпулятори: ");
                            if (int.TryParse(Console.ReadLine(), out int c))
                            {
                                try
                                {
                                    rpg.EnableMultiplayer(c);
                                    Console.WriteLine("Мультиплеєр увімкнено!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ця гра не підтримує мультиплеєр!");
                        }
                        break;

                    case "5":
                        if (game is ITransmittable t)
                        {
                            if (_device.Platform == Platform.Mobile)
                            {
                                try
                                {
                                    t.StreamToDevice("PC");
                                    Console.WriteLine("Стрім на PC!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"{ex.Message}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Стрім доступний тільки на мобільних пристроях!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Ця гра не підтримує стрім!");
                        }
                        break;

                    case "6":
                    case "0":
                        try
                        {
                            if (game.IsRunning)
                                game.Stop();
                        }
                        catch { }

                        try
                        {
                            game.Logout();
                        }
                        catch { }

                        _player.StopCurrentGame();
                        Console.Clear();
                        Console.WriteLine("Ви вийшли з гри!");
                        Console.WriteLine("Повертаєтесь в меню...");
                        System.Threading.Thread.Sleep(2000);
                        return;

                    default:
                        Console.WriteLine("Невірний вибір");
                        break;
                }

                if (_player.CurrentGame != null)
                    Console.ReadKey();
            }
        }
    }
}