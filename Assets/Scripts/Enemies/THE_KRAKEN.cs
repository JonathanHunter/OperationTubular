using UnityEngine;

namespace Assets.Scripts.Enemies
{
    class THE_KRAKEN : Enemy, Util.PlaysOnBeat
    {
        public Animator anim;
        public KRAKEN_TENTACLE[] tentacles;
        public float min = .1f;
        public float max = 10f;
        public float arrivalSpeed;
        public float swipeTime;
        public GameObject swipeTentacle;
        public float swipeSpeed;

        private float dist;
        private float dist2;
        private float[] smashTimers;
        private bool doOnce;
        private bool doOnce2;
        private bool doOnce3;
        private bool doOnce4;
        private int phase;
        private float swipe;
        private bool doSwipe;
        private int shots;
        private bool left;
        private Vector2 gridLeft, gridRight;

        public bool isDead;

        protected override void Die()
        {
            isDead = true;
            health = 1;
            damage = 0;
            hit = false;
            dead = false;
            dist = 0;
        }

        protected override void Init()
        {
            gameObject.transform.localScale = new Vector3(max, max, 1);
            GetComponent<Collider2D>().enabled = false;
            dist = 0;
            dist2 = -.5f;
            doOnce = false;
            doOnce2 = false;
            doOnce3 = false;
            doOnce4 = false;
            swipe = swipeTime;
            smashTimers = new float[tentacles.Length];
            for (int i = 0; i < tentacles.Length; i++)
            {
                smashTimers[i] = Util.SinusoidalRandom.Range(0, 1);
                tentacles[i].gameObject.SetActive(false);
            }
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
            swipeTentacle.transform.position = new Vector3(-1000000, 0, 0);
            if (Manager.EnemyBombManager.instance == null)
                FindObjectOfType<Manager.EnemyBombManager>().Init();
            gridLeft = new Vector2(0 * Manager.EnemyBombManager.instance.cellSize.y - Manager.GameManager.xBounds.y, -2 * Manager.EnemyBombManager.instance.cellSize.x - Manager.GameManager.yBounds.y);
            gridRight = new Vector2(11 * Manager.EnemyBombManager.instance.cellSize.y - Manager.GameManager.xBounds.y, -2 * Manager.EnemyBombManager.instance.cellSize.x - Manager.GameManager.yBounds.y);
        }

        protected override void Run()
        {
            if (isDead)
            {
                dist += Time.deltaTime * arrivalSpeed;
                Vector2 v = Vector2.LerpUnclamped(new Vector2(0, 2.39f), new Vector2(0, -10), dist);
                transform.position = new Vector3(v.x, v.y, transform.position.z);
                if(dist >= 1)
                {
                    Util.TempoManager.instance.objects.Remove(this);
                    Destroy(this.gameObject);
                }
            }
            else
            {

                if (dist >= 1 && !doOnce)
                {
                    doOnce = true;
                    phase = 1;
                }
                if (dist < 1)
                {
                    dist += Time.deltaTime * arrivalSpeed;
                    Vector2 v = Vector2.LerpUnclamped(new Vector2(0, -10), new Vector2(0, 2.39f), dist);
                    transform.position = new Vector3(v.x, v.y, transform.position.z);
                }
                else
                {
                    if (phase == 1)
                    {
                        if (!doOnce2)
                        {
                            GetComponent<Collider2D>().enabled = false;
                            foreach (KRAKEN_TENTACLE kt in tentacles)
                            {
                                kt.dead = false;
                                kt.health = kt.maxHealth;
                                kt.gameObject.SetActive(true);
                                kt.anim.SetInteger("state", 0);
                            }
                            doOnce2 = true;
                            doOnce4 = false;
                        }
                    }
                    if ((swipe -= Time.deltaTime) <= 0)
                    {
                        if (!doOnce3)
                        {
                            left = Util.SinusoidalRandom.Range(0, 2) > 1;
                            if (left)
                                swipeTentacle.transform.localScale = new Vector3(1, 1, 1);
                            else
                                swipeTentacle.transform.localScale = new Vector3(-1, 1, 1);
                            doSwipe = true;
                            doOnce3 = true;
                        }
                        dist2 += Time.deltaTime * swipeSpeed;
                        if (!left)
                            swipeTentacle.transform.position = Vector2.LerpUnclamped(gridLeft, gridRight, dist2);
                        else
                            swipeTentacle.transform.position = Vector2.LerpUnclamped(gridRight, gridLeft, dist2);
                        if (dist2 > 1.5 && shots > 10)
                        {
                            swipe = swipeTime;
                            doSwipe = false;
                            shots = 0;
                            swipeTentacle.transform.position = new Vector3(-1000000, 0, 0);
                            doOnce3 = false;
                            dist2 = -.5f;
                        }

                    }
                    if (phase == 2)
                    {
                        if (!doOnce4)
                        {
                            foreach (KRAKEN_TENTACLE kt in tentacles)
                                kt.dead = false;
                            doOnce2 = false;
                            GetComponent<Collider2D>().enabled = true;
                            if (health < 2)
                                anim.SetInteger("phase", 3);
                            else
                                anim.SetInteger("phase", 2);
                        }
                    }
                }
            }
        }

        public void PlayOnBeat()
        {
            if (phase == 1)
            {
                int count = 0;
                for (int i = 0; i < tentacles.Length; i++)
                {
                    if (!tentacles[i].dead)
                    {
                        if ((smashTimers[i] -= Time.deltaTime) <= 0)
                        {
                            tentacles[i].DoAttack();
                            smashTimers[i] = Util.SinusoidalRandom.Range(0, 1);
                        }
                    }
                    else
                        count++;
                }
                if (count == tentacles.Length)
                    phase++;
            }
            if (doSwipe)
            {
                if (shots < 11)
                {
                    if (left)
                        Manager.EnemyBombManager.instance.SpawnAt(3, 11 - shots);
                    else
                        Manager.EnemyBombManager.instance.SpawnAt(3, shots);
                    shots++;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isDead)
            {
                if (collision.tag == "Bullet")
                {
                    anim.SetInteger("phase", 1);
                    phase = 1;
                    damage = 1;
                    hit = true;
                    Manager.SFXManager.instance.Spawn("KrakenGetHit");
                }
            }
        }
    }
}
