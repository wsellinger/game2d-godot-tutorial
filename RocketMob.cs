using Godot;

namespace Game2D;

public partial class RocketMob : TargetedMob
{

    private const int LINEAR_VELOCITY = 400;

    //TODO spawn these in and test it

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();

        if (Target is not null)
        {
            LookAt(Target.Position);
            LinearVelocity = new Vector2(LINEAR_VELOCITY, 0).Rotated(Rotation);
        }
    }
}
