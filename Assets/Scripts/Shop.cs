using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Shop
{
    public enum Shoptype {normal};
    private Shoptype shoptype;

    private string status = "";

    GameController gameController;

    ItemGenerator itemGenerator;
    Transform shopPanel;

    private List<Item> lineUp = new List<Item>();

    public Shoptype Shoptype1 { get => shoptype; set => shoptype = value; }

    public Shop(GameController gameController)
    {
        this.gameController = gameController;
        itemGenerator = gameController.ItemGenerator;
        Init();
    }

    public void Init()
    {
        InitLineUp();
    }

    public void InitLineUp()
    {
        int itemNum = UnityEngine.Random.Range(4, 8);
        for (int i = 0; i < itemNum; i++)
        {
            Item item = itemGenerator.GetItemRandomlyByRatio();
            lineUp.Add(item);
        }
    }

    public void OpenBuyingWindow()
    {
        this.status = "buying";
        gameController.ItemWindowManager.SetTwoColumnItemWindow("buying", lineUp);
    }

    public void OpenSellingWindow()
    {
        this.status = "selling";
        List<Item> items = gameController.AllyManager.Allies[0].Items;
        gameController.ItemWindowManager.SetTwoColumnItemWindow("selling", lineUp);
    }



}

