using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static EnumHolder;


public static class Utility
{
    public static readonly Exception ItemWindowError = new Exception("item window error!");

    public static string ConvertSkillNameToString(Enum e)
    {
        string s = Enum.GetName(typeof(SkillName), e);
        return s;
    }

    public static string ConvertPlusOrMinusString(int num)
    {
        return string.Format("{0}", num.ToString("+#;-#;"));
    }

    //ポアソン
    public static int Poisson(double lambda)
    {
        double xp;
        int k = 0;
        System.Random r = new System.Random();
        xp = r.NextDouble();
        while (xp >= System.Math.Exp(-lambda))
        {
            xp = xp * r.NextDouble();
            k = k + 1;
        }
        return (k);
    }

    public static T GetRandomFromList<T>(List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T GetAtRandom<T>(List<T> list)
    {
        if (list.Count == 0)
        {
            Debug.LogError("リストが空です！");
        }

        return list[UnityEngine.Random.Range(0, list.Count)];
    }


    public static T RandomEnumValue<T>()
    {
        var v = Enum.GetValues(typeof(T));
        return (T)v.GetValue(new System.Random().Next(v.Length));
    }

    public static string GetStringOfEnum<T>(T name)
    {
        var s = Enum.GetName(typeof(T), name);
        //var s = "";
        return s;

    }

    //カラーセッティング
    //TODO:色設定を別クラスに分ける

    public static void SetTextColor(TextMeshProUGUI text, Color color, float alpha = 1.0f)
    {
        Color c = color;
        c.a = alpha;
        text.color = color;
    }

    public static void SetBackGroundImageColor(Image image, Color color, float alpha = 1.0f)
    {
        Color c = color;
        c.a = alpha;
        image.color = c;
    }

    public static void SetColorOfButtonObject(Transform buttonObject, Color color, float alpha = 0.3f)
    {
        Image buttonImage = buttonObject.GetComponent<Image>();
        Color c = color;
        c.a = alpha;
        buttonImage.color = c;
    }


    public static List<Unit> GetAllyUnitListFromUnits(List<Unit> units, Unit fromUnit)
    {
        List<Unit> allyUnits = new List<Unit>();
        if (fromUnit.GetType() == typeof(Ally))
        {
            foreach (Unit u in units)
            {
                if (u.GetType() == typeof(Ally))
                {
                    allyUnits.Add(u);
                }
            }
        }
        else
        {
            foreach (Unit u in units)
            {
                if (u.GetType() == typeof(Enemy))
                {
                    allyUnits.Add(u);
                }
            }
        }
        return allyUnits;
    }

    public static List<Unit> GetEnemyUnitListFromUnits(List<Unit> units, Unit fromUnit)
    {
        List<Unit> opponentUnits = new List<Unit>();
        if (fromUnit.GetType() == typeof(Ally))
        {
            foreach (Unit u in units)
            {
                if (u.GetType() == typeof(Enemy))
                {
                    opponentUnits.Add(u);
                }
            }
        }
        else
        {
            foreach (Unit u in units)
            {
                if (u.GetType() == typeof(Ally))
                {
                    opponentUnits.Add(u);
                }
            }
        }
        return opponentUnits;
    }

    public static List<Unit> CastAlliesToUnits(List<Ally> allies)
    {
        List<Unit> units = new List<Unit>();
        foreach (Ally a in allies)
        {
            units.Add(a);
        }
        return units;
    }

}

