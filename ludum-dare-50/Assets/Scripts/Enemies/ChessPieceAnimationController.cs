using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using UnityEngine;
using Zenject;

namespace Enemies
{
    public class ChessPieceAnimationController : MonoBehaviour
    {
        [SerializeField] public GameObject snapKill;
        [SerializeField] public GameObject snapDeath;
        [SerializeField] GameObject _blood1;
        [SerializeField] GameObject _blood2;

        [Inject] SignalBus _signalBus;

        void Start()
        {
            _signalBus.Subscribe<GameEvent>(OnGameEventReceived);
        }

        void OnGameEventReceived(GameEvent gameEvent)
        {
            if (gameEvent.Is(GameEventType.LevelRestart))
                Reset();
        }

        public void SetBloodActive(int index, bool active)
        {
            if (index == 0)
                _blood1.SetActive(active);
            else if (index == 1)
                _blood2.SetActive(active);
        }

        public void Reset()
        {
            SetBloodActive(0, false);
            SetBloodActive(1, false);
        }
    }
}