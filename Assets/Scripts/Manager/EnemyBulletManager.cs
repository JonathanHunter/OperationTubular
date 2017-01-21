using UnityEngine;

namespace Assets.Scripts.Manager
{
    class EnemyBulletManager : Bullets.BulletPool
    {
        public static EnemyBulletManager instance;

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
    }
}