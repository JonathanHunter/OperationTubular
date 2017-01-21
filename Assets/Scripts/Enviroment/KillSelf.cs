using UnityEngine;

namespace Assets.Scripts.Enviroment
{
    class KillSelf : MonoBehaviour
    {
        public void Die()
        {
            Destroy(this.gameObject);
        }
    }
}
