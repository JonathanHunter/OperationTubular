using UnityEngine;

namespace Assets.Scripts.Enemies
{
    class KRAKEN_TENTACLE : MonoBehaviour
    {
        public float speed;
        public int maxHealth;
        public Animator anim;
        public bool dead;

        public int health;

        public float invulerabilityTime = 1f;
        private float invulerability;
        private bool render;
        private bool hit;
        private int damage;

        enum State { Intro, Idle, Prep, Smash};
        private State state;
        private bool attack;

        private void Start()
        {
            health = maxHealth;
        }

        public void playSound()
        {
            Manager.SFXManager.instance.Spawn("KrakenAttack");
        }

        public void SmashDone()
        {
            state = State.Idle;
            anim.SetInteger("state", (int)state);
        }

        public void IntroDone()
        {
            state = State.Idle;
            anim.SetInteger("state", (int)state);
        }

        public void PrepDone()
        {
            state = State.Smash;
            anim.SetInteger("state", (int)state);
            attack = true;
        }

        public void DoAttack()
        {
            state = State.Prep;
            anim.SetInteger("state", (int)state);
        } 

        private void Update()
        {
            if (hit)
            {
                if (invulerability <= 0)
                {
                    health -= damage;
                    invulerability = invulerabilityTime;
                    Manager.SFXManager.instance.Spawn("EnemyGetHit");
                }
                hit = false;
                damage = 0;
            }
            if (invulerability > 0)
            {
                render = !render;
                GetComponent<SpriteRenderer>().enabled = render;
                invulerability -= Time.deltaTime;
            }
            else if (!render)
            {
                render = true;
                GetComponent<SpriteRenderer>().enabled = render;
            }
            if (health <= 0)
            {
                dead = true;
                gameObject.SetActive(false);
            }
            if(attack)
            {
                attack = false;
                int y = (int)(((transform.position.x - Manager.GameManager.xBounds.x) / (Manager.GameManager.xBounds.y - Manager.GameManager.xBounds.x)) * Manager.EnemyBombManager.instance.gridSize.y);
                Manager.EnemyBombManager.instance.SpawnAt(0, y);
                Manager.EnemyBombManager.instance.SpawnAt(1, y);
                Manager.EnemyBombManager.instance.SpawnAt(2, y);
                Manager.EnemyBombManager.instance.SpawnAt(3, y);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                hit = true;
                damage = (int)collision.gameObject.GetComponent<Bullets.Bullet>().damage;
            }
            if (collision.gameObject.tag == "Shockwave")
            {
                hit = true;
                damage = 1000000;
            }
        }
    }
}