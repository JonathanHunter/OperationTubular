using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager
{
    class GameManager : MonoBehaviour
    {
        private static bool paused;

        public static bool IsPaused { get { return paused; } }

        public static float horizontalMax = 9;
        public static float verticalMax = 5;

        public static Vector2 xBounds = new Vector2(-horizontalMax, horizontalMax);
        public static Vector2 yBounds = new Vector2(verticalMax, -verticalMax);

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
            if (players.Length == 0)
            {
                Debug.Log("A");
                GameOverManager.instance.show = true;
            }
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene("Game");
            MusicManager.instance.ChangeMusic(MusicManager.song.level, false);
        }

        public void LoadMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        public void LoadTitle()
        {
            SceneManager.LoadScene("Title");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
