using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Bullets
{
    public class WaveGenerator : MonoBehaviour, Util.PlaysOnBeat
    {
        public Transform start, middle, end;
        public GameObject wave;
        public BoxCollider2D boxCol;

        public bool disable = false;

        bool spawnWave = false, toMiddle = false, toEnd = false;

        private float waveTime, maxWaveTime;

        private int cooldown = 0, cooldownMax = 16;

        // Use this for initialization
        void Start()
        {
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
            boxCol.enabled = false;
            maxWaveTime = Util.TempoManager.instance.beat * 4 * 4;
            waveTime = maxWaveTime / 2f;
            //Spawns a wave to test, comment out when script implemented
            //SpawnWave();
        }

        // Update is called once per frame
        void Update()
        {
            if (toMiddle)
            {
                wave.transform.position = Vector3.Lerp(start.position, middle.position, 1 - waveTime / (maxWaveTime / 2f));
                wave.transform.localScale = Vector3.Lerp(start.localScale, middle.localScale, 1 - waveTime / (maxWaveTime / 2f));
                if (waveTime > 0)
                {
                    waveTime -= Time.deltaTime;
                }
                else{
                    waveTime = maxWaveTime / 4f;
                    toMiddle = false;
                    toEnd = true;
                }
            }
            else if (toEnd)
            {
                wave.transform.position = Vector3.Lerp(middle.position, end.position, 1 - waveTime / (maxWaveTime / 4f));
                wave.transform.localScale = Vector3.Lerp(middle.localScale, end.localScale, 1 - waveTime / (maxWaveTime / 4f));
                if (waveTime > 0)
                {
                    if (waveTime > maxWaveTime / 16f)
                    {
                        boxCol.enabled = true;
                    }
                    else
                    {
                        boxCol.enabled = false;
                    }
                    waveTime -= Time.deltaTime;
                }
                else
                {
                    toEnd = false;
                    wave.SetActive(false);
                }
            }
        }

        public void SpawnWave()
        {
            spawnWave = true;
        }

        public void PlayOnBeat()
        {
            if (!disable)
            {
                if (cooldown > 0)
                {
                    cooldown--;
                }
                else if (Random.Range(0, 16) == 0)
                {
                    spawnWave = true;
                    cooldown = cooldownMax;
                }
                if (spawnWave)
                {
                    wave.SetActive(true);
                    wave.transform.position = start.position;
                    wave.transform.localScale = start.localScale;
                    spawnWave = false;
                    toMiddle = true;
                    waveTime = maxWaveTime / 2f;
                    Manager.SFXManager.instance.Spawn("WaveCrash");
                }
            }
        }

    }
}
