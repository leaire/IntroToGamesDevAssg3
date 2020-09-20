using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tweener : MonoBehaviour
{
    // Tween activeTween = null;
    List<Tween> activeTweens = new List<Tween>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If activeTween.Target is further than 0.1f units away from the activeTween.EndPos:
        // Mathf.Sqrt(Mathf.Pow(activeTween.Target.position.x - activeTween.EndPos.x, 2) +
        // Mathf.Pow(activeTween.Target.position.y - activeTween.EndPos.y, 2) +
        // Mathf.Pow(activeTween.Target.position.z - activeTween.EndPos.z, 2)) > 0.1f

        if (activeTweens != null)
        {
            foreach (Tween activeTween in activeTweens)
            {
                if (activeTween != null)
                {
                    if (Vector3.Distance(activeTween.Target.position, activeTween.EndPos) > 0.1f)
                    {
                        // Lerp towards
                        float timeFraction = Mathf.Pow((Time.time - activeTween.StartTime) / activeTween.Duration, 3);
                        activeTween.Target.position = Vector3.Lerp(activeTween.StartPos, activeTween.EndPos, timeFraction);
                    }
                    else
                    {
                        activeTween.Target.position = activeTween.EndPos;
                        // Likely to spit an error
                        activeTweens.Remove(activeTween);
                        break;
                    }
                }
            }
        }
    }

    public bool AddTween(Transform targetObject, Vector3 startPos, Vector3 endPos, float duration)
    {
        if (!TweenExists(targetObject))
        {
            activeTweens.Add(new Tween(targetObject, startPos, endPos, Time.time, duration));
            return true;
        }
        return false;
    }

    public bool TweenExists(Transform target)
    {
        foreach(Tween activeTween in activeTweens)
        {
            if (activeTween.Target == target)
            {
                return true;
            }
        }
        return false;
    }
}
