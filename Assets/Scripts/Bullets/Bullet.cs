using UnityEngine;
using Assets.Scripts.Util;

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

        public bool shouldRotate = false;
        public float spinDir;
        public float spinSped = 0.5f;
        public float speed;

        public Vector2 defaultSize = new Vector2(1f, 1f);

        private Vector2 begin, end;
        private float dist;
        private bool doOnce;
        private Transform target;

        public void Init(Vector2 position)
        {
            begin = gameObject.transform.position;
            end = new Vector2(position.x, position.y);
            if (enemyBullet)
                gameObject.transform.localScale = new Vector3(this.defaultSize.x * min, this.defaultSize.y * min, 1);
            else
                gameObject.transform.localScale = new Vector3(this.defaultSize.x * max, this.defaultSize.y * max, 1);
            GetComponent<Collider2D>().enabled = false;
            doOnce = false;
            dist = 0;
        }

        public void Init(Transform trans)
        {
            begin = gameObject.transform.position;
            target = trans;
            if (enemyBullet)
                gameObject.transform.localScale = new Vector3(this.defaultSize.x * min, this.defaultSize.y * min, 1);
            else
                gameObject.transform.localScale = new Vector3(this.defaultSize.x * max, this.defaultSize.y * max, 1);
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
                Vector2 l;
                if(target == null)
                    l = Vector2.Lerp(begin, end, dist);
                else
                    l = Vector2.Lerp(begin, target.position, dist);
                gameObject.transform.position = new Vector3(l.x, l.y, transform.position.z);

                if(this.shouldRotate){
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, dist * 360f * spinDir * spinSped * Time.deltaTime));
                } else {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }

                Vector2 resultSizeMin = new Vector2(min * this.defaultSize.x, min * this.defaultSize.y);//x min - y max
                Vector2 resultSizeMax = new Vector2(max * this.defaultSize.x, max * this.defaultSize.y);//x min - y max

                print("min " + resultSizeMin + " max " + resultSizeMax);
                if (enemyBullet)
                    gameObject.transform.localScale =
                        new Vector3(Util.Lerp1D.Lerp(resultSizeMin.x, resultSizeMax.y, dist), Util.Lerp1D.Lerp(resultSizeMin.x, resultSizeMax.y, dist), 1);
                else
                    gameObject.transform.localScale =
                        new Vector3(Util.Lerp1D.Lerp(resultSizeMax.x, resultSizeMin.y, dist), Util.Lerp1D.Lerp(resultSizeMax.x, resultSizeMin.y, dist), 1);
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
