using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public static class SkillGenerator
{
    //public string Name = "";
    ////idは可変
    //public string ID = "";
    //public string description = "";
    //public string message = "";
    //private int spConsumption = 0;
    //private Job jobType = Job.warrior;

    //public DamageType damageType = DamageType.damage;

    ////戦闘中に使用可能か
    //public bool isAvairableOnBattle = true;
    ////消費タイプか、恒常タイプか
    //public bool isActive = true;
    //探索時に使用可能か
    //private bool isAvairableOnFloor = false;

    //public Target target = Target.enemy;
    //public Scope scope = Scope.single;

    //攻撃スキルに必要な要素
    //private AttackType classification = AttackType.physics;
    //private Attribution attribution = Attribution.normal;
    //private Ailment additional;
    //private int skillPower = 100;

    //最低限必要なのは、名前,ID,AbilityNameとDamageType

    //    public static Skill GenerateEnemySkill(EnemySkillName abilityName)
    //    {
    //        Skill skill = new Skill();
    //        skill.Type = AbilityType.skill;
    //        skill.EnemyAbility = abilityName;

    //        if (abilityName == EnemySkillName.攻撃)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.Description = "";
    //            skill.Message1 = "の攻撃！";
    //            skill.SpConsumption = 0;
    //            skill.SkillPower = 100;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.DamageTypes.Add(DamageType.damage);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.single;
    //        }
    //        else if (abilityName == EnemySkillName.殴りつける)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.Description = "";
    //            skill.Message1 = "は力の限り殴りつけた！";
    //            skill.SpConsumption = 0;
    //            skill.SkillPower = 150;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.stun;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.damage);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.single;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }

    //        return skill;

    //    }

    //    public static Skill GenerateSkill(SkillName abilityName)
    //    {
    //        Skill skill = new Skill();
    //        skill.Type = AbilityType.skill;
    //        skill.AbilityName = abilityName;

    //        if (abilityName == SkillName.疾風の舞)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "shippu_no_mai";
    //            skill.Description = "5ターンの間、必ずターンの最初に動けるようになる";
    //            skill.Message1 = "は疾風の如く舞った！";
    //            skill.SpConsumption = 8;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.Target = Target.self;
    //            skill.Scope = Scope.single;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.怒涛の舞)
    //        {
    //            //skill.Name = "怒涛の舞";
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "dotou_no_mai";
    //            skill.Description = "5ターンの間、物理攻撃力が上昇する";
    //            skill.Message1 = "は怒涛の如く舞った！";
    //            skill.SpConsumption = 8;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.Target = Target.self;
    //            skill.Scope = Scope.single;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.ソニックバット)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "sonic_bat";
    //            skill.Description = "必ず先制できる一撃を放つ";
    //            skill.Message1 = "は<color=orange>ソニックバット</color>を放った！";
    //            skill.SpConsumption = 2;
    //            skill.SkillPower = 100;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.single;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.nothing;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.なぎ払い)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "nagiharai";
    //            skill.Description = "敵全体にダメージを与える";
    //            skill.Message1 = "は大きくなぎ払った！";
    //            skill.SpConsumption = 3;
    //            skill.SkillPower = 70;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.entire;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.nothing;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.パワースマッシュ)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "power_smash";
    //            skill.Description = "敵単体にダメージを与える　追加効果：スタン";
    //            skill.Message1 = "は<color=orange>パワースマッシュ</color>を放った！";
    //            skill.SpConsumption = 4;
    //            skill.SkillPower = 180;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.single;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.stun;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.メテオストライク)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "meteo_strike";
    //            skill.Description = "敵単体に大ダメージを与える　追加効果：スタン";
    //            skill.Message1 = "は<color=orange>メテオストライク</color>を放った！";
    //            skill.SpConsumption = 10;
    //            skill.SkillPower = 350;
    //            skill.JobType = Job.warrior;
    //            skill.DamageTypes.Add(DamageType.damage);
    //            skill.Target = Target.opponent;
    //            skill.Scope = Scope.single;
    //            skill.AttackType = AttackType.physics;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.stun;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.捨て身)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "meteo_strike";
    //            skill.Description = "防御力が半減するが、物理攻撃力が上がる";
    //            skill.Message1 = "";
    //            skill.SpConsumption = 0;
    //            skill.SkillPower = 0;
    //            skill.JobType = Job.warrior;
    //            skill.SkillType = SkillType.passive;
    //            skill.DamageTypes.Add(DamageType.buff);
    //            skill.IsAvairableOnBattle = false;
    //            skill.IsAvairableOnFloor = false;
    //        }
    //        else if (abilityName == SkillName.ヒール)
    //        {
    //            skill.Name = Enum.GetName(typeof(SkillName), abilityName);
    //            skill.ID = "hear";
    //            skill.Description = "味方一人のHPを小回復する";
    //            skill.Message1 = "は<color=orange>ヒールⅠ</color>の魔法を唱えた!";
    //            skill.SpConsumption = 4;
    //            skill.SkillPower = 200;
    //            skill.JobType = Job.cleric;
    //            skill.DamageTypes.Add(DamageType.hpHeal);
    //            skill.Target = Target.ally;
    //            skill.Scope = Scope.single;
    //            skill.AttackType = AttackType.magic;
    //            skill.Attribution = Attribution.nothing;
    //            skill.AdditionalAilment = Ailment.nothing;
    //            skill.IsAvairableOnBattle = true;
    //            skill.IsAvairableOnFloor = true;
    //        }


    //        return skill;
    //    }

    //}
}
