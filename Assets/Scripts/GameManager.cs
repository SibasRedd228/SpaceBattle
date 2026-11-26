using UnityEngine;

// Останавливает игру и показывает надпись Game Over.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static bool IsOver { get; private set; }

    [SerializeField] GameObject gameOverText;
    [SerializeField] EnemySpawner spawner;

    void Awake()
    {
        Instance = this;
        IsOver = false;
        Time.timeScale = 1f;
        if (gameOverText != null)
            gameOverText.SetActive(false);
    }

    public void GameOver()
    {
        if (IsOver)
            return;

        IsOver = true;
        Time.timeScale = 0f;
        if (spawner != null)
            spawner.enabled = false;
        if (gameOverText != null)
            gameOverText.SetActive(true);
    }

    void OnDestroy()
    {
        // timeScale глобальный и иначе остаётся 0 после выхода из Play Mode.
        Time.timeScale = 1f;
        if (Instance == this)
            IsOver = false;
    }
}
