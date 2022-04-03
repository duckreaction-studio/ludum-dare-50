using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

public class UITestShotController : MonoBehaviour
{
    [Inject] LevelState _levelState;
    [Inject] SignalBus _signalBus;

    Button _restart;
    Label _scoreLabel;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _restart = root.Q<Button>("restart-button");
        _scoreLabel = root.Q<Label>("score-label");

        _restart.clicked += OnRestartClicked;

        _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
    }

    void OnGameEventReceived(GameEvent gameEvent)
    {
        if (gameEvent.Is(GameEventType.LevelGameOver))
            _scoreLabel.text = "Game Over";
        else if (gameEvent.Is(GameEventType.LevelWin))
        {
            var score = gameEvent.GetParam<Score>();
            _scoreLabel.text = "Win " + score.type.ToString() + " " + score.perfectHit + " " +
                               score.enemyDistanceFromSquare;
        }
        else if (gameEvent.Is(GameEventType.LevelRestart))
            _scoreLabel.text = "";
    }

    void OnRestartClicked()
    {
        // TODO fixme
        // _levelState.Restart();
    }
}