using UnityEngine;

namespace Assets.Scripts.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public BulletPool manager;
        public int index;
        public bool enemyBullet;
        public float damage;
        public float min = .1f;
        public float max = 10f;
        public float spinDir;
        public float speed;

        private Vector2 begin, end;
        private float dist;
        private bool doOnce;

        public void Init(Vector2 position)
        {
            begin = gameObject.transform.position;
            end = position;
            if (enemyBullet)
                gameObject.transform.localScale = new Vector3(min, min, 1);
            else
                gameObject.transform.localScale = new Vector3(max, max, 1);
            GetComponent<Collider2D>().enabled = false;
            doOnce = false;
            dist = 0;
        }

        private void Update()
        {
            if (dist > .90 && !doOnce)
            {
                doOnce = true;
                GetComponent<Collider2D>().enabled = true;
            }
            if (dist < 1)
            {
                dist += Time.deltaTime * speed;
                gameObject.transform.position = Vector2.Lerp(begin, end, dist);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, dist * 360f * spinDir));
                if (enemyBullet)
                    gameObject.transform.localScale =
                        new Vector3(Util.Lerp1D.Lerp(min, max, dist), Util.Lerp1D.Lerp(min, max, dist), 1);
                else
                    gameObject.transform.localScale =
                        new Vector3(Util.Lerp1D.Lerp(max, min, dist), Util.Lerp1D.Lerp(max, min, dist), 1);
            }
            else
            {
                manager.Recover(index);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            manager.Recover(index);
        }
    }
}
