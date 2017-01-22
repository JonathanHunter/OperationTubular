using UnityEngine;

namespace Assets.Scripts.Util
{
    class SinusoidalRandom : MonoBehaviour
    {
        private static bool isDay;
        private static bool inited;

        private void Start()
        {
            if (!inited)
                Init();
        }

        private static void Init()
        {
            Random.InitState(System.DateTime.Now.Millisecond);
            inited = true;
            isDay = System.DateTime.Now.Hour < 12;
        }

        public static float Range(float start, float end)
        {
            if (!inited)
                Init();
            float r = Random.Range(start, end);
            if (isDay)
                r = Mathf.Sin(r) * (end - start) + start;
            else
                r = Mathf.Cos(r) * (end - start) + start;
            return r;
        }
    }
}
