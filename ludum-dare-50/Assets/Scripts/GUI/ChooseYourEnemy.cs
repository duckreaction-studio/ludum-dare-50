using System;
using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using Enemies;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace GUI
{
    public class ChooseYourEnemy : MonoBehaviour
    {
        [Inject(Optional = true)] MainGameState _gameState;
        [Inject(Optional = true)] SignalBus _signalBus;

        List<Tuple<string, ChessPiece.Type>> _types = new()
        {
            new("rook", ChessPiece.Type.Rook),
            new("knight", ChessPiece.Type.Knight),
            new("bishop", ChessPiece.Type.Bishop),
        };

        VisualElement _root;
        bool _queenIsLocked = true;

        void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            foreach (var (item1, item2) in _types)
            {
                AddButtonListener(item1, item2);
            }

            AddButtonListener("queen", ChessPiece.Type.Queen);

            //  _signalBus?.Subscribe<GameEvent>(OnGameEventReceived);
        }

        /*       void OnGameEventReceived(GameEvent gameEvent)
               {
               }
               */

        void AddButtonListener(string className, ChessPiece.Type type)
        {
            var button = _root.Q<VisualElement>(className).Q<Button>(className: "unity-button");
            button.clicked += () => PlayerChoose(type);
        }

        void PlayerChoose(ChessPiece.Type type)
        {
            Debug.Log("Player choose " + type);
            if (type == ChessPiece.Type.Queen && _queenIsLocked)
                return;
            GetComponentInParent<GameUIController>().PlayerChoose(type);
        }

        [ContextMenu("Test one star")]
        public void TestOneStar()
        {
            SetStarCount("knight", 1);
        }

        [ContextMenu("Test three stars")]
        public void TestThreeStars()
        {
            SetStarCount("bishop", 3);
        }

        [ContextMenu("Test unlock queen")]
        public void TestUnlockQueen()
        {
            SetLockQueen(false);
        }

        void SetStarCount(string className, int startCount)
        {
            var element = _root.Q<VisualElement>(className);
            ClearStarCount(element);
            element.AddToClassList("count" + startCount);
        }

        static void ClearStarCount(VisualElement element)
        {
            for (int i = 0; i <= 3; i++)
                element.RemoveFromClassList("count" + i);
        }

        void SetLockQueen(bool locked)
        {
            var element = _root.Q<VisualElement>("queen");
            element.RemoveFromClassList("locked");
            if (locked)
                element.AddToClassList("locked");
            _queenIsLocked = locked;
        }

        public void Refresh()
        {
            if (_gameState)
            {
                foreach (var (item1, item2) in _types)
                {
                    SetStarCount(item1, _gameState.GetStarCount(item2));
                }

                SetLockQueen(_gameState.QueenIsLocked);
            }
        }
    }
}