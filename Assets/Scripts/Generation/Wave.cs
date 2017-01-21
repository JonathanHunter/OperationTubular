using UnityEngine;

namespace Assets.Scripts.Generation
{
    class Wave : MonoBehaviour
    {
        public WaveUnit[] units;

        public void Spawn()
        {
            foreach(WaveUnit u in units)
            {
                GameObject obj = Instantiate(u.unit);
                obj.transform.position = new Vector3(u.transform.localPosition.x, u.transform.localPosition.y, u.transform.localPosition.z);
            }
        }
    }
}
