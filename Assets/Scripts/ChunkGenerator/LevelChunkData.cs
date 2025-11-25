using UnityEngine;

namespace ChunkGenerator
{
    [CreateAssetMenu(menuName = "LevelChunkData")]
    public class LevelChunkData : ScriptableObject
    {
        public enum Direction
        {
            North,
            East,
            South,
            West
        }

        public Vector2 chunkSize;
        public GameObject[] levelChunks;
        public Direction entryDirection;
        public Direction exitDirection;
    }
}