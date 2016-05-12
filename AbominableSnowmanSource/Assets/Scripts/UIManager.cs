﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class UIManager : MonoBehaviour {

    [SerializeField] private Text waveCountDown;
    [SerializeField] private Text waveCounter;
    [SerializeField] private Text hazardPoints;
    [SerializeField] private Text GameOverWave;
    [SerializeField] private Text GameOverKillCount;
    [SerializeField] private Text GameOverScore;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Image background;

    private bool isGameOverShowing;
    private bool isPauseMenuShowing;
    private InstructionsController instructionController;
    private Animator anim;

    void Start() {
        anim = GetComponent<Animator>();
        instructionController = GameObject.FindGameObjectWithTag("Instruction").GetComponent<InstructionsController>();
        instructionController.gameObject.SetActive(false);
    }

    public void showGameOverPanel(bool show) {
        gameOverPanel.SetActive(show);
        isGameOverShowing = show;
        backgroundManage();
    }

    public void showPauseMenu(bool show) {
        pauseMenu.SetActive(show);
        isPauseMenuShowing = show;
        backgroundManage();
    }

    public void showCountDown(bool show) {
        waveCountDown.enabled = show;
    }

    public void showTrapMenu(bool show) {
        if (show)
            anim.SetBool("In", true);
        else
            anim.SetBool("In", false);
    }

    private void backgroundManage() {
        background.enabled = !isGameOverShowing && !isPauseMenuShowing ? false : true;
    }

    public void UpdateCounters(int waveCount, int hazardpoints) {
        waveCounter.text = "Wave: " + waveCount;
        hazardPoints.text = "Hazard Points: " + hazardpoints;
    }

    public void UpdateCountDown(int count) {
        waveCountDown.text = "Next wave starts in: " + count;
    }

    public void SetGameOverStats(int wave, int killed, int score) {
        GameOverWave.text = "Wave: " + wave;
        GameOverKillCount.text = "Enemies killed: " + killed;
        GameOverScore.text = "Score: " + score;
    }

    public void ShowInstruction(string instruction) {
        instructionController.gameObject.SetActive(true);
        instructionController.ShowInstruction(instruction);
    }

    public void UnShowInstruction() {
        instructionController.gameObject.SetActive(false);
    }
}
