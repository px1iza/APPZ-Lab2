namespace GameSimulation
{
    public interface ISaveable
    {
        void SaveState(string slotName);
        void LoadState(string slotName);
    }
}