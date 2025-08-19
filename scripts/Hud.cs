using Godot;
using System.Threading.Tasks;

namespace Game2D;

public partial class Hud : CanvasLayer
{
	[Signal] public delegate void StartGameEventHandler();

    //Stage

    private Label _scoreLabel;
    private Label _highScoreLabel;
	private Label _messageLabel;
    private Label _startLabel;
	private Button _startButton;

    private AnimationPlayer _highScoreAnimation;

    private Timer _messageTimer;

    private const string SCORE_LABEL_NAME = "ScoreLabel";
    private const string HIGH_SCORE_LABEL_NAME = "HighScoreLabel";
    private const string MESSAGE_LABEL_NAME = "MessageLabel";
    private const string START_LABEL_NAME = "StartLabel";
    private const string START_BUTTON_NAME = "StartButton";
    private const string MESSAGE_TIMER_NAME = "MessageTimer";
    
    private const string HIGH_SCORE_ANIMATION_PLAYER_NAME = "AnimationPlayer";
    private static readonly StringName HIGH_SCORE_ANIMATION_NAME = "Flash";

    private const string GAME_START_TEXT = "Dodge the Creeps!";
    private const string GET_READY_TEXT = "Get Ready!";
    private const string GAME_OVER_TEXT = "Game Over";
    private const string HIGH_SCORE_TEXT = "New High Score!";

    private const double GET_READY_DURATION = 2.0;
    private const double GAME_OVER_DURATION = 2.0;
    private const double HIGH_SCORE_DURATION = 5.5;
    private const double START_BUTTON_DELAY = 1.0;

    public override void _Ready()
	{
        _scoreLabel = GetNode<Label>(SCORE_LABEL_NAME);
        _highScoreLabel = GetNode<Label>(HIGH_SCORE_LABEL_NAME);
        _messageLabel = GetNode<Label>(MESSAGE_LABEL_NAME);
        _startLabel = GetNode<Label>(START_LABEL_NAME);
        _startButton = GetNode<Button>(START_BUTTON_NAME);
        _messageTimer = GetNode<Timer>(MESSAGE_TIMER_NAME);

        _highScoreAnimation = _highScoreLabel.GetNode<AnimationPlayer>(HIGH_SCORE_ANIMATION_PLAYER_NAME);

        _scoreLabel.Hide();
        _highScoreLabel.Hide();
        _startLabel.Hide();
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
        _messageLabel.Hide();
        _highScoreLabel.Hide();
        _scoreLabel.Hide();

        ShowMessage(GET_READY_TEXT, GET_READY_DURATION, _startLabel);
        await ToSignal(_messageTimer, Timer.SignalName.Timeout);
        
        _startLabel.Hide();
        _scoreLabel.Show();
    }

	public async Task ShowGameOver(bool newHighScore, uint highScore)
	{
        //Game Over
		ShowMessage(GAME_OVER_TEXT, GAME_OVER_DURATION, _messageLabel);
		await ToSignal(_messageTimer, Timer.SignalName.Timeout);

        //High Score
        if (newHighScore)
        {
            ShowMessage(HIGH_SCORE_TEXT, HIGH_SCORE_DURATION, _messageLabel);
            //_highScoreLabel.Show();
            _highScoreAnimation.Play(HIGH_SCORE_ANIMATION_NAME);
            await ToSignal(_messageTimer, Timer.SignalName.Timeout);
        }

        //Dodge the Creeps!
        if (highScore > 0)
        {
            _highScoreLabel.Show();
            UpdateScore(highScore);
        }

        ShowMessage(GAME_START_TEXT, START_BUTTON_DELAY, _messageLabel);
        await ToSignal(_messageTimer, Timer.SignalName.Timeout);
		_startButton.Show();
    }

    public void UpdateScore(uint score)
    {
        _scoreLabel.Text = score.ToString();
    }

    public void ShowHighScore(uint score)
    {
        _scoreLabel.Text = score.ToString();
        _scoreLabel.Show();
        _highScoreLabel.Show();
    }

    private void OnStartButtonPressed()
    {
        _startButton.Hide();
        EmitSignal(SignalName.StartGame);
    }
}
