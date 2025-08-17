using Godot;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Game2D;

public partial class Player : Area2D
{
    [Signal] public delegate void HitEventHandler();

	[Export] public int MaxSpeed { get; set; } = 400;
    [Export] public int Acceleration { get; set; } = 100;
    [Export] public float Friction { get; set; } = 0.85f;

    public Vector2 ScreenSize;

    private AnimatedSprite2D _sprite;
    private CollisionShape2D _collision;

    private Vector2 _velocity;

    private const float MIN_NONZERO_VELOCITY = 0.001f;

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
        var acceleration = GetAcceleration(Acceleration);
        _velocity = GetVelocity(acceleration);

        Animate(acceleration);
        Move(fDelta, _velocity);

        //Local Methods

        static Vector2 GetAcceleration(int acceleration)
        {
            var accelerationVec = Vector2.Zero;

            if (Input.IsActionPressed(InputActions.MoveUp))
                accelerationVec.Y -= 1;

            if (Input.IsActionPressed(InputActions.MoveDown))
                accelerationVec.Y += 1;

            if (Input.IsActionPressed(InputActions.MoveLeft))
                accelerationVec.X -= 1;

            if (Input.IsActionPressed(InputActions.MoveRight))
                accelerationVec.X += 1;

            return accelerationVec.Normalized() * acceleration;
        }

        Vector2 GetVelocity(Vector2 acceleration)
        {
            if (acceleration != Vector2.Zero)
            {                
                return (_velocity + acceleration).LimitLength(MaxSpeed);
            }
            else if (_velocity.Length() > MIN_NONZERO_VELOCITY)
            {
                return _velocity * Friction;
            }
            else
            {
                return Vector2.Zero;
            }

        }

        void Animate(Vector2 acceleration)
        {
            if (acceleration.X != 0)
            {
                _sprite.Animation = AnimationNames.WALK;
                _sprite.FlipH = acceleration.X < 0;
                _sprite.FlipV = false;
                _sprite.Play();
            }
            else if (acceleration.Y != 0) 
            { 
                _sprite.Animation = AnimationNames.UP; 
                _sprite.FlipV = acceleration.Y > 0;
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
