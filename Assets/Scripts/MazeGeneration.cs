using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneration : MonoBehaviour {
    public GameObject Cube;
    public GameObject Player;
    private GameObject currentPlayer;
    private GameObject currentCamera;
    public GameObject Enemy;
    public GameObject Enemy2;
    public GameObject HealthAid;
    public GameObject ArmorAid;
    public GameObject RocketAid; 

    private static int Width = 20;
    private static int Height = 20;
    public static int[,] Maze;
    const int Empty = 102;
    const int Wall = 101;
    public const int ExpandConst = 4;

    public static Vector3 EndPoint;

    public static ArrayList WallList = new ArrayList();
    public static ArrayList EnemyList = new ArrayList();
    public static ArrayList ItemList = new ArrayList();

    // Use this for initialization
    void Start() {
        Player.SetActive(false); 
        SetPlayer();
        Maze = GenerateMaze(Width, Height, ExpandConst, currentPlayer);

     //   PrintFinalMaze(Maze);

        GenerateCubeInstances(Cube, Maze, ExpandConst); 
     }

    int maxEnemies = 30; 
    int enemyAmount = 0;
    int maxItem = 20;  
    int itemAmount = 0;
    void GenerateCubeInstances(GameObject cube, int[ , ] maze, int expand)
    {
        int enemyDist = (maze.GetLength(0) - 1) * (maze.GetLength(1) - 1) / maxEnemies;
        int stuffDistCount = 0;
        enemyAmount = 0;
        itemAmount = 0;
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if(maze[i, j] == 1)
                {
                    GameObject newCube = Instantiate(cube, CorrespondingPosition(i, j, Width, Height, expand), Quaternion.identity);
                    WallList.Add(newCube);
                //    newCube.hideFlags = HideFlags.HideAndDontSave;
                }
                else
                {
                    if(enemyAmount < maxEnemies && System.Math.Round(Random.value * 100, 0) > 95 && stuffDistCount >= enemyDist) 
                    {
                        stuffDistCount = 0; 
                        enemyAmount++;
                        GameObject currentEnemy = System.Math.Round(Random.value * 100, 0) > 50 ? Enemy : Enemy2;
                        GameObject newEnemy = Instantiate(currentEnemy, CorrespondingPosition(i, j, Width, Height, expand), Quaternion.identity);
                        newEnemy.SetActive(true);
                        newEnemy.GetComponent<EnemyAI>().Player = currentPlayer; 
                        EnemyList.Add(newEnemy);
                    }
                    else if(itemAmount < maxItem && System.Math.Round(Random.value * 100, 0) > 95 && stuffDistCount >= enemyDist)
                    {
                        itemAmount++;

                        int randNum = (int)(Random.value * 100);
                        GameObject newItem;
                        if(randNum < 33)
                        {
                            newItem = HealthAid;
                        }
                        else if(randNum > 66)
                        {
                            newItem = ArmorAid;
                        }
                        else
                        {
                            newItem = RocketAid;
                        }

                        GameObject currentNewItem = Instantiate(newItem, CorrespondingPosition(i, j, Width, Height, expand), Quaternion.identity);
                        Vector3 position = currentNewItem.transform.position;
                        currentNewItem.transform.position -= Vector3.up;
                        currentNewItem.SetActive(true);
                        ItemList.Add(currentNewItem); 
                    }
                    stuffDistCount++;
                }
            }
        }

    }

    public void ResetScene()
    {
        ClearAllClone();
        SetPlayer(); 
        Maze = GenerateMaze(Width, Height, ExpandConst, currentPlayer);
        GenerateCubeInstances(Cube, Maze, ExpandConst); 
    }

    public void SetPlayer()
    {
        currentPlayer = Instantiate(Player, Vector3.zero, Quaternion.identity);
        currentPlayer.SetActive(true);
        currentCamera = currentPlayer.transform.GetChild(0).gameObject; 
    }

    public void ClearAllClone()
    {
        for (int i = 0; i < WallList.Count; i++)
        {
            Destroy((GameObject)WallList[i]);
        }
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if((GameObject)EnemyList[i] != null)
            {
                Destroy((GameObject)EnemyList[i]);
            }
        }
        for (int i = 0; i < ItemList.Count; i++)
        {
            if ((GameObject)ItemList[i] != null)
            {
                Destroy((GameObject)ItemList[i]);
            }
        }

        WallList = new ArrayList();
        EnemyList = new ArrayList();
        ItemList = new ArrayList(); 
        Destroy(currentPlayer);
        Destroy(currentCamera); 
    }

    float yPosition = 2.5f;

    

    Vector3 CorrespondingPosition(int x, int y, int width, int height, int expand)
    {
        int tempX = x - width * expand / 2;
        int tempZ = y - height * expand / 2;
        
        Vector3 temp = new Vector3(tempX + 0.5f, yPosition, tempZ + 0.5f); 
        return temp;
    }

    int[ , ] GenerateMaze(int width, int height, int expand, GameObject player)
    {
        int[ , ] maze = new int[width, height];
        Random rand = new Random();

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (i == 0 || j == 0 || i == maze.GetLength(0) - 1 || j == maze.GetLength(1) - 1)
                {
                    maze[i, j] = Wall;
                }
                else
                {
                    maze[i, j] = (int)(System.Math.Round(Random.value, 2) * 100);
                }

            }
        }

        Stack<Vector2> stack = new Stack<Vector2>();
        Vector2 entrance = new Vector2(GenerateEntrance(Width), 0);
        Vector2 exit = new Vector2();
        stack.Push(new Vector2(entrance.x, 1));

        int max = Width * Height;
        int index = 0;

        while (stack.Count > 0)
        {

            if (index >= max)
            {
                break;
            }

            int x = (int)stack.Peek().x;
            int y = (int)stack.Peek().y;
            maze[x, y] = Empty;

            if(y == maze.GetLength(1) - 2)
            {
                exit = new Vector2(x, y + 1);
            }

            int[] arr = new int[] { maze[x - 1, y], maze[x, y + 1], maze[x + 1, y], maze[x, y - 1] };
            PrintArray(arr);
            int[] arrTemp = new int[] { maze[x - 1, y], maze[x, y + 1], maze[x + 1, y], maze[x, y - 1] };
            System.Array.Sort(arrTemp);
            int min = arrTemp[0];

            if (min >= Wall)
            {
                stack.Pop();
                continue;
            }
            else
            {
                int[] arrTemp2 = new int[] { maze[x - 1, y], maze[x, y + 1], maze[x + 1, y], maze[x, y - 1] };
                for (int i = 0; i < arrTemp.Length; i++)
                {
                    if (arrTemp[i] >= Wall)
                    {
                        stack.Pop();
                        break;
                    }
                    int id = System.Array.IndexOf(arrTemp2, arrTemp[i]);
                    arrTemp2[id] = -1;
                    Vector2 nextPos = new Vector2();
                    switch (id)
                    {
                        case 0:
                            nextPos = new Vector2(x - 1, y);
                            break;
                        case 1:
                            nextPos = new Vector2(x, y + 1);
                            break;
                        case 2:
                            nextPos = new Vector2(x + 1, y);
                            break;
                        case 3:
                            nextPos = new Vector2(x, y - 1);
                            break;
                    }

                    int nextX = (int)nextPos.x;
                    int nextY = (int)nextPos.y;
                    bool isLeftUpAllEmpty = IsA4EmptyCellBlock(nextX - 1, nextY - 1, nextX, nextY, maze);
                    bool isRightUpAllEmpty = IsA4EmptyCellBlock(nextX - 1, nextY + 1, nextX, nextY, maze);
                    bool isLeftDownAllEmpty = IsA4EmptyCellBlock(nextX + 1, nextY - 1, nextX, nextY, maze);
                    bool isRightDownAllEmpty = IsA4EmptyCellBlock(nextX + 1, nextY + 1, nextX, nextY, maze);

                    if (!isLeftUpAllEmpty && !isRightUpAllEmpty && !isLeftDownAllEmpty && !isRightDownAllEmpty)
                    {
                        stack.Push(nextPos);
                        break;
                    }
                }
            }
            index++;
        }

        maze[(int)exit.x, (int)exit.y] = Empty; 

        int[,] mazeFinal = new int[width * expand, height * expand];

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {

                for(int k = 0; k < expand; k++)
                {
                    for (int l = 0; l < expand; l++)
                    {
                        mazeFinal[i * expand + k, j * expand + l] = maze[i, j] == Empty ? 0 : 1;
                    }
                }
            }
        }

        Vector3 startPos = CorrespondingPosition((int)entrance.x * ExpandConst, (int)(entrance.y + 1) * ExpandConst, Width, Height, ExpandConst);
        startPos.x = startPos.x - .5f + ExpandConst / 2;
        startPos.z = startPos.z - .5f + ExpandConst / 2; 

        player.transform.position = startPos; 

        EndPoint = CorrespondingPosition((int)exit.x * ExpandConst, (int)(exit.y) * ExpandConst, Width, Height, ExpandConst);

        return mazeFinal;
    }

    int GenerateEntrance(int width)
    {
        return 1 + (int)(Random.value * (width - 2));
    }

    bool IsA4EmptyCellBlock(int i, int j, int i2, int j2, int[,] maze)
    {
        bool isEmptyCell = false;

        if (maze[i, j] != Empty)
        {
            return isEmptyCell;
        }

        int left = maze[i2, j2 - 1];
        int right = maze[i2, j2 + 1];
        int up = maze[i2 - 1, j2];
        int down = maze[i2 + 1, j2];

        if (i < i2 && j < j2)
        {
            isEmptyCell = left == Empty && up == Empty;
        }
        else if(i < i2 && j > j2)
        {
            isEmptyCell = right == Empty && up == Empty;
        }
        else if (i > i2 && j < j2)
        {
            isEmptyCell = left == Empty && down == Empty;
        }
        else if (i > i2 && j > j2)
        {
            isEmptyCell = right == Empty && down == Empty;
        }

        return isEmptyCell;
    }

    void PrintArray(int[] arr)
    {
        string str = "";
        for(int i = 0; i < arr.Length; i++)
        {
            str += arr[i] + " ";
        }
    }

    void PrintMaze(int[ , ] maze)
    {
        string str = "";
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == Wall)
                {
                    str += "xx ";
                }else if(maze[i, j] == Empty)
                {
                    str += "ee ";
                }
                else
                {
                    if(maze[i, j] >= 10)
                    {
                        str += maze[i, j] + " ";
                    }
                    else
                    {
                        str += "0" + maze[i, j] + " ";
                    }
                }
            }

            str += "\n";
        }

        Debug.Log(str);
    }

    void PrintFinalMaze(int[,] maze)
    {
        string str = "";
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                if (maze[i, j] == 1)
                {
                    str += "xx";
                }
                else
                {
                    str += "oo";
                }

                
            }
            str += "\n";
        }
        Debug.Log(str);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.R) && GameState.CurrentState != GameState.State.START)
        {
            GameState.CurrentState = GameState.State.START; 
            ResetScene(); 
        }

    }
}
