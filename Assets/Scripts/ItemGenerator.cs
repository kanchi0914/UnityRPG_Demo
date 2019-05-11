using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static EnumHolder;

public class ItemGenerator
{

    private TextAsset csvFile; // CSVファイル
    private List<string[]> itemData = new List<string[]>(); // CSVの中身を入れるリスト
    private List<string[]> equipmentData = new List<string[]>();

    private List<Item> itemInfo = new List<Item>();
    private List<Equipment> equipmentInfo = new List<Equipment>();

    private int height = 0; // CSVの行数

    public List<Item> ItemInfo { get => itemInfo; private set => itemInfo = value; }
    public List<Equipment> EquipmentInfo { get => equipmentInfo; private set => equipmentInfo = value; }

    public ItemGenerator()
    {

    }

    public void Load()
    {
        csvFile = Resources.Load("CsvData/itemData_temp") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            itemData.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }

        csvFile = Resources.Load("CsvData/equipmentData_temp") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        reader = new StringReader(csvFile.text);

        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            equipmentData.Add(line.Split(',')); // リストに入れる
            height++; // 行数加算
        }

        //武器の情報を登録
        foreach(string[] data in itemData)
        {
            Item item = Generate(data[0]);
            itemInfo.Add(item);
        }

        //防具の情報を登録
        foreach(string[] data in equipmentData)
        {
            if (data[2] == "weapon")
            {
                Weapon weapon = GenerateWeapon(data[0]);
                equipmentInfo.Add(weapon);
            }
            else
            {
                Guard guard = GenerateGuard(data[0]);
                equipmentInfo.Add(guard);
            }
        }

    }

    public Item Generate(string itemName)
    {

        Item item = new Item();
        item.ItemName = (ItemName)Enum.Parse(typeof(ItemName), itemName, true);
        string[] data = itemData.Find(s => s[0] == itemName);

        item.Name = data[0];
        item.ID = data[1];

        item.Description = data[2];
        item.PurchasePrice = int.Parse(data[3]);
        item.SellingPrice = int.Parse(data[4]);
        item.Value = int.Parse(data[5]);

        item.isConsumable = Convert.ToBoolean(data[6]);

        //アイテムタイプ(素材、本、薬…)
        if (!string.IsNullOrEmpty(data[7]))
            item.ItemType = (ItemType)Enum.Parse(typeof(ItemType), data[7], true);

        item.Message1 = data[8];

        item.Target = (Target)Enum.Parse(typeof(Target), data[9], true);

        string[] damageTypes = data[10].Split('/');
        for (int i = 0; i < damageTypes.Length; i++)
        {
            item.DamageTypes.Add((DamageType)Enum.Parse(typeof(DamageType), damageTypes[i], true));
        }

        item.Attribution = (Attribution)Enum.Parse(typeof(Attribution), data[11], true);

        item.Ailment = (Ailment)Enum.Parse(typeof(Ailment), data[12], true);

        item.Scope = (Scope)Enum.Parse(typeof(Scope), data[13], true);

        item.IsAvairableOnBattle = Convert.ToBoolean(data[14]);
        item.IsAvairableOnFloor = Convert.ToBoolean(data[15]);

        //出現率
        item.AppearanceRatio = int.Parse(data[16]);

        //特殊効果

        return item;

    }

    public Weapon GenerateWeapon(string name)
    {
        Weapon eq = new Weapon();
        string[] data = equipmentData.Find(s => s[0] == name);

        eq.Name = data[0];
        eq.ID = data[1];

        eq.Description = data[3];
        eq.PurchasePrice = int.Parse(data[4]);
        eq.SellingPrice = int.Parse(data[5]);
        eq.Rarity = int.Parse(data[6]);
        eq.NormalValue = int.Parse(data[7]);
        eq.HitAboidProb = int.Parse(data[8]);

        //ユニーク
        //data[9]
        eq.IsUnique = Convert.ToBoolean(data[9]);

        //eq.GemSlotsNum = int.Parse(data[10]);

        //修正
        //data[11]

        eq.UppedStatuses[Status.STR] = int.Parse(data[12]);
        eq.UppedStatuses[Status.DEF] = int.Parse(data[13]);
        eq.UppedStatuses[Status.INT] = int.Parse(data[14]);
        eq.UppedStatuses[Status.MNT] = int.Parse(data[15]);
        eq.UppedStatuses[Status.TEC] = int.Parse(data[16]);
        eq.UppedStatuses[Status.AGI] = int.Parse(data[17]);
        eq.UppedStatuses[Status.LUK] = int.Parse(data[18]);

        eq.RequiredStatus[Status.STR] = int.Parse(data[19]);
        eq.RequiredStatus[Status.DEF] = int.Parse(data[20]);
        eq.RequiredStatus[Status.INT] = int.Parse(data[21]);
        eq.RequiredStatus[Status.MNT] = int.Parse(data[22]);
        eq.RequiredStatus[Status.TEC] = int.Parse(data[23]);
        eq.RequiredStatus[Status.AGI] = int.Parse(data[24]);
        eq.RequiredStatus[Status.LUK] = int.Parse(data[25]);

        eq.Attribution = (Attribution)Enum.Parse(typeof(Attribution), data[26], true);

        if (data[27] != "nothing")
        {
            EnemyType enemyType = (EnemyType)Enum.Parse(typeof(EnemyType), data[27], true);
            eq.Effectivenesses[enemyType] += 50;
        }

        if (data[28] != "nothing")
        {
            Ailment ailment = (Ailment)Enum.Parse(typeof(Ailment), data[28], true);
            eq.Ailments[ailment] += 50;
        }

        return eq;
    }

    public Guard GenerateGuard(string name)
    {
        Guard eq = new Guard();
        string[] data = equipmentData.Find(s => s[0] == name);

        eq.Name = data[0];
        eq.ID = data[1];

        //防具タイプ（防具、アクセ）
        eq.GuardType = (GuardType)Enum.Parse(typeof(GuardType), data[2], true);

        eq.Description = data[3];
        eq.PurchasePrice = int.Parse(data[4]);
        eq.SellingPrice = int.Parse(data[5]);
        eq.Rarity = int.Parse(data[6]);
        eq.NormalValue = int.Parse(data[7]);
        eq.HitAboidProb = int.Parse(data[8]);

        //ユニーク
        eq.IsUnique = Convert.ToBoolean(data[9]);

        eq.GemSlotsNum = int.Parse(data[10]);

        //修正
        //data[11]

        eq.UppedStatuses[Status.STR] = int.Parse(data[12]);
        eq.UppedStatuses[Status.DEF] = int.Parse(data[13]);
        eq.UppedStatuses[Status.INT] = int.Parse(data[14]);
        eq.UppedStatuses[Status.MNT] = int.Parse(data[15]);
        eq.UppedStatuses[Status.TEC] = int.Parse(data[16]);
        eq.UppedStatuses[Status.AGI] = int.Parse(data[17]);
        eq.UppedStatuses[Status.LUK] = int.Parse(data[18]);

        eq.RequiredStatus[Status.STR] = int.Parse(data[19]);
        eq.RequiredStatus[Status.DEF] = int.Parse(data[20]);
        eq.RequiredStatus[Status.INT] = int.Parse(data[21]);
        eq.RequiredStatus[Status.MNT] = int.Parse(data[22]);
        eq.RequiredStatus[Status.TEC] = int.Parse(data[23]);
        eq.RequiredStatus[Status.AGI] = int.Parse(data[24]);
        eq.RequiredStatus[Status.LUK] = int.Parse(data[25]);

        eq.Attribution = (Attribution)Enum.Parse(typeof(Attribution), data[26], true);

        eq.ResistibleAilments.Add((Ailment)Enum.Parse(typeof(Ailment), data[28], true));

        return eq;
    }

    public Item GetItemRandomly()
    {
        Item item = ItemInfo.GetAtRandom<Item>();
        Item newitem = Generate("薬草");
        Item ite2 = Generate(Utility.GetStringOfEnum(item.ItemName));
        return newitem;
    }

    public Item GetItemRandomlyByRatio()
    {
        int sum = 0;
        foreach (Item i in itemInfo)
        {
            sum += i.AppearanceRatio;
        }

        var randomNum = UnityEngine.Random.Range(0, sum);

        int temp = 0;
        Item item = new Item();
        foreach (Item i in itemInfo)
        {
            temp += i.AppearanceRatio;
            if (temp >= randomNum)
            {
                item = Generate(Utility.GetStringOfEnum(i.ItemName));
                break;
            }
        }

        if (item.IsNull())
        {
            Debug.Log("ItemObject is Null");
            throw new Exception("Item not found");
            //return null;
        }
        else
        {
            return item;
        }

    }


}
