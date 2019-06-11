using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Item : Ability
{
    public string Name
    {
        get
        {
            return ConvertItemNameToString(ItemName);
        }
        set { }
    }

    //idは可変
    public string ID = "";

    //アイテム名
    public ItemType ItemType { get; set; }
    public ItemName ItemName { get; set; } = ItemName.無し;

    //アイテムの種類(装備、薬、本…)
    public ItemEffecor ItemEffecor;
    //装備品かどうか
    public bool IsEquipment()
    {
        if (this is Equipment)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //消費アイテムか
    public bool IsConsumable { get; set; } = true;

    //買値
    public int PurchasePrice { get; set; } = 100;
    //売値
    public int SellingPrice { get; set; } = 50;
    //効果値　スキルによって用途を変える
    public int Value { get; set; } = 100;

    //レアリティ
    public int Rarity { get; set; }
    //出現率
    public int AppearanceRatio { get; set; }


    public Item()
    {

    }


    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        Item item = (Item)obj;
        return (this.ID == item.ID);
    }

    public bool IsNull()
    {
        if (this.Name == "" || ItemName == ItemName.無し)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
