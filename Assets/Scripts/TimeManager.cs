using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    private int lastTime;
    private float timer;
    const float moveWait = 2.0f;
    const float scaleWait = 4.0f;

    [SerializeField]
    private Transform[] transformArray;

    // Start is called before the first frame update
    void Start()
    {
        transformArray = new Transform[2];
        transformArray[0] = GameObject.FindWithTag("Red").transform;
        transformArray[1] = GameObject.FindWithTag("Blue").transform;

        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 2.0f;
        ResetTime();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        int currentTime = (int)Time.time;
        if (currentTime > lastTime)
        {
            Debug.Log(currentTime);
            lastTime = currentTime;
        }
        */

        timer += Time.deltaTime;
        if ((int)timer > lastTime)
        {
            Debug.Log((int)timer);
            lastTime = (int)timer;
            if ((int)timer % (int)moveWait == 0)
            {
                MoveObjects();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.0f;
            }
            else
                Time.timeScale = 1.0f;
            Debug.Log("Spacebar pressed");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ResetTime();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            float rand = Random.Range(0.25f,0.75f);
            StartCoroutine(RotateObjects(rand));
        }
    }

    IEnumerator RotateObjects(float randomDelay)
    {
        // This coroutine should wait for <randomDelay> seconds, then rotate
        // the two gameobjects by positive 90 degrees around the z axis, then
        // repeat 3 more times(e.g.wait-rotate - wait - rotate and so on. The
        // objects turn a full 360 degrees before exiting the method)
        // (tip: see WaitForSeconds(...)). This should obey the Pausing
        // functionality but not the Reset functionality(i.e.the objects
        // should finish their full rotation)

        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(randomDelay);
            transformArray[0].Rotate(0.0f, 0.0f, 90.0f, Space.Self);
            transformArray[1].Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        }
        yield break;
    }

    void ResetTime()
    {
        lastTime = 0;
        timer = 0;
        Debug.Log((int)timer);
        CancelInvoke();

        // Use InvokeRepeating(...) to call ScaleObjects() once every <scaleWait> seconds
        // This behavior should obey the Pausing and Reset functionality (tip: see CancelInvoke(...))
        InvokeRepeating("ScaleObjects", scaleWait, scaleWait);
    }

    void MoveObjects()
    {
        if (transformArray[0].position.y > transformArray[1].position.y)
        {
            if (transformArray[0].position.x > transformArray[1].position.x)
            {
                float trans = transformArray[0].position.y;
                transformArray[0].position = new Vector3(transformArray[0].position.x,
                                                     transformArray[1].position.y,
                                                     transformArray[0].position.z);
                transformArray[1].position = new Vector3(transformArray[1].position.x,
                                                     trans,
                                                     transformArray[1].position.z);
            }
            else
            {
                float trans = transformArray[0].position.x;
                transformArray[0].position = new Vector3(transformArray[1].position.x,
                                                     transformArray[0].position.y,
                                                     transformArray[0].position.z);
                transformArray[1].position = new Vector3(trans,
                                                     transformArray[1].position.y,
                                                     transformArray[1].position.z);
            }
        }
        else if (transformArray[0].position.x > transformArray[1].position.x)
        {
            float trans = transformArray[0].position.x;
            transformArray[0].position = new Vector3(transformArray[1].position.x,
                                                 transformArray[0].position.y,
                                                 transformArray[0].position.z);
            transformArray[1].position = new Vector3(trans,
                                                 transformArray[1].position.y,
                                                 transformArray[1].position.z);
        }
        else
        {
            float trans = transformArray[0].position.y;
            transformArray[0].position = new Vector3(transformArray[0].position.x,
                                                 transformArray[1].position.y,
                                                 transformArray[0].position.z);
            transformArray[1].position = new Vector3(transformArray[1].position.x,
                                                 trans,
                                                 transformArray[1].position.z);
        }
    }

    void ScaleObjects()
    {
        foreach (Transform scale in transformArray)
        {
            if (scale.transform.localScale.x > 1.5f)
            {
                scale.transform.localScale /= 1.2f;
            }
            else
            {
                scale.transform.localScale *= 1.2f;
            }
        }
    }
}

//  = transformArray[0].position.y

/*  transformArray[0].position = new Vector3(transformArray[1].position.x,
                                             transformArray[1].position.y,
                                             transformArray[0].position.z);
    transformArray[1].position = new Vector3(transformArray[1].position.x,
                                                 transY,
                                                 transformArray[1].position.z);
*/
