using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Util
{
    public class TempoManager : MonoBehaviour
    {
        public static TempoManager instance;
        public List<PlaysOnBeat> objects;
        public int bpm = 120;
        public float beat, timer;

        private bool inited;

        // Use this for initialization
        void Start()
        {
            if (!inited)
                Init();
        }

        public void Init()
        {
            inited = true;
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
            SetBeat(bpm);
            objects = new List<PlaysOnBeat>();
        }

        public void SetBeat(int bpm)
        {
            this.bpm = bpm;
            beat = (60f / bpm)/4f;
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
                Beat16();
                timer = beat;
            }
        }

        public void Beat16()
        {
            foreach (PlaysOnBeat o in objects)
                o.PlayOnBeat();
        }
    }
}
