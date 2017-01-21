namespace Assets.Scripts.Util
{
    public class Lerp1D
    {
        public static float Lerp(float min, float max, float f)
        {
            return min + f * (max - min);
        }
    }
}
