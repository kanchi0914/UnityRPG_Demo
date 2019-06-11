using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static EnumHolder;

public class Ally : Unit
{

    public List<Item> Items = new List<Item>();
    private Race race = Race.人間;
    private Job job = Job.warrior;
    private Personality personality = Personality.normal;

    private GameObject allyPanel;

    //public AllyManager allyManager = new AllyManager();
    private GameController gameController;


    private int statusUpPer = 40;

    private int maxSkillNum = 8;

    private int maxItemNum = 20;
    private int expSum = 0;

    //次レベルまでの経験値
    private int nextLvExp = 100;

    private List<int> lvUpHps = new List<int>();
    private List<int> lvUpSps = new List<int>();

    private int lvUpStack = 0;

    //装備の情報
    private Weapon equippedWeapon = new Weapon();
    private Guard equippedArmor = new Guard();
    private Guard equippedAccessory = new Guard();

    private List<Guard> equippedGuards = new List<Guard>();

    //仲間のレベルアップ内容
    private List<LvUpInformation> lvUpInformationsOfNPC = new List<LvUpInformation>();


    private Dictionary<Status, int> uppedStatus = new Dictionary<Status, int>();

    private List<LvUpInformation> lvUpHistory = new List<LvUpInformation>();

    //public List<Guard> EquippedGuards { get => equippedGuards; set => equippedGuards = value; }

    public Weapon EquippedWeapon { get => equippedWeapon; set => equippedWeapon = value; }
    public int MaxItemNum { get => maxItemNum; set => maxItemNum = value; }
    public int NextLvExp { get => nextLvExp; set => nextLvExp = value; }
    public int ExpSum { get => expSum; set => expSum = value; }
    public Dictionary<Status, int> InitialStatuses { get => initialStatuses; set => initialStatuses = value; }
    public Race Race { get => race; set => race = value; }
    public Job Job { get => job; set => job = value; }
    public List<int> LvUpHps { get => lvUpHps; set => lvUpHps = value; }
    public List<int> LvUpSps { get => lvUpSps; set => lvUpSps = value; }
    public Guard EquippedArmor { get => equippedArmor; set => equippedArmor = value; }
    public Guard EquippedAccessory { get => equippedAccessory; set => equippedAccessory = value; }
    public int UppedLv { get => lvUpStack; set => lvUpStack = value; }
    public Dictionary<Status, int> UppedStatus { get => uppedStatus; set => uppedStatus = value; }
    public Personality Personality { get => personality; private set => personality = value; }
    public List<LvUpInformation> LvUpInformationsOfNPC { get => lvUpInformationsOfNPC; set => lvUpInformationsOfNPC = value; }
    public List<LvUpInformation> LvUpRecord { get => lvUpHistory; private set => lvUpHistory = value; }
    public int MaxSkillNum { get => maxSkillNum; set => maxSkillNum = value; }
    public GameObject AllyPanel { get => allyPanel; set => allyPanel = value; }




    //レベルアップの記録
    //leveluprecord  = ..



    public struct LvUpInformation
    {
        public int UppedHP;
        public int UppedSP;
        public (Status status, int value) StatusUp;
        public Skill Skill;
    }

    private Dictionary<Status, int> initialStatuses = new Dictionary<Status, int>()
    {
        {Status.Lv,0},
        {Status.MaxHP, 0},
        {Status.currentHP, 0},
        {Status.MaxSP, 0},
        {Status.currentSP, 0},
        {Status.STR, 0},
        {Status.DEF, 0},
        {Status.INT, 0},
        {Status.MNT, 0},
        {Status.TEC, 0},
        {Status.AGI, 0},
        {Status.LUK, 0}
    };

    //コンストラクタ
    public Ally(GameController gameController) : base (gameController)
    {
        this.gameController = gameController;
        //UnitType1 = UnitType.ally;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || this.GetType() != obj.GetType())
        {
            return false;
        }

        Unit u = (Unit)obj;
        return (this.ID1 == u.ID1);
    }



    //============================================================
    //コンポーネント関連
    //============================================================

