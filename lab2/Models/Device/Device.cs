namespace GameSimulation
{
    public class Device
    {
        public string Name { get; set; }
        public Platform Platform { get; set; }
        public OperatingSystemType OS { get; set; }
        public Hardware Hardware { get; set; }
        public int FreeHdd { get; set; }
        public Device(string name, Platform platform, OperatingSystemType os, Hardware hardware, int freeHdd)
        {
            if (freeHdd < 0)
                throw new ArgumentException("HDD не може бути від’ємним");

            Name = name;
            Platform = platform;
            OS = os;
            Hardware = hardware;
            FreeHdd = freeHdd;
        }
        public void ConsumeHdd(int size)
        {
            if (FreeHdd < size)
                throw new Exception("Недостатньо місця на HDD");

            FreeHdd -= size;
        }

        public void FreeHddSpace(int size)
        {
            FreeHdd += size;
        }
    }
}