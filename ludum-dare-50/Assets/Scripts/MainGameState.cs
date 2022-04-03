using System;
using System.Collections;
using System.Collections.Generic;
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

    [Inject] SceneService _sceneService;
    [Inject] SignalBus _signalBus;

    ChessPiece.Type _currentEnemyType;

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
        else if (state == State.EnemyIntro)
            StatePlay();
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

    void StatePlay()
    {
        state = State.Play;
    }
}