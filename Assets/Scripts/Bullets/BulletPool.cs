using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Bullets
{

    public class BulletPool : MonoBehaviour
    {
        public Bullet prefab;
        public int length;
        private Bullet[] bullets;
        private Vector2 standbyPos = new Vector2(-1000, 0);

        void Init()
        {
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

        public void Spawn(Vector3 startPos, Vector2 target) {
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

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
