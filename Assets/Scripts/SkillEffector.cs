using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;
using static GameSettings;
using System;

public class SkillEffector
{

    GameController gameController;

    public SkillEffector(GameController gameController)
    {
        this.gameController = gameController;
    }

    public string Use(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    {
        string message = "";
        message += fromUnit.Name + skill.Message1 + "\n";

        //SP消費
        fromUnit.ChangeSP(-skill.SpConsumptions[skill.SkillLevel]);
        if (fromUnit.UnitType1 == Unit.UnitType.ally)
        {
            Ally ally = (Ally)fromUnit;
            gameController.AllyManager.SpDamageEffect(fromUnit);
        }

        List<Unit> allyUnits = Utility.GetAllyUnitListFromUnits(units, fromUnit);
        List<Unit> opponentUnits = Utility.GetEnemyUnitListFromUnits(units, fromUnit);

        int value = skill.ValueByLv[skill.SkillLevel];

        //通常コマンド--------------------------------------------------------------------------------
        if (skill.SkillName == SkillName.攻撃)
        {
            SetSingleDamage(skill, units, fromUnit, toUnit);
        }
        else if (skill.SkillName == SkillName.防御)
        {
        }
        else if (skill.SkillName == SkillName.何もしない)
        {
        }
        else if (skill.SkillName == SkillName.混乱)
        {
            SetSingleDamage(skill, units, fromUnit, toUnit);
        }
        //敵のコマンド------------------------------------------------


        //============================================================
        //単体攻撃系
        //============================================================
        else if (skill.SkillName == SkillName.パワースマッシュ ||
            skill.SkillName == SkillName.殴りつける ||
            skill.SkillName == SkillName.毒攻撃 ||
            skill.SkillName == SkillName.氷攻撃 ||
            skill.SkillName == SkillName.即死攻撃 ||
            skill.SkillName == SkillName.噛みつき ||
            skill.SkillName == SkillName.ファイアボール
            )
        {
            message += SetSingleDamage(skill, units, fromUnit, toUnit);
        }

        //攻撃系---------------------------------------------------------------------------------------

        else if (skill.SkillName == SkillName.なぎ払い ||
            skill.SkillName == SkillName.炎のブレス ||
            skill.SkillName == SkillName.炎のブレス ||
            skill.SkillName == SkillName.フロストウェーブ)
        {
            message += SetMultiDamage(skill, units, fromUnit, toUnit, opponentUnits);
        }

        else if (skill.SkillName == SkillName.サンダーブレイク)
        {
            SetRandomDamge(skill, units, fromUnit, toUnit, opponentUnits);
        }

        //回復系---------------------------------------------------------------------------------------
        else if (skill.SkillName == SkillName.ヒール)
        {
            message += SetSingleHealing(skill, units, fromUnit, toUnit);
        }
        else if (skill.SkillName == SkillName.ヒールオール)
        {
            foreach (Unit u in allyUnits)
            {
                int healValue = CalculateHealingValue(skill.ValueByLv[skill.SkillLevel], fromUnit);
                message += SetDamageByAbility(fromUnit, u, skill, -healValue);
            }
        }
        else if (skill.SkillName == SkillName.キュア)
        {
            if (toUnit.Ailments.Count > 0)
            {
                toUnit.Ailments.Clear();
                message += $"{fromUnit}の状態異常が回復した！";
            }
            else
            {
                message += $"しかし効果がなかった。";
            }
        }
        else if (skill.SkillName == SkillName.キュアオール)
        {
            bool isUsed = false;
            foreach (Unit u in allyUnits)
            {
                if (u.Ailments.Count > 0)
                {
                    u.Ailments.Clear();
                    message += $"{fromUnit}の状態異常が回復した！";
                    isUsed = true;
                }
            }
            if (!isUsed) message += "しかし効果がなかった。";
        }
        else if (skill.SkillName == SkillName.リザレクション)
        {
            if (toUnit.IsDeath)
            {
                toUnit.IsDeath = false;
                int damage = toUnit.Statuses[Status.MaxHP] / 2;
                SetDamageByAbility(fromUnit, toUnit, skill, damage, false);
                message += $"{toUnit}は復活した！";
            }
            else
            {
                message += "しかし効果がなかった。";
            }
        }

        //魔法系-------------------------------------------------------


        //状態異常系-------------------------------------------------------------------------------------------
        else if (skill.SkillName == SkillName.痺れる粉)
        {
            foreach (Unit u in opponentUnits)
            {
                message += SetAilmentInProb(fromUnit, u, Ailment.paralysis);
            }
        }
        else if (skill.SkillName == SkillName.毒の粉)
        {
            foreach (Unit u in opponentUnits)
            {
                message += SetAilmentInProb(fromUnit, u, Ailment.poison);
            }
        }
        else if (skill.SkillName == SkillName.眠りの歌)
        {
            foreach (Unit u in opponentUnits)
            {
                message += SetAilmentInProb(fromUnit, u, Ailment.sleep);
            }
        }
        else if (skill.SkillName == SkillName.叫び声)
        {
            foreach (Unit u in opponentUnits)
            {
                message += SetAilmentInProb(fromUnit, u, Ailment.terror);
            }
        }

        //バフ・デバフ系---------------------------------------------------------------------------------------
        else if (skill.SkillName == SkillName.粘液)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "敏捷↓";
            buff.Statuses[Status.AGI] = -0.5;
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"の敏捷が減少した！" + "\n";
        }
        else if (skill.SkillName == SkillName.テンションアップ)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "敏捷↑";
            buff.Statuses[Status.AGI] = 0.5;
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"の敏捷が上昇した！" + "\n";
        }
        else if (skill.SkillName == SkillName.怒涛の舞)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "筋力↑、呪い・恐怖耐性↑";
            buff.Statuses[Status.STR] = 0.5;
            //buff.AilmentResists.Add(Ailment.curse, 1);
            buff.AilmentResists.Add(Ailment.terror, 1);
            fromUnit.Buffs.Add(buff);
            message += toUnit.Name + $"の筋力が上昇した！" + "\n";
        }
        else if (skill.SkillName == SkillName.チャージ)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "筋力+100%";
            buff.LeftTurn = 2;
            buff.Statuses[Status.STR] = 1.0;
        }
        else if (skill.SkillName == SkillName.シェルアーマー)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "耐久↑";
            buff.Statuses[Status.AGI] = 0.5;
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"の耐久が上昇した！" + "\n";
        }
        //特殊系---------------------------------------------------------
        else if (skill.SkillName == SkillName.狙いすまし)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = $"命中+{skill.GetSkillPower()}%";
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"の命中が上昇した！" + "\n";
        }
        else if (skill.SkillName == SkillName.挑発)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "狙われやすさ上昇";
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"は敵から狙われやすくなった！" + "\n";
        }
        else if (skill.SkillName == SkillName.疾風の舞)
        {
            Unit.Buff buff = new Unit.Buff(skill);
            buff.Description = "ターンの最初に行動";
            toUnit.Buffs.Add(buff);
            message += toUnit.Name + $"は早く動けるようになった！" + "\n";
        }

        return message;

    }

    public string UseInField(Skill skill, List<Unit> unitList, Ally ally)
    {
        List<Ally> allies = gameController.AllyManager.Allies;
        Unit toUnit = allies.Find(a => a.ID1 == gameController.AllyManager.SelectedAllyID);

        string message = "";

        if (skill.SkillName == SkillName.テレポート)
        {
            message = skill.Message1;
        }
        else
        {
            message = Use(skill, unitList, ally, toUnit);

        }

        gameController.AllyManager.UpdatePanels();

        return message;
    }


    public string SetSingleDamage(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    {
        int damage = CalculateDamage(skill, units, fromUnit, toUnit);
        string message = SetDamageByAbility(fromUnit, toUnit, skill, damage);
        return message;
    }

    public string SetMultiDamage(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit, List<Unit> opponentUnits)
    {
        string message = "";

        foreach (Unit u in opponentUnits)
        {
            int damage = CalculateDamage(skill, units, fromUnit, u);
            message += SetDamageByAbility(fromUnit, u, skill, damage);
        }

        return message;

    }

    public string SetRandomDamge(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit, List<Unit> opponentUnits)
    {
        string message = "";
        for (int i = 0; i < 3; i++)
        {
            int random = UnityEngine.Random.Range(0, opponentUnits.Count);
            Unit unit = opponentUnits[random];
            int damage = CalculateDamage(skill, units,  unit, toUnit);
            message += SetDamageByAbility(fromUnit, unit, skill, damage);
            if (unit.IsDeath == true)
            {
                opponentUnits.Remove(unit);
            }
        }
        return message;
    }

    public string SetSingleHealing(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    {
        string message = "";
        int healValue = CalculateHealingValue(skill.ValueByLv[skill.SkillLevel], fromUnit);
        message += SetDamageByAbility(fromUnit, toUnit, skill, -healValue);
        return message;
    }

    //public string SetSingleAilment(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    //{
    //    message += SetAilmentInProb(fromUnit, u, Ailment.paralysis);
    //}

    //public string SetMultiAilment(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    //{

    //}

    


    //public bool CheckHit(Unit fromUnit, Unit toUnit)
    //{
    //    int random = UnityEngine.Random.Range(0, 100);
    //    double randomUp = UnityEngine.Random.Range(1.0f, 1.2f);
    //    double hitProb = fromUnit.GetHitProb() * randomUp;
    //    double avoidProb = toUnit.GetAvoidProb();
    //    double per = (hitProb / avoidProb) / 1.5 * 100;
    //    Debug.Log(per);
    //    return (per > random);
    //}

    ////状態異常の付与
    ////statusは成功確率の依存ステータス
    //public string SetAilmentInProb(Unit fromUnit, Unit toUnit, Ailment ailment, Status status)
    //{
    //    string message = "";

    //    if (!toUnit.Ailments.Contains(ailment))
    //    {
    //        int random = UnityEngine.Random.Range(0, 100);
    //        //int per = ((fromUnit.Statuses[status] + fromUnit.Statuses[Status.LUK])
    //        //    / ((toUnit.Statuses[Status.MNT] + toUnit.Statuses[Status.LUK]) * 2) * 100);

    //        int per = (fromUnit.Statuses[Status.Lv] * fromUnit.Statuses[Status.INT]) /
    //            (toUnit.Statuses[Status.Lv] * toUnit.Statuses[Status.INT] * 2) * 100; 

    //        if (per > random)
    //        {
    //            message += $"{toUnit.Name}は{ailment.GetAilmentName()}状態になった！\n";
    //            toUnit.Ailments.Add(ailment);
    //        }
    //        else
    //        {
    //            message += $"{toUnit.Name}は何ともなかった！\n";
    //            toUnit.Ailments.Add(ailment);
    //        }
    //        Debug.Log(per);
    //    }
    //    return message;
    //}

    ////スキルによる回復値
    //public int CalculateHealing(int value, Unit fromUnit)
    //{
    //    int healValue = 0;
    //    int mnt = fromUnit.GetStatus(Status.MNT);
    //    float random = UnityEngine.Random.Range(0.90f, 1.10f);
    //    healValue = (int)((mnt * value / 100) * random + value / 10);
    //    if (healValue > 999) healValue = 999;
    //    //精神値による回復量変化に加えて、10％の基本回復量をプラス
    //    return healValue;
    //}
}
