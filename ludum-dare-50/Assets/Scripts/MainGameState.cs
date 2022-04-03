using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
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

    void Start()
    {
        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.ClickNext))
            Next();
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

    void StateTutorial()
    {
        state = State.Tutorial;
        _sceneService.StartSceneTransition(_firstScenes, _playScenes);
    }

    void StatePlay()
    {
        state = State.Play;
    }
}