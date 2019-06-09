using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnumHolder;


public static class GameSettings
{
    public static readonly int AilmentInitialTurn = 4;
    public static readonly int MaxHPValue = 999;

    static bool isCovered = false;

    /// <summary>
    /// 命中判定
    /// </summary>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <returns></returns>
    public static bool CheckHit(Unit fromUnit, Unit toUnit)
    {
        int random = UnityEngine.Random.Range(0, 100);
        double randomUp = UnityEngine.Random.Range(1.0f, 1.2f);
        double hitProb = fromUnit.GetHitProb() * randomUp;
        double avoidProb = toUnit.GetAvoidProb();
        double per = (hitProb / avoidProb) / 1.5 * 100;
        return (per > random);
    }

    /// <summary>
    /// 状態異常の確率付与
    /// </summary>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <param name="ailment"></param>
    /// <param name="value">状態異常の確率,％表示</param>
    /// <returns></returns>
    public static string SetAilmentInProb(Unit fromUnit, Unit toUnit, Ailment ailment, int value = 100)
    {
        string message = "";
        if (!toUnit.Ailments.ContainsKey(ailment))
        {
            int random = UnityEngine.Random.Range(0, 100);

            int per = value - ((toUnit.AilmentResists[ailment] / 2) * value);

            if (per > random)
            {
                toUnit.SetAilment(ailment);
            }
            else
            {
                message += $"{toUnit.Name}は何ともなかった！\n";
            }
        }
        return message;
    }

