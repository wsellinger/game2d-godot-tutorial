using Godot;

namespace Game2D
{
    public partial class SpawnTimer : Node
    {
        [Signal] public delegate void OnSpawnEventHandler();

        public record Data(double StartDelay, RangeDouble SpawnFrequency, double SpawnAcceleration);

        private Timer _startTimer;
        private Timer _spawnTimer;

        private readonly RangeDouble _spawnFrequency;
        private readonly double _spawnAcceleration;

        public SpawnTimer(Data data)
        {
            _startTimer = InitTimer(data.StartDelay, OnStartTimer);
            _spawnTimer = InitTimer(data.SpawnFrequency.Start, OnSpawnTimer);
            
            _spawnFrequency = data.SpawnFrequency;
            _spawnAcceleration = data.SpawnAcceleration;

            _startTimer.OneShot = true;

            //Local

            Timer InitTimer(double waitTime, System.Action callback)
            {
                var timer = new Timer { WaitTime = waitTime };
                timer.Timeout += callback;
                AddChild(timer);
                return timer;
            }
        }

        public void Start()
        {
            _startTimer.Start();
        }

        public void Stop()
        {
            _startTimer.Stop();
            _spawnTimer.Stop();
        }

        private void OnStartTimer()
        {
            _spawnTimer.WaitTime = _spawnFrequency.Start;
            _spawnTimer.Start();
        }

        private void OnSpawnTimer()
        {
            //Update Spawn Frequency
            var waitTime = _spawnTimer.WaitTime;
            if (waitTime > _spawnFrequency.End)
                _spawnTimer.WaitTime = Mathf.Max(waitTime - _spawnAcceleration, _spawnFrequency.End);

            EmitSignal(SignalName.OnSpawn);            
        }
    }
}
