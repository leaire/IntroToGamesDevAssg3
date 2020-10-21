using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
