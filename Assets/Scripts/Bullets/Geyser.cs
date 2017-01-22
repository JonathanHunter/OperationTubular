using UnityEngine;

namespace Assets.Scripts.Bullets
{
    class Geyser : MonoBehaviour
    {
        public Animator anim;
        public Manager.EnemyGeyserManager manager;
        public int index;
        public int damage;

        private int countDown;

        public void Die()
        {
            manager.Recover(index);
        }

        public void Init()
        {
            countDown = 3;
            GetComponent<Collider2D>().enabled = false;
            anim.SetInteger("Count", countDown);
        }

        public bool PlayOnBeat()
        {
            countDown--;
            anim.SetInteger("Count", countDown);
            if (countDown == 0)
            {
                GetComponent<Collider2D>().enabled = true;
                return true;
            }
            return false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Player")
            {
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}
