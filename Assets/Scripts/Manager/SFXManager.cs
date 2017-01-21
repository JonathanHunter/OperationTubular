using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Manager
{

    public class SFXManager : MonoBehaviour
    {
        public static SFXManager instance;

        private AudioSource[] SFXPool;
        public AudioSource prefab;
        public AudioSource[] sources;
        private Dictionary<string, AudioSource> SFXDict = new Dictionary<string, AudioSource>();
        public int length = 20;
        private bool inited;

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

        void Init()
        {
            inited = true;
            SFXPool = new AudioSource[length];
            for (int i = 0; i < length; i++)
            {
                SFXPool[i] = Instantiate(prefab);
            }
            for (int i = 0; i < sources.Length; i++)
            {
                SFXDict.Add(sources[i].name, sources[i].GetComponent<AudioSource>());
            }
        }

        private int FindAvailable()
        {
            for (int i = 0; i < SFXPool.Length; i++)
            {
                if (!SFXPool[i].GetComponent<AudioSource>().isPlaying)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Spawn(string SFXName)
        {
            if (!inited)
                Init();
            int index = FindAvailable();
            if (index > -1)
            {
                AudioSource temp;
                SFXDict.TryGetValue(SFXName, out temp);
                if (temp != null)
                {
                    SFXPool[index].GetComponent<AudioSource>().clip = temp.clip;
                    SFXPool[index].GetComponent<AudioSource>().Play();
                }
            }
            
        }

        private void Start()
        {
            if (!inited)
                Init();
        }
    }
}
