using Godot;

namespace Game2D
{
    public partial class SpawnTimer : Node
    {
        [Signal] public delegate void OnSpawnEventHandler();

        public record Data(double StartDelay, double SpawnFrequency);

        private Timer _startTimer;
        private Timer _spawnTimer;

        public SpawnTimer(Data data)
        {
            _startTimer = InitTimer(data.StartDelay, OnStartTimer);
            _spawnTimer = InitTimer(data.SpawnFrequency, OnSpawnTimer);

            _startTimer.OneShot = true;

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

        private void OnStartTimer() => _spawnTimer.Start();

        private void OnSpawnTimer() => EmitSignal(SignalName.OnSpawn);
    }
}
