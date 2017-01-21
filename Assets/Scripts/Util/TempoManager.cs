using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Util
{
    public class TempoManager : MonoBehaviour
    {



        public int bpm = 120;
        public float beat, timer;

        // Use this for initialization
        void Start()
        {
            SetBeat(bpm);
        }

        public void SetBeat(int bpm)
        {
            this.bpm = bpm;
            beat = (60f / bpm);
            timer = beat;
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                Beat();
                timer = beat;
            }
        }

        public void Beat()
        {
            //TODO PUT DA CODE HERE JONNY BOY
        }
    }
}
