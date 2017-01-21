using UnityEngine;

namespace Assets.Scripts.Bullets
{
    public class Bullet : MonoBehaviour
    {
        public BulletPool manager;
        public int index;
        public bool enemyBullet;
        public float damage;

        private float min = .1f, max = 10f;
        private Vector2 begin, end;
        private float dist;
        
        public void Init(Vector2 position)
        {
            begin = gameObject.transform.position;
            end = position;
            if(enemyBullet)
                gameObject.transform.localScale = new Vector3(min, min, 1);
            else
                gameObject.transform.localScale = new Vector3(max, max, 1);
        }

        private void Update()
        {
            gameObject.transform.position = Vector2.Lerp(begin, end, dist);
            if(dist < 1)
            {
                dist += Time.deltaTime;
                if (enemyBullet)
                    gameObject.transform.localScale = 
                        new Vector3(Lerp(min, max, dist), Lerp(min, max, dist), 1);
                else
                    gameObject.transform.localScale = 
                        new Vector3(Lerp(max, min, dist), Lerp(max, min, dist), 1);
            }
        }

        private float Lerp(float min, float max, float f)
        {
            return min + f * (max - min);
        }
    }
}
