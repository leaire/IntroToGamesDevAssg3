using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    SpriteRenderer spriterenderer;
    Tweener tweener;
    Animator animator;
    float s = 1.395f;

    [SerializeField]
    AudioClip[] clips;

    int currentClip;
    AudioSource source;
    float walk = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
        spriterenderer = GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        gameObject.transform.position = new Vector3(-4.5f * s, 3.5f * s, -1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spriterenderer.enabled == true)
        {
            if (!tweener.TweenExists())
            {
                if (gameObject.transform.position.x > 0.0f)
                {
                    if (gameObject.transform.position.y > 0.0f)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector3(4.5f * s, -2.5f * s, -1f), 7.0f);
                        animator.SetTrigger("Down");
                        //Debug.Log("Tween added");
                    }
                    else
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector3(-4.5f * s, -2.5f * s, -1f), 7.0f);
                        //Debug.Log("Tween added");
                        animator.SetTrigger("Left");
                    }
                }
                else
                {
                    if (gameObject.transform.position.y > 0.0f)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector3(4.5f * s, 3.5f * s, -1f), 7.0f);
                        //Debug.Log("Tween added");
                        animator.SetTrigger("Right");
                    }
                    else
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector3(-4.5f * s, 3.5f * s, -1f), 7.0f);
                        //Debug.Log("Tween added");
                        animator.SetTrigger("Up");
                    }
                }
            }
            else if (walk > 0.25f)
            {
                if (currentClip == 0)
                {
                    currentClip = 1;
                }
                else
                {
                    currentClip = 0;
                }
                source.clip = clips[currentClip];
                source.Play();
                walk = 0.0f;
            }
            walk += Time.deltaTime;
        }
    }
}
