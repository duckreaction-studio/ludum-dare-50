using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DuckReaction.Common;
using Enemies;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class MainGameState : MonoBehaviour
{
    public enum State
    {
        Unknown,
        Start,
        Tutorial,
        GameOverTutorial,
        ChooseEnemy,
        EnemyIntro,
        Play,
        Win,
        GameOver,
        Victory
    }

    [SerializeField] string[] _firstScenes = {"Scenes/Home"};
    [SerializeField] string[] _playScenes = {"Scenes/MainScene"};

    public State state { get; private set; } = State.Unknown;

    public int TotalStars => _stars.Values.Sum();

    public bool QueenIsLocked => TotalStars < 7;

    [Inject] SceneService _sceneService;
    [Inject] SignalBus _signalBus;

    ChessPiece.Type _currentEnemyType;
    Dictionary<ChessPiece.Type, int> _stars;

    void Start()
    {
        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.ClickNext))
            Next();
        else if (gameEvent.Is(GameEventType.EndScreenTransition))
            OnEndScreenTransition();
        else if (gameEvent.Is(GameEventType.LevelAnimationEnd))
            StateChooseEnemy();
        else if (gameEvent.Is(GameEventType.EnemySelected))
            StatePlay(gameEvent.GetParam<ChessPiece.Type>());
        else if (gameEvent.Is(GameEventType.LevelGameOver))
            OnLevelGameOver();
        else if (gameEvent.Is(GameEventType.LevelWin))
            OnLevelWin(gameEvent.GetParam<Score>());
    }

    void Update()
    {
        if (state == State.Unknown)
        {
            StateStart();
        }
    }

    void StateStart()
    {
        state = State.Start;
        if (SceneManager.sceneCount == 1)
            _sceneService.StartSceneTransition(Array.Empty<string>(), _firstScenes);
    }

    void Next()
    {
        if (state == State.Start)
            StateTutorial();
    }

    [ContextMenu("Start tutorial")]
    public void TestTutorial()
    {
        state = State.Tutorial;
        _currentEnemyType = ChessPiece.Type.Pawn;
        OnEndScreenTransition();
    }

    void OnEndScreenTransition()
    {
        if (state == State.Tutorial)
        {
            _signalBus.Fire(new GameEvent(GameEventType.PlayGame, _currentEnemyType));
        }
    }

    void StateTutorial()
    {
        state = State.Tutorial;
        _currentEnemyType = ChessPiece.Type.Pawn;
        _sceneService.StartSceneTransition(_firstScenes, _playScenes);
    }

    void StatePlay(ChessPiece.Type type)
    {
        state = State.Play;
        _currentEnemyType = type;
    }

    void StateChooseEnemy()
    {
        state = State.ChooseEnemy;
        _signalBus.Fire(new GameEvent(GameEventType.StartChooseEnemy));
    }

    void OnLevelWin(Score score)
    {
        var starCount = score.StarCount;
        _stars[_currentEnemyType] = Math.Max(_stars[_currentEnemyType], starCount);
        StateChooseEnemy();
    }

    void OnLevelGameOver()
    {
        _stars.Clear();
        StateChooseEnemy();
    }

    public int GetStarCount(ChessPiece.Type item2)
    {
        return _stars[item2];
    }
}