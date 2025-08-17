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

	//TODO do seekers ever leave the screen?
	//TODO turning logic is broken if you cross over 0, need to use quaternions or something

    public override void _PhysicsProcess(double delta)
    {
        if (Target is not null)
        {
            var angularVelocity = ANGULAR_VELOCITY * (float)delta;
            var targetAngle = (Target.GlobalPosition - GlobalPosition).Angle();
            var angleDiff = Mathf.Abs(Rotation - targetAngle);

            if (angleDiff < angularVelocity)
            {
                Rotation = targetAngle;
            }
            else if (targetAngle > Rotation)
            {
                Rotation += angularVelocity;
            }
            else if (targetAngle < Rotation)
            {
                Rotation -= angularVelocity;
            }

            LinearVelocity = new Vector2(LINEAR_VELOCITY, 0).Rotated(Rotation);
        }
    }
}
