using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Skill : Ability
{

    public string Name { get { return ConvertSkillNameToString(SkillName); } }

    //スキル名
    public SkillName SkillName { get; set; }

    //表示されるスキル名
    public string DisplayedSkillName
    {
        get { return ConvertSkillNameToString(SkillName) + "Lv" + SkillLevel.ToString(); }
        set { }
    }

    public bool IsAilmentSkill = false;

    //クラスタイプ
    public Job JobType { get ; set ; } = Job.warrior;
    //物理か魔法科
    public AttackType AttackType { get ; set ; } = AttackType.physics;
    //属性
    public Attribution Attribution { get ; set ; } = Attribution.nothing;
    //攻撃の追加効果
    public Ailment AdditionalAilment { get ; set; } = Ailment.nothing;
    //発動タイプ
    public ActiveType ActiveType { get ; set; } = ActiveType.active;
    //スキルタイプ
    public SkillType SkillType { get; set; }

    //スキルレベル(1~3)
    public int SkillLevel { get; set; } = 1;

    //消費SP量
    public int[] SpConsumptions { get; set; } = new int[3];

    //スキルレベル毎の効果値
    public int[] ValueByLv { get; set; } = new int[3] { 100, 100, 100 };
    public int GetSkillPower()
    {
        return ValueByLv[SkillLevel];
    }

    //スキル詳細のマスターテキスト
    public string DescriptionMaster { get; set; } = "デフォルト値";

    //スキルレベル毎の差分テキスト
    public string[] DescriptionByLv { get; set; } = new string[3];


    public string Description
    {
        get
        {
            return DescriptionMaster.Replace("$VALUE$", DescriptionByLv[SkillLevel]);
        }
        set
        { }
    }

    //通常攻撃が初期値になるようにする
    public Skill()
    {

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


}


