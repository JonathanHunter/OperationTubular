using UnityEngine;

namespace Assets.Scripts.Manager
{
    class GameManager : MonoBehaviour
    {
        private static bool paused;

        public static bool IsPaused { get { return paused; } }

        public static Vector2 xBounds = new Vector2(-9, 9);
        public static Vector2 yBounds = new Vector2(5, -5);
    }
}
