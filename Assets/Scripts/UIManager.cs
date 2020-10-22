using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text timeText;
    [SerializeField]
    GameStateManager state;
    int score = 0;
    float time = 0;
    int[] msc = { 0, 0, 0 };
    [SerializeField]
    Image[] lives;
    int lifeCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            timeText.text = "";
            time = PlayerPrefs.GetFloat("Best Time");

            msc[0] = (int)time / 60;
            msc[1] = (int)time - (msc[0] * 60);
            msc[2] = (int)((time - (int)time) * 100);

            foreach (int i in msc)
            {
                if (i == 0)
                    timeText.text += "00";
                else if (i < 10)
                    timeText.text += "0" + i;
                else
                    timeText.text += "" + i;
                timeText.text += ":";
            }
            timeText.text = timeText.text.Remove(timeText.text.Length - 1);

            scoreText.text = "" + PlayerPrefs.GetInt("High Score");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            // Update time and set time text property
            if (state.state == GameStateManager.GameState.Walking || state.state == GameStateManager.GameState.Scared)
            {
                timeText.text = "";
                time += Time.deltaTime;

                msc[0] = (int)time / 60;
                msc[1] = (int)time - (msc[0] * 60);
                msc[2] = (int)((time - (int)time) * 100);

                foreach (int i in msc)
                {
                    if (i == 0)
                        timeText.text += "00";
                    else if (i < 10)
                        timeText.text += "0" + i;
                    else
                        timeText.text += "" + i;
                    timeText.text += ":";
                }
                timeText.text = timeText.text.Remove(timeText.text.Length - 1);
            }
        }
    }

    public void SaveHighScore()
    {
        if ((score > PlayerPrefs.GetInt("High Score")) || ((score == PlayerPrefs.GetInt("High Score")) && (time < PlayerPrefs.GetFloat("Best Time"))))
        {
            PlayerPrefs.SetInt("High Score", score);
            PlayerPrefs.SetFloat("Best Time", time);
            PlayerPrefs.Save();
        }
    }

    public bool OutOfLives()
    {
        if (lifeCount < 0)
            return true;
        return false;
    }

    public void LoseLife()
    {
        // Debug.Log("Lost a life!");
        Destroy(lives[lifeCount]);
        lifeCount -= 1;
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = "" + score;
    }

    // Button methods
    public void OpenLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
