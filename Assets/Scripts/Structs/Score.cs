namespace Structs
{
    public struct Score
    {
        public int Attempts;
        
        public int DamageTaken;
        public int EnemiesKilled;
        public int AxesThrown;
        public int PowerUpsPickedUp;
        public int TrapsOpened;
        
        public int TotalScore;

        public void Clear()
        {
            DamageTaken = 0;
            EnemiesKilled = 0;
            AxesThrown = 0;
            PowerUpsPickedUp = 0;
            TrapsOpened = 0;
            
            TotalScore = 0;
        }
    }
}