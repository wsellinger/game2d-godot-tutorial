using Godot;
using System.Threading.Tasks;

namespace Game2D;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

    private Label _scoreLabel;
	private Label _messageLabel;
	private Button _startButton;
    private Timer _messageTimer;

	private const double GAME_OVER_DURATION = 2.0;
    private const double START_BUTTON_DELAY = 1.0;
    private const string SCORE_LABEL_NAME = "ScoreLabel";
    private const string MESSAGE_LABEL_NAME = "MessageLabel";
    private const string START_BUTTON_NAME = "StartButton";
    private const string MESSAGE_TIMER_NAME = "MessageTimer";

    private const string GAME_OVER_TEXT = "Game Over";
    private const string GAME_START_TEXT = "Dodge the Creeps!";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _scoreLabel = GetNode<Label>(SCORE_LABEL_NAME);
        _messageLabel = GetNode<Label>(MESSAGE_LABEL_NAME);
        _startButton = GetNode<Button>(START_BUTTON_NAME);
        _messageTimer = GetNode<Timer>(MESSAGE_TIMER_NAME);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public void ShowMessage(string text, double waitTime)
	{
        _messageLabel.Text = text;
        _messageLabel.Show();
		_messageTimer.WaitTime = waitTime;
		_messageTimer.Start();                     
	}

	public async Task ShowGameOver()
	{
		ShowMessage(GAME_OVER_TEXT, GAME_OVER_DURATION);
		await ToSignal(_messageTimer, Timer.SignalName.Timeout);
        ShowMessage(GAME_START_TEXT, START_BUTTON_DELAY);
        await ToSignal(_messageTimer, Timer.SignalName.Timeout);
		_startButton.Show();
    }

    public void UpdateScore(int score)
    {
        _scoreLabel.Text = score.ToString();
    }

    private void OnStartButtonPressed()
    {
        _startButton.Hide();
        EmitSignal(SignalName.StartGame);
    }

    private void OnMessageTimerTimeout()
    {
        _messageLabel.Hide();

    }
}
