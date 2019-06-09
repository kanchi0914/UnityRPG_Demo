using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;
using static GameSettings;
using System;

using System.Linq;

public class Unit
{

    private GameController gameController;

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

    public List<Skill> skills = new List<Skill>();

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


    public Unit(GameController gameController)
    {
        this.gameController = gameController;
    }

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

    ////allyクラス、enemyクラスのメソッドが呼び出される
    //public virtual void SetDamage(int damage)
    //{

    //}

    /// <summary>
    /// ダメージの数値を受け取り、実際のHPを変化させ、メッセージを返す。
    /// </summary>
    /// <param name="damage">ダメージの数値。回復する場合はマイナスの値を与える</param>
    /// <param name="isHit">命中しない場合はfalse</param>
    /// <returns></returns>
    public string SetDamage(int damage = 0, bool isHit = true)
    {
        string message = "";

        if (!IsDeath)
        {
            //状態異常によるダメージの判定
            if (Ailments.ContainsKey(Ailment.sleep))
            {
                damage = (int)(damage * 1.5);
            }

            //ダメージ、回復はマイナス
            Statuses[EnumHolder.Status.currentHP] -= damage;

            //ダメージエフェクト
            if (this.GetType() == typeof(Ally))
            {
                gameController.AllyManager.DamageEffect(this, damage, isHit);
            }
            else
            {
                gameController.EnemyManager.SetDamageEffect(this, damage, isHit);
            }

            //ダメージテキスト
            if (damage > 0)
            {
                if (this.GetType() == typeof(Ally))
                {
                    message += $"{Name}は{damage}のダメージを受けた\n。";
                }
                else
                {
                    message += $"{Name}に{damage}のダメージを与えた\n。";
                }
                
            }
            //回復
            else if (damage < 0)
            {
                if (Statuses[Status.currentHP] >= Statuses[Status.MaxHP])
                {
                    message += $"{Name}のHPが全回復した\n。";
                }
                else
                {
                    message += $"{Name}のHPが{-damage}回復した\n。";
                }
            }

            //死亡確認
            message += CheckDeath();

            //最大HPの確認
            CheckMaxHP();

            //ダメージを受けたときに解除される状態の判定
            if (damage > 0)
            {
                message += CheckRemove();
            }
        }

        return message;
    }

    /// <summary>
    /// 死亡確認し、メッセージを返す
    /// </summary>
    /// <returns></returns>
    public string CheckDeath()
    {
        string message = "";
        if (Statuses[EnumHolder.Status.currentHP] < 1)
        {
            Statuses[EnumHolder.Status.currentHP] = 0;
            if (this.GetType() == typeof(Ally))
            {
                message += $"{Name}は死亡した\n!";
            }
            else
            {
                message += $"{Name}を倒した\n!";
            }
           
            //死亡;
            IsDeath = true;
        }
        return message;
    }

    public void CheckMaxHP()
    {
        //最大体力
        if (Statuses[EnumHolder.Status.currentHP] >= Statuses[EnumHolder.Status.MaxHP])
        {
            Statuses[EnumHolder.Status.currentHP] = Statuses[EnumHolder.Status.MaxHP];
        }
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
        return 0;
    }

    //装備を合わせた魔法防御力
    public virtual int GetMagicalDefensivePower()
    {
        return 0;
    }

    //装備を合わせた命中
    public virtual int GetHitProb()
    {
        return 0;
    }

    //装備を合わせた回避
    public virtual int GetAvoidProb()
    {
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


    /// <summary>
    /// 状態状を付与し、メッセージを返す
    /// </summary>
    /// <param name="ailment"></param>
    /// <returns></returns>
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


    /// <summary>
    /// 状態異常を回復し、メッセージを返す
    /// </summary>
    /// <param name="ailment">状態異常</param>
    /// <param name="prob">回復確率</param>
    /// <returns></returns>
    public string RecoverAilment(Ailment ailment = Ailment.nothing, int prob = 100)
    {
        string message = "";
        if (ailment == Ailment.nothing)
        {
            if (Ailments.Count > 0)
            {
                Ailments.Clear();
                message += $"{Name}の状態異常が回復した！";
            }
            else
            {
                message += $"しかし効果がなかった。";
            }
        }
        else
        {
            if (Ailments.ContainsKey(ailment))
            {
                Ailments.Remove(ailment);
                message += $"{Name}の{ailment.GetAilmentName()}状態が回復した！";

            }
            else
            {
                message += $"しかし効果がなかった。";
            }
        }
        return message;
    }

    //public string RercoverAilmentInProb(Ailment ailment, int prob)
    //{
    //    string message = "";
    //    int random = UnityEngine.Random.Range(0, 100);
    //    if (prob > random)
    //    {
    //        message += $"{Name}の{ailment.GetAilmentName()}状態が回復した！";
    //    }
    //    return message;
    //}

    /// <summary>
    /// ユニットの死亡状態を回復し、メッセージを返す
    /// </summary>
    /// <returns></returns>
    public string Resurrect()
    {
        string message = "";
        if (IsDeath)
        {
            IsDeath = false;
            int damage = Statuses[Status.MaxHP] / 2;
            //SetDamageByAbility(fromUnit, toUnit, skill, damage, false);
            SetDamage(-damage);
            message += $"{Name}は復活した！";
        }
        else
        {
            message += "しかし効果がなかった。";
        }
        return message;
    }

    /// <summary>
    /// ダメージを受けた時の解除判定
    /// </summary>
    /// <returns></returns>
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