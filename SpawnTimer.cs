using Godot;

namespace Game2D
{
    public partial class SpawnTimer : Node
    {
        //Signals

        [Signal] public delegate void OnSpawnDroneMobEventHandler();
        [Signal] public delegate void OnSpawnRocketMobEventHandler();
        [Signal] public delegate void OnSpawnSeekerMobEventHandler();

        private Timer _droneSpawnTimer;
        private Timer _rocketSpawnTimer;
        private Timer _seekerSpawnTimer;

        private Timer[] _timers;

        private const double DRONE_INIT_SPAWN_SPEED = 0.5;
        private const double ROCKET_INIT_SPAWN_SPEED = 2;
        private const double SEEKER_INIT_SPAWN_SPEED = 5;

        public override void _Ready()
        {
            _droneSpawnTimer = InitTimer(DRONE_INIT_SPAWN_SPEED, OnDroneSpawnTimerTimeout);
            _rocketSpawnTimer = InitTimer(ROCKET_INIT_SPAWN_SPEED, OnRocketSpawnTimerTimeout);
            _seekerSpawnTimer = InitTimer(SEEKER_INIT_SPAWN_SPEED, OnSeekerSpawnTimerTimeout);
            _timers = [_droneSpawnTimer, _rocketSpawnTimer, _seekerSpawnTimer];
        }

        public void Start()
        {
            foreach (var timer in _timers)
            {
                timer.Start();
            }
        }

        public void Stop()
        {
            foreach (var timer in _timers)
            {
                timer.Stop();
            }
        }

        private void OnDroneSpawnTimerTimeout() => EmitSignal(SignalName.OnSpawnDroneMob);
        private void OnRocketSpawnTimerTimeout() => EmitSignal(SignalName.OnSpawnRocketMob);
        private void OnSeekerSpawnTimerTimeout() => EmitSignal(SignalName.OnSpawnSeekerMob);

        private Timer InitTimer(double waitTime, System.Action callback)
        {
            var timer = new Timer { WaitTime = waitTime };
            timer.Timeout += callback;
            AddChild(timer);
            return timer;
        }
    }
}
