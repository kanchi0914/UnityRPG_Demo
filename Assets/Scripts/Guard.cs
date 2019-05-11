using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EnumHolder;

public class Guard : Equipment
{
    private Ailment ailment;

    private List<Ailment> ailments = new List<Ailment>();
    private List<Attribution> attributions = new List<Attribution>();
    private Dictionary<GuardAbility, int> guardAbilities = new Dictionary<GuardAbility, int>();
    private GuardType guardType = GuardType.armor;

    public List<Attribution> ResistibleAttributions { get => attributions; set => attributions = value; }
    public Ailment ResistibleAilment { get => ailment; set => ailment = value; }
    public List<Ailment> ResistibleAilments { get => ailments; set => ailments = value; }

    private List<(Attribution attribution, int value)> resistibleAttributionTapples;

    public Dictionary<GuardAbility, int> GuardAbilities { get => guardAbilities; set => guardAbilities = value; }
    public GuardType GuardType { get => guardType; set => guardType = value; }
    public List<(Attribution attribution, int value)> ResistibleAttributionTapples
    { get => resistibleAttributionTapples; private set => resistibleAttributionTapples = value; }

    public Guard(string name = "装備無し", string id = "nothing"
        , int value = 0, int avoid = 0, GuardType guardType = GuardType.armor)
    {
        this.ItemType = ItemType.guard;
        this.Name = name;
        this.ID = id;
        this.HitAboidProb = avoid;
        this.NormalValue = value;
        this.guardType = GuardType;
    }

    public void SetAllStatus(string name = "装備無し", string id = "nothing"
        , int value = 0, int avoid = 0)
    {
        this.Name = name;
        this.ID = id;
        this.HitAboidProb = avoid;
        this.NormalValue = value;
    }

    public bool IsArmor()
    {
        if (this.GuardType == GuardType.armor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
