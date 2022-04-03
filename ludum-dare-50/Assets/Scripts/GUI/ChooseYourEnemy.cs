using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI
{
    public class ChooseYourEnemy : MonoBehaviour
    {
        void Start()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            var knight = root.Q<VisualElement>("knight");
        }
    }
}