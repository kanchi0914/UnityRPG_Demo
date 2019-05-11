using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;
using static GameSettings;
using System;

using System.Linq;

public class Unit
{
    private Sprite image;

    private bool isClickable = false;

    //防御判定用
    private Ability currentAbility;

    public int number;

    private string ID = "unit";
    //private string Name = "ユニット";
    private string name = "ユニット";

    private int tempAGI;

    public enum UnitType { enemy, ally }
    private UnitType unitType = UnitType.enemy;

    //状態異常のリスト
    //private HashSet<Ailment> ailments = new HashSet<Ailment>();

    private HashSet<Ailment> additionalAilment = new HashSet<Ailment>();

    //状態異常耐性
    private Dictionary<Ailment, int> ailmentResists = new Dictionary<Ailment, int>()
    {
        {Ailment.nothing, 0},
        //{Ailment.bleeding, 0},
        //{Ailment.burn, 0},
        {Ailment.confusion, 0},
        //{Ailment.curse, 0},
        {Ailment.death, 0},
        //{Ailment.frost, 0},
        {Ailment.paralysis, 0},
        {Ailment.poison, 0},
        {Ailment.seal, 0},
        {Ailment.sleep, 0},
        {Ailment.stun, 0},
        {Ailment.terror, 0},
        //{Ailment.turnover, 0},
    };



    //属性耐性
    private Dictionary<Attribution, int> attributionResists = new Dictionary<Attribution, int>()
    {
        {Attribution.nothing, 0},
        {Attribution.fire, 0},
        {Attribution.ice, 0},
        {Attribution.thunder, 0},
        {Attribution.holy, 0},
        {Attribution.dark, 0}
    };

    //バフクラス
    public class Buff
    {
        //public SkillName SkillName;
        //public ItemName ItemName;
        public string UnitID = "";

        public Skill Skill { get; set; }
        public Item Item { get; set; }

        public bool IsBuff = true;

        public string Description = "";
        //残りターン
        public int LeftTurn = 6;

        //ステータス変化
        public Dictionary<Status, double> Statuses
        = new Dictionary<Status, double>()
        {
            {Status.STR, 0},
            {Status.DEF, 0},
            {Status.INT, 0},
            {Status.MNT, 0},
            {Status.TEC, 0},
            {Status.AGI, 0},
            {Status.LUK, 0}
        };

        public Attribution attribution = Attribution.nothing;

        //状態異常耐性
        public Dictionary<Ailment, int> AilmentResists = new Dictionary<Ailment, int>();

        //状態異常の追加効果
        public Dictionary<Ailment, int> AilmentEffectivenesses = new Dictionary<Ailment, int>();

        public Buff(Skill skill = null, Item item = null,
            string unitID = "")
        {
            this.Skill = skill;
            this.Item = item;
            this.UnitID = unitID;
        }
    
        public override bool Equals(object obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                return false;
            }

            Buff buff = (Buff)obj;

