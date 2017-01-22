using UnityEngine;
using Assets.Scripts.Bullets;
using System;
using Assets.Scripts.Util;

namespace Assets.Scripts.Manager
{
    class EnemyGeyserManager : MonoBehaviour, Util.PlaysOnBeat
    {
        public static EnemyGeyserManager instance;

        public Vector2 gridSize = new Vector2(4, 12);
        public Geyser prefab;
        public int length;

        private Vector2 cellSize;

        private Geyser[] pool;
        private Geyser[,] grid;
        private Vector2 standbyPos = new Vector2(-1000, 0);
        private bool inited;

        private void Start()
        {
            if (!inited)
                Init();

        }

        void Init()
        {
            inited = true;
            if (instance == null)
            {
                //DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
                return;
            }
            grid = new Geyser[(int)gridSize.x, (int)gridSize.y];
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
            pool = new Geyser[length];
            for (int i = 0; i < length; i++)
            {
                pool[i] = Instantiate(prefab);
                pool[i].index = i;
                pool[i].manager = this;
                pool[i].transform.position = standbyPos;
                pool[i].gameObject.SetActive(false);
            }
            cellSize = new Vector2((GameManager.yBounds.x - GameManager.yBounds.y) / grid.GetLength(0), (GameManager.xBounds.y - GameManager.xBounds.x) / grid.GetLength(1));
        }

        public Vector2 SpawnAt(int r, int c)
        {
            if (!inited)
                Init();
            int index = FindAvailable();
            if (index > -1 && grid[r, c] == null)
            {
                pool[index].gameObject.SetActive(true);
                pool[index].transform.position = 
                    new Vector3(c * cellSize.y - GameManager.xBounds.y - 1f, -1f, ZLayer.EnemyProjectileZ);
                pool[index].Init();
                grid[r, c] = pool[index];

            };


            return Vector2.zero;
        }

        public void PlayOnBeat()
        {
            for (int r = 0; r < grid.GetLength(0); r++)
            {
                for (int c = 0; c < grid.GetLength(1); c++)
                {
                    if (grid[r, c] != null && grid[r, c].PlayOnBeat())
                        grid[r, c] = null;
                }
            }
        }

        private int FindAvailable()
        {
            for (int i = 0; i < pool.Length; i++)
            {
                if (pool[i].gameObject.activeSelf == false)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Recover(int index)
        {
            pool[index].transform.position = standbyPos;
            pool[index].gameObject.SetActive(false);
        }
    }
}
