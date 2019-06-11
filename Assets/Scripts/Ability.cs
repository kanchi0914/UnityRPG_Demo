using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;
using System;

public class Ability
{

    public string Name = "";

    public string Description = "";

    public string Message1 { get; set; }
    public string Message2 { get; set; }

    //効果対象(味方、敵)
    public Target Target { get; set; } = Target.opponent;

    //範囲（単体、全体）
    public Scope Scope { get; set; } = Scope.single;

    //使用可能フラグ（SP不足、封印でfalseに）
    public bool IsUsable { get => isUsable; set => isUsable = value; }

    //戦闘中に使用可能か
    public bool IsAvairableOnBattle { get ; set ; }
    public bool IsAvairableOnFloor { get ; set ; }

    //ダメージのタイプ(ダメージ、HP回復、SP回復、補助…)
    //空のリストにしておく
    public List<DamageType> DamageTypes { get => damageTypes; set => damageTypes = value; }
    private List<DamageType> damageTypes = new List<DamageType>();


    public Ability()
    {

    


    }
    private bool isUsable = true;

    //ここに効果とか全部かく

    public void SetValue(string ID = "", string Name = "", string description = "", string message = "",
        Target target = Target.opponent, DamageType damageType = DamageType.damage
        ,Scope scope = Scope.single)
    {

    }

}
