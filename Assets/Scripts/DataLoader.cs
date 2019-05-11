using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

public class DataLoader : MonoBehaviour
{
    public List<Item> itemData = new List<Item>();
    public List<Enemy> enemyData = new List<Enemy>();

    Encoding encoding;

    private string musicName; // 読み込む譜面の名前
    private string level; // 難易度
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVの中身を入れるリスト
    private int height = 0; // CSVの行数

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void load5()
    {
        csvFile = Resources.Load("CsvData/enemyData") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }

        Skill skill = new Skill();
        //skill.JobType = (Job)Enum.Parse(typeof(Job), jobtype, true);

    }
}
