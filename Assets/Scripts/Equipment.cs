using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Equipment : Item
{

    private EquipType equipType;

    private bool isUnique = false;

    private string equippedAllyID = "";

    //修正値
    private int additionalPoint = 0;

    private int hitAboidProb = 0;

    //スロット数
    private int gemSlotsNum = 3;

    private List<Gem> gems = new List<Gem>();

    //基礎能力値
    private int normalValue = 0;
    //修正値1ごとに、能力がどれだけ上がるか
    private double additionalScale = 1;

    private Dictionary<Status, int> uppedStatuses
    = new Dictionary<Status, int>()
    {
            {Status.STR, 0},
            {Status.DEF, 0},
            {Status.INT, 0},
            {Status.MNT, 0},
            {Status.TEC, 0},
            {Status.AGI, 0},
            {Status.LUK, 0}
    };

    private Dictionary<Status, int> uniqueUppedStatus
        = new Dictionary<Status, int>()
    {
            {Status.STR, 0},
            {Status.DEF, 0},
            {Status.INT, 0},
            {Status.MNT, 0},
            {Status.TEC, 0},
            {Status.AGI, 0},
            {Status.LUK, 0}
    };

    private Dictionary<Status, int> requiredStatus
        = new Dictionary<Status, int>()
        {
            {Status.STR, 0},
            {Status.DEF, 0},
            {Status.INT, 0},
            {Status.MNT, 0},
            {Status.TEC, 0},
            {Status.AGI, 0},
            {Status.LUK, 0}
        };

    //装備状況
    public bool isEquiped = false;

    public int AdditionalPoint { get => additionalPoint; set => additionalPoint = value; }
    public Dictionary<Status, int> UppedStatuses { get => uppedStatuses; set => uppedStatuses = value; }
    public Dictionary<Status, int> RequiredStatus { get => requiredStatus; set => requiredStatus = value; }
    public bool IsUnique { get => isUnique; set => isUnique = value; }
    public Dictionary<Status, int> UniqueUppedStatus { get => uniqueUppedStatus; set => uniqueUppedStatus = value; }
    public int GemSlotsNum { get => gemSlotsNum; set => gemSlotsNum = value; }
    public List<Gem> Gems { get => gems; set => gems = value; }
    public int NormalValue { get => normalValue; set => normalValue = value; }
    public double AdditionalScale { get => additionalScale; set => additionalScale = value; }
    public int HitAboidProb { get => hitAboidProb; set => hitAboidProb = value; }
    public string EquippedAllyID { get => equippedAllyID; set => equippedAllyID = value; }
    public EquipType EquipType { get => equipType; set => equipType = value; }

    public Equipment()
    {
        isConsumable = false;
        Target = Target.ally;
    }

    public void SetStatus(string name = "", string id = "e")
    {
        this.Name = name;
    }
    //修正値を加えたあとの能力値
    public int GetAddedValue()
    {
        return (int)(NormalValue + additionalPoint * AdditionalScale);
    }

    //＋○○の形で名前を取得
    public string GetPlusedName()
    {
        return ($"{Name}+{additionalPoint}");
    }


}
