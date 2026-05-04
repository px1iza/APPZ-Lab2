namespace GameSimulation
{
    public class Hardware
    {
        public int CPU { get; set; }
        public int RAM { get; set; }
        public int GPU { get; set; }
        public int HDD { get; set; }

        public Hardware(int cpu, int ram, int gpu, int hdd)
        {
            if (cpu <= 0 || ram <= 0 || gpu <= 0 || hdd <= 0)
                throw new ArgumentException("Характеристики повинні бути > 0");

            CPU = cpu;
            RAM = ram;
            GPU = gpu;
            HDD = hdd;
        }

        public bool CanRunOn(Hardware deviceHardware)
        {
            return deviceHardware.CPU >= CPU &&
                   deviceHardware.RAM >= RAM &&
                   deviceHardware.GPU >= GPU &&
                   deviceHardware.HDD >= HDD;
        }
    }
}