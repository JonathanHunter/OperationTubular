using UnityEngine;

namespace Assets.Scripts.Bullets
{
    class Bomb : MonoBehaviour
    {
        public Animator anim;
        public Manager.EnemyBombManager manager;
        public int index;
        public int damage;

        private int countDown;

        public void ExplosionDone()
        {
            manager.Recover(index);
            Manager.SFXManager.instance.Spawn("KrakenAttack");
        }

        public void ActivateCollider()
        {
            GetComponent<Collider2D>().enabled = true;
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
