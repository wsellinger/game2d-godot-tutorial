using Godot;
using System.Diagnostics.CodeAnalysis;

namespace Game2D;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();

	[Export]
	public int Speed { get; set; } = 400;

	public Vector2 ScreenSize;

    private AnimatedSprite2D _sprite;
    private CollisionShape2D _collision;

    private class AnimationNames
    {
        static public readonly StringName WALK = "walk";
        static public readonly StringName UP = "up";
    }


    public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
        _sprite = GetNode<AnimatedSprite2D>("Sprite");
        _collision = GetNode<CollisionShape2D>("Collision");

        Hide();
    }

	public override void _Process(double delta)
    {
        var fDelta = (float)delta;
        var velocity = GetVelocity(Speed);
        //TODO lets make the movement juicier, maybe there's acceleration and deceleration, maybe some drifting
        Animate(velocity);
        Move(fDelta, velocity);


        //Local Methods

        static Vector2 GetVelocity(int speed)
        {
            var velocity = Vector2.Zero;

            if (Input.IsActionPressed(InputActions.MoveUp))
                velocity.Y -= 1;

            if (Input.IsActionPressed(InputActions.MoveDown))
                velocity.Y += 1;

            if (Input.IsActionPressed(InputActions.MoveLeft))
                velocity.X -= 1;

            if (Input.IsActionPressed(InputActions.MoveRight))
                velocity.X += 1;

            return velocity.Normalized() * speed;
        }

        void Animate(Vector2 velocity)
        {
            if (velocity.X != 0)
            {
                _sprite.Animation = AnimationNames.WALK;
                _sprite.FlipH = velocity.X < 0;
                _sprite.FlipV = false;
                _sprite.Play();
            }
            else if (velocity.Y != 0) 
            { 
                _sprite.Animation = AnimationNames.UP; 
                _sprite.FlipV = velocity.Y > 0;
                _sprite.Play();
            }
            else
            {
                _sprite.Stop();
            }
        }

        void Move(float fDelta, Vector2 velocity)
        {
            Position += velocity * fDelta;
            Position = new Vector2(
                x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
                y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
            );
        }
    }

    public void Start(Vector2 position)
    {
        Position = position;
        _collision.Disabled = false;
        Show();
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Signature must match")]
    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        _collision.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
