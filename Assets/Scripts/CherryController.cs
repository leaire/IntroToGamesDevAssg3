using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    [SerializeField]
    GameObject cherry;
    float elapsedTime;
    float dis = 1.395f;
    [SerializeField]
    float waitTime = 30f;
    [SerializeField]
    float cherrySpeed = 3.5f;
    GameObject activeCherry;
    Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        elapsedTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment timer
        elapsedTime += Time.deltaTime;

        // Spawn cherry after set time (and resets timer)
        if (elapsedTime > waitTime && !tweener.TweenExists())
        {
            spawnCherry();
            elapsedTime -= waitTime;
            Debug.Log("Cherry spawned");
        }
        // Debug.Log("Time until next cherry spawn: " + (waitTime - elapsedTime));
    }

    void spawnCherry()
    {
        // Destroy previous cherry
        if (activeCherry != null)
            Destroy(activeCherry);

        // Random position generation
        float y;
        if (Random.Range(-1, 1) >= 0)
            y = 24f;
        else
            y = -24f;
        float x = Random.Range(-13.5f * dis, 13.5f * dis);

        // Instantiate cherry
        activeCherry = Instantiate(cherry, new Vector2(x, y), Quaternion.identity);
        activeCherry.transform.parent = gameObject.transform;

        // Lerp spawned cherry
        tweener.AddTween(activeCherry, activeCherry.transform, activeCherry.transform.position, new Vector2(-x, -y), cherrySpeed);
    }
}
