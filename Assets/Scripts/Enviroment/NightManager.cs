using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Environment
{
    public class NightManager : MonoBehaviour
    {
        public static NightManager instance;
        public SpriteRenderer night, nightOcean;
        public ParticleSystem rain;

        void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(gameObject);
                return;
            }
        }

        // Use this for initialization
        void Start()
        {
            rain.Stop();
            night = GetComponent<SpriteRenderer>();
            //ToNight();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //Call this to trigger night
        public void ToNight()
        {
            StartCoroutine(FadeNightIn());
            rain.Play();
        }


        //Call this to trigger day
        public void ToDay()
        {
            StartCoroutine(FadeNightOut());
            rain.Stop();
        }

        private IEnumerator FadeNightIn()
        {
            for (float i = 0f; i < 1f; i += 0.05f)
            {
                night.color = new Color(night.color.r, night.color.g, night.color.b, i);
                nightOcean.color = new Color(nightOcean.color.r, nightOcean.color.g, nightOcean.color.b, i);
                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator FadeNightOut()
        {
            for (float i = 1f; i > 0f; i -= 0.05f)
            {
                night.color = new Color(night.color.r, night.color.g, night.color.b, i);
                nightOcean.color = new Color(night.color.r, night.color.g, night.color.b, i);
                yield return new WaitForSeconds(0.1f);
            }
        }



    }
}
