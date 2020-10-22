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
    int score = 0;
    float time = 0;
    int[] msc = { 0, 0, 0 };
    string minutes; string seconds; string centiseconds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = "";

        // Update time and set time text property
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
