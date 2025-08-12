using Godot;

namespace Game2D;

public partial class DroneMob : Mob
{
    private const double MIN_VELOCITY = 150.0;
    private const double MAX_VELOCITY = 250.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Rotation += Utils.PI_HALF + Utils.RandRangef(-Utils.PI_QUARTER, Utils.PI_QUARTER);
        LinearVelocity = new Vector2(Utils.RandRangef(MIN_VELOCITY, MAX_VELOCITY), 0).Rotated(Rotation);
    }

    //TODO should these get a random curve to their path?
}
