using System;
using System.Collections;
using System.Collections.Generic;
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
        Play,
        Win,
        GameOver,
        Victory
    }

    [SerializeField] string[] _firstScenes = {"Scenes/Home"};

    public State state { get; private set; } = State.Unknown;

    [Inject] SceneService _sceneService;

    void StateStart()
    {
        if (SceneManager.sceneCount == 1)
            _sceneService.StartSceneTransition(Array.Empty<string>(), _firstScenes);
    }


    public void Update()
    {
        if (state == State.Unknown)
        {
            state = State.Start;
            StateStart();
        }
    }
}