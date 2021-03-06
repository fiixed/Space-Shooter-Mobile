﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait, startWait, waveWait;
    public Text scoreText, gameOverText;
    public GameObject restartButton;

    private int score;
    private bool gameOver;
    private bool restart;

    private void Start() {
        gameOver = false;
        restart = false;
        restartButton.SetActive(false);
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(SpawnWaves());
    }

    //private void Update() {
    //    if (restart) {
    //        if (Input.GetKeyDown(KeyCode.R)) {
    //            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //        }
    //    }
    //}

    IEnumerator SpawnWaves() {
        yield return new WaitForSeconds(startWait);
        while (true) {
            for (int i = 0; i < hazardCount; i++) {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (gameOver) {
                restartButton.SetActive(true);
                //restartText.text = "Press 'R' for Restart";
                restart = true;
                break;
            }
        }     
    }

    public void AddScore(int newScoreValue) {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore() {
        scoreText.text = "Score: " + score;
    }

    public void GameOver() {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
