using Godot;

namespace Game2D;

public partial class DroneMob : Mob
{
    private const double MIN_VELOCITY = 100.0;
    private const double MAX_VELOCITY = 200.0;

    public override void _Ready()
    {
        Rotation += Utils.PI_HALF + Utils.RandRangef(-Utils.PI_QUARTER, Utils.PI_QUARTER);
        LinearVelocity = new Vector2(Utils.RandRangef(MIN_VELOCITY, MAX_VELOCITY), 0).Rotated(Rotation);
    }
}
