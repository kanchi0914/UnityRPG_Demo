using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static EnumHolder;
using System;

public class AllyGenerator
{
    ItemGenerator itemGenerator;
    SkillGenerator2 skillGenerator2;

    private TextAsset csvFile; // CSVファイル
    private List<string[]> raceData = new List<string[]>(); // CSVの中身を入れるリスト
    private List<string[]> jobData = new List<string[]>(); // CSVの中身を入れるリスト

    private List<Ally> raceInfos = new List<Ally>();
    private List<Ally> jobInfos = new List<Ally>();

    private int height = 0; // CSVの行数

    public List<Ally> RaceInfos { get => raceInfos; private set => raceInfos = value; }
    public List<Ally> JobInfos { get => jobInfos; set => jobInfos = value; }

    public AllyGenerator(ItemGenerator itemGenerator, SkillGenerator2 skillGenerator)
    {
        this.itemGenerator = itemGenerator;
        this.skillGenerator2 = skillGenerator;
    }

    public void Load()
    {
        csvFile = Resources.Load("CsvData/raceData") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            raceData.Add(line.Split(',')); // リストに入れる
        }

        csvFile = Resources.Load("CsvData/jobData") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            jobData.Add(line.Split(',')); // リストに入れる
        }

    }

    public Ally Generate(string name, string raceName, string jobName, GameController gameController)
    {
        Ally ally = new Ally(gameController);
        string[] data = raceData.Find(s => s[1] == raceName);

        ally.Job = (Job)Enum.Parse(typeof(Job), jobName, true);

        ally.Name = name;
        ally.ID1 = name;
        //ally.SetID(name);

        ally.Race = (Race)Enum.Parse(typeof(Race), data[1], true);

        int index = 3;

        ally.Statuses[Status.Lv] = 1;
        ally.Statuses[Status.MaxHP] = int.Parse(data[3]);
        ally.Statuses[Status.currentHP] = int.Parse(data[3]);
        ally.Statuses[Status.MaxSP] = int.Parse(data[4]);
        ally.Statuses[Status.currentSP] = int.Parse(data[4]);
        ally.Statuses[Status.STR] = int.Parse(data[5]);
        ally.Statuses[Status.DEF] = int.Parse(data[6]);
        ally.Statuses[Status.INT] = int.Parse(data[7]);
        ally.Statuses[Status.MNT] = int.Parse(data[8]);
        ally.Statuses[Status.TEC] = int.Parse(data[9]);
        ally.Statuses[Status.AGI] = int.Parse(data[10]);
        ally.Statuses[Status.LUK] = int.Parse(data[11]);

        foreach(Status key in ally.Statuses.Keys)
        {
            ally.InitialStatuses[key] = ally.Statuses[key];
        }

        string[] hps = data[12].Split('/');
        foreach (String s in hps)
        {
            ally.LvUpHps.Add(int.Parse(s));
        }

        string[] sps = data[13].Split('/');
        foreach (String s in sps)
        {
            ally.LvUpSps.Add(int.Parse(s));
        }

        return ally;

        
    }

}
