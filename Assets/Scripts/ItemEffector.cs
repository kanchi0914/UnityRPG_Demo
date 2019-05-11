using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EnumHolder;
using UnityEngine;

public class ItemEffecor
{
    GameController gameController;

    public ItemEffecor(GameController gameController)
    {
        this.gameController = gameController;
    }

    public string Use(Item item, List<Unit> unitList, Unit fromUnit, Unit toUnit)
    {
        string message = "";
        message += $"{fromUnit}は{item.Name}を使った！\n";

        List<Unit> allyUnits = new List<Unit>();
        List<Unit> opponentUnits = new List<Unit>();

//        if (fromUnit.UnitType1 == Unit.UnitType.ally)
//        {
//            foreach (Unit u in unitList)
//            {
//                if (u.UnitType1 == Unit.UnitType.ally)
//                {
//                    allyUnits.Add(u);
//                }
//                else
//                {
//                    opponentUnits.Add(u);
//;               }
//            }
//        }
        if (fromUnit.GetType() == typeof(Ally))
        {
            foreach (Unit u in unitList)
            {
                if (u.GetType() == typeof(Ally))
                {
                    allyUnits.Add(u);
                }
                else
                {
                    opponentUnits.Add(u);
                }
            }
        }
        else
        {
            foreach (Unit u in unitList)
            {
                if (u.GetType() == typeof(Enemy))
                {
                    allyUnits.Add(u);
                }
                else
                {
                    opponentUnits.Add(u);
                }
            }
        }


        //============================================================
        //回復系
        //============================================================


        if (item.ItemName == ItemName.薬草)
        {
            int healValue = Random.Range(50, 60);
            message += SetDamage(fromUnit, toUnit, item, -healValue);
            //message += $"{fromUnit}のHPが{healValue}回復した！";
        }
        else if (item.ItemName == ItemName.上薬草)
        {
            int healValue = Random.Range(110, 130);
            message += SetDamage(fromUnit, toUnit, item, -healValue);
            //message += $"{fromUnit}のHPが{healValue}回復した！";
        }
        else if (item.ItemName == ItemName.特薬草)
        {
            int healValue = Random.Range(230, 270);
            message += SetDamage(fromUnit, toUnit, item, -healValue);
            //message += $"{fromUnit}のHPが{healValue}回復した！";
        }
        else if (item.ItemName == ItemName.特効薬)
        {
            int healValue = Random.Range(230, 999);
            message += SetDamage(fromUnit, toUnit, item, -healValue);
            toUnit.Ailments.Clear();
            message += $"{fromUnit}のHPが全回復した！";
            message += $"{fromUnit}の状態異常が回復した！";
        }
        else if (item.ItemName == ItemName.毒消し草)
        {
            if (toUnit.Ailments.ContainsKey(Ailment.poison))
            {
                toUnit.Ailments.Remove(Ailment.poison);
                message += $"{fromUnit}の毒状態が回復した！";
            }
            else
            {
                message += $"しかし効果がなかった。";
            }
        }
        else if (item.ItemName == ItemName.万能薬)
        {
            if (toUnit.Ailments.Count > 0)
            {
                toUnit.Ailments.Clear();
                message += $"{fromUnit}の状態以上が回復した！";
            }
            else
            {
                message += $"しかし効果がなかった。";
            }
        }
        else if (item.ItemName == ItemName.特効薬)
        {
            toUnit.SetDamage(-999);
            //allyManager.DamageEffect(toUnit, -999);
            message += $"{fromUnit}のHPが全回復した！";
            message += $"{fromUnit}の状態異常が回復した！";
        }

        //============================================================
        //ダメージ系
        //============================================================

        else if (item.ItemName == ItemName.火炎ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                int damage = CalculateDamage(toUnit, item);
                message += SetDamage(fromUnit, u, item, damage);
            }
        }
        else if (item.ItemName == ItemName.電撃ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                int damage = CalculateDamage(toUnit, item);
                message += SetDamage(fromUnit, u, item, damage);
            }
        }
        else if (item.ItemName == ItemName.氷結ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                int damage = CalculateDamage(toUnit, item);
                message += SetDamage(fromUnit, u, item, damage);
            }
        }
        else if (item.ItemName == ItemName.毒ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                SetAilmentInProb(fromUnit, toUnit, Ailment.poison, Status.LUK);
            }
        }

        //アイテムの消費
        if (item.isConsumable) fromUnit.GetItems().Remove(item);

        return message;
    }

    public string UseInField(Item item, List<Unit> unitList, Ally ally)
    {
        List<Ally> allies = gameController.AllyManager.Allies;
        Unit toUnit = allies.Find(a => a.ID1 == gameController.AllyManager.SelectedAllyID);

        string message = "";
        message += Use(item, unitList, ally, toUnit);

        //消費
        //List<Item> items = ally.Items;
        //if (item.isConsumable) items.Remove(items.Find(i => i.ID == item.ID));

        return message;
    }

    public string SetDamage(Unit fromUnit, Unit toUnit, Item item, int damage, bool isMessage = true)
    {
        string message = "";

        if (!toUnit.IsDeath)
        {
            //状態異常によるダメージの判定
            if (toUnit.Ailments.ContainsKey(Ailment.sleep))
            {
                damage = (int)(damage * 1.5);
            }

            //ダメージ、回復はマイナス
            toUnit.Statuses[EnumHolder.Status.currentHP] -= damage;

            //ダメージテキスト
            if (isMessage)
            {
                if (damage > 0)
                {
                    if (toUnit.UnitType1 == Unit.UnitType.ally)
                    {
                        message += $"{toUnit.Name}に{damage}のダメージ！";
                    }
                    else
                    {
                        message += $"{toUnit.Name}に{damage}のダメージ！";
                    }

                }
                else
                {
                    if (toUnit.Statuses[Status.currentHP] == toUnit.Statuses[Status.MaxHP])
                    {
                        message += $"{toUnit.Name}のHPが全回復した！";
                    }
                    else
                    {
                        message += $"{toUnit.Name}のHPが{-damage}回復した！";
                    }
                }
            }

            //ダメージエフェクト
            if (toUnit.UnitType1 == Unit.UnitType.ally)
            {
                Ally ally = (Ally)toUnit;
                gameController.AllyManager.DamageEffect(toUnit, damage);
            }
            else
            {
                Enemy enemy = (Enemy)toUnit;
                //Ally ally = (Ally)fromUnit;
                enemy.enemyManager.SetDamageEffect(toUnit, damage);
            }


            //死亡確認
            if (toUnit.Statuses[EnumHolder.Status.currentHP] < 0)
            {
                toUnit.Statuses[EnumHolder.Status.currentHP] = 0;
                if (toUnit.UnitType1 == Unit.UnitType.ally)
                {
                    message += $"{toUnit.Name}は死亡した！";
                }
                else
                {
                    message += $"{toUnit.Name}を倒した！";
                }

                //死亡;
                toUnit.IsDeath = true;
            }
            //最大体力
            else if (toUnit.Statuses[EnumHolder.Status.currentHP] > toUnit.Statuses[EnumHolder.Status.MaxHP])
            {
                toUnit.Statuses[EnumHolder.Status.currentHP] = toUnit.Statuses[EnumHolder.Status.MaxHP];
            }

            //状態異常
            if (item.Ailment != Ailment.nothing)
            {

                if (!toUnit.Ailments.ContainsKey(item.Ailment))
                {
                    int random = UnityEngine.Random.Range(0, 100);
                    int per = (fromUnit.Statuses[Status.LUK] * 2 / (toUnit.Statuses[Status.MNT]
                        + toUnit.Statuses[Status.LUK]) * 5) * 100;

                    if (toUnit.AilmentResists[item.Ailment] == 2)
                    {
                        per = 0;
                    }
                    else if (toUnit.AilmentResists[item.Ailment] == 1)
                    {
                        per = per / 2;
                    }
                    else if (toUnit.AilmentResists[item.Ailment] == -1)
                    {
                        per = (int)(per * 1.5);
                    }
                    else if (toUnit.AilmentResists[item.Ailment] == -2)
                    {
                        per = per * 2;
                    }

                    if (per > random)
                    {
                        message += $"{toUnit}は{item.Ailment.GetAilmentName()}状態になった！";
                        toUnit.SetAilment(item.Ailment);
                    }
                }
            }

            

            //ダメージを受けたときに解除される状態の判定
            if (damage > 0)
            {
                message += toUnit.CheckRemove();
            }

        }

        return message;
    }

    private int CalculateDamage(Unit toUnit, Item item)
    {

        float random = UnityEngine.Random.Range(0.90f, 1.10f);
        Attribution attribution = item.Attribution;
        int damage = (int)(item.Value * random);

        if (toUnit.AttributionResists.ContainsKey(attribution))
        {
            if (toUnit.AttributionResists[attribution] == 2)
            {
                damage = 1;
            }
            else if (toUnit.AttributionResists[attribution] == 1)
            {
                damage /= 2;
            }
            else if (toUnit.AttributionResists[attribution] == -1)
            {
                damage = (int)(1.5 * damage);
            }
            else if (toUnit.AttributionResists[attribution] == -2)
            {
                damage = (int)(2.0 * damage);
            }
        }

        return damage;
    }

    //状態異常の付与
    //statusは成功確率の依存ステータス
    public string SetAilmentInProb(Unit fromUnit, Unit toUnit, Ailment ailment, Status status)
    {
        string message = "";
        if (!toUnit.Ailments.ContainsKey(ailment))
        {
            int random = UnityEngine.Random.Range(0, 100);
            int per = ((fromUnit.Statuses[status] + fromUnit.Statuses[Status.LUK])
                / ((toUnit.Statuses[Status.MNT] + toUnit.Statuses[Status.LUK]) * 2) * 100);

            if (per > random)
            {
                message += $"{toUnit}は{ailment.GetAilmentName()}状態になった！";
                toUnit.SetAilment(ailment);
            }
        }
        return message;
    }


}
