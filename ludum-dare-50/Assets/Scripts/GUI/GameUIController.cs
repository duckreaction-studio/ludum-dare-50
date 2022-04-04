using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using Enemies;
using UnityEngine;
using Zenject;

namespace GUI
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField] ChooseYourEnemy _chooseUi;
        [SerializeField] StartEndLevel _startEndLevelUi;

        [Inject(Optional = true)] SignalBus _signalBus;
        [Inject(Optional = true)] MainGameState _gameState;

        ChessPiece.Type _currentType;

        void Start()
        {
            SetVisibleAllUis(false);
            _signalBus?.Subscribe<GameEvent>(OnGameEventReceived);
        }

        void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.StartChooseEnemy))
                StartChooseEnemy();
            else if (gameEvent.Is(GameEventType.StartShowScore))
                StartShowScore();
            else if (gameEvent.Is(GameEventType.PlayGame))
                SetVisibleAllUis(false);
        }

        void SetVisibleAllUis(bool isVisible)
        {
            _chooseUi.gameObject.SetActive(isVisible);
            _startEndLevelUi.gameObject.SetActive(isVisible);
        }

        void SetVisibleUi(int uiIndex, bool isVisible)
        {
            if (uiIndex == 0)
                _chooseUi.gameObject.SetActive(isVisible);
            else if (uiIndex == 1)
                _startEndLevelUi.gameObject.SetActive(isVisible);
        }

        [ContextMenu("Start choose enemy")]
        void StartChooseEnemy()
        {
            SetVisibleUi(0, true);
            _chooseUi.Refresh();
        }

        [ContextMenu("Start show score")]
        void StartShowScore()
        {
            SetVisibleUi(1, true);
            _startEndLevelUi.ShowScore(_gameState == null
                ? new()
                {
                    type = Score.Type.Fail
                }
                : _gameState.LastScore);
        }

        [ContextMenu("Test select the rook")]
        void TestPlayerChoose()
        {
            PlayerChoose(ChessPiece.Type.Rook);
        }

        public void PlayerChoose(ChessPiece.Type type)
        {
            _currentType = type;
            SetVisibleUi(0, false);
            SetVisibleUi(1, true);
            _startEndLevelUi.ShowEnemyName(_currentType.ToString().ToLower());
        }

        public void EndShowTitle(StartEndLevel.Type type)
        {
            Debug.Log("End " + type.ToString());
            SetVisibleAllUis(false);
            if (type == StartEndLevel.Type.showEnemy)
            {
                _signalBus?.Fire(new GameEvent(GameEventType.EnemySelected, _currentType));
            }
            else
            {
                _signalBus?.Fire(new GameEvent(GameEventType.EndShowScore, _currentType));
            }
        }
    }
}