/*using System;
using System.Globalization;*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] tiles;
    [SerializeField]
    GameObject[] pellets;

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

    // The displacement between instantiated tiles
    float dis = 1.395f;
    // The ordinate of the top-leftmost tile
    float startX = -20.2275f + 1.395f;
    float startY = 20.2275f - 1.395f;
    float moveX = 0.0f;
    float moveY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        int column = 0;
        int row = 0;
        int n = 0;
        int s = 0;
        int e = 0;
        int w = 0;
        float rotation = 0.0f;
        //int quad = 0;
        bool inverse;
        int exc;
        int rowChange = 0;
        int cornerFudge = 0;
        GameObject temp;
        //int x = 1;
        //int y = 1;
        int[,] activeLevelMap =
    {
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

        int[,] exceptions =
        {
            // Corner Pieces
            {2,5},
            {2,11},
            {4,2},
            {4,7},
            {4,13},
            {6,5},
            {6,8},
            {7, 2},
            {7, 10},
            {7, 13},
            {9, 0},
            {9, 5},
            {9, 8},
            {9, 11},
            {10, 13},
            {13, 7},
            // Straight Pieces
            {1, 13},
            {2, 13},
            {3, 2},
            {3, 5},
            {3, 7},
            {3, 11},
            {3, 13},
            {7, 7},
            {7, 8},
            {8, 7},
            {8, 8},
            {8, 13},
            {9, 7},
            {9, 13},
            {10, 7},
            {11, 7},
            {11, 8},
            {12, 7},
            {12, 8},
            {13, 10},
            {14, 10},
        };

        //Determine sprite and rotation of tile
        /*while (quad < 4)
        {
            rowChange = quad / 2;

            int count = 0;
            int countRow = 0;
            if (quad == 0)
            {
                startX = -20.2275f;
                foreach (int num in levelMap)
                {
                    activeLevelMap[countRow, count] = num;
                    count++;
                    countRow = count / 14;
                }
            }
            if (quad == 1)
            {
                startX = dis;
                foreach (int num in levelMap)
                {
                    activeLevelMap[countRow, 14 - count] = num;
                    count++;
                    countRow = count / 14;
                }
            }
            if (quad == 2)
            {
                activeLevelMap = new int[,]
            {
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            };
                startY = -dis;
                foreach (int num in levelMap)
                {
                    activeLevelMap[countRow, 14 - count] = num;
                    count++;
                    countRow = count / 14;
                    if (countRow > 13)
                    {
                        break;
                    }
                }
                Array.Reverse(activeLevelMap);
            }
            if (quad == 3)
            {
                startX = -20.2275f;
                foreach (int num in levelMap)
                {
                    activeLevelMap[countRow, count] = num;
                    count++;
                    countRow = count / 14;
                    if (countRow > 13)
                    {
                        break;
                    }
                }
                Array.Reverse(activeLevelMap);
            }*/

        foreach (int i in levelMap)
        {
                rotation = 0.0f;

                // Sets values of adjacent tiles
                if (row > 0) { n = levelMap[row - 1, column]; }
                else { n = 0; }
                if (row < 14 - rowChange) { s = levelMap[row + 1, column]; }
                else { s = 0; }
                if (column < 13) { e = levelMap[row, column + 1]; }
                else { e = 0; }
                if (column > 0) { w = levelMap[row, column - 1]; }
                else { w = 0; }

                // Determines rotation of tile

                if (i == 1)
                {
                    if (s == 2 && e == 2)
                    {
                        rotation = 0.0f;
                    }
                    else if (e == 2 && n == 2)
                    {
                        rotation = 90.0f;
                    }
                    else if (n == 2 && w == 2)
                    {
                        rotation = 180.0f;
                    }
                    else if (w == 2 && s == 2)
                    {
                        rotation = 270.0f;
                    }
                }

                if (i == 2)
                {
                    if ((e == 0 && w > 4) || (w == 0 && e > 4) || (e == 0 && w == 0)) //(e == 0 || w == 0) 
                    {
                        rotation = 90.0f;
                    }
                }

                if (i == 3)
                {
                    if (countAdjacent(3, n, s, e, w) + countAdjacent(4, n, s, e, w) == 2)
                    {
                        if ((s == 4 || s == 3) && (e == 4 || e == 3))
                        {
                            rotation = 180.0f;
                        }
                        else if ((n == 4 || n == 3) && (e == 4 || e == 3))
                        {
                            rotation = 270.0f;
                        }
                        else if ((n == 4 || n == 3) && (w == 4 || w == 3))
                        {
                            rotation = 0.0f;
                        }
                        else if ((s == 4 || s == 3) && (w == 4 || w == 3))
                        {
                            rotation = 90.0f;
                        }
                    }
                    else if (countAdjacent(3, n, s, e, w) + countAdjacent(4, n, s, e, w) == 1)
                    {
                        if (n == 4)
                        {
                            if (row == 0)
                            {
                                rotation = 0.0f;
                            }
                            else
                            {
                                rotation = 270.0f;
                            }
                        }
                        else
                        {
                            if (row == 0)
                            {
                                rotation = 90.0f;
                            }
                            else
                            {
                                rotation = 180.0f;
                            }
                        }
                    }
                    else
                    {
                        int ne = 1;
                        int se = 1;
                        int nw = 1;
                        int sw = 1;

                        if (row > 0 && column < 13) { ne = levelMap[row - 1, column + 1]; }
                        if (row < (14 - rowChange) && column < 13) { se = levelMap[row + 1, column + 1]; }
                        if (row > 0 && column > 0) { nw = levelMap[row - 1, column - 1]; }
                        if (row < (14 - rowChange) && column > 0) { sw = levelMap[row + 1, column - 1]; }

                        if (ne == 0 || ne == 5 || ne == 6) { rotation = 270.0f; }
                        else if (se == 0 || se == 5 || se == 6) { rotation = 180.0f; }
                        else if (nw == 0 || nw == 5 || nw == 6) { rotation = 0.0f; }
                        else /*(sw == 0 || sw == 5 || sw == 6)*/ { rotation = 90.0f; }

                        cornerFudge = 5;
                    }
                }

                if (i == 4)
                {
                    if (n == 5 || n == 6)
                    {
                        rotation = 0.0f;
                    }
                    else if (w == 5 || w == 6)
                    {
                        rotation = 90.0f;
                    }
                    else if (s == 5 || s == 6)
                    {
                        rotation = 180.0f;
                    }
                    else if (e == 5 || e == 6)
                    {
                        rotation = 270.0f;
                    }
                    else
                    {
                        if (n == 0 && s != 0 && e != 0 && w != 0)
                        {
                            rotation = 0.0f;
                        }
                        else if (w == 0 && s != 0 && e != 0 && n != 0)
                        {
                            rotation = 90.0f;
                        }
                        else if (s == 0 && n != 0 && e != 0 && w != 0)
                        {
                            rotation = 180.0f;
                        }
                        else if (e == 0 && s != 0 && n != 0 && w != 0)
                        {
                            rotation = 270.0f;
                        }
                        else
                        {
                            if (e == 0 && w == 0)
                            {
                                if (column > 6)
                                {
                                    rotation = 90.0f;
                                }
                                else
                                {
                                    rotation = 270.0f;
                                }
                            }
                            else
                            {
                                if (row > 6)
                                {
                                    rotation = 0.0f;
                                }
                                else
                                {
                                    rotation = 180.0f;
                                }
                            }
                        }
                    }
                }

                if (i == 7)
                {
                    if (w == 2)
                    {
                        rotation = 0.0f;
                    }
                    else if (s == 2)
                    {
                        rotation = 90.0f;
                    }
                    else if (e == 2)
                    {
                        rotation = 180.0f;
                    }
                    else if (n == 2)
                    {
                        rotation = 270.0f;
                    }
                }

            // Instantiate tile
            if (tiles[i] != null) // (i != 0 || i != 5 || i != 6)
            {
                // Manually fixes inverted tiles using the array called exceptions
                inverse = false;
                exc = 0;
                while (exc < 37) {
                    if ((row == exceptions[exc, 0]) && (column == exceptions[exc, 1]))
                    {
                        inverse = true;
                        //Debug.Log(exc + ": Set inverse true for exception ");
                        break; // Just breaks the while loop, right?
                    }
                    exc++;
                }

                temp = Instantiate(tiles[i + cornerFudge], new Vector2(column * dis + startX, startY - row * dis), Quaternion.identity);
                temp.transform.Rotate(new Vector3(0f, 0f, rotation));
                temp.transform.parent = gameObject.transform;

                moveX = (0.6975f - temp.transform.position.x);
                moveY = (0.6975f + temp.transform.position.y);

                temp = Instantiate(tiles[i + cornerFudge], new Vector2((column * dis + startX) + (2*moveX) - 1.395f, startY - row * dis), Quaternion.identity);
                temp.transform.Rotate(new Vector3(0f, 0f, rotation));
                temp.transform.parent = gameObject.transform;
                /*if (((i == 1) && (temp.transform.rotation.z == 90.00001f)) || ((i == 3) && (temp.transform.rotation.z == 90.00001f)))
                { temp.transform.localScale = new Vector3(1f, -1f, 1f); }
                else
                { temp.transform.localScale = new Vector3(-1f, 1f, 1f); }*/
                /*if (i == 3 && Mathf.Abs(rotation) == 90.0f)
                {
                    temp.transform.localScale = new Vector3(1f, 1f, 1f);
                    temp.transform.Rotate(new Vector3(0f, 0f, rotation + 180.0f));
                }
                else
                {
                    temp.transform.localScale = new Vector3(-1f, 1f, 1f);
                }*/
                if (inverse) { temp.transform.localScale = new Vector3(1f, -1f, 1f); /*Debug.Log(exc + ": Top-Right instance inverted");*/ }
                else { temp.transform.localScale = new Vector3(-1f, 1f, 1f); }

                temp = Instantiate(tiles[i + cornerFudge], new Vector2((column * dis + startX) + (2*moveX) - 1.395f, ((startY - row * dis)) - (2*moveY)), Quaternion.identity);
                temp.transform.Rotate(new Vector3(0f, 0f, rotation));
                temp.transform.localScale = new Vector3(-1f, -1f, 1f);
                temp.transform.parent = gameObject.transform;

                temp = Instantiate(tiles[i + cornerFudge], new Vector2(column * dis + startX, ((startY - row * dis)) - (2*moveY)), Quaternion.identity);
                temp.transform.Rotate(new Vector3(0f, 0f, rotation));
                temp.transform.parent = gameObject.transform;
                /*if (((i == 1) && (temp.transform.rotation.z == 90.00001f)) || ((i == 3) && (temp.transform.rotation.z == 90.00001f)))
                { temp.transform.localScale = new Vector3(1f, -1f, 1f); }
                else
                { temp.transform.localScale = new Vector3(-1f, 1f, 1f); }*/
                /*if (i == 3 && Mathf.Abs(rotation) == 90.0f)
                {
                    temp.transform.localScale = new Vector3(1f, 1f, 1f);
                    temp.transform.Rotate(new Vector3(0f, 0f, rotation + 180.0f));
                }
                else
                {
                    temp.transform.localScale = new Vector3(-1f, 1f, 1f);
                }*/
                if (inverse) { temp.transform.localScale = new Vector3(-1f, 1f, 1f); /*Debug.Log(exc + ": Bottom-Left instance inverted");*/ }
                else { temp.transform.localScale = new Vector3(1f, -1f, 1f); }

            }

            // Instantiate pellet
            if (i == 5)
            {
                moveX = (0.6975f - (column * dis + startX));
                moveY = (0.6975f + (startY - row * dis));

                Instantiate(pellets[0], new Vector2(column * dis + startX, startY - row * dis), Quaternion.identity);
                Instantiate(pellets[0], new Vector2((column * dis + startX) + (2 * moveX) - 1.395f, startY - row * dis), Quaternion.identity);
                Instantiate(pellets[0], new Vector2((column * dis + startX) + (2 * moveX) - 1.395f, ((startY - row * dis)) - (2 * moveY)), Quaternion.identity);
                Instantiate(pellets[0], new Vector2(column * dis + startX, ((startY - row * dis)) - (2 * moveY)), Quaternion.identity);
            }
            else if (i == 6)
            {
                moveX = (0.6975f - (column * dis + startX));
                moveY = (0.6975f + (startY - row * dis));

                Instantiate(pellets[1], new Vector2(column * dis + startX, startY - row * dis), Quaternion.identity);
                Instantiate(pellets[1], new Vector2((column * dis + startX) + (2 * moveX) - 1.395f, startY - row * dis), Quaternion.identity);
                Instantiate(pellets[1], new Vector2((column * dis + startX) + (2 * moveX) - 1.395f, ((startY - row * dis)) - (2 * moveY)), Quaternion.identity);
                Instantiate(pellets[1], new Vector2(column * dis + startX, ((startY - row * dis)) - (2 * moveY)), Quaternion.identity);
            }

            // Debug.Log("Row: " + row + ", Column: " + column + ", n/s/e/w: " + n + "/" + s + "/" + e + "/" + w + ", Rotation: " + rotation + ", Tile: " + i);

            // Reset/Increment variables
            column++;
            cornerFudge = 0;
            // x = 1;
            // y = 1;

            if (column == 14)
            {
                    column = 0;
                    row++;
            }
            }

            /*quad++;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int countAdjacent(int i, int n, int s, int e, int w)
    {
        int count = 0;
        if (i == n) { count++; }
        if (i == s) { count++; }
        if (i == e) { count++; }
        if (i == w) { count++; }
        return count;
    }
}
