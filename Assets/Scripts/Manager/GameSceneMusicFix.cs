using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Manager
{

    public class GameSceneMusicFix : MonoBehaviour
    {
        MusicManager music;

        // Use this for initialization
        void Start()
        {
            music = FindObjectOfType<MusicManager>();
            if (Util.TempoManager.instance == null)
                FindObjectOfType<Util.TempoManager>().Init();
            music.beat = Util.TempoManager.instance.beat;
            Util.TempoManager.instance.objects.Add(music);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
