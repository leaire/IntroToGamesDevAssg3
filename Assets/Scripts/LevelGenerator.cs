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
    float start = 20.2275f;

    // Start is called before the first frame update
    void Start()
    {
        bool first = true;
        int column = 0;
        int row = 0;
        int tile = 0;
        int n = 8;
        int s = 8;
        int e = 8;
        int w = 8;
        int prevTile = 7;
        float rotation = 0.0f;
        float prevRotation = 0.0f;
        GameObject temp;
        int x = 1;
        int y = 1;

        //Determine sprite and rotation of tile
        foreach (int i in levelMap)
        {
            if (!first) {
                if (row > 0) { n = levelMap[row - 1, column]; }
                else { n = 0; }
                if (row < 14) { s = levelMap[row + 1, column]; } // Will need to fix to work for lower 2 quadrants
                else { s = 0; }
                if (column < 13) { e = levelMap[row, column + 1]; }
                else { e = 0; }
                if (column > 0) { w = levelMap[row, column - 1]; }
                else { w = 0; }

                if (i == 0 || i == 5 || i == 6) // Tiles 0, 6 Done
                {
                    /*if (((prevTile == 2) && prevRotation == 90.0f) || ((prevTile == 3) && prevRotation <= 90.0f) || ((prevTile == 4) && prevRotation == 90.0f) || prevTile == 0)
                    {
                        tile = 0;
                    }
                    if ((prevTile == 4 && prevRotation == 90.0f) || prevTile == 6)
                    {
                        tile = 6;
                    }
                    else
                    {
                        tile = 9;
                    }*/

                    if (i == 5 || i == 6)
                    {
                        tile = 0;
                    }
                    else
                    {
                        if (n == 7 || n == 1 || n == 2 || s == 7 || s == 1 || s == 2 || e == 7 || e == 1 || e == 2 || w == 7 || w == 1 || w == 2)
                        {
                            if (row > 13)
                            {
                                tile = 0;
                            }
                            else
                            {
                                tile = 12;
                            }
                        }
                        else
                        {
                            if (numberOfAdjacentXTiles(n, s, e, w, 4) > 2)
                            {
                                tile = 6;
                            }
                            else
                            {
                                if ((row == 0 || row > 12) && (column < 3 || column > 10)) // Hardcoded grass for ghost area, causes issues
                                {
                                    tile = 6;
                                }
                                else
                                {
                                    if (n == 0 && s == 0 && e == 0 && w == 0)
                                    {
                                        tile = 10;
                                    }
                                    else
                                    {
                                        tile = 0;
                                    }
                                }
                            }
                        }
                    }
                    

                    int rotationRand = Random.Range(0, 5);
                    rotation = rotationRand * 90;
                }

                if (i == 1) // Tiles 1, 8
                {
                    if (i > 3) { // Dummy test
                        tile = 1;
                    }
                    else
                    {
                        tile = 8;
                    }

                    if (prevTile > 9)
                    {
                        rotation = 0.0f;
                    }
  
                }

                if (i == 2) // Tile 2 Done?
                {
                    tile = 2;
                    // Facing a pellet tile
                    if (n != 7 && n > 4)
                    {
                        rotation = 180.0f;
                    }
                    else if (s != 7 && s > 4)
                    {
                        rotation = 0.0f;
                    }
                    else if (e != 7 && e > 4)
                    {
                        rotation = 20.0f;
                    }
                    else if (w != 7 && w > 4)
                    {
                        rotation = 270.0f;
                    }
                    // Facing a pelletless tile
                    else if (n == 0 && s == 0)
                    {
                        if (row < 7)
                        {
                            if (row < 3)
                            {
                                rotation = 180.0f;
                            }
                            else
                            {
                                rotation = 0.0f;
                            }
                        }
                        else
                        {
                            if (row < 11)
                            {
                                rotation = 180.0f;
                            }
                            else
                            {
                                rotation = 0.0f;
                            }
                        }
                    }
                }

                if (i == 3) // Tiles 3, 9
                {
                    tile = 3;
                }

                if (i == 4) // Tile 4
                {
                    tile = 4;
                    // Facing pellet tile
                    if ((n != 7 && n > 4))
                    {
                        rotation = 180.0f;
                    }
                    if ((s != 7 && s > 4))
                    {
                        rotation = 90.0f;
                    }
                    if ((e != 7 && e > 4))
                    {
                        rotation = 90.0f;
                    }
                    if ((w != 7 && w > 4))
                    {
                        rotation = 270.0f;
                    }
                    // Facing a pelletless tile
                    if (n == 0 && s == 0)
                    {
                        if (row < 7)
                        {
                            rotation = 0.0f;
                        }
                        else
                        {
                            rotation = 180.0f;
                        }
                    }
                }

                if (i == 7) // Tile 5
                {
                    tile = 5;
                    if (row > 0)
                    {
                        y = -1;
                    }
                    if (column == 0)
                    {
                        x = -1;
                    }
                }
            }
            else
            {
                tile = 1;
                first = false;
            }

            // Instantiate tile
            if (tile < 9)
            {
                temp = Instantiate(tiles[tile], new Vector2(column * dis - start, start - row * dis), new Quaternion(0.0f, 0.0f, rotation, 0.0f));
                temp.transform.localScale = new Vector2(x, y);
            }

            // Instantiate pellet
            if (i == 5)
            {
                ;
            }
            else if (i == 6)
            {
                ;
            }

            // Reset/Increment variables
            column++;
            n = 8;
            s = 8;
            e = 8;
            w = 8;
            prevRotation = rotation;
            prevTile = tile;
            x = 1;
            y = 1;

            if (column == 14)
            {
                column = 0;
                row++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int numberOfAdjacentXTiles(int n, int s, int e, int w, int x)
    {
        int count = 0;
        int[] array = {n, s, e, w};

        foreach (int tile in array)
        {
            if (tile == x)
            {
                count++;
            }
        }

        return count;
    }
}
