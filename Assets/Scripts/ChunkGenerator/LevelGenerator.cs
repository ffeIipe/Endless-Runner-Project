using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChunkGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        public List<LevelChunkData> levelChunkData;
        public LevelChunkData firstChunk;
        public Transform spawnOrigin;

        private Vector3 _spawnPosition;
        public int chunksToSpawn = 6;

        private LevelChunkData _previousChunk;

        private void Start()
        {
            _previousChunk = firstChunk;
            for (var i = 0; i < chunksToSpawn; i++)
            {
                PickAndSpawnChunk();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                PickAndSpawnChunk();
            }
        }

        private void PickAndSpawnChunk()
        {
            var chunkToSpawn = PickNextChunk();
            var objectFromChunk = chunkToSpawn.levelChunks[Random.Range(0, chunkToSpawn.levelChunks.Length)];
            _previousChunk = chunkToSpawn;
            Instantiate(objectFromChunk, _spawnPosition + spawnOrigin.position, Quaternion.identity);
        }

        private LevelChunkData PickNextChunk()
        {
            var allowedChunks = new List<LevelChunkData>();
            LevelChunkData nextChunk = null;
            var nextRequiredDirection = LevelChunkData.Direction.North;

            switch (_previousChunk.exitDirection)
            {
                case LevelChunkData.Direction.North:
                    nextRequiredDirection = LevelChunkData.Direction.South;
                    _spawnPosition += new Vector3(0f, 0f, _previousChunk.chunkSize.y);
                    break;
                case LevelChunkData.Direction.East:
                    nextRequiredDirection = LevelChunkData.Direction.West;
                    _spawnPosition += new Vector3(_previousChunk.chunkSize.x, 0f, 0f);
                    break;
                case LevelChunkData.Direction.South:
                    nextRequiredDirection = LevelChunkData.Direction.North;
                    _spawnPosition += new Vector3(0f, 0f, -_previousChunk.chunkSize.y);
                    break;
                case LevelChunkData.Direction.West:
                    nextRequiredDirection = LevelChunkData.Direction.East;
                    _spawnPosition += new Vector3(-_previousChunk.chunkSize.x, 0f, 0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var chunk in levelChunkData)
            {
                if (chunk.entryDirection == nextRequiredDirection)
                {
                    allowedChunks.Add(chunk);
                }
            }

            nextChunk = allowedChunks[Random.Range(0, allowedChunks.Count)];
            return nextChunk;
        }
    }
}