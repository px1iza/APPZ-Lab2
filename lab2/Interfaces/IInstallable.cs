namespace GameSimulation
{
    public interface IInstallable
    {
        void Install(Device device);

        void Uninstall(Device device);
    }
}