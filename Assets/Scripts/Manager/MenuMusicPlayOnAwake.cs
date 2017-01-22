using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Manager {

    public class MenuMusicPlayOnAwake : MonoBehaviour
    {
        MusicManager music;
        // Use this for initialization
        void Start()
        {
            music = FindObjectOfType<MusicManager>();
            music.ChangeMusic(MusicManager.song.mainMenu, true, true);
            //music.GetComponent<AudioSource>().Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
