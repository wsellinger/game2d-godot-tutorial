using Godot;

namespace Game2D;

public partial class RocketMob : TargetedMob
{
    private const int LINEAR_VELOCITY = 400;

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
