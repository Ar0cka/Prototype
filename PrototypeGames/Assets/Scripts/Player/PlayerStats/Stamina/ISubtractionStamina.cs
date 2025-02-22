namespace Player.PlayerStats.Stamina
{
    public interface ISubtractionStamina
    {
        int CurrentStamina { get; }
        void SubtractionStamina(int countSubtraction);
    }
}