using UnityEngine;

namespace Assets.Scripts.Manager
{
    class KrakenManager : MonoBehaviour, Util.PlaysOnBeat
    {
        public Enemies.THE_KRAKEN kraken;
        public int songBeatDelay;

        public static KrakenManager instance;

        private int beats;
        private bool introPlaying;
        private Enemies.THE_KRAKEN krakenObj;
        private bool checkLive;

        private void Start()
        {
            if (instance == null)
            {
                //DontDestroyOnLoad(this.gameObject);
                instance = this;
            }
            else if (this != instance)
            {
                Destroy(this.gameObject);
                return;
            }
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            Util.TempoManager.instance.objects.Add(this);
        }

        private void Update()
        {
            if(checkLive)
            {
                if(krakenObj.isDead)
                {
                    MusicManager.instance.ChangeMusic(MusicManager.song.bossVictory, true);
                    checkLive = false;
                    Environment.NightManager.instance.ToDay();
                }
            }
        }

        public void SpawnKraken()
        {
            krakenObj = Instantiate(kraken);
            MusicManager.instance.ChangeMusic(MusicManager.song.bossApproach, false);
            beats = 0;
            introPlaying = true;
            checkLive = true;
        }

        public void PlayOnBeat()
        {
            if (introPlaying)
            {
                if (beats > songBeatDelay)
                {
                    MusicManager.instance.ChangeMusic(MusicManager.song.boss, true);
                    introPlaying = false;
                }
                else
                    beats++;
            }
        }
    }
}
