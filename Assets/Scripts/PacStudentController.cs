using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour
{
    // Walking audio variables
    [SerializeField]
    AudioClip[] clips;
    int currentClip;
    AudioSource source;
    float walk = 0.0f;

    enum Direction { Up, Down, Left, Right };

    float speed = 7.0f;
    Direction lastInput;
    Direction currentInput;
    Tweener tweener;
    Animator animator;
    SpriteRenderer rend;

    int[,] levelMap =
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
    int column;
    int row;
    float dis = 1.395f;
    int hor; int ver;

    // Start is called before the first frame update
    void Start()
    {
        column = 1;
        row = 1;
        gameObject.transform.position = new Vector2(-12.5f * dis, 13.5f * dis);
        tweener = GetComponent<Tweener>();
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
        rend = GetComponent<SpriteRenderer>();
        setAdjacent(0, 0);
        currentInput = Direction.Left;
        lastInput = Direction.Left;
        animator.SetTrigger("Left");
    }

    // Update is called once per frame
    void Update()
    {
        setInput();
        rend.sortingOrder = (int) gameObject.transform.position.y;

        if (!tweener.TweenExists())
        {
            if ((gameObject.transform.position.x) > 0) hor = 1;
            else hor = -1;
            if ((gameObject.transform.position.y) > 0) ver = 1;
            else ver = -1;

            if (lastInput == Direction.Up && isWalkable(n))
            {
                tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + dis), speed);
                if (lastInput != currentInput)
                    animator.SetTrigger("Up");
                row -= ver;
                setAdjacent(0, dis);
                currentInput = lastInput;
            }

            else if (lastInput == Direction.Down && isWalkable(s))
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

            else if (lastInput == Direction.Right && isWalkable(e))
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

            else if (lastInput == Direction.Left && isWalkable(w))
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

            else if (currentInput == Direction.Up && isWalkable(n))
            {
                tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + dis), speed);
                row -= ver;
                setAdjacent(0, dis);
            }

            else if (currentInput == Direction.Down && isWalkable(s))
            {
                tweener.AddTween(gameObject.transform, gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - dis), speed);
                if (row == 14)
                    row = 13;
                else
                    row += ver;
                setAdjacent(0, -dis);
            }

            else if (currentInput == Direction.Right && isWalkable(e))
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

            else if (currentInput == Direction.Left && isWalkable(w))
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

        /*else if (walk > 0.25f)
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
        walk += Time.deltaTime;*/
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

    void setInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            lastInput = Direction.Up;
        if (Input.GetKeyDown(KeyCode.A))
            lastInput = Direction.Left;
        if (Input.GetKeyDown(KeyCode.S))
            lastInput = Direction.Down;
        if (Input.GetKeyDown(KeyCode.D))
            lastInput = Direction.Right;
    }
}
