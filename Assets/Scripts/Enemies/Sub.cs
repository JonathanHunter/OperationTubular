using UnityEngine;
using Assets.Scripts.Manager;
using Assets.Scripts.Util;

namespace Assets.Scripts.Enemies
{
    class Sub : Enemy, Util.PlaysOnBeat
    {
        public Animator anim;
        public float min = .1f;
        public float max = 10f;
        public float arrivalSpeed;
        public float movementSpeed;

        private float dist;
        private float shootTime;
        private float shootTimer;
        private bool left;
        private bool doOnce;
        private bool doOnce2;
        private int y;
        private int shots;

        public void Submerged()
        {
            if (shots >= 3)
                Die();
        }

        protected override void Die()
        {
            Util.TempoManager.instance.objects.Remove(this);
            Destroy(this.gameObject);
        }

        protected override void Init()
        {
            gameObject.transform.localScale = new Vector3(min, min, ZLayer.EnemyZ);
            left = Util.SinusoidalRandom.Range(0, 2) > 1;
            GetComponent<Collider2D>().enabled = false;
            dist = 0;
            shootTime = Util.SinusoidalRandom.Range(0, 2);
            shootTimer = 0;
            doOnce = false;
            doOnce2 = false;
            shots = 0;
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
            y = -1;
        }

        protected override void Run()
        {
            if (dist > .25 && !doOnce)
            {
                doOnce = true;
                GetComponent<Collider2D>().enabled = true;
            }
            if (dist < 1)
            {
                dist += Time.deltaTime * arrivalSpeed;
                gameObject.transform.localScale =
                    new Vector3(Util.Lerp1D.Lerp(min, max, dist), Util.Lerp1D.Lerp(min, max, dist), 1);
            }
            else
            {
                if (transform.position.x < GameManager.xBounds.x)
                    left = false;
                else if (transform.position.x > GameManager.xBounds.y)
                    left = true;
                if (left)
                    transform.Translate(Vector2.left * Time.deltaTime * movementSpeed);
                else
                    transform.Translate(Vector2.right * Time.deltaTime * movementSpeed);

                if ((shootTimer += Time.deltaTime) > shootTime)
                {
                    if (shots >= 3)
                        Die();
                    if (!doOnce2)
                    {
                        doOnce2 = true;
                        y = Mathf.Abs((int)Util.SinusoidalRandom.Range(1f, EnemyBombManager.instance.gridSize.y));
                        anim.SetTrigger("Sink");
                        GetComponent<Collider2D>().enabled = false;
                    }
                }
            }
        }

        public void PlayOnBeat()
        {
            if (shots < 3 & y > -1)
            {
                if (y < 1)
                    y = 1;
                if (y > 11)
                    y = 11;
                EnemyBombManager.instance.SpawnAt(3 - shots, y);
                EnemyBombManager.instance.SpawnAt(3 - shots, y - 1);
                shots++;
                if(shots >= 3)
                {
                    EnemyGeyserManager.instance.SpawnAt(3, y);
                    shootTimer = 0;
                }
            }
        }
    }
}
