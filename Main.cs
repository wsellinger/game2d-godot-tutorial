using Godot;

namespace Game2D;

public partial class Main : Node
{
    [Export] public PackedScene DroneMobScene { get; set; }
    [Export] public PackedScene RocketMobScene { get; set; }
    [Export] public PackedScene SeekerMobScene { get; set; }

    private int _score = 0;

    //Nodes In Scene

    private Hud _hud;
    private Player _player;
    private Marker2D _startPosition;
    private Timer _startTimer;
    private PathFollow2D _mobSpawnPoint;
    private AudioStreamPlayer _music;
    private AudioStreamPlayer _deathSound;

    //Custom Nodes

    private SpawnTimer _spawnTimer;

    private const string MOBS_GROUP_NAME = "mobs";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _hud = GetNode<Hud>("Hud");
        _player = GetNode<Player>("Player");
        _startPosition = GetNode<Marker2D>("StartPosition");
        _startTimer = GetNode<Timer>("StartTimer");
        _mobSpawnPoint = GetNode<PathFollow2D>("MobSpawnPath/MobSpawnPoint");
        _music = GetNode<AudioStreamPlayer>("Music");
        _deathSound = GetNode<AudioStreamPlayer>("DeathSound");

        _spawnTimer = new SpawnTimer();
        _spawnTimer.OnSpawnDroneMob += OnSpawnDroneMob;
        _spawnTimer.OnSpawnRocketMob += OnSpawnRocketMob;
        _spawnTimer.OnSpawnSeekerMob += OnSpawnSeekerMob;
        AddChild(_spawnTimer);
    }

    public void GameOver()
    {
        _spawnTimer.Stop();
        _music.Stop();
        _deathSound.Play();

        _ = _hud.ShowGameOver();
    }

    public void NewGame()
    {
        _score = 0;
        _hud.UpdateScore(_score);
        _ = _hud.ShowGameStart();

        _player.Start(_startPosition.Position);
        GetTree().CallGroup(MOBS_GROUP_NAME, Node.MethodName.QueueFree);

        _startTimer.Start();
        _music.Play();
    }

    private void OnStartTimerTimeout()
    {
        _spawnTimer.Start();
    }

    //TODO make timer more robust so we have a progression of slow spawning of simple mobs to a
    //     limit, then start adding in RocketMobs to a limit then SeekerMobs, then start increasing game speed?

    private void OnSpawnDroneMob()
    {
        _hud.UpdateScore(++_score);
        SpawnDroneMob();

        //Local

        void SpawnDroneMob()
        {
            var drone = DroneMobScene.Instantiate<DroneMob>();
            _mobSpawnPoint.ProgressRatio = GD.Randf();
            drone.Position = _mobSpawnPoint.Position;
            drone.Rotation = _mobSpawnPoint.Rotation;
            AddChild(drone);
        }
    }

    private void OnSpawnRocketMob()
    {
        _hud.UpdateScore(++_score);
        SpawnTargetedMob<RocketMob>(RocketMobScene);
    }

    private void OnSpawnSeekerMob()
    {
        _hud.UpdateScore(++_score);
        SpawnTargetedMob<SeekerMob>(SeekerMobScene);
    }

    private void SpawnTargetedMob<T>(PackedScene targetedMobScene) where T : TargetedMob
    {
        var targetedMob = targetedMobScene.Instantiate<T>();
        _mobSpawnPoint.ProgressRatio = GD.Randf();
        targetedMob.Position = _mobSpawnPoint.Position;
        targetedMob.Target = _player;
        AddChild(targetedMob);
    }
}
