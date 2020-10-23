using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBorderGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject egg;

    int rate;

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log("Literally anything");

        for (int i = 0; i < rate; i++)
        {
            Instantiate(egg, new Vector3(-18f + (i * (36 / rate)), 18f, 100f), Quaternion.identity);
        }

        for (int i = 0; i < rate; i++)
        {
            Instantiate(egg, new Vector3(18f, 18f - (i * (36 / rate)), 100f), Quaternion.identity);
        }

        for (int i = 0; i < rate; i++)
        {
            Instantiate(egg, new Vector3(18f - (i * (36 / rate)), -18f, 100f), Quaternion.identity);
        }
        for (int i = 0; i < rate; i++)
        {
            Instantiate(egg, new Vector3(18f, -18f + (i * (36 / rate)), 100f), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
