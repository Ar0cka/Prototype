namespace Player.PlayerStats.ManaPool
{
    public interface ISubtractionMana
    {
        int CurrentManaPool { get; }
        void SubtractionMana(int spellCost);
    }
}