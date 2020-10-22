using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    [SerializeField]
    GameStateManager state;
    public GameStateManager.GameState ghostState;

    // Start is called before the first frame update
    void Start()
    {
        ghostState = GameStateManager.GameState.Walking;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
