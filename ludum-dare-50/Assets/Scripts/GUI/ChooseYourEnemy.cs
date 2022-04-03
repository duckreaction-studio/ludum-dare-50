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
    }
}