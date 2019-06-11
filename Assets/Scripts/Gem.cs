using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Gem : Item
{
    //private Ailment plusAilment = Ailment.nothing;
    //private Ailment minusAilment = Ailment.nothing;
    private int plusValue = 0;
    private (Status status, int value) plusStatus = (Status.STR, 0);
    private (Ailment ailment, bool isNormal) ailment = (EnumHolder.Ailment.nothing, true);
    private (EnemyType effectiveness, bool isNormal) effectiveness
        = (EnemyType.normal, true);
    private (Status, int value) minusStatus = (Status.STR, 0);
    private (Attribution attribution, bool isPlus) attribution = (EnumHolder.Attribution.nothing, true);
    private bool isCursed = false;
    private EquipType equipType = EquipType.weapon;

    //HP吸収、SP吸収

    public Gem()
    {
        IsConsumable = false;
        IsUsable = false;
        IsAvairableOnBattle = false;
        IsAvairableOnFloor = false;
    }

    public void SetPlusStatuse((Status status, int value) plusStatus)
    {
        this.plusStatus = plusStatus;
    }

    public void SetPlusAilment((Ailment ailment, bool isNormal) plusAilment)
    {
        this.ailment = plusAilment;
    }

    public void SetEffective((EnemyType effectiveness, bool isNormal) effectiveness)
    {
        this.effectiveness = effectiveness;
    }

    //public Ailment PlusAilment { get => plusAilment; set => plusAilment = value; }
    //public Ailment MinusAilment { get => MinusAilment1; set => MinusAilment1 = value; }
   // public (Status, int value) PlusStatus { get => plusStatus; set => plusStatus = value; }
    public (Status, int value) MinusStatus { get => minusStatus; set => minusStatus = value; }
    public bool IsCursed { get => isCursed; set => isCursed = value; }
    public EquipType EquipType { get => equipType; set => equipType = value; }
    public (Status status, int value) PlusStatus { get => PlusStatus1; set => PlusStatus1 = value; }
   // public Ailment MinusAilment1 { get => minusAilment; set => minusAilment = value; }
    public int PlusValue { get => plusValue; set => plusValue = value; }
    public (Status status, int value) PlusStatus1 { get => plusStatus; set => plusStatus = value; }
    public (Ailment ailment, bool isNormal) Ailment { get => ailment; set => ailment = value; }
    public (EnemyType effectiveness, bool isNormal) Effectiveness { get => effectiveness; set => effectiveness = value; }
    public (Attribution attribution, bool isPlus) Attribution { get => attribution; set => attribution = value; }
    //public (Attribution attribution, bool isNormal) Attribution1 { get => attribution; set => attribution = value; }
}
