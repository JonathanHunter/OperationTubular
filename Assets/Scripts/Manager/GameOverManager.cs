using UnityEngine;

namespace Assets.Scripts.Manager
{
    class GameOverManager : MonoBehaviour
    {
        public static GameOverManager instance;
        public GameObject canvas;

        public float delay;
        private float delayTimer;
        public float titleDelay;
        private float titleDelayTimer;

        public bool show;

        private void Start()
        {
            if (instance == null)
            {
                //DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(gameObject);
                return;
            }
            delayTimer = delay;
            titleDelayTimer = titleDelay;
        }

        private void Update()
        {
            if(show)
            {
                if ((delayTimer -= Time.deltaTime) <= 0)
                {
                    canvas.SetActive(true);
                    if ((titleDelayTimer -= Time.deltaTime) <= 0)
                        Manager.GameManager.instance.LoadMenu();
                }
            }
        }
    }
}
