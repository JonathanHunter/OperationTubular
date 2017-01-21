using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Generation
{
    class Generator : MonoBehaviour
    {
        public float spawnTime;
        public float difficultyTime;
        public Wave[] waves;

        public int Difficulty { get { return difficulty; } }

        private int difficulty;
        private float difficultyTimer;
        private float spawnTimer;
        private float mySpawnTime;

        private void Start()
        {
            difficulty = 3;
            difficultyTimer = 0;
            spawnTimer = 0;
            mySpawnTime = spawnTime;
        }

        private void Update()
        {
            if ((difficultyTimer += Time.deltaTime) >= difficultyTime)
            {
                mySpawnTime -= Mathf.Min(Mathf.Abs(spawnTime - Mathf.Sqrt(difficulty)), 0.02f);
                difficulty++;
                difficultyTimer = 0f;
            }
            if ((spawnTimer += Time.deltaTime) >= mySpawnTime)
            {
                waves[(int)SinusoidalRandom.Range(0, Mathf.Min(difficulty, waves.Length))].Spawn();
                spawnTimer = 0f;
            }
        }

    }
}
