using Godot;

namespace Game2D;

public partial class SeekerMob : Mob
{
	public Node2D Target { private get; set; }

	private const int SPEED = 100;
	private const float ANGULAR_VELOCITY = 0.02f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

        LookAt(Target.Position);
    }

	//TODO add blue throb animation to seekers
	//TODO do seekers ever leave the screen?

    public override void _PhysicsProcess(double delta)
    {
        if (Target is not null)
		{
            var targetAngle = (Target.GlobalPosition - GlobalPosition).Angle();
			var angleDiff = Mathf.Abs(Rotation - targetAngle);

			if (angleDiff < ANGULAR_VELOCITY)
			{
				Rotation = targetAngle;
			}
			else if (targetAngle > Rotation)
			{
				Rotation += ANGULAR_VELOCITY;
			}
			else if (targetAngle < Rotation)
			{
				Rotation -= ANGULAR_VELOCITY;
			}

            LinearVelocity = new Vector2(SPEED, 0).Rotated(Rotation);
        }
    }
}
