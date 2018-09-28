using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager gm;

    void Awake() {
        if (gm == null) {
            gm = this.gameObject.GetComponent<GameManager>();
        }

    }

    void Start() {
        initializeGameStatesCanvas();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameState == GameStates.Playing)
                Pause();
        }
    }

    #region GameStates And Canvas

    public GameObject GameOverCanvas;
    public GameObject GameStartCanvas;
    public GameObject GamePauseCanvas;
    public GameObject GamePlay;

    public enum GameStates {
        BeforeStart,
        Playing,
        GameOver,
        Pause
    }

    private GameStates GameState = GameStates.BeforeStart;

    private void initializeGameStatesCanvas() {
        GameStartCanvas.SetActive(true);
        GameOverCanvas.SetActive(false);
        GamePauseCanvas.SetActive(false);
    }

    public void GameOver() {
        GameState = GameStates.GameOver;
        GameOverCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void NewGame() {
        GameState = GameStates.Playing;
        Time.timeScale = 1f;
        GameStartCanvas.SetActive(false);
    }

    public void ClearGame() {
        Time.timeScale = 0;
    }

    public void Replay() {
        GamePauseCanvas.SetActive(false);
        GameOverCanvas.SetActive(false);
        ClearGame();
        NewGame();
    }

    private void Pause() {
        Time.timeScale = 0;
        GamePauseCanvas.SetActive(true);
    }

    public void Resume() {
        Time.timeScale = 1f;
        GamePauseCanvas.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }

    #endregion

}

