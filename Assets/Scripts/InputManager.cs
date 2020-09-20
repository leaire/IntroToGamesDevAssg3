using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject initial;

    List<GameObject> items = new List<GameObject>();

    Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GetComponent<Tweener>();
        items.Add(initial);
        //items.FindLast().transform = new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            items.Add(Instantiate(initial));
            items[items.Count - 1].transform.position = new Vector3(0, 0.5f, 0);
        }

        if (items != null)
        {
            foreach (GameObject item in items)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    if (tweener.AddTween(item.transform, item.transform.position, new Vector3(-2.0f, 0.5f, 0.0f), 1.5f))
                    {
                        break;
                    }
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    if (tweener.AddTween(item.transform, item.transform.position, new Vector3(2.0f, 0.5f, 0.0f), 1.5f))
                    {
                        break;
                    }
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    if (tweener.AddTween(item.transform, item.transform.position, new Vector3(0.0f, 0.5f, -2.0f), 0.5f))
                    {
                        break;
                    }
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    if (tweener.AddTween(item.transform, item.transform.position, new Vector3(0.0f, 0.5f, 2.0f), 0.5f))
                    {
                        break;
                    }
                }
            }
        }
    }
}
