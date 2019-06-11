using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {

    Player player;

    public enum CellType
    {
        wall, plane, enemy, shop, goal,
        trap, Event, human, fountain, treasure, rareTreasure
    }

    private CellType type;
    private (int x, int y) coord;
    private (double x, double y) position;

    //店なら
    private Shop shop;

    //敵のリスト
    private List<Enemy> enemies;

    public GameController gameController;

    private GameObject cellObject;
    private GameObject topPlane;
    private GameObject parentPanel;
    
    private GameObject planeCell;

    private GameObject darkCell;
    private GameObject darkCellParent;

    public Shop Shop { get => shop; set => shop = value; }
    public List<Enemy> Enemies { get => enemies; set => enemies = value; }
    public CellType Type { get => type; set => type = value; }

    // Use this for initialization
    void Start () {
        planeCell = (GameObject)Resources.Load("Prefab/PlaneCell");
    } 
	
	// Update is called once per frame
	void Update () {

    }

    public void Init(GameController gameController, GameObject cellObject, int coordX, int coordY, int width,
        GameObject parent, Player player, CellType cellType, GameObject darkCell)
    {

        this.gameController = gameController;

        this.coord = (coordX, coordY);

        this.player = player;
        this.cellObject = Instantiate(cellObject) as GameObject;
        this.darkCell = Instantiate(darkCell) as GameObject;
        this.parentPanel = parent;

        this.Type = cellType;

        if (this.Type == CellType.wall)
        {
            position = (coord.x * width, coord.y * width + 0.5);
        }
        else
        {
            position = (coord.x * width, coord.y * width);
        }
        

        topPlane = this.cellObject.transform.Find("plane").gameObject;

        this.cellObject.transform.position = new Vector3((float)position.x, (float)position.y, 0);
        this.cellObject.transform.SetParent(parent.transform, false);
        string name = "";
        name += coord.x.ToString() + "," + coord.y.ToString();

        darkCellParent = GameObject.Find("DarkCells");
        position = (coord.x * width, coord.y * width);
        this.darkCell.transform.position = new Vector3((float)position.x, (float)position.y, 0);
        this.darkCell.transform.SetParent(darkCellParent.transform, true);

        SetActivePlane(false);

        this.cellObject.name = name;
    }


    public void ConvertCellType(CellType cellType)
    {
        GameObject newCellObject;
        this.Type = cellType;

        string newName = this.cellObject.name;
        Vector3 pos = this.cellObject.transform.position;
        double y = pos.y;
        if (pos.y % 1 != 0 && cellType != CellType.wall)
        {
            pos = new Vector3((float)pos.x, (float)(pos.y - 0.5), (float)pos.z);
        }

        switch (cellType)
        {
            case CellType.enemy:
                newCellObject = (GameObject)Resources.Load("Prefab/EnemyCell");
                break;
            case CellType.Event:
                newCellObject = (GameObject)Resources.Load("Prefab/EventCell");
                break;
            case CellType.fountain:
                newCellObject = (GameObject)Resources.Load("Prefab/FountainCell");
                break;
            case CellType.human:
                newCellObject = (GameObject)Resources.Load("Prefab/HumanCell");
                break;
            case CellType.plane:
                newCellObject = (GameObject)Resources.Load("Prefab/PlaneCell");
                break;
            case CellType.treasure:
                newCellObject = (GameObject)Resources.Load("Prefab/TreasureCell");
                break;
            case CellType.rareTreasure:
                newCellObject = (GameObject)Resources.Load("Prefab/RareTreasureCell");
                break;
            case CellType.shop:
                newCellObject = (GameObject)Resources.Load("Prefab/ShopCell");
                break;
            case CellType.trap:
                newCellObject = (GameObject)Resources.Load("Prefab/TrapCell");
                break;
            case CellType.wall:
                newCellObject = (GameObject)Resources.Load("Prefab/WallCell");
                break;
            case CellType.goal:
                newCellObject = (GameObject)Resources.Load("Prefab/GoalCell");
                break;
            default:
                newCellObject = (GameObject)Resources.Load("Prefab/PlaneCell");
                break;
        }

        Destroy(this.cellObject);

        this.cellObject = Instantiate(newCellObject) as GameObject;
        this.cellObject.transform.position = pos;
        this.cellObject.transform.SetParent(parentPanel.transform, false);
        this.cellObject.name = newName;


        if (cellType == CellType.shop)
        {
            CreateShop();
        }
        else if (cellType == CellType.enemy)
        {
            CreteEnemies();
            Debug.Log(enemies.Count);
        }

        topPlane = this.cellObject.transform.Find("plane").gameObject;

        SetActivePlane(true);

    }


    public void ClearDark()
    {
        this.darkCell.SetActive(false);
    }


    public void CreateShop()
    {
        shop = new Shop(gameController);
    }

    public void CreteEnemies()
    {
        enemies = gameController.EnemyGenerator.CreateEnemiesByCurrentFloorNum();
    }

    public void SetCellObject(GameObject g)
    {
        this.cellObject = g;
    }

    public void SetCoord(int x, int y)
    {
        coord = (x, y);
    }

    public (int x, int y) GetCoord()
    {
        return coord;
    }

    public void SetPosition(int x, int y)
    {
        this.position = (x, y);
    }

    //public (int x, int y) GetPosition()
    //{
    //    return position;
    //}

    public CellType GetCellType()
    {
        return Type;
    }

    public void SetCellType(CellType c)
    {
        this.Type = c;
    }

    public void SetActivePlane(bool b)
    {
        topPlane.SetActive(b);
    }

}
