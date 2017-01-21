using UnityEngine;

namespace Assets.Scripts.Manager
{
    class GameManager : MonoBehaviour
    {
        private static bool paused;

        public static bool IsPaused { get { return paused; } }

        public static Vector2 xBounds = new Vector2(-9, 9);
        public static Vector2 yBounds = new Vector2(5, -5);

        public static GameManager instance;

        public GameObject[] players;

        private void Start()
        {
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
        }

        public void Remove(GameObject obj)
        {
            System.Collections.Generic.List<GameObject> objs = new System.Collections.Generic.List<GameObject>();
            foreach (GameObject g in players)
                if (g != obj)
                    objs.Add(g);
            players = objs.ToArray();
        }
    }
}
