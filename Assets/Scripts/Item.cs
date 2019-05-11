using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Item : Ability
{
    public ItemEffecor itemEffecor;

    public bool isConsumable = true;

    private ItemName itemName;

    private int purchasePrice = 100;
    private int sellingPrice = 50;

    private int rarity = 1;

    private int value = 50;

    private int appearanceRatio = 0;

    private ItemType itemType;

    //private HashSet<Ailment> ailments = new HashSet<Ailment>();
    private Ailment ailment = Ailment.nothing;

    private Attribution attribution = Attribution.nothing;


    public int PurchasePrice { get => purchasePrice; set => purchasePrice = value; }
    public int SellingPrice { get => sellingPrice; set => sellingPrice = value; }
    public int Value { get => value; set => this.value = value; }
    public ItemType ItemType { get => itemType; set => itemType = value; }
    public ItemName ItemName { get => itemName; set => itemName = value; }
    public Ailment Ailment { get => ailment; set => ailment = value; }
    public Attribution Attribution { get => attribution; set => attribution = value; }
    public int Rarity { get => rarity; set => rarity = value; }
    public int AppearanceRatio { get => appearanceRatio; set => appearanceRatio = value; }

    //public HashSet<Ailment> Ailments { get => ailments; set => ailments = value; }

    private List<DamageType> d = new List<DamageType>();

    //public string message = "のHPが回復した";

    public Item()
    {
        //DamageTypes.Add(DamageType.heal);
        SetType(AbilityType.item);
        SetValue(message: "を使った！");
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
        if (this.Name == "" || itemName == ItemName.無し)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void SetValue(string ID = "item", string Name = "", string description = "", string message = "を使った！"
        , Target target = Target.opponent, DamageType damageType = DamageType.damage
        , Scope scope = Scope.single, bool isAvailableOnBattle = true, bool isAvailableOnFloor = false
        , bool isConsumable = true)
    {
        this.ID = ID;
        this.Name = Name;
        this.Description = description;
        this.Message1 = message;
        this.Target = target;
        this.DamageTypes.Add(damageType);
        this.Scope = scope;
        this.isConsumable = isConsumable;
        this.IsAvairableOnFloor = isAvailableOnBattle;
        this.IsAvairableOnFloor = isAvailableOnFloor;
    }

    public string Use(List<Unit> unitList, Unit fromUnit, Unit toUnit, EnemyManager enemyManager, AllyManager allyManager)
    {

        return itemEffecor.Use(this, unitList, fromUnit, toUnit);

    }

    //public string UseInField(Ally fromUnit, AllyManager allyManager)
    //{
    //    List<Ally> allies = allyManager.GetAllyList();
    //    Unit toUnit = allies.Find(a => a.GetID() == allyManager.selectedAllyID);
    //    string message = fromUnit.GetName() + $"は{this.Name}" + this.Message1 + "\n";
    //    int damage = 0;

    //    if (DamageType == DamageType.heal)
    //    {
    //        if (Scope == Scope.single)
    //        {
    //            damage = -(CalculateHealing());
    //            message += toUnit.GetName() + $"のHPが{-damage}回復した！" + "\n";
    //            allyManager.DamageEffect(toUnit, damage);
    //        }
    //        else if (Scope == Scope.entire)
    //        {
    //            List<int> damages = new List<int>();
    //            List<string> texts = new List<string>();
    //            //texts.Add(fromUnit.GetName() + this.message + "\n");
    //            foreach (Ally a in allies)
    //            {
    //                damage = -(CalculateHealing());
    //                damages.Add(damage);
    //                texts.Add(a.GetName() + $"のHPが{-damage}回復した！" + "\n");
    //            }
    //            message += String.Join("", texts);
    //        }
    //    }
    //    else if (DamageType == DamageType.other)
    //    {

    //    }
    //    else
    //    {
    //        message += "しかし　何も起こらなかった。";
    //    }

    //    int beforeHP = toUnit.GetStatus(Status.currentHP);
    //    toUnit.SetDamage(-damage);

    //    if (beforeHP > 0 && toUnit.IsDeath)
    //    {
    //        toUnit.IsDeath = true;
    //        message += toUnit.GetName() + "は死亡した！" + "\n";
    //    }

    //    //消費
    //    List<Item> items = fromUnit.itemList;
    //    Item item = items.Find(s => s.ID == this.ID);
    //    if (isConsumable) items.Remove(item);

    //    return message;
    //}

   

    private int CalculateHealing()
    {
        float random = UnityEngine.Random.Range(0.90f, 1.10f);
        return (int)(GetValue() * random);
    }

    public int GetValue()
    {
        return Value;
    }

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

}
