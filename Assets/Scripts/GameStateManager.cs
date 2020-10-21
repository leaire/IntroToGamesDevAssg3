using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    AudioSource audiosource;

    [SerializeField]
    AudioClip audioclip;
    [SerializeField]
    GameObject ready;
    [SerializeField]
    GameObject life;
    [SerializeField]
    GameObject star;

    bool introOver = false;

    // Start is called before the first frame update
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        // StartCoroutine(getReady());
    }

    // Update is called once per frame
    void Update()
    {
        if (!audiosource.isPlaying && audiosource.clip != audioclip)
        {
            introOver = true;
            audiosource.clip = audioclip;
            audiosource.loop = true;
            audiosource.Play();

            /*GameObject[] actors = GameObject.FindGameObjectsWithTag("Actor");
            foreach (GameObject actor in actors)
            {
                SpriteRenderer rend = actor.GetComponent<SpriteRenderer>();
                Animator anim = actor.GetComponent<Animator>();
                //InputManager inp = actor.GetComponent<InputManager>();
                anim.enabled = true;
                rend.enabled = true;
                //inp.enabled = true;
            }*/

            /*GameObject temp;
            temp = Instantiate(life, new Vector2(-22f, -19.5f), Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp = Instantiate(life, new Vector2(-22, -17f), Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp = Instantiate(life, new Vector2(-22f, -14.5f), Quaternion.identity);
            temp.transform.parent = gameObject.transform;
            temp = Instantiate(star, new Vector2(22f, -19.5f), Quaternion.identity);
            temp.transform.localScale = new Vector2(1.5f, 1.5f);
            temp.transform.parent = gameObject.transform;*/
        }
    }

    /*IEnumerator getReady()
    {
        GameObject temp = Instantiate(ready);
        temp.transform.parent = gameObject.transform;
        //float timer = 0.0f;

        while (!introOver)
        {
            //timer += Time.deltaTime;
            yield return null;
        }

        temp.SetActive(false);
    }*/
}
