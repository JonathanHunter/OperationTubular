using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Enemies
{
    class Helicopter : Enemy
    {
        public float shootTime;
        public float numShots;
        public float delay;
        public float min = .1f;
        public float max = 10f;
        public float arrivalSpeed;
        public float movementSpeed;

        private float dist;
        private float shootTimer;
        private float shots;
        private bool left;
        private bool doOnce;

        protected override void Die()
        {
            Destroy(this.gameObject);
        }

        protected override void Init()
        {
            gameObject.transform.localScale = new Vector3(min, min, 1);
            left = false;
            GetComponent<Collider2D>().enabled = false;
            dist = 0;
            shootTimer = 0;
            shots = 0;
            doOnce = false;
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

                if((shootTimer += Time.deltaTime) > shootTime)
                {
                    shots++;
                    if(shots < numShots)
                        shootTimer = shootTime - delay;
                    else
                    {
                        shootTimer = 0;
                        shots = 0;
                    }
                    int i = (int)Util.SinusoidalRandom.Range(0, GameManager.instance.players.Length);
                    EnemyBulletManager.instance.Spawn(this.transform.position, GameManager.instance.players[i].transform.position);
                }
            }
        }
    }
}
