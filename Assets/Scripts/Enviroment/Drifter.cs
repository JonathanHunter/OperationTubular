using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    class Drifter : MonoBehaviour
    {
        public Vector2 bounds;
        public float speed;

        private float dist;
        private bool left;

        private void Start()
        {
            dist = Util.SinusoidalRandom.Range(0, 1);
            left = Util.SinusoidalRandom.Range(0, 2) < 1;
            transform.position = new Vector3(Util.Lerp1D.Lerp(bounds.x, bounds.y, dist), transform.position.y, transform.position.z);
        }

        private void Update()
        {
            if (transform.position.x > bounds.y)
                dist = 0;
            dist += Time.deltaTime * speed;
            transform.position = new Vector3(Util.Lerp1D.Lerp(bounds.x, bounds.y, dist), transform.position.y, transform.position.z);
        }
    }
}
