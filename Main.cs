using Godot;

namespace Game2D;

public partial class Main : Node
{
	[Export] public PackedScene DroneMobScene { get; set; }
    [Export] public PackedScene SeekerMobScene { get; set; }

	private int _score = 0;
	private int _dronesSpawned = 0;
    private int _seekersSpawned = 0;

	private Hud _hud;
    private Player _player;
    private Marker2D _startPosition;
    private Timer _startTimer;
	private Timer _spawnTimer;
	private PathFollow2D _mobSpawnPoint;
	private AudioStreamPlayer _music;
    private AudioStreamPlayer _deathSound;

    private const double GET_READY_DURATION = 2.0;
    private const string GET_READY_TEXT = "Get Ready!";
    private const string MOBS_GROUP_NAME = "mobs";
    private const int SEEKER_SPAWN_FREQUENCY = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _hud = GetNode<Hud>("Hud");
        _player = GetNode<Player>("Player");
		_startPosition = GetNode<Marker2D>("StartPosition");
        _startTimer = GetNode<Timer>("StartTimer");
        _spawnTimer = GetNode<Timer>("SpawnTimer");
        _mobSpawnPoint = GetNode<PathFollow2D>("MobSpawnPath/MobSpawnPoint");
        _music = GetNode<AudioStreamPlayer>("Music");
        _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
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
		_hud.ShowMessage(GET_READY_TEXT, GET_READY_DURATION);

		_player.Start(_startPosition.Position);
		GetTree().CallGroup(MOBS_GROUP_NAME, Node.MethodName.QueueFree);

		_startTimer.Start();
		_music.Play();
	}

	private void OnStartTimerTimeout()
    {
        _spawnTimer.Start();
	}

    //TODO make RocketMob that aims at player and goes fast across board (red throb animation)
    //TODO make timer more robust so we have a progression of slow spawning of simple mobs to a
    //     limit, then start adding in RocketMobs to a limit then SeekerMobs, then start increasing game speed?

	private void OnSpawnTimerTimeout()
    {
        _hud.UpdateScore(++_score);

        SpawnDrone();

        if (_dronesSpawned % SEEKER_SPAWN_FREQUENCY == 0)
        {
            SpawnSeeker();
        }

        //Local Methods

        void SpawnDrone()
        {
            var drone = DroneMobScene.Instantiate<DroneMob>();
            _mobSpawnPoint.ProgressRatio = GD.Randf();
            drone.Position = _mobSpawnPoint.Position;
            drone.Rotation = _mobSpawnPoint.Rotation;
            AddChild(drone);

            _dronesSpawned++;
        }

        void SpawnSeeker()
        {
            var seeker = SeekerMobScene.Instantiate<SeekerMob>();
            _mobSpawnPoint.ProgressRatio = GD.Randf();
            seeker.Position = _mobSpawnPoint.Position;
            seeker.Target = _player;
            AddChild(seeker);
        }
    }
}
