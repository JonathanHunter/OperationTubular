using UnityEngine;
using Assets.Scripts.Manager;

namespace Assets.Scripts.Enemies
{
    class Helicopter : Enemy
    {
        private float min = .1f, max = 10f;
        private float dist;
        private bool left;

        protected override void Die()
        {
            Destroy(this.gameObject);
        }

        protected override void Init()
        {
            gameObject.transform.localScale = new Vector3(min, min, 1);
            left = false;
        }

        protected override void Run()
        {
            if (dist < 1)
            {
                dist += Time.deltaTime;
                gameObject.transform.localScale =
                    new Vector3(Lerp(min, max, dist), Lerp(min, max, dist), 1);
            }
            else
            {
                if (transform.position.x < GameManager.xBounds.x)
                    left = false;
                else if (transform.position.x > GameManager.xBounds.y)
                    left = true;
                if (left)
                    transform.Translate(Vector2.left * Time.deltaTime);
                else
                    transform.Translate(Vector2.right * Time.deltaTime);
            }
        }
    }
}
