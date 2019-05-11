using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using static EnumHolder;


public class EnemyGenerator
{
    //Encoding encoding;
    GameController gameController;

    ItemGenerator itemGenerator;
    SkillGenerator2 skillGenerator2;

    private List<string[]> monsterTable = new List<string[]>();

    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVの中身を入れるリスト
   
    private int height = 0; // CSVの行数

    public EnemyGenerator(GameController gameController)
    {
        this.gameController = gameController;
        itemGenerator = gameController.ItemGenerator; 
        skillGenerator2 = gameController.SkillGenerator;
        //Init();
    }

    void Init()
    {
        Load();
    }

    public void Load()
    {
        csvFile = Resources.Load("CsvData/enemyData") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(',')); // リストに入れる
        }

        LoadMonstarTable();

    }

    public void LoadMonstarTable()
    {
        csvFile = Resources.Load("CsvData/monsterTable") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            monsterTable.Add(line.Split(',')); // リストに入れる
        }
    }


    public Enemy Generate(string name)
    {

        Enemy enemy = new Enemy(gameController);
        string[] data = csvData.Find(s => s[1] == name);

        //Sprite sprite = 
        Debug.Log(data[0]);

        enemy.Image = Resources.Load<Sprite>("EnemyImage/" + data[0]);

        enemy.ID1 = data[0];

        enemy.Name = data[1];
        enemy.MinLv = int.Parse(data[2]);
        enemy.MaxLv = int.Parse(data[3]);
        enemy.Exp = int.Parse(data[4]);
        enemy.Gold = int.Parse(data[5]);

        //ドロップアイテム
        string[] dropItems = data[6].Split('/');
        string[] dropItemProbs = data[7].Split('/');
        for (int i = 0; i < dropItems.Length; i++)
        {
            Item item = itemGenerator.Generate(dropItems[i]);
            enemy.DropItems.Add(item, int.Parse(dropItemProbs[i]));
        }

        //ステータス
        enemy.Statuses[Status.Lv] = enemy.MinLv;
        enemy.Statuses[Status.MaxHP] = int.Parse(data[8]);
        enemy.Statuses[Status.currentHP] = int.Parse(data[8]);
        enemy.Statuses[Status.MaxSP] = int.Parse(data[9]);
        enemy.Statuses[Status.currentSP] = int.Parse(data[9]);
        enemy.Statuses[Status.STR] = int.Parse(data[10]);
        enemy.Statuses[Status.DEF] = int.Parse(data[11]);
        enemy.Statuses[Status.INT] = int.Parse(data[12]);
        enemy.Statuses[Status.MNT] = int.Parse(data[13]);
        enemy.Statuses[Status.TEC] = int.Parse(data[14]);
        enemy.Statuses[Status.AGI] = int.Parse(data[15]);
        enemy.Statuses[Status.LUK] = int.Parse(data[16]);

        // スキル
        string[] skills = data[17].Split('/');
        string[] skillProbs = data[18].Split('/');
        for (int i = 0; i < skills.Length; i++)
        {
            Skill skill = skillGenerator2.Generate(skills[i]);
            enemy.skills.Add(skill);
            int prob = int.Parse(skillProbs[i]);
            enemy.EnemyActions.Add(skill, prob);
        }

        //タイプ
        string[] types = data[19].Split('/');
        for (int i = 0; i < types.Length; i++)
        {
            enemy.EnemyTypes.Add((EnemyType)Enum.Parse(typeof(EnemyType), types[i], true));
        }

        //ユニークモンスター
        enemy.IsUnique = Convert.ToBoolean(data[20]);

        //属性耐性
        if (!string.IsNullOrEmpty(data[21]))
            enemy.AttributionResists[Attribution.fire] = int.Parse(data[21]);
        if (!string.IsNullOrEmpty(data[22]))
            enemy.AttributionResists[Attribution.thunder] = int.Parse(data[22]);
        if (!string.IsNullOrEmpty(data[23]))
            enemy.AttributionResists[Attribution.ice] = int.Parse(data[23]);
        if (!string.IsNullOrEmpty(data[24]))
            enemy.AttributionResists[Attribution.holy] = int.Parse(data[24]);
        if (!string.IsNullOrEmpty(data[25]))
            enemy.AttributionResists[Attribution.dark] = int.Parse(data[25]);
        //状態異常耐性
        if (!string.IsNullOrEmpty(data[26]))
            enemy.AilmentResists[Ailment.poison] = int.Parse(data[26]);
        if (!string.IsNullOrEmpty(data[27]))
            enemy.AilmentResists[Ailment.sleep] = int.Parse(data[27]);
        if (!string.IsNullOrEmpty(data[28]))
            enemy.AilmentResists[Ailment.paralysis] = int.Parse(data[28]);
        if (!string.IsNullOrEmpty(data[29]))
            enemy.AilmentResists[Ailment.confusion] = int.Parse(data[29]);
        if (!string.IsNullOrEmpty(data[30]))
            enemy.AilmentResists[Ailment.seal] = int.Parse(data[30]);
        if (!string.IsNullOrEmpty(data[31]))
            enemy.AilmentResists[Ailment.terror] = int.Parse(data[31]);
        if (!string.IsNullOrEmpty(data[32]))
        //    enemy.AilmentResists[Ailment.curse] = int.Parse(data[32]);
        //if (!string.IsNullOrEmpty(data[33]))
        //    enemy.AilmentResists[Ailment.bleeding] = int.Parse(data[33]);
        //if (!string.IsNullOrEmpty(data[34]))
        //    enemy.AilmentResists[Ailment.turnover] = int.Parse(data[34]);
        //if (!string.IsNullOrEmpty(data[35]))
            enemy.AilmentResists[Ailment.stun] = int.Parse(data[35]);
        //if (!string.IsNullOrEmpty(data[36]))
        //    enemy.AilmentResists[Ailment.death] = int.Parse(data[36]);

        return enemy;
    }

    public List<Enemy> CreateEnemiesByCurrentFloorNum()
    {
        var floor = gameController.Floor;
        var enemies = new List<Enemy>();
        Dictionary<string, int> monsterProb = new Dictionary<string, int>();

        string[] table = monsterTable[floor - 1];
        foreach (string s in table)
        {
            string[] probs = s.Split('/');
            monsterProb.Add(probs[0], int.Parse(probs[1]));
        }

        int num = UnityEngine.Random.Range(1, 3);

        for (int i = 0; i < num; i++)
        {
            int random = UnityEngine.Random.Range(0, 100);
            int currentNum = 0;
            foreach (KeyValuePair<string, int> kvp in monsterProb)
            {
                currentNum += kvp.Value;
                if (currentNum > random)
                {
                    AddEnemyToEnemies(enemies, kvp.Key);
                    break;
                }
            }
        }
        return enemies;
    }

    public void AddEnemyToEnemies(List<Enemy> enemies, string name)
    {
        Enemy enemy = Generate(name);
        int number = 1;

        AddLevel(enemy);

        enemy.ID1 = (enemy.Name + number.ToString());
        enemy.Name = ($"{enemy.Name}Lv{enemy.Statuses[Status.Lv]}");

        while (true)
        {
            if (enemies.Exists(e => e.ID1 == enemy.ID1))
            {
                number++;
                enemy.ID1 = (enemy.Name + number.ToString());
            }
            else
            {
                break;
            }
        }

        enemies.Add(enemy);
        //CreateEnemyObject(enemy);
    }

    public void AddLevel(Enemy enemy)
    {
        int uppedLv = Utility.Poisson(1.0);

        //モンスターのレベル
        enemy.AddLv(uppedLv);
    }


}
