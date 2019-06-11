using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static EnumHolder;

public class Weapon : Equipment {
    
    EnemyType effectivenessKeys;
    Ailment additonKeys;

    public Attribution Attribution { get; set; }

    public Dictionary<EnemyType, int> Effectivenesses { get; set; } = new Dictionary<EnemyType, int>();
    //public Dictionary<EnemyType, int> AdditonalEffectivenesses { get; set; }
    public Dictionary<Status, int> Statuses { get; set; } = new Dictionary<Status, int>();
    //public Dictionary<Status, int> AdditonalStatuses { get; set; }
    public Dictionary<Ailment, int> Ailments { get; set; } = new Dictionary<Ailment, int>();
    //public Dictionary<Ailment, int> AdditionalAilments { get; set; }
    public Dictionary<Attribution, int> Attributions { get; set; } = new Dictionary<Attribution, int>();
    public Dictionary<WeaponAbility, int> WeaponAbilities { get; set; } = new Dictionary<WeaponAbility, int>();

    public Weapon(string name = "装備無し", string id = "nothing"
        , int value = 0, int hit = 0)
    {

        this.EquipType = EquipType.weapon;
        Target = Target.ally;

        this.Name = name;
        this.ID = id;
        this.NormalValue = value;
        this.HitAboidProb = hit;

        foreach (EnemyType e in Enum.GetValues(typeof(EnumHolder.EnemyType)))
        {
            Effectivenesses.Add(e, 0);
        }

        foreach (Status s in Enum.GetValues(typeof(EnumHolder.Status)))
        {
            Statuses.Add(s, 0);
        }

        foreach (Ailment a in Enum.GetValues(typeof(EnumHolder.Ailment)))
        {
            Ailments.Add(a, 0);
        }

        foreach (Attribution a in Enum.GetValues(typeof(EnumHolder.Attribution)))
        {
            Attributions.Add(a, 0);
        }

        foreach (WeaponAbility w in Enum.GetValues(typeof(EnumHolder.WeaponAbility)))
        {
            WeaponAbilities.Add(w, 0);
        }


    }

    public void SetAllStatus(string name = "装備無し", string id = "nothing"
        , int value = 0, int hit = 0)
    {
        this.Name = name;
        this.HitAboidProb = hit;
        this.NormalValue = value;
    }



}
