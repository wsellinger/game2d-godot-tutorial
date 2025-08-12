using Godot;

namespace Game2D
{
    internal class Utils
    {
        public const float PI_HALF = Mathf.Pi / 2;
        public const float PI_QUARTER = Mathf.Pi / 4;

        static public float RandRangef(double from, double to) => (float)GD.RandRange(from, to);
    }
}
