using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumHolder;

public class Enemy : Unit {

    private GameController gameController;
    //行動確率
    //public Dictionary<EnemySkillName, int> enemyActionProb
    //    = new Dictionary<EnemySkillName, int>();

    public EnemyManager enemyManager = new EnemyManager();

    private Dictionary<Skill, int> enemyActions = new Dictionary<Skill, int>();

    private List<EnemyType> enemyTypes = new List<EnemyType>();

    //ドロップアイテムのリスト
    //private List<Item> dropItemList = new List<Item>();
    private Dictionary<Item, int> dropItems = new Dictionary<Item, int>();

    private bool isUnique = false;

    //基礎経験値
    private int exp = 150;
    //基礎GOld
    private int gold = 100;

    //private int minLv = 1;
    private int maxLv = 1;

    public int Gold { get => gold; set => gold = value; }
    public int MinLv { get; set; } = 1; 
    public Dictionary<Item, int> DropItems { get => dropItems; set => dropItems = value; }
    public int Exp { get => exp; set => exp = value; }
    public int Gold1 { get => gold; set => gold = value; }
    public int MaxLv { get => maxLv; set => maxLv = value; }
    public Dictionary<Skill, int> EnemyActions { get => enemyActions; set => enemyActions = value; }
    public List<EnemyType> EnemyTypes { get => enemyTypes; set => enemyTypes = value; }
    public bool IsUnique { get => isUnique; set => isUnique = value; }

    public Enemy(GameController gameController) : base (gameController)
    {
        this.gameController = gameController;
        enemyManager = gameController.EnemyManager;
        MinLv = Statuses[Status.Lv];
    }

    public void AddLv(int num)
    {
        List<Status> keyList = new List<Status>(Statuses.Keys);

        Statuses[Status.Lv] += num;

        foreach (Status key in keyList)
        {
            if (key == Status.Lv) continue;
            //Statuses[key] += (int)(Statuses[key] * (num/10)); 
            Statuses[key] += (int)(Statuses[key] * ((double)num / (double)10));
        }

        //Debug.Log(Statuses);
        //Debug.Log(Statuses);
    }

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
            //gameController.AllyManager.DamageEffect(this, damage);
            enemyManager.SetDamageEffect(this, damage, isHit);

            //ダメージテキスト
            if (damage > 0)
            {
                message += $"{Name}に{damage}のダメージを与えた\n";
            }
            else if (damage < 0)
            {
                if (Statuses[Status.currentHP] >= Statuses[Status.MaxHP])
                {
                    message += $"{Name}のHPが全回復した\n";
                }
                else
                {
                    message += $"{Name}のHPが{-damage}回復した\n";
                }
            }

            //死亡確認
            message += CheckDeath();
            CheckMaxHP();

            //ダメージを受けたときに解除される状態の判定
            if (damage > 0)
            {
                message += CheckRemove();
            }
        }

        return message;
    }

    public string CheckDeath()
    {
        string message = "";
        if (Statuses[EnumHolder.Status.currentHP] < 1)
        {
            Statuses[EnumHolder.Status.currentHP] = 0;
            message += $"{Name}を倒した！\n";
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

    //敵の合わせた物理攻撃力
    public override int GetOffensivePower()
    {
        int lv = Statuses[Status.Lv];
        int str = Statuses[Status.STR];
        double attack;
        attack = str * 1.8 + str * lv * 0.05;
        return (int)attack;
    }

    //装備を合わせた物理防御力
    public override int GetPhysicalDefensivePower()
    {
        int lv = Statuses[Status.Lv];
        int def = Statuses[Status.DEF];
        return (int)(def * (1.2 + lv * 0.05));
    }

    //装備を合わせた命中
    public override int GetHitProb()
    {
        //int lv = GetStatus(StatusKey.Lv);
        int tec = Statuses[Status.TEC];
        int agi = Statuses[Status.AGI];
        int luk = Statuses[Status.LUK];
        int hitProb = (int)((tec + luk) / 1.5);
        return hitProb;
    }

    //装備を合わせた回避
    public override int GetAvoidProb()
    {
        //int lv = GetStatus(StatusKey.Lv);
        int agi = Statuses[Status.AGI];
        int luk = Statuses[Status.LUK];
        int hitProb = (int)((agi + luk) / 1.5);
        return hitProb;
    }

    public int GetExp()
    {
        return ((int)((Statuses[EnumHolder.Status.Lv] - MinLv) *0.2* Exp + Exp));
    }

    public int GetGold()
    {
        return ((int)((Statuses[EnumHolder.Status.Lv] - MinLv) * 0.2 * gold + gold));
    }


    //private void CreateSample()
    //{
    //    AddAction(EnemySkillName.攻撃, 50);
    //    AddAction(EnemySkillName.殴りつける, 50);
    //    Item item = new Item();
    //    item.SetValue(Name: "原石", isAvailableOnBattle: false, isAvailableOnFloor: false, isConsumable: true);
    //    this.DropItems.Add(item, 100);
    //}

    //public void AddAction(EnemySkillName ability, int prob)
    //{
    //    enemyActionProb.Add(ability, prob);
    //    skillList.Add(new Skill());
        
    //}

}
