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
            AddItem(item);
        }
    }

    public void OpenBuyingWindow()
    {
        this.status = "buying";
        gameController.ItemWindowManager.SetTwoColumnItemWindow("buying", lineUp, this);
    }

    public void OpenSellingWindow()
    {
        this.status = "selling";
        List<Item> items = gameController.AllyManager.Allies[0].Items;
        gameController.ItemWindowManager.SetTwoColumnItemWindow("selling", items, this);
    }


    public void BuyItem(Item item)
    {
        lineUp.Remove(item);
        gameController.Gold -= item.PurchasePrice;
        gameController.AllyManager.AddItem(item);

    }

    public void SellItem(Item item)
    {
        gameController.AllyManager.Allies[0].Items.Remove(item);
        gameController.Gold += item.SellingPrice;
    }

    public void AddItem(Item item)
    {

        for (int i = 0; i < 1000; i++)
        {
            string newName = item.ID + i.ToString();
            if (!lineUp.Exists(x => x.ID == newName))
            {
                item.ID = newName;
                break;
            }
        }

        this.lineUp.Add(item);
    }



}

