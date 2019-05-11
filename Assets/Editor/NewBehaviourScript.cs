using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static EnumHolder;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum TestE {a,b }


    public void load()
    {
        Convert.ToBoolean("true");

  
    }

    [Test]
    public void AllyTest()
    {
        ItemGenerator itemGenerator = new ItemGenerator();
    }

    [Test]
    public void ItemGenerateTest()
    {
        //Utility.GetStringOfEnum<ItemName>(ItemName.薬草);

        GameController gameController = new GameController();
        //gameController.Init();
        //ItemWindowManager itemWindowManager = new ItemWindowManager();
        //itemWindowManager.Init(gameController);
        //Item item = gameController.ItemGenerator.Generate("薬草");
        Item item = gameController.ItemGenerator.GetItemRandomlyByRatio();
    }

    [Test]
    public void ItemWindowTest()
    {
        GameController gameController = new GameController();
        ItemWindowManager itemWindowManager = new ItemWindowManager();
        itemWindowManager.Init(gameController);
        Item item = gameController.ItemGenerator.Generate("薬草");
        List<Item> items = new List<Item>() { item };
        itemWindowManager.SetTwoColumnItemWindow("menu", items);
        Debug.Log(item);
    }

}
