namespace Structs
{
    //[System.Serializable]
    public struct Score
    {
        public int Attempts;
        public int TimesDamaged;
        public int EnemiesKilled;
        public int PowerUpsPickedUp;
        public int TrapsOpened;
        public int TotalScore;
        public float TotalTime;

        public void Clear()
        {
            TimesDamaged = 0;
            EnemiesKilled = 0;
            PowerUpsPickedUp = 0;
            TrapsOpened = 0;
            TotalScore = 0;
            TotalTime = 0;
        }
    }
}