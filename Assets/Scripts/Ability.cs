using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;
using System;

public class Ability{

    

    public string Name = "";
    //idは可変
    public string ID = "";
    //詳細説明
    private string description = "";
    //使用時のメッセージ
    private string message1 = "";
    private string message2 = "";

    private AbilityType type = AbilityType.item;
    private SkillName abilityName = SkillName.無し;

    //ターゲット(味方、敵)
    private Target target = Target.opponent;

    //private DamageType damageType = DamageType.damage;

    //範囲（単体、全体）
    private Scope scope = Scope.single;

    private EnemySkillName enemyAbility;

    //回復可能な状態異常
    private List<Ailment> recoverbleAilments;

    //ダメージのタイプ(ダメージ、HP回復、SP回復、補助…)
    //空のリストにしておく
    private List<DamageType> damageTypes = new List<DamageType>();



    //使用可能フラグ（SP不足、封印でfalseに）
    private bool isUsable = true;
    //戦闘中に使用可能か
    private bool isAvairableOnBattle = true;
    //探索時に使用可能か
    private bool isAvairableOnFloor = false;
    //消費タイプか、恒常タイプか
    private bool isActive = true;

    public AbilityType Type { get => type; set => type = value; }
    public Target Target { get => target; set => target = value; }
    //public DamageType DamageType { get => damageType; set => damageType = value; }
    public Scope Scope { get => scope; set => scope = value; }
    public bool IsUsable { get => isUsable; set => isUsable = value; }
    public bool IsAvairableOnBattle { get => isAvairableOnBattle; set => isAvairableOnBattle = value; }
    public bool IsAvairableOnFloor { get => isAvairableOnFloor; set => isAvairableOnFloor = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public EnemySkillName EnemyAbility { get => enemyAbility; set => enemyAbility = value; }
    public string Description { get => description; set => description = value; }
    public string Message1 { get => message1; set => message1 = value; }
    public string Message2 { get => message2; set => message2 = value; }
    public List<Ailment> RecoverbleAilments { get => recoverbleAilments; set => recoverbleAilments = value; }
    public List<DamageType> DamageTypes { get => damageTypes; set => damageTypes = value; }
    public SkillName AbilityName { get => abilityName; set => abilityName = value; }


    //ここに効果とか全部かく

    public Ability()
    {
        SetValue();
    }

    public void SetType(AbilityType type)
    {
        this.type = type;
    }


    //public AbilityType GetType()
    //{
    //    return this.type;
    //}



    public void SetValue(string ID = "", string Name = "", string description = "", string message = "",
        Target target = Target.opponent, DamageType damageType = DamageType.damage
        ,Scope scope = Scope.single)
    {
        this.ID = ID;
        this.Name = Name;
        this.Description = description;
        this.Message1 = message;
        this.Target = target;
        this.DamageTypes.Add(damageType);
        this.Scope = scope;
    }

    //public virtual int CalculateDamage(Unit fromUnit, Unit toUnit, Skill skill)
    //{
    //    int attack, defense;
    //    int def = toUnit.GetStatus(StatusKey.DEF);
    //    int mnt = toUnit.GetStatus(StatusKey.MNT);

    //    int damage;
    //    float random = UnityEngine.Random.Range(0.90f, 1.10f);

    //    if (skill.AttackType == AttackType.physics)
    //    {
    //        attack = (int)(fromUnit.GetOffensivePower());
    //        defense = toUnit.GetPhysicalDefensivePower();
    //        damage = (int)((skill.GetSkillPower() / 100) * (random * attack - defense));
    //    }
    //    else if (skill.AttackType == AttackType.magic)
    //    {
    //        attack = (int)(fromUnit.GetMagicalOffensivePower());
    //        defense = toUnit.GetMagicalDefensivePower();
    //        damage = (int)((skill.GetSkillPower() / 100) * (random * attack - defense));
    //    }
    //    else damage = 0;

    //    if (damage < 0) damage = 0;

    //    return damage;
    //}

    //スキルによる回復値
    //public int CalculateHealing(Unit fromUnit, Skill skill)
    //{
    //    int mnt = fromUnit.GetStatus(StatusKey.MNT);
    //    float random = UnityEngine.Random.Range(0.90f, 1.10f);
    //    return (int)((mnt * skill.GetSkillPower() / 100) * random + skill.GetSkillPower() / 10);
    //}


    private int CalculateItemHealing(Unit fromUnit, Item item)
    {
        float random = UnityEngine.Random.Range(0.90f, 1.10f);
        return (int)(item.GetValue() * random);
    }


    public void GetEffect(Unit from, Unit to, List<Unit> unitList)
    {
        
    }
}