            if (this.Skill != null)
            {
                return (this.Skill.SkillName == buff.Skill.SkillName);
            }
            else
            {
                return (this.Item.ItemName == buff.Item.ItemName);
            }

        }

    }

    //public Dictionary<Skill, int> skillBuffs = new Dictionary<Skill, int>();
    private List<Buff> buffs = new List<Buff>();
    //private List<Buff> skilBuffs = new List<Buff>();
    //public Dictionary<Skill, int> skillDebuffs = new Dictionary<Skill, int>();

    //private string unitType = "enemy";

    public List<Skill> skills;

    private bool isDeath = false;





    private Dictionary<Status, int> statuses = new Dictionary<Status, int>()
    {
        {EnumHolder.Status.Lv, 1},
        {EnumHolder.Status.MaxHP, 50},
        {EnumHolder.Status.currentHP, 50},
        {EnumHolder.Status.MaxSP, 20},
        {EnumHolder.Status.currentSP, 20},
        {EnumHolder.Status.STR, 10},
        {EnumHolder.Status.DEF, 10},
        {EnumHolder.Status.INT, 10},
        {EnumHolder.Status.MNT, 10},
        {EnumHolder.Status.AGI, 10},
        {EnumHolder.Status.LUK, 10}
    };

    //HPの％を返す
    public float GetPerHP()
    {   
        float per = (Statuses[Status.currentHP] / Statuses[Status.MaxHP]);
        return Math.Min(1, Math.Max(0, per));
    }

    public float GetPerSP()
    {
        float per = (Statuses[Status.currentSP] / Statuses[Status.MaxSP]);
        return Math.Min(1, Math.Max(0, per));
    }

    public Unit()
    {
        skills = new List<Skill>();
        tempAGI = Statuses[EnumHolder.Status.AGI];
    }

    //ここを使う
    //public void SetAllStatus(string ID = "", string Name = "",
    //    int Lv = 1, int maxHP = 50, int maxSP = 50, int str = 10, int def = 10, int intel = 10,
    //    int mnt = 10, int tec = 10, int agi = 10, int luk = 10)
    //{
    //    SetID(ID);
    //    SetName(Name);

    //    SetStatus(EnumHolder.Status.Lv, Lv);
    //    SetStatus(EnumHolder.Status.MaxHP, maxHP);
    //    SetStatus(EnumHolder.Status.currentHP, maxHP);
    //    SetStatus(EnumHolder.Status.MaxSP, maxSP);
    //    SetStatus(EnumHolder.Status.currentSP, maxSP);
    //    //SetStatus(StatusKey.currentHP maxHP);
    //    //SetStatus("currentSP", maxSP);
    //    SetStatus(EnumHolder.Status.STR, str);
    //    SetStatus(EnumHolder.Status.DEF, def);
    //    SetStatus(EnumHolder.Status.INT, intel);
    //    SetStatus(EnumHolder.Status.MNT, mnt);
    //    SetStatus(EnumHolder.Status.TEC, tec);
    //    SetStatus(EnumHolder.Status.AGI, agi);
    //    SetStatus(EnumHolder.Status.LUK, luk);
    //}

    //IDが等しければ同じ
    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }

        Unit u = (Unit)obj;
        return (this.ID1 == ID1);
    }

    //allyクラス、enemyクラスのメソッドが呼び出される
    public virtual void SetDamage(int damage)
    {

    }

    //装備を合わせた物理攻撃力
    public virtual int GetOffensivePower()
    {
        return 0;
    }

    public virtual int GetMagicalOffensivePower()
    {
        return 0;
    }

    //装備を合わせた物理防御力
    public virtual int GetPhysicalDefensivePower()
    {
        //int lv = GetStatus(EnumHolder.Status.Lv);
        //int def = GetStatus(EnumHolder.Status.DEF);
        //return (int)(def * (1.2 + lv * 0.05));
        return 0;
    }

    //装備を合わせた魔法防御力
    public virtual int GetMagicalDefensivePower()
    {
        //int lv = GetStatus(EnumHolder.Status.Lv);
        //int mnt = GetStatus(EnumHolder.Status.MNT);
        //int def = GetStatus(EnumHolder.Status.DEF);
        //int equipDef = 0;
        //return (int)((def * 0.7 + mnt * 0.3) * (1.1 + lv * 0.05));
        return 0;
    }

    //装備を合わせた命中
    public virtual int GetHitProb()
    {
        //int lv = GetStatus(StatusKey.Lv);
        //int tec = GetStatus(EnumHolder.Status.TEC);
        //int agi = GetStatus(EnumHolder.Status.AGI);
        //int luk = GetStatus(EnumHolder.Status.LUK);
        //int hitProb = (int)((tec * 2 + agi + luk) * (0.8));
        //return hitProb;
        return 0;
    }

    //装備を合わせた回避
    public virtual int GetAvoidProb()
    {
        //int lv = GetStatus(StatusKey.Lv);
        //int agi = GetStatus(EnumHolder.Status.AGI);
        //int luk = GetStatus(EnumHolder.Status.LUK);
        //int hitProb = (int)((agi + luk) * (1.2));
        //return hitProb;
        return 0;
    }



    public virtual List<Item> GetItems()
    {
        return null;
    }


    public bool IsDeath
    {
        get;
        set;
    }
    public List<Buff> Buffs { get => buffs; set => buffs = value; }
    //public HashSet<Ailment> Ailments { get => ailments; set => ailments = value; }
    public Dictionary<Ailment, int> Ailments { get; set; }
    public int TempAGI { get => tempAGI; set => tempAGI = value; }
    public Dictionary<Status, int> Statuses { get => statuses; set => statuses = value; }
    public Sprite Image { get => image; set => image = value; }
    public Dictionary<Ailment, int> AilmentResists { get => ailmentResists; set => ailmentResists = value; }
    public Dictionary<Attribution, int> AttributionResists { get => attributionResists; set => attributionResists = value; }
    public UnitType UnitType1 { get => unitType; set => unitType = value; }
    public HashSet<Ailment> AdditionalAilment { get => additionalAilment; set => additionalAilment = value; }
    //public Ability CurrentAbility { get => currentAbility; set => currentAbility = value; }
    public Command CurrentCommand { get; set; }

    public string Name { get => name; set => name = value; }
    public string ID1 { get => ID; set => ID = value; }

    //public string GetID()
    //{
    //    return ID1;
    //}

    //public void SetID(string id)
    //{
    //    this.ID1 = id;
    //}

    //public string GetName()
    //{
    //    return Name;
    //}

    //public void SetName(string name)
    //{
    //    this.Name = name;
    //}

    ////バフ、デバフの効果を考慮
    //public int GetStatus(Status key)
    //{

    //    return this.Statuses[key];
    //}

    //public void SetStatus(Status key, int value)
    //{
    //    this.Statuses[key] = value;
    //}

    //public void AddStatus(Status key, int value)
    //{
    //    this.Statuses[key] += value;
    //}

    //public List<string> GetCondition()
    //{
    //    return condition;
    //}

    //状態異常を食らった
    public string SetAilment(Ailment ailment)
    {
        string message = "";
        message += $"{Name}は{ailment.GetAilmentName()}状態になった！";
        if (Ailments.ContainsKey(ailment))
        {
            Ailments[ailment] = AilmentInitialTurn;
        }
        else
        {
            Ailments.Add(ailment, AilmentInitialTurn);
        }
        return message;
    }

    //public string SetAilmentInProb(Ailment ailment, int prob)
    //{
    //    //状態異常耐性
    //    if (ailmentResists.ContainsKey(ailment))
    //    {
    //        if (ailmentResists[ailment] == 1)
    //        {
    //            prob /= 2;
    //        }
    //        if (ailmentResists[ailment] == 2)
    //        {
    //            prob = 0;
    //        }
    //        if (ailmentResists[ailment] == -1)
    //        {
    //            prob *= 2;
    //        }
    //        if (ailmentResists[ailment] == -2)
    //        {
    //            prob = 100;
    //        }
    //    }

    //    string message = "";
    //    int random = UnityEngine.Random.Range(0,100);
    //    if (prob > random)
    //    {
    //        message += $"{Name}は{ailment.GetAilmentName()}状態になった！";
    //    }
    //    return message;
    //}

    //状態異常の回復
    public string RecoverAilment(Ailment ailment)
    {
        string message = "";
        Ailments.Remove(ailment);
        message += $"{Name}の{ailment.GetAilmentName()}状態が回復した！";
        return message;
    }

    public string RercoverAilmentInProb(Ailment ailment, int prob)
    {
        string message = "";
        int random = UnityEngine.Random.Range(0, 100);
        if (prob > random)
        {
            message += $"{Name}の{ailment.GetAilmentName()}状態が回復した！";
        }
        return message;
    }

    //ダメージを受けたときの解除判定
    public string CheckRemove()
    {
        string message = "";

        if (Ailments.ContainsKey(EnumHolder.Ailment.sleep))
        {
            message += $"{this.Name}は目が覚めた！\n";
            this.Ailments.Remove(EnumHolder.Ailment.sleep);
        }
        var buff = buffs.Find(b => b.Skill.SkillName == SkillName.ハイボルテージ);
        if (this.buffs.Contains(buff))
        {
            message += $"{this.Name}のハイ・ボルテージが解除された！\n";
            buffs.Remove(buff);
        }

        return message;
    }


    public void ChangeSP(int n)
    {
        Statuses[EnumHolder.Status.currentSP] += n;
        if (Statuses[EnumHolder.Status.currentSP] < 0)
        {
            Statuses[EnumHolder.Status.currentSP] = 0;
        }
        else if (Statuses[EnumHolder.Status.currentSP] > Statuses[EnumHolder.Status.MaxSP])
        {
            Statuses[EnumHolder.Status.currentSP] = Statuses[EnumHolder.Status.MaxSP];
        }
    }

    public List<Unit> GetMates(List<Unit> units)
    {
        List<Unit> mates = new List<Unit>();
        foreach (Unit u in units)
        {
            //死んでいない相手から選ぶ
            if (!u.IsDeath)
            {
                if (u.UnitType1 == this.UnitType1)
                {
                    mates.Add(u);
                }
            }
        }
        return mates;
    }

    public List<Unit> GetOpponents(List<Unit> units)
    {
        List<Unit> opponents = new List<Unit>();
        foreach (Unit u in units)
        {
            //死んでいない相手から選ぶ
            if (!u.IsDeath)
            {
                if (u.UnitType1 != this.UnitType1)
                {
                    opponents.Add(u);
                }
            }
        }
        return opponents;
    }

    public Unit ChooseRandomly(List<Unit> unitList)
    {
        List<Unit> opponents = new List<Unit>();
        foreach (Unit u in unitList)
        {
            //死んでいない相手から選ぶ
            if (!u.IsDeath)
            {
                if (u.UnitType1 != this.UnitType1)
                {
                    opponents.Add(u);
                }
            }
        }

        if (opponents.Count != 0)
        {
            Unit chosen = opponents[UnityEngine.Random.Range(0, opponents.Count)];
            return chosen;
        }
        else
        {
            Debug.Log("could not choose.");
            return null;
        }

    }

    

}