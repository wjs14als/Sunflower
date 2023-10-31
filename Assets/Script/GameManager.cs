using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { menu, inGame, gameOver}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState currentGameState = GameState.menu;
    public GameObject gameover;

    private void Awake()
    {
        instance = this;
    }
    public void StartGame()
    {
        SetGameState(GameState.inGame); 
    }

    public void GameOver()
    {
        if(HpSystem.hp == 0)
        {
            Time.timeScale = 0;
            gameover.SetActive(true);
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60;
        StartGame();
    }

    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu) { }
        else if(newGameState == GameState.inGame) { }
        else if(newGameState == GameState.gameOver) { }

        currentGameState = newGameState;
    }

    private void Update()
    {
        GameOver();
    }
}
