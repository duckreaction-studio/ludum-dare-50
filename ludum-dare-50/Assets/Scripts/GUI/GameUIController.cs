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

        [Inject(Optional = true)] SignalBus _signalBus;

        void Start()
        {
            Hide();
            _signalBus?.Subscribe<GameEvent>(OnGameEventReceived);
        }

        void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.StartChooseEnemy))
                StartChooseEnemy();
            else if (gameEvent.Is(GameEventType.PlayGame))
                Hide();
        }

        void Hide()
        {
            _chooseUi.gameObject.SetActive(false);
        }

        [ContextMenu("Start choose enemy")]
        void StartChooseEnemy()
        {
            _chooseUi.gameObject.SetActive(true);
            _chooseUi.Refresh();
        }

        public void PlayerChoose(ChessPiece.Type type)
        {
            // TODO afficher le nom de l'enemi
            _signalBus.Fire(new GameEvent(GameEventType.EnemySelected, type));
        }
    }
}