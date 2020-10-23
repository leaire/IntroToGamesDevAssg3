using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    public GameStateManager.GameState ghostState;
    GameStateManager.GameState prevState;

    // Walking audio variables
    [SerializeField]
    GameStateManager state;
    [SerializeField]
    Transform player;

    enum Direction { Up, Down, Right, Left};
    public enum Behaviour { Away, Toward, Random, Clockwise };
    static System.Random random = new System.Random();

    float speed = 7.2f;
    Direction lastInput;
    Direction currentInput;
    public Behaviour defaultBehaviour;
    public Behaviour behaviour;
    Tweener tweener;
    Animator animator;
    SpriteRenderer rend;

    int[,] levelMap = // Rows 0-14, Columns 0-13
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };
    int n;
    int s;
    int e;
    int w;
    int row;
    int column;
    [SerializeField]
    int spawnColumn;
    [SerializeField]
    int spawnRow;
    [SerializeField]
    float xSpawnPosition;
    [SerializeField]
    float ySpawnPosition;
    float dis = 1.395f;
    int hor; int ver;
    float[] adjacentDistance = new float[4];
    bool[] prevAdjacentWalkable = { false, false, false, false };

    Direction[] clockwiseInstructionsToStart = { Direction.Right, Direction.Down, Direction.Down, Direction.Right, Direction.Right, Direction.Down, Direction.Down, Direction.Left, Direction.Left };
    Direction[,] clockwiseInstructions =
    {
        { Direction.Down, Direction.Right, Direction.Up, Direction.Left, Direction.Up },
        { Direction.Right, Direction.Up, Direction.Left, Direction.Down, Direction.Left },
        { Direction.Up, Direction.Left, Direction.Down, Direction.Right, Direction.Down },
        { Direction.Left, Direction.Down, Direction.Right, Direction.Up, Direction.Right },
    };
    int clockwiseQuad;
    int clockwiseStep;
    bool clockwiseStarted;

    // Start is called before the first frame update
    void Start()
    {
        ghostState = GameStateManager.GameState.Walking;
        prevState = ghostState;
        behaviour = defaultBehaviour;
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();

        column = spawnColumn;
        row = spawnRow;
        gameObject.transform.position = new Vector2(xSpawnPosition * dis, ySpawnPosition * dis);
        setAdjacent(0, 0);
        currentInput = Direction.Left;
        lastInput = Direction.Left;
        animator.SetTrigger("Left");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Ghost 1's state: " + ghostState);

        rend.sortingOrder = 250 - (int)(gameObject.transform.position.y * 20);

        if (prevState != ghostState)
        {
            animator.SetInteger("GameState", (int)ghostState);
            prevState = ghostState;

            if (ghostState == GameStateManager.GameState.Walking) speed = 7.2f;
            if (ghostState == GameStateManager.GameState.Scared || ghostState == GameStateManager.GameState.Recovering) speed = 6f;
        }

        // Debug.Log("Up: " + prevAdjacentWalkable[0].ToString() + ", Down: " + prevAdjacentWalkable[1].ToString() + ", Right: " + prevAdjacentWalkable[2].ToString() + ", Left: " + prevAdjacentWalkable[3].ToString());
        if (ghostState == GameStateManager.GameState.Dead)
        {
            if (!tweener.TweenExists())
            {
                if (column > 9 && row > 11) // If ghost is in spawn area…
                {
                    row = spawnRow; column = spawnColumn;
                    gameObject.transform.position = new Vector2(xSpawnPosition * dis, ySpawnPosition * dis);
                    // Debug.Log("Ghost in spawn without tween");
                    if (state.state == GameStateManager.GameState.Scared)
                    {
                        if (state.ghostsAreRecovering)
                            ghostState = GameStateManager.GameState.Recovering;
                        else
                            ghostState = GameStateManager.GameState.Scared;
                    }
                    else
                        ghostState = GameStateManager.GameState.Walking;
                }
                else
                {
                    // Debug.Log("Ghost tweened back to spawn");
                    tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(xSpawnPosition, ySpawnPosition), speed * 0.8f);
                    row = spawnRow; column = spawnColumn;
                }
            }
        }

        if (state.state != GameStateManager.GameState.Intro && ghostState != GameStateManager.GameState.Dead && state.state != GameStateManager.GameState.Dead)
        {
            if (!tweener.TweenExists())
            {
                if ((gameObject.transform.position.x) > 0) hor = 1;
                else hor = -1;
                if ((gameObject.transform.position.y) > 0) ver = 1;
                else ver = -1;

                // gameObject.transform.position = new Vector2(dis * (13.5f - column) * hor, dis * (14.5f - row) * ver);

                if (isAtJunction())
                {
                    setInput();

                    if (lastInput == Direction.Up)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + dis), speed);
                        if (lastInput != currentInput)
                            animator.SetTrigger("Up");
                        row -= ver;
                        setAdjacent(0, dis);
                        currentInput = lastInput;
                    }

                    else if (lastInput == Direction.Down)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - dis), speed);
                        if (lastInput != currentInput)
                            animator.SetTrigger("Down");
                        if (row == 14)
                            row = 13;
                        else
                            row += ver;
                        setAdjacent(0, -dis);
                        currentInput = lastInput;
                    }

                    else if (lastInput == Direction.Right)
                    {
                        if (column == 0 && hor > 0)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x * -1, gameObject.transform.position.y);
                        }
                        else
                        {
                            tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x + dis, gameObject.transform.position.y), speed);
                            if (lastInput != currentInput)
                                animator.SetTrigger("Right");
                            if (column < 13 || hor > 0)
                                column -= hor;
                            setAdjacent(dis, 0);
                            currentInput = lastInput;
                        }
                    }

                    else if (lastInput == Direction.Left)
                    {
                        if (column == 0 && hor < 0)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x * -1, gameObject.transform.position.y);
                        }
                        else
                        {
                            tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x - dis, gameObject.transform.position.y), speed);
                            if (lastInput != currentInput)
                                animator.SetTrigger("Left");
                            if (column < 13 || hor < 0)
                                column += hor;
                            setAdjacent(-dis, 0);
                            currentInput = lastInput;
                        }
                    }
                }

                else
                {
                    if (currentInput == Direction.Up)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + dis), speed);
                        row -= ver;
                        setAdjacent(0, dis);
                    }

                    else if (currentInput == Direction.Down)
                    {
                        tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - dis), speed);
                        if (row == 14)
                            row = 13;
                        else
                            row += ver;
                        setAdjacent(0, -dis);
                    }

                    else if (currentInput == Direction.Right)
                    {
                        if (column == 0 && hor > 0)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x * -1, gameObject.transform.position.y);
                        }
                        else
                        {
                            tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x + dis, gameObject.transform.position.y), speed);
                            if (column < 13 || hor > 0)
                                column -= hor;
                            setAdjacent(dis, 0);
                        }
                    }

                    else if (currentInput == Direction.Left)
                    {
                        if (column == 0 && hor < 0)
                        {
                            gameObject.transform.position = new Vector2(gameObject.transform.position.x * -1, gameObject.transform.position.y);
                        }
                        else
                        {
                            tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x - dis, gameObject.transform.position.y), speed);
                            if (column < 13 || hor < 0)
                                column += hor;
                            setAdjacent(-dis, 0);
                        }
                    }
                }
            }
        }
    }

    public void killGhost()
    {
        ghostState = GameStateManager.GameState.Dead;
        animator.SetTrigger("Dead");
    }

    bool isAtJunction()
    {
        int[] ordinates = { n, s, e, w };
        bool atJunction = false;
        int i = 0;

        foreach (int tile in ordinates)
        {
            if (prevAdjacentWalkable[i] != isWalkable(tile))
            {
                prevAdjacentWalkable[i] = isWalkable(tile);
                atJunction = true;
            }
            i++;
        }

        if (atJunction)
        {
            // Debug.Log("Is at junction!");
            return true;
        }

        return false;
    }

    bool isWalkable(int direction)
    {
        if (direction == 0 || direction == 5 || direction == 6)
            return true;
        return false;
    }

    void setAdjacent(float x, float y)
    {
        if ((gameObject.transform.position.x + x) > 0) hor = 1;
        else hor = -1;
        if ((gameObject.transform.position.y + y) > 0) ver = 1;
        else ver = -1;

        // Debug.Log("Row: " + row + ", Column: " + column + ", (n/s/e/w): " + n + "/" + s + "/" + e + "/" + w + ", Hor: " + hor + ", Ver: " + ver);

        try
        {
            n = levelMap[row - ver, column];

            if (row == 14)
                s = levelMap[row - ver, column];
            else
                s = levelMap[row + ver, column];

            if (column == 13 && hor < 0)
                e = levelMap[row, column];
            else
                e = levelMap[row, column - hor];

            if (column == 13 && hor > 0)
                w = levelMap[row, column];
            else
                w = levelMap[row, column + hor];
        }

        catch (IndexOutOfRangeException e)
        {
            /*gameObject.transform.position = new Vector2(gameObject.transform.position.x * -1, gameObject.transform.position.y);
            column = 0;*/
        }
    }

    void setInput() // n/s/e/w for adjacentDistance[] 0/1/2/3
    {
        if (column > 9 && row > 11) // If ghost is in spawn area…
        {
            // Reset clockwise behaviour
            clockwiseQuad = 0;
            clockwiseStep = 0;
            clockwiseStarted = false;

            // Try and leave
            if (xSpawnPosition > 0)
            {
                if (isWalkable(s)) lastInput = Direction.Down;
                else lastInput = Direction.Left;
            }
            else
            {
                if (isWalkable(n)) lastInput = Direction.Up;
                else lastInput = Direction.Right;
            }
        }
        else
        {
            // Defines opposite direction (which will later be removed as a possible input)
            Direction opposite = Direction.Right;
            if (currentInput == Direction.Up) opposite = Direction.Down;
            if (currentInput == Direction.Down) opposite = Direction.Up;
            if (currentInput == Direction.Right) opposite = Direction.Left;
            if (currentInput == Direction.Left) opposite = Direction.Right;

            // List & adjacent tile values for basis of logic
            var list = new List<Direction>();
            int[] ordinates = { n, s, e, w };

            if (behaviour == Behaviour.Away || behaviour == Behaviour.Toward)
            {
                float ghostDistance = Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), player.position));

                // Sets distance from Duckman of adjacent tiles 
                adjacentDistance[0] = Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x, transform.position.y + dis), player.position));
                adjacentDistance[1] = Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x, transform.position.y - dis), player.position));
                adjacentDistance[2] = Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x + dis, transform.position.y), player.position));
                adjacentDistance[3] = Mathf.Abs(Vector2.Distance(new Vector2(transform.position.x - dis, transform.position.y), player.position));

                // Stores respective inputs for each direction in list if the distance is greater/less than
                if (behaviour == Behaviour.Away)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if ((adjacentDistance[i] > ghostDistance) && isWalkable(ordinates[i]))
                        {
                            list.Add((Direction)i);
                        }
                    }
                }
                else if (behaviour == Behaviour.Toward)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if ((adjacentDistance[i] < ghostDistance) && isWalkable(ordinates[i]))
                        {
                            list.Add((Direction)i);
                        }
                    }
                }
                removeIllegalInputs(list, opposite);

                if (list.Count < 1)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        if (isWalkable(ordinates[i]))
                        {
                            list.Add((Direction)i);
                        }
                    }
                    removeIllegalInputs(list, opposite);
                }

                // Randomly selects input from list
                lastInput = list[random.Next(list.Count)];
            }
            if (behaviour == Behaviour.Random)
            {
                // Adds walkable directions to list
                for (int i = 0; i < 4; i++)
                {
                    if (isWalkable(ordinates[i]))
                    {
                        list.Add((Direction)i);
                    }
                }
                removeIllegalInputs(list, opposite);

                // Randomly selects input from list
                lastInput = list[random.Next(list.Count)];
            }
            if (behaviour == Behaviour.Clockwise)
            {
                if (!clockwiseStarted)
                {
                    lastInput = clockwiseInstructionsToStart[clockwiseStep];
                    clockwiseStep++;
                    if (clockwiseStep >= clockwiseInstructionsToStart.Length)
                    {
                        clockwiseStarted = true;
                        clockwiseStep = 0;
                    }
                }
                else
                {
                    ;
                }
            }
        }
    }

    void removeIllegalInputs(List<Direction> list, Direction opposite)
    {
        // Walking in opposite direction
        list.Remove(opposite);

        // Walking off-screen
        if (row == 14 && column == 6)
        {
            if (hor > 0)
                list.Remove(Direction.Right);
            else list.Remove(Direction.Left);
        }

        // Walking back into spawn
        if (row == 11 && column == 13)
        {
            if (ver > 0)
                list.Remove(Direction.Down);
            else list.Remove(Direction.Up);
        }
    }
}
