using Godot;

namespace Game2D;

public partial class TargetedMob : Mob
{
    public Node2D Target { protected get; set; }

    public override void _Ready()
    {
        base._Ready();
    }
}
