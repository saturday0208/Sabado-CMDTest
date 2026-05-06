using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject gameOverPanel;
    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);


    }


    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM");

    }

    // Update is called once per frame
    void Update()
    {
        if(isGameOver) Time.timeScale = 0.0f;
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        isGameOver = true;
    }

    public void RestartGame()
    {
        isGameOver = false;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene");
    }
}
