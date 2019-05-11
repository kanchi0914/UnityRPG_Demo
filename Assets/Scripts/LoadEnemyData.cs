using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using UnityEngine;

using System.Text;
using System.IO;
using static EnumHolder;
using System;

public class LoadEnemyData : MonoBehaviour {
    Encoding encoding;

    private string musicName; // 読み込む譜面の名前
    private string level; // 難易度
    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト
    private int height = 0; // CSVの行数

    // Use this for initialization
    void Start () {
        //load4();
        load5();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void load4()
    {
        string path = Application.dataPath + "/data/enemyData.json";
        Debug.Log(path);

        using (var file = new System.IO.FileStream(
            path,
            System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            byte[] bytes = new byte[file.Length];

            // ファイルを読み込み
            file.Read(bytes, 0, bytes.Length);

            // 読み込んだbyte配列をJSON形式の文字列に変換
            string jsonString = System.Text.Encoding.UTF8.GetString(bytes);

            // JSON形式の文字列をセーブデータのクラスに変換
            EnemyClass enemyClass = JsonUtility.FromJson<EnemyClass>(jsonString);

            Debug.Log(enemyClass.skill[0]);
        }
    }


    private void load5()
    {
        csvFile = Resources.Load("csvData") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }

        string jobtype = csvDatas[2][4];
        

        Skill skill = new Skill();
        skill.JobType = (Job)Enum.Parse(typeof(Job), jobtype, true);

    }

}

 
