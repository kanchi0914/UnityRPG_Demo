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
            int healValue = Random.Range(50, 70);
            message += toUnit.SetDamage(-healValue);
        }
        else if (item.ItemName == ItemName.上薬草)
        {
            int healValue = Random.Range(110, 130);
            message += toUnit.SetDamage(-healValue);
        }
        else if (item.ItemName == ItemName.特薬草)
        {
            int healValue = Random.Range(230, 270);
            message += toUnit.SetDamage(-healValue);
        }
        else if (item.ItemName == ItemName.特効薬)
        {
            int healValue = 999;
            message += toUnit.SetDamage(-healValue);
            toUnit.Ailments.Clear();
        }
        else if (item.ItemName == ItemName.毒消し草)
        {
            message += toUnit.RecoverAilment(Ailment.poison);
        }
        else if (item.ItemName == ItemName.万能薬)
        {
            message += toUnit.RecoverAilment();
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
                message += toUnit.SetDamage(damage);
            }
        }
        else if (item.ItemName == ItemName.電撃ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                int damage = CalculateDamage(toUnit, item);
                message += toUnit.SetDamage(damage);
            }
        }
        else if (item.ItemName == ItemName.氷結ビン)
        {
            foreach (Unit u in opponentUnits)
            {
                int damage = CalculateDamage(toUnit, item);
                message += toUnit.SetDamage(damage);
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
        if (item.IsConsumable) gameController.AllyManager.Allies[0].Items.Remove(item);

        return message;
    }

    public string UseInField(Item item, List<Unit> unitList, Ally ally)
    {
        List<Ally> allies = gameController.AllyManager.Allies;
        Unit toUnit = allies.Find(a => a.ID1 == gameController.AllyManager.SelectedAllyID);

        string message = "";
        message += Use(item, unitList, ally, toUnit);

        return message;
    }

    private int CalculateDamage(Unit toUnit, Item item)
    {

        float random = UnityEngine.Random.Range(0.90f, 1.10f);
        //Attribution attribution = item.Attribution;
        int damage = (int)(item.Value * random);

        //if (toUnit.AttributionResists.ContainsKey(attribution))
        //{
        //    if (toUnit.AttributionResists[attribution] == 2)
        //    {
        //        damage = 1;
        //    }
        //    else if (toUnit.AttributionResists[attribution] == 1)
        //    {
        //        damage /= 2;
        //    }
        //    else if (toUnit.AttributionResists[attribution] == -1)
        //    {
        //        damage = (int)(1.5 * damage);
        //    }
        //    else if (toUnit.AttributionResists[attribution] == -2)
        //    {
        //        damage = (int)(2.0 * damage);
        //    }
        //}

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
