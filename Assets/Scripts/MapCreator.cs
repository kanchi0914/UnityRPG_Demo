using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    public GameController gameController;

    public enum WallType
    {
        horizon, vertical, topLeft, topRight,
        bottomLeft, bottomRight, top, left, right, bottom,
        leftT, topT, rightT, bottomT, cross, pillar
    }
    WallType wallType;

    //private (int x, int y) initialPos;
    private (int x, int y) initialCoord;
    private int mapSize;
    private Cell[,] cells;
    private int width;

    int[,] map;

    public GameObject planeCell;
    public GameObject wallCell;
    public GameObject trapCell;
    public GameObject shopCell;
    public GameObject humanCell;
    public GameObject treasureCell;
    public GameObject fountainCell;
    public GameObject rareTreasureCell;
    public GameObject eventCell;

    public GameObject horizon;
    public GameObject vertical;
    public GameObject topLeft;
    public GameObject topRight;
    public GameObject bottomLeft;
    public GameObject bottomRight;
    public GameObject top;
    public GameObject left;
    public GameObject right;
    public GameObject bottom;

    public GameObject topT;
    public GameObject bottomT;
    public GameObject leftT;
    public GameObject rightT;
    public GameObject cross;
    public GameObject pillar;

    public GameObject mazeParent;

    public Transform mapPanel;

    //public GameObject mainCamera;

    //TODO: 後で統合
    private GameObject playerObject;
    private Player player;

    //public GameObject dark;

    private int playerPosX = 0;
    private int playerPosY = 0;

    private int hideCellCount = 99;

    (int x, int y) initialPlayerCoord = (0, 0);

    private List<(int, int)> planePanelCoords = new List<(int, int)>();

    private MazeCreator_Extend mazeCreator;

    // Use this for initialization
    void Start()
    {

    }

    public void Init(GameController gameController, GameObject playerObject,
        int mapSize, Cell[,] cells, int width)
    {
        this.gameController = gameController;
        this.player = gameController.Player;
        this.playerObject = playerObject;
        this.mapSize = mapSize;
        this.cells = cells;
        this.width = width;

        mazeCreator = new MazeCreator_Extend(mapSize, mapSize);

        //MazeCreator_Extend.DebugPrint(map);

        //InitMap();

        //playerPosX = UnityEngine.Random.Range(1, 9);
        //playerPosY = UnityEngine.Random.Range(-9, -1);

        //playerObject.transform.position = new Vector3((float)initialPlayerCoord.x * width, 
        //    initialPlayerCoord.y * width, 0);

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void InitMap()
    {
        //mazeCreator = new MazeCreator_Extend(mapSize, mapSize);

        map = mazeCreator.CreateMaze();

        BreakWallPre(30);
        CreateMap2();

        initialPlayerCoord = planePanelCoords[UnityEngine.Random.Range(0, planePanelCoords.Count)];
        //要注意
        planePanelCoords.Remove(initialPlayerCoord);

        //AddCellType(Cell.CellType.enemy, 10);

        AddCellType(Cell.CellType.shop, 10);
        //AddCellType(Cell.CellType.rareTreasure, 1);
        //AddCellType(Cell.CellType.trap, 5);
        //AddCellType(Cell.CellType.Event, 5);
        //AddCellType(Cell.CellType.fountain, 5);
        //AddCellType(Cell.CellType.human, 50);
        //AddCellType(Cell.CellType.shop, 5);

        AddGoal();

    }

    public (int x, int y) GetInitialPlayerCoord()
    {
        return initialPlayerCoord;
    }

    public (int x, int y) ConvertToMatrixNum(int coordX, int coordY)
    {
        return (-coordY, coordX);
    }

    public WallType CheckWallType(int[,] map, int coordX, int coordY)
    {
        WallType wallType;

        (int x, int y) topLeft = ConvertToMatrixNum(coordX - 1, coordY + 1);
        (int x, int y) top = ConvertToMatrixNum(coordX, coordY + 1);
        (int x, int y) topRight = ConvertToMatrixNum(coordX + 1, coordY + 1);
        (int x, int y) right = ConvertToMatrixNum(coordX + 1, coordY);
        (int x, int y) bottomRight = ConvertToMatrixNum(coordX + 1, coordY - 1);
        (int x, int y) bottom = ConvertToMatrixNum(coordX, coordY - 1);
        (int x, int y) bottomLeft = ConvertToMatrixNum(coordX - 1, coordY - 1);
        (int x, int y) left = ConvertToMatrixNum(coordX - 1, coordY);

        //外壁
        if (coordX == 0)
        {
            if (coordY == 0)
            {
                return WallType.topLeft;
            }
            else if (map[right.x, right.y] == 1)
            {
                return WallType.rightT;
            }
            else
            {
                return WallType.vertical;
            }
        }
        else if (coordY == 0)
        {
            if (coordX == mapSize - 1)
            {
                return WallType.topRight;
            }
            else if (map[bottom.x, bottom.y] == 1)
            {
                return WallType.bottomT;
            }
            else
            {
                return WallType.horizon;
            }
        }
        else if (coordX == mapSize - 1)
        {
            if (coordY == -(mapSize - 1))
            {
                return WallType.bottomLeft;
            }
            else if (map[left.x, left.y] == 1)
            {
                return WallType.leftT;
            }
            else
            {
                return WallType.vertical;
            }
        }
        else if (coordY == -(mapSize - 1))
        {
            if (coordX == mapSize - 1)
            {
                return WallType.bottomRight;
            }
            else if (map[top.x, top.y] == 1)
            {
                return WallType.topT;
            }
            else
            {
                return WallType.horizon;
            }
        }

        //内壁
        if (map[top.x, top.y] == 1)
        {
            if (map[right.x, right.y] == 1)
            {
                if (map[left.x, left.y] == 1)
                {
                    if (map[bottom.x, bottom.y] == 1)
                    {
                        return WallType.cross;
                    }
                    return WallType.topT;
                }
                else if (map[bottom.x, bottom.y] == 1)
                {
                    return WallType.rightT;
                }
                else
                {
                    return WallType.bottomLeft;
                }
            }
            else if (map[left.x, left.y] == 1)
            {
                if (map[bottom.x, bottom.y] == 1)
                {
                    return WallType.leftT;
                }
                return WallType.bottomRight;
            }
            else if (map[bottom.x, bottom.y] == 1)
            {
                if (map[left.x, left.y] == 1 && map[right.x, right.y] == 1)
                {
                    return WallType.bottomT;
                }
                return WallType.vertical;
            }
            else
            {
                return WallType.bottom;
            }

        }
        else if (map[bottom.x, bottom.y] == 1)
        {
            if (map[right.x, right.y] == 1)
            {
                if (map[left.x, left.y] == 1)
                {
                    return WallType.bottomT;
                }
                else
                {
                    return WallType.topLeft;
                }

            }
            else if (map[left.x, left.y] == 1)
            {
                return WallType.topRight;
            }
            else
            {
                return WallType.top;
            }
        }
        else if (map[right.x, right.y] == 1)
        {
            if (map[left.x, left.y] == 1)
            {
                return WallType.horizon;
            }
            else
            {
                return WallType.left;
            }
        }
        else if (map[left.x, left.y] == 1)
        {
            return WallType.right;
        }
        else
        {
            return WallType.pillar;
        }



    }

    //暗闇を追加する場合
    public void HideCells()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (cells[i, j].GetCellType() != Cell.CellType.wall ||
                    cells[i, j].GetCellType() != Cell.CellType.plane)
                {
                    if (Math.Abs(cells[i, j].GetCoord().x - player.playerCoord.x) < hideCellCount &&
                        Math.Abs(cells[i, j].GetCoord().y - player.playerCoord.y) < hideCellCount)
                    {
                        cells[i, j].SetActivePlane(false);
                    }
                    else
                    {
                        //cells[i,j].SetActivePlane(true);
                    }
                }

            }

        }
    }

    void CreateMap2()
    {

        foreach (Transform child in mapPanel)
        {
            if (child.name != "Player")
            {
                Destroy(child.gameObject);
            }
        }


        planePanelCoords = new List<(int, int)>();
        GameObject normal = planeCell;
        GameObject wall = wallCell;

        //int currentPosX = initialPos.x;
        //int currentPosY = initialPos.y;

        //int currentCoordX = initialPos.x;
        //int currentCoordY = initialPos.y;
        int currentCoordX = 0;
        int currentCoordY = 0;

        for (int i = 0; i < map.GetLength(0); i++)
        {
            currentCoordX = 0;
            for (int j = 0; j < map.GetLength(1); j++)
            {
                GameObject obj;
                cells[i, j] = new Cell();

                //道
                if (map[i, j] == 0)
                {
                    cells[i, j].Init(gameController, normal, currentCoordX, currentCoordY, width,
                        mazeParent, player, Cell.CellType.plane);
                    planePanelCoords.Add((currentCoordX, currentCoordY));
                }
                //壁
                else
                {
                    GameObject wallObject;
                    //if (i != 0 && i != map.GetLength(0) - 1
                    //    && j != 0 && j != map.GetLength(0) - 1)
                    //{
                    WallType wallType = CheckWallType(map, currentCoordX, currentCoordY);
                    switch (wallType)
                    {
                        case WallType.bottom:
                            wallObject = bottom;
                            break;
                        case WallType.bottomLeft:
                            wallObject = bottomLeft;
                            break;
                        case WallType.bottomRight:
                            wallObject = bottomRight;
                            break;
                        case WallType.horizon:
                            wallObject = horizon;
                            break;
                        case WallType.left:
                            wallObject = left;
                            break;
                        case WallType.right:
                            wallObject = right;
                            break;
                        case WallType.top:
                            wallObject = top;
                            break;
                        case WallType.topLeft:
                            wallObject = topLeft;
                            break;
                        case WallType.topRight:
                            wallObject = topRight;
                            break;
                        case WallType.vertical:
                            wallObject = vertical;
                            break;
                        case WallType.leftT:
                            wallObject = leftT;
                            break;
                        case WallType.rightT:
                            wallObject = rightT;
                            break;
                        case WallType.topT:
                            wallObject = topT;
                            break;
                        case WallType.bottomT:
                            wallObject = bottomT;
                            break;
                        case WallType.cross:
                            wallObject = cross;
                            break;
                        case WallType.pillar:
                            wallObject = pillar;
                            break;
                        default:
                            wallObject = pillar;
                            break;
                    }

                    cells[i, j].Init(gameController, wallObject, currentCoordX, currentCoordY, width,
                        mazeParent, player, Cell.CellType.wall);
                }

                //逆になるので注意
                cells[i, j].SetCoord(j, -i);

                cells[i, j].SetActivePlane(true);

                currentCoordX += 1;
            }
            currentCoordY -= 1;
        }
    }

    void BreakWallPre(int prob)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (map[i,j] == 1)
                {
                    if (i != 0 && i != map.GetLength(0) - 1 && j != 0 && j !=
                        map.GetLength(1) - 1)
                    {
                        int r = random.Next(100);
                        if (r < prob)
                        {
                            map[i, j] = 0;
                        }
                    }

                }
            }
        }
    }

    void AddGoal()
    {
        System.Random random = new System.Random();
        int r = random.Next(planePanelCoords.Count);
        for (int i = 0; i < planePanelCoords.Count; i++)
        {
            if (i == r)
            {
                (int x, int y) matrix = ConvertToMatrixNum(planePanelCoords[i].Item1,
                    planePanelCoords[i].Item2);
                cells[matrix.x, matrix.y].ConvertCellType(Cell.CellType.goal);
                //planePanelCoords.Remove();
                break;
            }
        }
    }

    void AddCellType(Cell.CellType celltype, int prob)
    {
        System.Random random = new System.Random();
        List<(int x, int y)> newCoords = new List<(int x, int y)>();
        foreach ((int x, int y) current in planePanelCoords)
        {
            int r = random.Next(100);
            if (r < prob)
            {
                (int x, int y) matrix = ConvertToMatrixNum(current.x, current.y);
                cells[matrix.x, matrix.y].ConvertCellType(celltype);
            }
            else
            {
                newCoords.Add(current);
            }
        }
        planePanelCoords = new List<(int, int)>(newCoords);
    }

    void BreakWall2()
    {
        //GameObject normal = planeCell;
        //System.Random random = new System.Random();
        //int currentPosX = 0;
        //int currentPosY = 0;
        //for (int i = 0; i < cells.GetLength(0); i++)
        //{
        //    currentPosX = 0;
        //    for (int j = 0; j < cells.GetLength(1); j++)
        //    {
        //        if (cells[i, j].GetCellType() == Cell.CellType.wall)
        //        {
        //            //外壁でない
        //            if (i != 0 && i != map.GetLength(0) - 1 && j != 0 && j !=
        //                map.GetLength(1) - 1)
        //            {
        //                int r = random.Next(100);
        //                if (r > 60)
        //                {
        //                    cells[i, j].ConvertCellType(Cell.CellType.plane);
        //                }
        //            }
        //        }
        //        currentPosX += width;
        //    }
        //    currentPosY -= width;
        //}
    }

    void CreateEnemy2()
    {
        //System.Random random = new System.Random();
        //int currentPosX = 0;
        //int currentPosY = 0;
        //for (int i = 0; i < cells.GetLength(0); i++)
        //{
        //    currentPosX = 0;
        //    for (int j = 0; j < cells.GetLength(1); j++)
        //    {
        //        if (cells[i, j].GetCellType() != Cell.CellType.wall)
        //        {
        //            //外壁でない
        //            if (i != 0 && i != map.GetLength(0) - 1 && j != 0 && j !=
        //                map.GetLength(1) - 1)
        //            {
        //                int r = random.Next(100);
        //                if (r > 74)
        //                {
        //                    cells[i, j].ConvertCellType(Cell.CellType.enemy);
        //                }
        //            }
        //        }
        //        currentPosX += width;
        //    }
        //    currentPosY -= width;
        //}
    }

    public Cell[,] GetCells()
    {
        return this.cells;

    }

}
