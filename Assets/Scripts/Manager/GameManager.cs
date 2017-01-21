using UnityEngine;

namespace Assets.Scripts.Manager
{
    class GameManager : MonoBehaviour
    {
        private static bool paused;

        public static bool IsPaused { get { return paused; } }        
    }
}
