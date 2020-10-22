using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    AudioSource audiosource;

    [SerializeField]
    AudioClip[] clips;
    [SerializeField]
    GameObject ready;
    [SerializeField]
    GameObject life;
    [SerializeField]
    GameObject star;
    [SerializeField]
    Text countDown;
    [SerializeField]
    Text ghostTimer;
    [SerializeField]
    GameObject[] ghosts;
    [SerializeField]
    UIManager ui;
    [SerializeField]
    PacStudentController player;
    [SerializeField]
    LevelGenerator level;
    float timer = 0;
    float introLength = 3.541f;
    string[] countDownText = { "3", "2", "1", "GO!" };
    public int remainingPellets = 220;

    public enum GameState { Intro, Walking, Scared, Dead, GameOver };
    public GameState state = GameState.Intro;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        audiosource.clip = clips[0];
        audiosource.loop = false;
        audiosource.Play();
        ghostTimer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingPellets <= 0 && (state != GameState.GameOver))
        {
            EndGame();
        }

        if (state == GameState.Intro)
        {
            timer += Time.deltaTime;
            if (audiosource.isPlaying && (audiosource.clip == clips[0]))
            {
                if (((int)(timer / (introLength / 4))) > 3)
                    countDown.text = countDownText[3];
                else
                    countDown.text = countDownText[(int)(timer / (introLength / 4))];
            }
            else
            {
                countDown.enabled = false;
                state = GameState.Walking;
                audiosource.clip = clips[1];
                audiosource.loop = true;
                audiosource.Play();
                timer = 0;
            }
        }

        if (state == GameState.Scared)
        {
            if (timer < 10)
            {
                timer += Time.deltaTime;
                ghostTimer.text = "" + (10 - (int)timer);
                if (ghostTimer.text.Equals("0"))
                    ghostTimer.text = "1";
            }
            else
            {
                ghostTimer.enabled = false;
                state = GameState.Walking;
                audiosource.clip = clips[1];
                audiosource.Play();
                timer = 0;
            }
        }

        if (state == GameState.Dead)
        {
            timer += Time.deltaTime;

            if (timer > 2.2f)
            {
                if (ui.OutOfLives())
                {
                    EndGame();
                }
                else
                {
                    player.Respawn();
                    state = GameState.Walking;
                    audiosource.clip = clips[1];
                    audiosource.Play();
                    timer = 0;
                }
            }
        }

        if (state == GameState.GameOver)
        {
            timer += Time.deltaTime;

            if (timer > 3f)
                SceneManager.LoadScene(0);
        }

        // Debug.Log(state);
    }

    void EndGame()
    {
        Destroy(level.gameObject);
        Destroy(player.gameObject);
        foreach (GameObject ghost in ghosts)
        {
            Destroy(ghost);
        }
        countDown.enabled = true;
        countDown.text = "Game Over!";
        state = GameState.GameOver;
        timer = 0;
        ui.SaveHighScore();
    }

    public void KillPacman()
    {
        audiosource.Stop();
        ui.LoseLife();
        if (ghostTimer.enabled)
            ghostTimer.enabled = false;
        timer = 0;
        state = GameState.Dead;
    }

    public void changeToScaredState()
    {
        timer = 0;
        if (state != GameState.Scared)
        {
            state = GameState.Scared;
            audiosource.clip = clips[2];
            audiosource.Play();
            ghostTimer.enabled = true;
            ghostTimer.text = "10";
        }
    }
}
