using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Manager
{
    public class MusicManager : MonoBehaviour, Util.PlaysOnBeat
    {

        public static MusicManager instance;

        public AudioSource mainMenu, level, bossApproach, boss, victory;
        private AudioSource source, newSong;
        public enum song { mainMenu, level, bossApproach, boss, bossVictory};
        float beat;
        bool changeSong = false, fadeOut = false, playImmediately = false;
        float fadeTimer = 0;

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
            source = GetComponent<AudioSource>();
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            beat = Util.TempoManager.instance.beat;
            Util.TempoManager.instance.objects.Add(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (fadeOut)
            {
                if (fadeTimer > 0)
                {
                    fadeTimer -= Time.deltaTime;
                    source.volume = (fadeTimer / (beat * 16));
                }
                else
                {
                    PlayNewSong();
                    fadeOut = false;
                }
            }
        }

        public void ChangeMusic(song nextSong, bool playImmediately)
        {
            changeSong = true;
            this.playImmediately = playImmediately;
            switch (nextSong) {
                case song.mainMenu:
                    newSong = mainMenu;
                    newSong.loop = true;
                    break;
                case song.level:
                    newSong = level;
                    newSong.loop = true;
                    break;
                case song.bossApproach:
                    newSong = bossApproach;
                    newSong.loop = false;
                    break;
                case song.boss:
                    newSong = boss;
                    newSong.loop = true;
                    break;
                case song.bossVictory:
                    newSong = victory;
                    newSong.loop = false;
                    break;
            }
        }

        void PlayNewSong()
        {
            source.Stop();
            source.clip = newSong.clip;
            source.volume = newSong.volume;
            source.Play();
            Callback();
        }

        void Callback()
        {
            //Do stuff here
        }

        public void PlayOnBeat()
        {
            if (playImmediately)
            {
                PlayNewSong();
                playImmediately = false;
                changeSong = false;
            }
            else if (changeSong)
            {
                changeSong = false;
                fadeOut = true;
                fadeTimer = beat * 16;
            }
        }
    }
}
