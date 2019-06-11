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
    public void tesss()
    {
        var o = new Oaa();
        o.AAAA();
    }


    public class Oaa
    {
        public struct aaa { public string Name; };

        public void AAAA()
        {
            var b = getStrunct("aaaaaa");
            for (int i = 0; i < 10; i++)
            {
                aaa c = new aaa();

                c = getStrunct(i.ToString());
                Console.WriteLine(c);
            }
            Console.WriteLine(b);
        }

        public aaa getStrunct(string s)
        {
            var a = new aaa();
            a.Name = s;
            return a;
        }
    }


    [Test]
    public void AllyTest()
    {
        var aa = Resources.Load("Sounds/Purchased", typeof(AudioClip)) as AudioClip;
        //if (bbb == null)
        //{
        //    throw new Exception();
        //}
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        //audio.clip = aa;
        //audio.Play();
        //bbb.Play();
        //aa.Play();
    }

    //[Test]
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

    //[Test]
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
