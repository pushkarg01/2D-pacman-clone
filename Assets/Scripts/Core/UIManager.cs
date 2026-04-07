using TMPro;
using UnityEngine;
using Zenject;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI gameOverText;

    [Inject] private GameManager gameManager;
    [Inject] private GameEvents gameEvents;

    private void OnEnable()
    {
        if (gameEvents == null) return;

        gameEvents.ScoreChanged += OnScoreChanged;
        gameEvents.LivesChanged += OnLivesChanged;
        gameEvents.GameOverChanged += OnGameOverChanged;
    }

    private void Start()
    {
        if (gameManager == null) return;

        OnScoreChanged(gameManager.score);
        OnLivesChanged(gameManager.lives);
        OnGameOverChanged(gameManager.isGameOver);
    }

    private void OnDisable()
    {
        if (gameEvents == null) return;

        gameEvents.ScoreChanged -= OnScoreChanged;
        gameEvents.LivesChanged -= OnLivesChanged;
        gameEvents.GameOverChanged -= OnGameOverChanged;
    }

    private void Update()
    {
        if (gameManager != null && gameManager.isGameOver && Input.anyKeyDown)
        {
            gameManager.StartNewGame();
        }
    }

    private void OnScoreChanged(int value) => scoreText.text = value.ToString();
    private void OnLivesChanged(int value) => livesText.text = "x" + value;
    private void OnGameOverChanged(bool value) => gameOverText.gameObject.SetActive(value);
}
