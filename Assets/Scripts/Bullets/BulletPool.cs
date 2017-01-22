using UnityEngine;
using Assets.Scripts.Util;

namespace Assets.Scripts.Bullets
{

    public class BulletPool : MonoBehaviour
    {
        public Bullet prefab;
        public int length;

        private Bullet[] bullets;
        private Vector2 standbyPos = new Vector2(-1000, 0);
        private bool inited;

        void Init()
        {
            inited = true;
            bullets = new Bullet[length];
            for (int i = 0; i < length; i++)
            {
                bullets[i] = Instantiate(prefab);
                bullets[i].index = i;
                bullets[i].manager = this;
                bullets[i].transform.position = standbyPos;
                bullets[i].gameObject.SetActive(false);
            }
        }

        private int FindAvailable()
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i].gameObject.activeSelf == false)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Spawn(Vector3 startPos, Vector2 target)
        {
            if (!inited)
                Init();
            int index = FindAvailable();
            if (index > -1)
            {
                bullets[index].gameObject.SetActive(true);
                bullets[index].transform.position = startPos;
                bullets[index].Init(target);
            }

        }

        public void SpawnFollow(Vector3 startPos, Transform target)
        {
            if (!inited)
                Init();
            int index = FindAvailable();
            if (index > -1)
            {
                bullets[index].gameObject.SetActive(true);
                bullets[index].transform.position = startPos;
                bullets[index].Init(target);
            }

        }

        public void Recover(int index)
        {
            bullets[index].transform.position = standbyPos;
            bullets[index].gameObject.SetActive(false);
        }

        private void Start()
        {
            if (!inited)
                Init();
        }
    }
}
