using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI
{
    public class ChooseYourEnemy : MonoBehaviour
    {
        VisualElement _root;

        void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            AddButtonListener("rook", ChessPiece.Type.Rook);
            AddButtonListener("knight", ChessPiece.Type.Knight);
            AddButtonListener("bishop", ChessPiece.Type.Bishop);
            AddButtonListener("queen", ChessPiece.Type.Queen);
        }

        void AddButtonListener(string className, ChessPiece.Type type)
        {
            var button = _root.Q<VisualElement>(className).Q<Button>(className: "unity-button");
            button.clicked += () => PlayerChoose(type);
        }

        void PlayerChoose(ChessPiece.Type type)
        {
            Debug.Log("Player choose " + type);
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
        }
    }
}