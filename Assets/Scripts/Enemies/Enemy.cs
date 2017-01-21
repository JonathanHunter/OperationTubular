using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Enemies
{
    abstract class Enemy : MonoBehaviour
    {
        public float maxHealth;
        public GameObject explosion;

        protected float health;

        private bool paused;
        private bool dead;
        private bool hit;
        private float animSpeed;
        private float g;
        private float damage;
        private Vector2 vel;

        private void Start()
        {
            health = maxHealth;
            paused = false;
            dead = false;
            animSpeed = 0;
            g = 0;
            vel = Vector2.zero;
            Init();
        }

        private void Update()
        {
            if (!GameManager.IsPaused)
            {
                if (paused)
                {
                    paused = false;
                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                        anim.speed = animSpeed;
                    Rigidbody2D rgby2D = GetComponent<Rigidbody2D>();
                    if (rgby2D != null)
                    {
                        rgby2D.gravityScale = g;
                        rgby2D.velocity = vel;
                    }
                }
                if (hit)
                {
                    health -= damage;
                    hit = false;
                    if (health <= 0)
                    {
                        SFXManager.instance.Spawn("EnemyGetHit");
                        dead = true;
                    }
                }
                if (dead)
                {
                    SFXManager.instance.Spawn("EnemyDie");
                    Die();
                }
                else
                    Run();
            }
            else
            {
                if (!paused)
                {
                    Animator anim = GetComponent<Animator>();
                    if (anim != null)
                    {
                        animSpeed = anim.speed;
                        anim.speed = 0.0000001f;
                    }
                    Rigidbody2D rgby2D = GetComponent<Rigidbody2D>();
                    if (rgby2D != null)
                    {
                        g = rgby2D.gravityScale;
                        rgby2D.gravityScale = 0;
                        vel = rgby2D.velocity;
                        rgby2D.velocity = Vector2.zero;
                    }
                    paused = true;
                }
            }
        }

        protected abstract void Init();

        protected abstract void Run();

        protected abstract void Die();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Bullet")
            {
                hit = true;
                damage = collision.gameObject.GetComponent<Bullets.Bullet>().damage;
            }
            if (collision.gameObject.tag == "Shockwave")
            {
                hit = true;
                damage = 1000000;
            }
        }
    }
}
