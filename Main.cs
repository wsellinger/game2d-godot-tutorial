using Godot;

namespace Game2D;

public partial class Main : Node
{
	[Export] public PackedScene MobScene { get; set; }
    [Export] public PackedScene SeekerMobScene { get; set; }

	private int _score = 0;
	private int _mobsSpawned = 0;

	private Hud _hud;
    private Player _player;
    private Marker2D _startPosition;
    private Timer _startTimer;
    private Timer _scoreTimer;
	private Timer _spawnTimer;
	private PathFollow2D _mobSpawnPoint;
	private AudioStreamPlayer _music;
    private AudioStreamPlayer _deathSound;

    private const double GET_READY_DURATION = 2.0;
    private const string GET_READY_TEXT = "Get Ready!";
    private const string MOBS_GROUP_NAME = "mobs";
    private const double MIN_MOB_VELOCITY = 150.0;
    private const double MAX_MOB_VELOCITY = 250.0;
    private const int SEEKER_SPAWN_FREQUENCY = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _hud = GetNode<Hud>("Hud");
        _player = GetNode<Player>("Player");
		_startPosition = GetNode<Marker2D>("StartPosition");
        _startTimer = GetNode<Timer>("StartTimer");
        _scoreTimer = GetNode<Timer>("ScoreTimer");
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
		_scoreTimer.Stop();
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
        _scoreTimer.Start();
	}

	private void OnScoreTimerTimeout()
	{
		_hud.UpdateScore(++_score);
	}

    //TODO change score to go up every time a mob is spawned
    //TODO move mob logic below into new DroneMob (maybe have them curve instead of straight)
    //TODO make RocketMob that aims at player and goes fast across board (red throb animation)
    //TODO make timer more robust so we have a progression of slow spawning of simple mobs to a
    //     limit, then start adding in RocketMobs to a limit then SeekerMobs, then start increasing game speed?

	private void OnSpawnTimerTimeout()
    {
        SpawnMob();

        if (_mobsSpawned % SEEKER_SPAWN_FREQUENCY == 0)
        {
            SpawnSeeker();
        }

        //Local Methods

        void SpawnMob()
        {
            var rightAngle = Mathf.Pi / 2;
            var angleVariation = Mathf.Pi / 4;
            var mob = MobScene.Instantiate<Mob>();

            _mobSpawnPoint.ProgressRatio = GD.Randf();
            mob.Position = _mobSpawnPoint.Position;
            mob.Rotation = _mobSpawnPoint.Rotation + rightAngle + RandRangef(-angleVariation, angleVariation);
            mob.LinearVelocity = new Vector2(RandRangef(MIN_MOB_VELOCITY, MAX_MOB_VELOCITY), 0).Rotated(mob.Rotation);
            AddChild(mob);

            _mobsSpawned++;

            //Local Methods

            static float RandRangef(double from, double to) => (float)GD.RandRange(from, to);
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
