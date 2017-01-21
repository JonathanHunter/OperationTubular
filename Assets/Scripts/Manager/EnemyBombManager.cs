using UnityEngine;
using Assets.Scripts.Bullets;
using System;

namespace Assets.Scripts.Manager
{
    class EnemyBombManager : MonoBehaviour, Util.PlaysOnBeat
    {
        public static EnemyBombManager instance;

        public Vector2 gridSize = new Vector2(4, 12);
        public Bomb prefab;
        public int length;

        private Vector2 cellSize;

        private Bomb[] bombs;
        private Bomb[,] grid;
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
            grid = new Bomb[(int)gridSize.x, (int)gridSize.y];
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
            bombs = new Bomb[length];
            for (int i = 0; i < length; i++)
            {
                bombs[i] = Instantiate(prefab);
                bombs[i].index = i;
                bombs[i].manager = this;
                bombs[i].transform.position = standbyPos;
                bombs[i].gameObject.SetActive(false);
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
                bombs[index].gameObject.SetActive(true);
                bombs[index].transform.position = 
                    new Vector3(c * cellSize.y - GameManager.xBounds.y, -r * cellSize.x - GameManager.yBounds.y, bombs[index].transform.position.z);
                bombs[index].Init();
                grid[r, c] = bombs[index];

            }
            if (grid[r, c] != null)
                return grid[r, c].transform.position;
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
            for (int i = 0; i < bombs.Length; i++)
            {
                if (bombs[i].gameObject.activeSelf == false)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Recover(int index)
        {
            bombs[index].transform.position = standbyPos;
            bombs[index].gameObject.SetActive(false);
        }
    }
}
