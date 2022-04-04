using System.Collections;
using DuckReaction.Common;
using Enemies;
using Settings;
using UnityEngine;
using Zenject;


public class LevelState : MonoBehaviour
{
    [Inject] SignalBus _signalBus;
    [Inject] ScoreSettings _scoreSettings;

    [SerializeField] float _waitBeforeStart = 2f;
    bool _playerHasHitEnemy = false;
    bool _gameOver = false;

    void Start()
    {
        _signalBus.Subscribe<GameEvent>(OnReceiveGameEvent);
    }

    void OnReceiveGameEvent(GameEvent gameEvent)
    {
        if (!_gameOver && gameEvent.Is(GameEventType.PlayerShot))
        {
            OnPlayerShot(gameEvent);
        }
        else if (!_playerHasHitEnemy && gameEvent.Is(GameEventType.EnemyEndAttack))
        {
            StartCoroutine(OnEnemyEndAttack());
        }
        else if (gameEvent.Is(GameEventType.PlayGame))
            LevelStart(gameEvent.GetParam<ChessPiece.Type>());
    }

    void LevelStart(ChessPiece.Type type)
    {
        _gameOver = false;
        _playerHasHitEnemy = false;
        StartCoroutine(StartGame(type));
    }

    void OnPlayerShot(GameEvent gameEvent)
    {
        var hitInfo = gameEvent.GetParam<Player.HitInfo>();
        if (hitInfo.HasHitEnemy)
        {
            Debug.Log("Player hit enemy " + hitInfo.perfectHit);
            OnPlayerHitEnemy(hitInfo);
        }
        else
        {
            Debug.Log("Player has missed");
            GameOver();
        }
    }

    void OnPlayerHitEnemy(Player.HitInfo hitInfo)
    {
        var playerScore =
            _scoreSettings.CalculatePlayerScore(hitInfo.perfectHit, hitInfo.enemy.GetLowerDistanceFromDeathSquares());
        Debug.Log(
            "Score " + playerScore.type + " " + playerScore.perfectHit + " " + playerScore.enemyDistanceFromSquare);
        if (playerScore.type == Score.Type.Fail)
            GameOver();
        else
            PlayerWin(playerScore);
    }

    void PlayerWin(Score playerScore)
    {
        _playerHasHitEnemy = true;
        _signalBus.Fire(new GameEvent(GameEventType.LevelWin, playerScore));
    }

    void GameOver()
    {
        _gameOver = true;
        _signalBus.Fire(new GameEvent(GameEventType.LevelGameOver));
    }

    IEnumerator StartGame(ChessPiece.Type type)
    {
        _signalBus.Fire(new GameEvent(GameEventType.SpwanEnemy, type));
        yield return null;
        // Wait one frame to be sure enemy is active
        _signalBus.Fire(new GameEvent(GameEventType.LevelRestart));
        yield return new WaitForSeconds(_waitBeforeStart);
        Debug.Log("Start enemy attack");
        _signalBus.Fire(new GameEvent(GameEventType.EnemyStartAttack));
    }

    IEnumerator OnEnemyEndAttack()
    {
        Debug.Log("Enemy attack !");
        yield return new WaitForSeconds(_scoreSettings.killTimeout);
        if (!_playerHasHitEnemy)
            GameOver();
    }

    /*
    public void Restart()
    {
        _playerHasHitEnemy = false;
        _gameOver = false;
        _signalBus.Fire(new GameEvent(GameEventType.LevelRestart));
        StopAllCoroutines();
        StartCoroutine(StartGame());
    }
    */
}