    /// <summary>
    /// スキル効果値による状態異常付与
    /// (自分のLUK * Lv / 相手のLUK * Lv) * Value
    /// </summary>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <param name="ailment"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static string SetAilmentBySkillValue(Unit fromUnit, Unit toUnit, Ailment ailment, Skill skill)
    {
        string message = "";

        int value = skill.ValueByLv[skill.SkillLevel];

        int per = (fromUnit.Statuses[Status.Lv] * fromUnit.Statuses[Status.LUK]) /
                (toUnit.Statuses[Status.Lv] * toUnit.Statuses[Status.LUK]) * (value / 100);
        SetAilmentInProb(fromUnit, toUnit, ailment, per);
        return message;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="skillValue"></param>
    /// <param name="fromUnit"></param>
    /// <returns></returns>
    public static int CalculateSkillHealingValue(int skillValue, Unit fromUnit)
    {
        int healValue = 0;
        int intel = fromUnit.Statuses[Status.INT];
        float random = UnityEngine.Random.Range(0.90f, 1.10f);
        healValue = (int)((intel * skillValue / 10) * random + skillValue);
        if (healValue > MaxHPValue) healValue = MaxHPValue;
        return healValue;
    }

    /// <summary>
    /// スキルの効果値に応じてHPを回復
    /// </summary>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <param name="ailment"></param>
    /// <param name="skill"></param>
    /// <returns></returns>
    public static string SetHealingBySkill(Unit fromUnit, Unit toUnit, Skill skill)
    {
        int healValue = CalculateSkillHealingValue(skill.ValueByLv[skill.SkillLevel], fromUnit);
        return SetDamageByAbility(fromUnit, toUnit, skill, -healValue);
    }


    /// <summary>
    /// ダメージ計算
    /// ここでは数値を返すだけで、変化はさせない
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="units"></param>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <returns></returns>
    public static int CalculateDamage(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    {
        isCovered = false;

        CheckTargetChanged(skill, units, fromUnit, toUnit);

        int offensiveP, defensiveP;
        int defaultOffensiveP;
        int lv = fromUnit.Statuses[Status.Lv];

        int damage;
        float random = UnityEngine.Random.Range(0.90f, 1.10f);

        if (skill.AttackType == AttackType.physics)
        {
            offensiveP = fromUnit.GetOffensivePower();
            defensiveP = toUnit.GetPhysicalDefensivePower();
        }
        else
        {
            offensiveP = (int)(fromUnit.GetMagicalOffensivePower());
            defensiveP = toUnit.GetMagicalDefensivePower();
        }

        defaultOffensiveP = offensiveP;

        //基本ダメージ計算
        //引き算による通常ダメージと、
        //  (0以下切り捨て)((攻撃力)- (防御力)) + (攻撃力)/(防御力)　* (Lv * 0.2 + 0.2)
        if (skill.AttackType == AttackType.physics)
        {
            damage = (int)((skill.GetSkillPower() * random / 100)
                * ((ChangeMinusValueInNeed(offensiveP - defensiveP)
                + (offensiveP / defensiveP) * (lv * 0.02 + 0.2))));

        }
        else if (skill.AttackType == AttackType.magic)
        {
            damage = (int)((skill.GetSkillPower() * random / 100)
                * ((ChangeMinusValueInNeed(offensiveP - defensiveP)
                + (offensiveP / defensiveP) * (lv * 0.02 + 0.2))));
        }
        else damage = 0;
        if (damage < 0) damage = 0;

        //種族特攻
        damage = CalculateEnemyTypeDamage(damage, skill, fromUnit, toUnit);

        //状態異常によるダメージ増減
        damage = CalculateAilmentDamage(damage, toUnit);

        //相手の属性耐性によるダメージ増減
        damage = CalculateAttributionDamage(damage, skill, fromUnit, toUnit);

        //防御時のダメージ増減
        damage = CalculateGuardedDamage(damage, toUnit);

        damage = CalculateGuardedDamage(damage, toUnit);

        if (damage < 1) damage = 1;

        return damage;
    }

    /// <summary>
    /// Unitクラスのメソッドを呼び出し、パラメータを変化させる
    /// </summary>
    /// <param name="fromUnit"></param>
    /// <param name="toUnit"></param>
    /// <param name="skill"></param>
    /// <param name="damage"></param>
    /// <returns></returns>
    public static string SetDamageByAbility(Unit fromUnit, Unit toUnit, Skill skill, int damage)
    {

        string message = "";

        if (!toUnit.IsDeath)
        {
            bool isHit = true;
            if (skill.AttackType == AttackType.physics)
            {
                isHit = CheckHit(fromUnit, toUnit);
            }
            //攻撃が命中するなら
            if (isHit)
            {
                message += toUnit.SetDamage(damage);
                message += CheckAdditionalAilment(skill, fromUnit, toUnit);

            }
            else
            {
                message += $"{toUnit.Name}には当たらなかった！\n";
                toUnit.SetDamage(isHit: false);
            }
        }
        return message;
    }


    public static int CalculateEnemyTypeDamage(int damage, Skill skill, Unit fromUnit, Unit toUnit)
    {
        if (toUnit.GetType() == typeof(Enemy))
        {
            int rate = 100;
            Enemy enemy = toUnit as Enemy;
            Ally ally = fromUnit as Ally;
            foreach (EnemyType e in enemy.EnemyTypes)
            {
                if (ally.EquippedWeapon.Effectivenesses.ContainsKey(e))
                {
                    rate += ally.EquippedWeapon.Effectivenesses[e];
                }
            }

            damage *= rate;
            return damage;
        }
        else
        {
            return damage;
        }
    }


    public static int CalculateAttributionDamage(int damage, Skill skill, Unit fromUnit, Unit toUnit)
    {
        //属性によるダメージ増減
        HashSet<Attribution> attributions = new HashSet<Attribution>();
        //スキルの属性
        attributions.Add(skill.Attribution);

        //バフによる属性
        foreach (Unit.Buff b in fromUnit.Buffs)
        {
            attributions.Add(b.attribution);
        }

        //武器による属性
        if (fromUnit.GetType() == typeof(Ally))
        {
            //Ally ally = (Ally)fromUnit;
            ////武器の属性
            //attributions.Add(ally.EquippedWeapon.Attribution);
            ////ジェムの属性
            //foreach (Gem g in ally.EquippedWeapon.Gems)
            //{
            //    if (g.Attribution.attribution != Attribution.nothing)
            //    {
            //        attributions.Add(g.Attribution.attribution);
            //    }
            //}
        }

        foreach (Attribution a in attributions)
        {
            if (toUnit.AttributionResists[a] == 2)
            {
                damage = 1;
            }
            else if (toUnit.AttributionResists[a] == 1)
            {
                damage -= (int)(damage * 0.5);
            }
            else if (toUnit.AttributionResists[a] == -1)
            {
                damage += (int)(damage * 0.5);
            }
            else if (toUnit.AttributionResists[a] == -2)
            {
                damage -= (int)(damage * 1.0);
            }
        }
        return damage;
    }

    public static int CalculateAilmentDamage(int damage, Unit toUnit)
    {
        if (toUnit.Ailments.ContainsKey(Ailment.sleep))
        {
            damage = (int)(damage * 1.5);
        }
        return damage;
    }

    public static string CheckTargetChanged(Skill skill, List<Unit> units, Unit fromUnit, Unit toUnit)
    {
        string message = "";
        var allyUnits = Utility.GetAllyUnitListFromUnits(units, fromUnit);
        foreach (Unit u in allyUnits)
        {
            var currentSkill = u.CurrentCommand.Ability as Skill;
            if (!u.IsDeath && currentSkill.SkillName == SkillName.かばう && u.CurrentCommand.ToUnit == toUnit)
            {
                message += $"{u.Name}は攻撃をかばった！";
                toUnit = u;
                isCovered = true;
                break;
            }
        }
        return message;
    }

    /// <summary>
    /// 防御時のダメージを返す
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="toUnit"></param>
    /// <returns></returns>
    public static int CalculateGuardedDamage(int damage, Unit toUnit)
    {
        if (toUnit.CurrentCommand.Ability.GetType() == typeof(Skill))
        {
            Skill toUnitSkill = toUnit.CurrentCommand.Ability as Skill;
            if (toUnitSkill.SkillName == SkillName.防御)
            {
                double guardPer = 50;
                if (toUnit.skills.Exists(s => s.SkillName == SkillName.防御の心得))
                {
                    var skill = toUnit.skills.Find(s => s.SkillName == SkillName.防御の心得);
                    guardPer += skill.GetSkillPower();
                }
                return (int)(damage * (guardPer / 100));
            }
            else
            {
                return damage;
            }
        }
        else
        {
            return damage;
        }
    }

    public static int CalculateSkillGuardedDamage(int damage, Unit toUnit)
    {
        int d = damage;
        if (toUnit.CurrentCommand.Ability.GetType() == typeof(Skill))
        {
            var curretSkill = toUnit.CurrentCommand.Ability as Skill;
            if (curretSkill.SkillName == SkillName.かばう && isCovered)
            {
                int guardPer = curretSkill.GetSkillPower();
                d = (int)(damage * ((100 - guardPer) / 100));
            }
        }
        return damage;
    }


    public static string CheckAdditionalAilment(Skill skill, Unit fromUnit, Unit toUnit)
    {
        string message = "";

        //状態異常の追加効果
        Dictionary<Ailment, int> additionalAilments = new Dictionary<Ailment, int>();

        //物理攻撃なら、武器の効果を追加
        if (skill.AttackType == AttackType.physics)
        {
            if (fromUnit.UnitType1 == Unit.UnitType.ally)
            {
                //武器の追加効果
                Ally ally = (Ally)fromUnit;
                foreach (KeyValuePair<Ailment, int> kvp in ally.EquippedWeapon.Ailments)
                {
                    if (kvp.Value > 0)
                    {
                        additionalAilments.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        //スキルの追加効果
        //if (skill.AdditionalAilment != Ailment.nothing)
        //{
        //    additionalAilments.Add(skill.AdditionalAilment);
        //}

        //状態異常の付与判定
        foreach (KeyValuePair<Ailment, int> kvp in additionalAilments)
        {
            if (!toUnit.Ailments.ContainsKey(kvp.Key))
            {
                int random = UnityEngine.Random.Range(0, 100);
                //int luk = fromUnit.Statuses[Status.LUK] * 2;
                //double mnt = (toUnit.Statuses[Status.MNT] + toUnit.Statuses[Status.LUK]) * 5;
                //int per = (int)((luk / mnt) * 100);
                int per = kvp.Value;

                if (toUnit.AilmentResists[kvp.Key] == 2)
                {
                    per = 0;
                }
                else if (toUnit.AilmentResists[kvp.Key] == 1)
                {
                    per = per / 2;
                }
                else if (toUnit.AilmentResists[kvp.Key] == -1)
                {
                    per = (int)(per * 1.5);
                }
                else if (toUnit.AilmentResists[kvp.Key] == -2)
                {
                    per = per * 2;
                }

                if (per > random)
                {
                    message += $"{toUnit.Name}は{kvp.Key.GetAilmentName()}状態になった！\n";
                    toUnit.SetAilment(kvp.Key);
                }
                else
                {

                }
            }
        }

        return message;
    }

}

