using System.Collections;
using System.Collections.Generic;
using DuckReaction.Common;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace GUI
{
    public class FullScreenNextButton : MonoBehaviour
    {
        [SerializeField] bool _onlyOnce = true;
        [SerializeField] UIDocument _document;
        [Inject(Optional = true)] SignalBus _signalBus;
        int _clickCount;

        void Start()
        {
            _document.rootVisualElement.Q<Button>("fullscreen-button").clicked += OnButtonClicked;
        }

        void OnButtonClicked()
        {
            if ((_clickCount == 0 || !_onlyOnce) && Input.GetMouseButtonUp(0))
            {
                _clickCount++;
                _signalBus?.Fire(new GameEvent(GameEventType.ClickNext));
                Debug.Log("Next");
            }
        }
    }
}