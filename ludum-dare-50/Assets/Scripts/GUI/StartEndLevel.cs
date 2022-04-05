using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.UIElements;

namespace GUI
{
    public class StartEndLevel : MonoBehaviour
    {
        public enum Type
        {
            showEnemy,
            showScore
        }

        [SerializeField] float _showDuration = 3f;

        VisualElement _root;
        Type _currentType;
        static string _scoreContainer = "center-container2";

        void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _root.Q<Button>("fullscreen-button").clicked += OnButtonClicked;
        }

        void OnEnable()
        {
            Start();
        }

        void OnButtonClicked()
        {
            StopAllCoroutines();
            Close();
        }

        public void ShowEnemyName(string enemyName)
        {
            _currentType = Type.showEnemy;
            SetScoreVisible(false);
            SetTitle("the", enemyName);

            StartCoroutine(WaitAndSignalEndOfShowCoroutine());
        }

        public void ShowScore(Score score)
        {
            _currentType = Type.showScore;
            SetScoreVisible(score.type != Score.Type.Fail);
            SetTitle("you", score.type == Score.Type.Fail ? "died" : "win");
            SetStarCount(_scoreContainer, score.StarCount);
            SetScoreTitle(score.type);
            StartCoroutine(WaitAndSignalEndOfShowCoroutine());
        }

        void SetScoreTitle(Score.Type scoreType)
        {
            string label = "not bad";
            if (scoreType == Score.Type.Good)
                label = "good";
            else if (scoreType == Score.Type.Perfect)
                label = "perfect";
            _root.Q<Label>("info").text = label;
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

        void SetTitle(string left, string right)
        {
            _root.Q<Label>("left-part").text = left;
            _root.Q<Label>("right-part").text = right;
        }

        void SetScoreVisible(bool isVisible)
        {
            if (isVisible)
                _root.Q<GroupBox>(_scoreContainer).RemoveFromClassList("hidden");
            else
                _root.Q<GroupBox>(_scoreContainer).AddToClassList("hidden");
        }

        IEnumerator WaitAndSignalEndOfShowCoroutine()
        {
            yield return new WaitForSeconds(_showDuration);
            Close();
        }

        void Close()
        {
            GetComponentInParent<GameUIController>().EndShowTitle(_currentType);
        }
    }
}