    public void SetPanel()
    {
        TextMeshProUGUI lv = allyPanel.transform.Find("Status/Lv").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maxHP = allyPanel.transform.Find("Status/MaxHP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI currentHP = allyPanel.transform.Find("Status/CurrentHP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maxSP = allyPanel.transform.Find("Status/MaxSP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI currentSP = allyPanel.transform.Find("Status/CurrentSP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI allyName = allyPanel.transform.Find("Status/Name").GetComponent<TextMeshProUGUI>();

        allyPanel.name = ID1;
        lv.text = "Lv " + Statuses[Status.Lv].ToString();
        maxHP.text = Statuses[Status.MaxHP].ToString();
        currentHP.text = Statuses[Status.currentHP].ToString();
        maxSP.text = Statuses[Status.MaxSP].ToString();
        currentSP.text = Statuses[Status.currentSP].ToString();
        allyName.text = Name;
    }

  
    //============================================================
    //基本メソッド
    //============================================================

    public void SetHPDamage(bool isEffectOn)
    {

    }

    public void SetSPDamage()
    {

    }


    /// <summary>
    /// 経験値を加算し、上昇したレベルをスタックに追加
    /// </summary>
    /// <param name="exp"></param>
    public void AddExp(int exp)
    {
        ExpSum += exp;

        int leftExp = exp;
        int preLv = Statuses[Status.Lv];

        while (true)
        {
            if (leftExp - NextLvExp > 0)
            {
                leftExp -= NextLvExp;
                NextLvExp = 100 * preLv;
                preLv += 1;
                lvUpStack += 1;
            }
            else
            {
                NextLvExp -= leftExp;
                break;
            }
        }
    }

    /// <summary>
    /// 残っているレベルアップをすべて消化し、メッセージを返す
    /// </summary>
    /// <returns></returns>
    public string ConsumeLvUpStack()
    {
        List<LvUpInformation> infos = new List<LvUpInformation>();

        int uppedHPSum = 0;
        int uppedSPSum = 0;

        Dictionary<Status, int> uppedStatuses = new Dictionary<Status, int>()
        {
            {Status.Lv,0},
            {Status.MaxHP, 0},
            {Status.currentHP, 0},
            {Status.MaxSP, 0},
            {Status.currentSP, 0},
            {Status.STR, 0},
            {Status.DEF, 0},
            {Status.INT, 0},
            {Status.MNT, 0},
            {Status.TEC, 0},
            {Status.AGI, 0},
            {Status.LUK, 0}
        };

        List<string> acquiredSkillTexts = new List<string>();
        List<string> uppedSkillTexts = new List<string>();

        for (int i = 0; i < lvUpStack; i++)
        {
            LvUpInformation info = new LvUpInformation();
            info = GetLvUpBonusAutomatically();

            infos.Add(info);

            Statuses[Status.Lv] += 1;

            Statuses[Status.MaxHP] += info.UppedHP;
            uppedHPSum += info.UppedHP;

            Statuses[Status.MaxSP] += info.UppedSP;
            uppedSPSum += info.UppedSP;

            if (info.StatusUp.value > 0)
            {
                Statuses[info.StatusUp.status] += info.StatusUp.value;
                uppedStatuses[info.StatusUp.status] += info.StatusUp.value;
            }
            if (info.Skill != null && info.Skill.SkillName != SkillName.無し)
            {
                var a = info.Skill.SkillName;
                var learned = skills.Find(s => s.SkillName == a);
                if (learned != null)
                {
                    learned.SkillLevel += 1;
                    acquiredSkillTexts.Add($"{ConvertSkillNameToString(learned.SkillName)}:" +
                    $"Lv{learned.SkillLevel}");
                }
                else
                {
                    AddSkillToNPC(info.Skill);
                    acquiredSkillTexts.Add($"{ConvertSkillNameToString(info.Skill.SkillName)}:" +
                    $"Lv{info.Skill.SkillLevel}");
                }

            }
        }

        string message = "";

        message += $"{Name}がレベルアップ!\n";
        message += $"Lv {Statuses[Status.Lv] - lvUpStack} > {Statuses[Status.Lv]}\n";
        message += $"HP+{uppedHPSum} SP+{uppedSPSum}\n";

        //ステータス情報
        foreach (KeyValuePair<Status, int> kvp in uppedStatuses)
        {
            if (kvp.Value > 0)
            {
                var s = Utility.GetStringOfEnum(kvp.Key);
                message += $"{s}+{kvp.Value} ";
            }
        }
        message += "\n";

        //スキル情報
        //string skillText = "";
        if (acquiredSkillTexts.Count > 0)
        {
            message += "スキルを習得：\n";
        }
        foreach (string s in acquiredSkillTexts)
        {
            //skillText += s;
            message += $"{s}\n";
        }

        //if (uppedSkillTexts.Count > 0)
        //{
        //    message += "スキルLvが上昇：\n";
        //}
        //foreach (string s in uppedSkillTexts)
        //{
        //    //skillText += s;
        //    message += $"{s}\n";
        //}

        lvUpStack = 0;

        return message;
    }


    /// <summary>
    /// NPCのレベルを1あげる
    /// NPCを生成するときに使う
    /// 複数レベル上げるときは呼び出し元で
    /// </summary>
    public void ExecuteOneLvUpOfNPC()
    {
        var info = GetLvUpBonusAutomatically();
        lvUpHistory.Add(info);

        this.Statuses[Status.MaxHP] += info.UppedHP;
        Statuses[Status.MaxSP] += info.UppedSP;

        if (info.StatusUp.value > 0)
        {
            Statuses[info.StatusUp.status] += info.StatusUp.value;
        }
        if (info.Skill != null && info.Skill.SkillName != SkillName.無し)
        {
            if (skills.Contains(info.Skill))
            {
                var learned = skills.Find(s => s == info.Skill);
                learned.SkillLevel += 1;
            }
            else
            {
                AddSkillToNPC(info.Skill);
            }
        }
    }

    public void AddSkillToNPC(Skill skill)
    {
        if (skills.Count < 8)
        {
            AddSkill(skill);
        }
        //string skillName = Enum.GetName(typeof(SkillName), skill.SkillName);
        //Skill newSkill = skillGenerator.Generate(skillName);
        //if (skills.Count < 8)
        //{
        //    skills.Add(newSkill);
        //}
    }

    public void AddSkillToLeader(Skill skill)
    {
        //string skillName = Enum.GetName(typeof(SkillName), skill.SkillName);
        //Skill newSkill = skillGenerator.Generate(skillName);
        if (skills.Count < 8)
        {
            AddSkill(skill);
        }
        else
        {

        }
    }

    /// <summary>
    /// スキルを追加
    /// 既に存在する場合は追加しない
    /// </summary>
    /// <param name="skill"></param>
    public void AddSkill(Skill skill)
    {
        string newSkillName = Enum.GetName(typeof(SkillName), skill.SkillName);
        Skill newSkill = gameController.SkillGenerator.Generate(newSkillName);
        if (skills.Count < MaxSkillNum)
        {
            if (!skills.Contains(newSkill))
            {
                skills.Add(newSkill);
            }
            else
            {

            }
        }
    }

    //プレイヤーのレベルアップ内容
    //基本は三つ
    public List<LvUpInformation> GetLvUpBonusOptionsOfPlayer()
    {
        return null;
    }

    /// <summary>
    /// NPCのレベルアップ内容を自動的に決定
    /// 
    /// </summary>
    /// <returns></returns>
    public LvUpInformation GetLvUpBonusAutomatically()
    {
        LvUpInformation info = new LvUpInformation();

        int uppedHP = lvUpHps.GetAtRandom();
        int uppedSP = LvUpSps.GetAtRandom();

        info.UppedHP = uppedHP;
        info.UppedSP = uppedSP;

        int random = UnityEngine.Random.Range(0, 100);

        //Skill skill = GetSkillRandomly();
        var skill = LvUpSkillRandomly();

        //40%でスキルを覚える
        //ステータスアップ
        if (random > statusUpPer || skill == null)
        {
            info.StatusUp = GetLvUpStatusRandomly();
        }
        //スキル獲得
        else
        {
            info.Skill = skill;
        }

        return info;
    }

    //ランダムにひとつ選ぶ
    //コストを追加
    public Skill GetSkillRandomly()
    {
        var skillOptions = new List<Skill>();
        foreach (Skill s in gameController.SkillGenerator.SkillInfos)
        {
            if (s.JobType == Job)
            {
                if (!skills.Contains(s))
                {
                    Skill skill = gameController.SkillGenerator.Generate(s.Name);
                    skillOptions.Add(skill);
                }
            }
        }
        if (skillOptions.Count > 0)
        {
            Skill acquired = skillOptions.GetAtRandom();
            return acquired;
        }
        else
        {
            return null;
        }
    }


    public Skill LvUpSkillRandomly()
    {
        var skillOptions = new List<Skill>();
        foreach (Skill s in gameController.SkillGenerator.SkillInfos)
        {
            if (s.JobType == Job)
            {
                if (skills.Contains(s))
                {
                    var learned = skills.Find(sk => sk == s);
                    if (learned != null && learned.SkillLevel > 2)
                    {
                        break;
                    }
                }
                Skill skill = gameController.SkillGenerator.Generate(s.Name);
                skillOptions.Add(skill);
            }
        }
        if (skillOptions.Count > 0)
        {
            Skill acquired = skillOptions.GetAtRandom();
            return acquired;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    ///　ランダムにステータスを選ぶ
    ///　基本上昇値は２
    ///　後で設計しとく
    /// </summary>
    /// <returns></returns>
    public (Status status, int value) GetLvUpStatusRandomly()
    {
        int uppedPoint = 2;
        List<Status> options = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
        options.Remove(Status.Lv);
        options.Remove(Status.MaxHP);
        options.Remove(Status.MaxSP);
        options.Remove(Status.currentHP);
        options.Remove(Status.currentSP);
        Status status = options.GetAtRandom();
        var lvUpStatus = (status, uppedPoint);
        return lvUpStatus;
    }


    public void HealAll(bool isEffectOn = false)
    {
        SetDamage(-999, isEffectOn = false);
        Statuses[Status.currentSP] = Statuses[Status.MaxSP];
        gameController.AllyManager.SpDamageEffect(this);
        //Statuses[Status.currentSP] = Statuses[Status.MaxSP];
    }

    //装備を合わせた物理攻撃力
    public override int GetOffensivePower()
    {
        int lv = Statuses[Status.Lv];
        int str = Statuses[Status.STR];
        double attack;
        attack = (str * 1.5 + str * lv * 0.020 +
            EquippedWeapon.GetAddedValue());
        return (int)attack;
    }

    //装備を合わせた物理攻撃力
    //スキルを追加
    public override int GetMagicalOffensivePower()
    {
        int lv = Statuses[Status.Lv];
        int intel = Statuses[Status.INT];
        double attack;
        attack = intel * 2.0 + intel * lv * 0.020;
        return (int)attack;
    }

    //装備を合わせた物理防御力
    public override int GetPhysicalDefensivePower()
    {
        int lv = Statuses[Status.Lv];
        int def = Statuses[Status.DEF];
        int equipDef = 0;
        foreach (Guard g in equippedGuards)
        {
            equipDef += g.GetAddedValue();
        }
        return (int)(def * (1.2 + lv * 0.020) + equipDef);
    }

    //装備を合わせた魔法防御力
    public override int GetMagicalDefensivePower()
    {
        
        int lv = Statuses[Status.Lv];
        int intel = Statuses[Status.INT];
        //int mnt = GetStatus(EnumHolder.Status.MNT);
        int def = Statuses[Status.DEF];
        int equipDef = 0;
        foreach (Guard g in equippedGuards)
        {
            equipDef += g.GetAddedValue();
        }

        int damage = (int)((def * 0.3 + intel * 0.7) * (1.1 + lv * 0.020) + equipDef);

        if (Buffs.Exists(s => s != null && s.Skill.SkillName == SkillName.魔法ガード ))
        {
            Buff buff = Buffs.Find(s => s.Skill.SkillName == SkillName.魔法ガード);
            Skill skill = buff.Skill;
            damage = (int)(damage * ((100 - skill.GetSkillPower()) / 100));
        }

        return damage;
    }

    //装備を合わせた命中
    public override int GetHitProb()
    {
        int lv = Statuses[Status.Lv];
        int tec = Statuses[Status.TEC];
        int agi = Statuses[Status.AGI];
        //int luk = GetStatus(EnumHolder.Status.LUK);
        //int hitProb = (int)((tec * 2 + agi + luk) * (0.8) + EquippedWeapon.HitAboidProb);
        int value = (int)((tec * 0.7 + agi * 0.3) * 1.5);
        int hitProb = (int)(value + (value * lv * 0.020) + EquippedWeapon.HitAboidProb);
        //int hitProb = (int)((tec * 0.7 + agi * 0.3) / 1.5 + EquippedWeapon.HitAboidProb);

        if (Buffs.Exists(s => s != null && s.Skill.SkillName == SkillName.狙いすまし))
        {
            Buff buff = Buffs.Find(s => s.Skill.SkillName == SkillName.狙いすまし);
            Skill skill = buff.Skill;
            hitProb = (int)(hitProb * ((100 + skill.GetSkillPower()) / 100));
        }

        return hitProb;
    }

    //装備を合わせた回避
    public int GetAvoidProb()
    {
        //int lv = GetStatus(StatusKey.Lv);
        int lv = Statuses[Status.Lv];
        int tec = Statuses[Status.TEC];
        int agi = Statuses[Status.AGI];
        //int luk = GetStatus(EnumHolder.Status.LUK);
        int guardsProb = 0;
        foreach (Guard g in equippedGuards)
        {
            guardsProb += g.HitAboidProb;
        }
        int value = (int)((agi) * 1.5);
        int hitProb = (int)(value + (value * lv * 0.020) + guardsProb);
        return hitProb;
    }

    public Dictionary<Status, int> GetLvUppedStatuses(int num)
    {
        int random;
        Dictionary<Status, int> uppedStatus = new Dictionary<Status, int>()
        {
            { EnumHolder.Status.Lv, 0},
            { EnumHolder.Status.MaxHP, 0},
            { EnumHolder.Status.MaxSP, 0},
            { EnumHolder.Status.STR, 0},
            { EnumHolder.Status.DEF, 0},
            { EnumHolder.Status.INT, 0},
            { EnumHolder.Status.MNT, 0},
            { EnumHolder.Status.TEC, 0},
            { EnumHolder.Status.AGI, 0},
            { EnumHolder.Status.LUK, 0}
        };

        uppedStatus[EnumHolder.Status.Lv] = num;

        for (int i = 0; i < num; i++)
        {
            //            if (this.Race == Race.人間)
            if (true)
            {
                //HP,SPの成長
                random = UnityEngine.Random.Range(0, lvUpHps.Count - 1);
                uppedStatus[Status.MaxHP] += lvUpHps[random];

                random = UnityEngine.Random.Range(0, lvUpSps.Count - 1);
                uppedStatus[Status.MaxSP] += lvUpSps[random];

                //1レベルで2上昇
                for (int j = 0; j < 2; j++)
                {
                    random = UnityEngine.Random.Range(0, 100);

                    int sum = 0;
                    List<(Status, int)> nums = new List<(Status, int)>();

                    foreach (KeyValuePair<Status, int> kvp in InitialStatuses)
                    {
                        if (kvp.Key != Status.Lv && kvp.Key != Status.MaxHP &&
                            kvp.Key != Status.MaxSP && kvp.Key != Status.currentHP && kvp.Key != Status.currentSP)
                        {
                            sum += kvp.Value;
                            nums.Add((kvp.Key, kvp.Value));
                        }
                    }

                    int currentSum = 0;
                    random = UnityEngine.Random.Range(1, sum);


                    foreach ((Status s, int n) in nums)
                    {
                        currentSum += n;
                        if (currentSum > random)
                        {
                            uppedStatus[s] += 1;
                            break;
                        }
                    }

                }
            }
        }

        return uppedStatus;

    }

    //public override List<Item> GetItems()
    //{
    //    return Items;
    //}

    //TODO:直す
    public void AddItem(Item item)
    {

        for (int i = 0; i < 1000; i++)
        {
            string newName = item.ID + i.ToString();
            if (!Items.Exists(x => x.ID == newName))
            {
                item.ID = newName;
                break;
            }
        }

        this.Items.Add(item);
    }

    public void DiscardItems(List<Item> discardItems)
    {
        foreach (Item i in discardItems)
        {
            Items.Remove(i);
            Debug.Log("");
        }
    }

    public void SetItem(List<Item> items)
    {
        this.Items = new List<Item>(items);
    }

    public void SetSkills(List<Skill> skills)
    {
        this.skills = new List<Skill>(skills);
    }
}
