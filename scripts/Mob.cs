using Godot;

namespace Game2D;

public partial class Mob : RigidBody2D
{
    private AnimatedSprite2D _sprite;
    private CollisionShape2D _collision;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite2D>("Sprite");
        _collision = GetNode<CollisionShape2D>("Collision");

        string[] mobTypes = _sprite.SpriteFrames.GetAnimationNames();
        _sprite.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}
