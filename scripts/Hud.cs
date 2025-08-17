using Godot;
using System.Threading.Tasks;

namespace Game2D;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void StartGameEventHandler();

    private Label _scoreLabel;
	private Label _ctaLabel;
    private Label _startLabel;
	private Button _startButton;
    private Timer _messageTimer;

	private const double GAME_OVER_DURATION = 2.0;
    private const double START_BUTTON_DELAY = 1.0;
    private const string SCORE_LABEL_NAME = "ScoreLabel";
    private const string CTA_LABEL_NAME = "CTALabel";
    private const string START_LABEL_NAME = "StartLabel";
    private const string START_BUTTON_NAME = "StartButton";
    private const string MESSAGE_TIMER_NAME = "MessageTimer";

    private const string GAME_START_TEXT = "Dodge the Creeps!";
    private const string GET_READY_TEXT = "Get Ready!";
    private const string GAME_OVER_TEXT = "Game Over";

    private const double GET_READY_DURATION = 2.0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        _scoreLabel = GetNode<Label>(SCORE_LABEL_NAME);
        _ctaLabel = GetNode<Label>(CTA_LABEL_NAME);
        _startLabel = GetNode<Label>(START_LABEL_NAME);
        _startButton = GetNode<Button>(START_BUTTON_NAME);
        _messageTimer = GetNode<Timer>(MESSAGE_TIMER_NAME);
    }

	public void ShowMessage(string text, double waitTime, Label label)
	{
        label.Text = text;
        label.Show();
		_messageTimer.WaitTime = waitTime;
		_messageTimer.Start();                     
	}

    public async Task ShowGameStart()
    {
        _ctaLabel.Hide();
        ShowMessage(GET_READY_TEXT, GET_READY_DURATION, _startLabel);
        await ToSignal(_messageTimer, Timer.SignalName.Timeout);
        _startLabel.Hide();
    }

	public async Task ShowGameOver()
	{
		ShowMessage(GAME_OVER_TEXT, GAME_OVER_DURATION, _ctaLabel);
		await ToSignal(_messageTimer, Timer.SignalName.Timeout);
        ShowMessage(GAME_START_TEXT, START_BUTTON_DELAY, _ctaLabel);
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
}
