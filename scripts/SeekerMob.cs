using Godot;

namespace Game2D;

public partial class SeekerMob : TargetedMob
{
	private const int LINEAR_VELOCITY = 100;
	private const float ANGULAR_VELOCITY = 1f;

	public override void _Ready()
	{
		base._Ready();

        LookAt(Target.Position);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Target is not null)
        {
            var angularVelocity = ANGULAR_VELOCITY * (float)delta;
            var targetAngle = (Target.GlobalPosition - GlobalPosition).Angle();
            var angleDiff = Mathf.Wrap(targetAngle - Rotation, -Mathf.Pi, Mathf.Pi);
            
            if (Mathf.Abs(angleDiff) < angularVelocity)
            {
                Rotation = targetAngle;
            }
            else
            {
                Rotation += Mathf.Sign(angleDiff) * angularVelocity;
            }

            LinearVelocity = new Vector2(LINEAR_VELOCITY, 0).Rotated(Rotation);
        }
    }
}
