using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    Tween activeTween = null;
    //Animator animator;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // If activeTween.Target is further than 0.1f units away from the activeTween.EndPos:
        // Mathf.Sqrt(Mathf.Pow(activeTween.Target.position.x - activeTween.EndPos.x, 2) +
        // Mathf.Pow(activeTween.Target.position.y - activeTween.EndPos.y, 2) +
        // Mathf.Pow(activeTween.Target.position.z - activeTween.EndPos.z, 2)) > 0.1f

        if (activeTween != null)
        {
            if (Vector2.Distance(activeTween.Target.position, activeTween.EndPos) > 0.05f)
            {
                // Lerp towards
                timer += (Time.deltaTime) / activeTween.Duration;
                gameObject.transform.position = Vector2.Lerp(activeTween.StartPos, activeTween.EndPos, timer);
                //Debug.Log("Lerp completed");
            }
            else
            {
                activeTween.Target.position = activeTween.EndPos;
                activeTween = null;
                timer = 0f;
                //Debug.Log("Tween nulled");
            }
        }
    }

    public /*bool*/ void AddTween(Transform targetObject, Vector2 startPos, Vector2 endPos, float speed)
    {
        /*if (!TweenExists())
        {*/
        activeTween = new Tween(targetObject, startPos, endPos, Time.time, speed);
            /*return true;
        }
        return false;*/
    }

    public bool TweenExists()
    {
        if (activeTween != null)
        {
            return true;
        }
        return false;
    }
}
