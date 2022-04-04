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
        static string _scoreContainer = "center-container2";

        void Start()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
        }

        void OnEnable()
        {
            Start();
        }

        public void ShowEnemyName(string enemyName)
        {
            SetScoreVisible(false);
            SetTitle("the", enemyName);

            StartCoroutine(WaitAndSignalEndOfShowCoroutine(Type.showEnemy));
        }

        public void ShowScore(Score score)
        {
            SetScoreVisible(score.type != Score.Type.Fail);
            SetTitle("you", score.type == Score.Type.Fail ? "died" : "win");
            SetStarCount(_scoreContainer, score.StarCount);
            SetScoreTitle(score.type);
            StartCoroutine(WaitAndSignalEndOfShowCoroutine(Type.showScore));
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

        IEnumerator WaitAndSignalEndOfShowCoroutine(Type type)
        {
            yield return new WaitForSeconds(_showDuration);
            GetComponentInParent<GameUIController>().EndShowTitle(type);
        }
    }
}