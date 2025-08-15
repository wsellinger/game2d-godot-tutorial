using Godot;

namespace Game2D
{
    public partial class SpawnManager : Node
    {
        //Signals

        [Signal] public delegate void OnSpawnDroneMobEventHandler();
        [Signal] public delegate void OnSpawnRocketMobEventHandler();
        [Signal] public delegate void OnSpawnSeekerMobEventHandler();

        private SpawnTimer[] _timers;
        private SpawnTimer _droneSpawnTimer;
        private SpawnTimer _rocketSpawnTimer;
        private SpawnTimer _seekerSpawnTimer;

        private SpawnTimer.Data _droneSpawnData = new(StartDelay: 0, SpawnFrequency: 0.5);
        private SpawnTimer.Data _rocketSpawnData = new(StartDelay: 5, SpawnFrequency: 2);
        private SpawnTimer.Data _seekerSpawnData = new(StartDelay: 10, SpawnFrequency: 5);

        public override void _Ready()
        {
            _droneSpawnTimer = InitSpawnTimer(_droneSpawnData, OnDroneSpawnTimerTimeout);
            _rocketSpawnTimer = InitSpawnTimer(_rocketSpawnData, OnRocketSpawnTimerTimeout);
            _seekerSpawnTimer = InitSpawnTimer(_seekerSpawnData, OnSeekerSpawnTimerTimeout);
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

        SpawnTimer InitSpawnTimer(SpawnTimer.Data spawnData, SpawnTimer.OnSpawnEventHandler callback)
        {
            var spawnTimer = new SpawnTimer(spawnData);            
            spawnTimer.OnSpawn += callback;
            AddChild(spawnTimer);
            return spawnTimer;
        }
    }
}
