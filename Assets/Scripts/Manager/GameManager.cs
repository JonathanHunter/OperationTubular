using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Manager
{
    class GameManager : MonoBehaviour
    {
        private static bool paused;

        public static bool IsPaused { get { return paused; } set { paused = value; } }

        public static float horizontalMax = 9;
        public static float verticalMax = 5;

        public static Vector2 xBounds = new Vector2(-horizontalMax, horizontalMax);
        public static Vector2 yBounds = new Vector2(verticalMax, -verticalMax);

        public static GameManager instance;

        public GameObject[] players;

        private void Start()
        {
            //if (instance == null)
            //{
                //DontDestroyOnLoad(this.gameObject);
                instance = this;
            //}
            //else if (this != instance)
            //{
            //    Destroy(this.gameObject);
            //    return;
            //}
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
                GameOverManager.instance.show = true;
            }
        }

        public void LoadLevel()
        {
            paused = false;
            SceneManager.LoadScene("Game");
            MusicManager.instance.ChangeMusic(MusicManager.song.level, true);
        }

        public void LoadLevelSingle()
        {
            paused = false;
            SceneManager.LoadScene("Game 1");
            MusicManager.instance.ChangeMusic(MusicManager.song.level, true);
        }

        public void LoadMenu()
        {
            paused = false;
            MusicManager.instance.ChangeMusic(MusicManager.song.mainMenu, true);
            SceneManager.LoadScene("Menu");
        }

        public void LoadTitle()
        {
            paused = false;
            SceneManager.LoadScene("Title");
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
