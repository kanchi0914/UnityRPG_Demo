using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Skill : Ability
{
    
    //どのクラスのスキルか
    //private Job jobType = Job.warrior;
    //SP消費量
    //private int spConsumption = 0;

    //private int skillLevel = 1;

    //private SkillName skillName;

    //物理か魔法か
    //private AttackType classification = AttackType.physics;

    //private Attribution attribution = Attribution.nothing;

    //発動タイプ
    //private SkillType skillType = SkillType.active; 

    //状態異常の追加効果
    //private Ailment additionalAilment = Ailment.nothing;

    //private AbilityName abilityName;

    //private int skillPower = 100;

    public Job JobType { get ; set ; } = Job.warrior;
    //public int SpConsumption { get; set; } = 0;
    public AttackType AttackType { get ; set ; } = AttackType.physics;
    public Attribution Attribution { get ; set ; } = Attribution.nothing;
    //public AbilityName AbilityName { get => abilityName; set => abilityName = value; }
    //public int SkillPower { get => skillPower; set => skillPower = value; }
    public Ailment AdditionalAilment { get ; set; } = Ailment.nothing;
    public SkillType SkillType { get ; set; } = SkillType.active;
    public string DisplayedSkillName
    {
        get { return ConvertSkillNameToString(SkillName) + "Lv" + SkillLevel.ToString(); }
        set { }
    }
    public SkillName SkillName { get ; set; }

    public int SkillLevel { get; set; } = 1;

    public int[] SpConsumptions { get; set; } = new int[3];

    public string DescriptionMaster { get; set; } = "";
    public string Description
    {
        get
        {
            return DescriptionMaster.Replace("$VALUE$", DescriptionByLv[SkillLevel]);
        }
        set
        { }
    }
    public string[] DescriptionByLv { get; set; } = new string[3];
    public int[] ValueByLv { get; set; } = new int[3];

    private GameController gameController;

    //通常攻撃が初期値になるようにする
    public Skill()
    {
        //this.gameController = gameController;
        SetType(AbilityType.skill);
        SetValue(message: "の攻撃！");
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        Skill skill = (Skill)obj;
        return (SkillName == skill.SkillName);
    }

    public int GetSkillPower()
    {
        return ValueByLv[SkillLevel];
        //return SkillPower;
    }


    //public string Use(List<Unit>units, Unit fromUnit, Unit toUnit, EnemyManager enemyManager, AllyManager allyManager)
    //{
    //    return gameController.SkillEffector.Use(this, units, fromUnit, toUnit);
    //}

    //public int CalculateDamage(Unit fromUnit, Unit toUnit)
    //{

    //    int offensiveP, defensiveP;
    //    int lv = fromUnit.GetStatus(Status.Lv);
    //    int def = toUnit.GetStatus(Status.DEF);
    //    int mnt = toUnit.GetStatus(Status.MNT);

    //    int damage;
    //    float random = UnityEngine.Random.Range(0.90f, 1.10f);


    //    //ダメージ計算式
    //    //引き算による通常ダメージと、
    //    //  (0以下切り捨て)((攻撃力)- (防御力)) + (攻撃力)/(防御力)　* (Lv * 0.2 + 0.2)

    //    if (this.AttackType == AttackType.physics)
    //    {
    //        offensiveP = fromUnit.GetOffensivePower();
    //        defensiveP = toUnit.GetPhysicalDefensivePower();

    //        damage = (int)((this.GetSkillPower() * random / 100)
    //            * ((ChangeMinusValueInNeed(offensiveP - defensiveP)
    //            + (offensiveP / defensiveP) * (lv * 0.02 + 0.2))));

    //        //damage = (int)((this.GetSkillPower() / 100) * (offensiveP - defensiveP));
    //    }
    //    else if (this.AttackType == AttackType.magic)
    //    {
    //        offensiveP = (int)(fromUnit.GetMagicalOffensivePower());
    //        defensiveP = toUnit.GetMagicalDefensivePower();

    //        damage = (int)((this.GetSkillPower() * random / 100)
    //            * ((ChangeMinusValueInNeed(offensiveP - defensiveP)
    //            + (offensiveP / defensiveP) * (lv * 0.02 + 0.2))));
    //        //damage = (int)((this.GetSkillPower() / 100) * (random * offensiveP - defensiveP));
    //    }
    //    else damage = 0;

    //    if (damage < 0) damage = 0;

    //    //状態異常によるダメージの判定
    //    if (toUnit.Ailments.Contains(Ailment.sleep))
    //    {
    //        damage = (int)(damage * 1.5);
    //    }

    //    return damage;
    //}

    //スキルによる回復値
    //public int CalculateHealing(Unit fromUnit)
    //{
    //    int mnt = fromUnit.GetStatus(Status.MNT);
    //    float random = UnityEngine.Random.Range(0.90f, 1.10f);
    //    //精神値による回復量変化に加えて、10％の基本回復量をプラス
    //    return (int)((mnt * GetSkillPower() / 100) * random + GetSkillPower() / 10);
    //}



}


