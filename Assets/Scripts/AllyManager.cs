using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using static EnumHolder;
using System;
using System.Linq;

public class AllyManager : MonoBehaviour {
    
    private GameObject[] allyPanels;

    private List<Item> items;

    private GameController gameController;

    private Transform selectWhoPanel;
    public Transform inoperablePanelUnderAlly;
    public Transform inoperablePanelUnderEquippment;
    public Transform selectGuardPanel;

    public Transform infoPanel;
    private Transform buttonObject;
    TextMeshProUGUI topText;

    TextMeshProUGUI infoText;
    Button okButton;

    public List<Ally> Allies = new List<Ally>();

    private List<Item> restItems = new List<Item>();
    private Ally sampleAlly;

    [HideInInspector]
    private string selectedAllyID = "";

    private Guard removedGuard;
    private Guard addedGuard;

    [HideInInspector]
    private bool isSelectable = false;
    private bool isSelectingAlly = false;

    private bool isSelectingGuard = false;

    //アイテムが一杯か
    private bool isFull = false;

    private bool isOrdering = false;

    private bool[] isLevelUpped = {false, false, false};
    private bool isClicked = false;


    private List<Dictionary<Status, int>> uppedStatuses = new List<Dictionary<Status, int>>();

    public Camera camera_object; //カメラを取得
    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物

    GameObject clickedGameObject;

    public bool IsSelecting { get => isSelectingAlly; set => isSelectingAlly = value; }
    public bool IsOrdering { get => isOrdering; set => isOrdering = value; }
    public bool[] IsLevelUpped { get => isLevelUpped; set => isLevelUpped = value; }
    public string SelectedAllyID { get => selectedAllyID; set => selectedAllyID = value; }
    public Transform SelectWhoPanel { get => selectWhoPanel; set => selectWhoPanel = value; }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        allyPanels = gameController.AllyPanels;

        InitComponents();
        CreateSampleParty();
        CreatePanels();
    }


    //============================================================
    //初期設定
    //============================================================


    public void InitComponents()
    {

    }

    public void CreateSampleParty()
    {
        Ally ally4 = new Ally(gameController);

        ally4 = gameController.AllyGenerator.Generate("トーマス", "人間", "warrior", gameController);
        Skill skill = gameController.SkillGenerator.Generate("なぎ払い");
        ally4.AddSkill(skill);
        Allies.Add(ally4);
        Weapon wp = gameController.ItemGenerator.GenerateWeapon("シャランガ");
        ally4.Items.Add(wp);
        SetWeapon(ally4, wp);
        Guard g = gameController.ItemGenerator.GenerateGuard("アイアンシールド");
        ally4.Items.Add(g);
        SetGuard(ally4, g);
        Guard g1 = gameController.ItemGenerator.GenerateGuard("ヒスイのイヤリング");
        ally4.Items.Add(g1);
        SetGuard(ally4, g1);

        Item item = gameController.ItemGenerator.Generate("薬草");
        Item item1 = gameController.ItemGenerator.Generate("薬草");
        Item item2 = gameController.ItemGenerator.Generate("薬草");
        Item item3 = gameController.ItemGenerator.Generate("薬草");

        ally4.AddItem(item);
        ally4.AddItem(item1);
        ally4.AddItem(item2);
        ally4.AddItem(item3);

        Ally ally5 = gameController.AllyGenerator.Generate("エミー", "エルフ", "mage", gameController);
        skill = gameController.SkillGenerator.Generate("ファイアボール");
        ally5.skills.Add(skill);
        skill = gameController.SkillGenerator.Generate("毒の粉");
        ally5.skills.Add(skill);
        skill = gameController.SkillGenerator.Generate("ヒール");
        ally5.skills.Add(skill);
        Allies.Add(ally5);

        Ally ally6 = gameController.AllyGenerator.Generate("アドン", "デビル", "cleric", gameController);
        skill = gameController.SkillGenerator.Generate("パワースマッシュ");
        ally6.skills.Add(skill);
        skill = gameController.SkillGenerator.Generate("ファイアボール");
        ally6.skills.Add(skill);
        Allies.Add(ally6);


        for(int i = 0; i < Allies.Count; i++)
        {
            Allies[i].AllyPanel = allyPanels[i];
        }

    }

    //============================================================
    //ゲームオブジェクトの設定
    //============================================================

    public void CreatePanels()
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < Allies.Count)
            {
                Allies[i].SetPanel();
            }
            else
            {
                SetNullAlly(allyPanels[i]);
            }
        }

        //HideAllyPanel(isClear: true);
    }

    public void SetNullAlly(GameObject g)
    {
        TextMeshProUGUI lv = g.transform.Find("Status/Lv").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maxHP = g.transform.Find("Status/MaxHP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI currentHP = g.transform.Find("Status/CurrentHP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI maxSP = g.transform.Find("Status/MaxSP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI currentSP = g.transform.Find("Status/CurrentSP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI allyName = g.transform.Find("Status/Name").GetComponent<TextMeshProUGUI>();

        Image background = g.transform.Find("BackGround").GetComponent<Image>();
        Utility.SetBackGroundImageColor(background, Color.white, 0.3f);

        maxHP.text = "";
        currentHP.text = "";
        maxSP.text = "";
        currentSP.text = "";
        allyName.text = "";
    }

    public void UpdatePanels()
    {
        for (int i = 0; i < Allies.Count; i++)
        {
            Allies[i].SetPanel();

            float perHP = ((Allies[i].Statuses[Status.currentHP])
                / Allies[i].Statuses[Status.MaxHP]);

            if (perHP < 0) perHP = 0f;
            if (perHP > 1) perHP = 1.0f;

            float perSP = perSP = ((Allies[i].Statuses[Status.currentHP])
                / Allies[i].Statuses[Status.MaxSP]);

            if (perSP < 0) perSP = 0f;
            if (perSP > 1) perSP = 1.0f;

            Transform bar = allyPanels[i].transform.Find("Status/MaxHPBar/CurrentHPBar");
            //ＨＰバーの動き
            Vector3 scale = new Vector3(perHP, 1, 1);
            bar.DOScale(scale, 0.2f);

            //Transform bar2 = allyPanels[i].transform.Find("Status/MaxSPBar/CurrentSPBar");
            ////ＨＰバーの動き
            //Vector3 scale2 = new Vector3(perSP, 1, 1);
            //bar.DOScale(scale2, 0.2f);
        }
    }

    //戦闘時、現在の仲間を選択するときに使う
    public void HideAllyPanel(bool isClear = false, int index = 99)
    {
        for (int i = 0; i < allyPanels.Length; i++)
        {
            Transform hidePanel = allyPanels[i].transform.Find("HidePanel");
            Image image = hidePanel.GetComponent<Image>();

            if (i == index || isClear)
            {
                Utility.SetBackGroundImageColor(image, Color.white, 0f);
            }
            else
            {
                Utility.SetBackGroundImageColor(image, Color.black, 0.5f);
            }
        }
    }


    public List<Item> GetItems()
    {
        return Allies[0].Items;
    }


    //============================================================
    //
    //============================================================

    public void CheckLvUpOfPlayer()
    {

    }

    public void CheckLvUpOfNPC()
    {
        for (int i = 0; i < Allies.Count; i++)
        {
            if (Allies[i].UppedLv > 0)
            {
                string message = Allies[i].ConsumeLvUpStack();
                gameController.SetInfo("レベルアップ情報", message);
                Allies[i].HealAll();
                UpdatePanels();
                break;
            }
        }
    }

    public void CheckIsFullOfItems()
    {

    }

    public void AddItemToLeader(Item item)
    {

    }

    /// <summary>
    /// 仲間を選択
    /// </summary>
    public void CheckIsClickedAlly()
    {
        GameObject result = null;
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d && collition2d.transform.tag == "Ally")
            {
                string selectedId = collition2d.transform.gameObject.name;
                SelectedAllyID = selectedId;
            }
            else
            {
                //仲間パネル以外が選択された
                this.SelectedAllyID = "";
                IsSelecting = false;
            }
        }
    }

    public void SetSelectable(bool b)
    {
        isSelectable = b;
    }

    //全滅チェック
    public bool CheckAllDead()
    {
        bool isAllDead = true;
        foreach (Ally ally in Allies)
        {
            if (!ally.IsDeath)
            {
                isAllDead = false;
                break;
            }
        }
        return isAllDead;
    }

    public void AddItem(Item item)
    {
        List<Item> items = new List<Item>(){item};
        AddItems(items);
    }

    public void RemoveItem(Item item)
    {
        List<Item> items = Allies[0].Items;
        if (item.isConsumable) items.Remove(items.Find(i => i.ID == item.ID));
    }

    //アイテム追加---------------------------------------------------------
    public void AddItems(List<Item> requiredItems)
    {
        Debug.Log("sdfads");
        List<Item> requiredTemp = new List<Item>(requiredItems);

        int count = 0;
        foreach (Ally ally in Allies)
        {
            foreach (Item item in requiredItems)
            {
                if (ally.Items.Count < ally.MaxItemNum)
                {
                    ally.AddItem(item);
                    //ally.itemList.Add(item);
                    count++;
                    requiredTemp.Remove(item);
                }
                else
                {
                    requiredItems = new List<Item>(requiredTemp);
                    break;
                }
                requiredItems = new List<Item>(requiredTemp);
            }
        }
        if (requiredTemp.Count > 0)
        {
            restItems = requiredTemp;
            isFull = true;
        }
        else
        {
            isFull = false;
        }

    }
    

    public void AddExps(int exp)
    {
        for (int i = 0; i < Allies.Count; i++)
        {
            Allies[i].AddExp(exp);
        }
        //isClicked = true;
    }

    public string UseItemInField(Item item, List<Unit> unitList, Ally ally)
    {
        string message = "";

        //装備品なら
        if (item is Equipment)
        {
            Equipment eq = item as Equipment;
            message += Equip(eq);
        }
        else
        {
            var units = Utility.CastAlliesToUnits(Allies);
            message += gameController.ItemEffecor.UseInField(item, units, ally);
        }

        UpdatePanels();
        SelectedAllyID = "";
        return message;
    }

    //装備品着脱-----------------------------------------------------------
    public string Equip(Equipment eq)
    {
        Debug.Log(eq.EquippedAllyID);
        string message = "";
        Ally selectedAlly = Allies.Find(a => a.ID1 == SelectedAllyID);
        //装備する
        if (string.IsNullOrEmpty(eq.EquippedAllyID))
        {
            if (eq.ItemType == ItemType.weapon)
            {
                message += SetWeapon(selectedAlly, eq as Weapon);
            }
            else if (eq.ItemType == ItemType.guard)
            {
                message += SetGuard(selectedAlly, eq as Guard);
            }

        }
        //外す
        else
        {
            if (eq.ItemType == ItemType.weapon)
            {
                message += ReleaseWeapon(eq as Weapon);
            }
            else if (eq.ItemType == ItemType.guard || eq.ItemType == ItemType.accessary)
            {
                message += ReleaseGuard(eq as Guard);
            }
        }
        return message;
    }

    public string EquipToLeader(Equipment eq)
    {
        string message = "";
        Ally ally = Allies[0];
        //装備する
        if (string.IsNullOrEmpty(eq.EquippedAllyID))
        {
            if (eq.ItemType == ItemType.weapon)
            {
                message += SetWeapon(ally, eq as Weapon);
            }
            else if (eq.ItemType == ItemType.guard)
            {
                message += SetGuard(ally, eq as Guard);
            }

        }
        //外す
        else
        {
            if (eq.ItemType == ItemType.weapon)
            {
                message += ReleaseWeapon(eq as Weapon);
            }
            else if (eq.ItemType == ItemType.guard)
            {
                message += SetGuard(ally, eq as Guard);
            }
        }
        return message;
    }

    public string SetWeapon(Ally ally, Weapon weapon)
    {
        string message = "";
        //装備がないなら
        if (ally.EquippedWeapon.ID == "nothing")
        {
            ally.EquippedWeapon = weapon;
        }
        //装備しているなら、装備を外してから
        else
        {
            ReleaseWeapon(weapon);
            ally.EquippedWeapon = weapon;
        }
        weapon.EquippedAllyID = ally.ID1;
        message += $"{ally.Name}は{weapon.GetPlusedName()}を装備した。";
        return message;
    }

    public string SetGuard(Ally ally, Guard guard)
    {
        string message = "";

        if (guard.GuardType == GuardType.armor)
        {
            Debug.Log(guard);
            if (ally.EquippedArmor.ID == "nothing")
            {
                ally.EquippedArmor = guard;
            }
            //装備しているなら、装備を外してから
            else
            {
                ReleaseGuard(guard);
                ally.EquippedArmor = guard;
            }
        }
        else if (guard.GuardType == GuardType.accessory)
        {
            Debug.Log(guard);
            if (ally.EquippedAccessory.ID == "nothing")
            {
                ally.EquippedAccessory = guard;
            }
            //装備しているなら、装備を外してから
            else
            {
                ReleaseGuard(guard);
                ally.EquippedAccessory = guard;
            }
        }
        else
        {
            Debug.Log("something error");
        }
        guard.EquippedAllyID = ally.ID1;
        message += $"{ally.Name}は{guard.GetPlusedName()}を装備した。";
        return message;

    }

    public string ReleaseWeapon(Weapon weapon)
    {
        string message = "";
        Ally ally = Allies.Find(a => a.ID1 == weapon.EquippedAllyID);
        Weapon w = new Weapon();
        //空のオブジェクトを装備
        ally.EquippedWeapon = w;

        weapon.EquippedAllyID = "";
        message += $"{ally.Name}は{weapon.GetPlusedName()}を外した。";
        return message;
    }

    public string ReleaseGuard(Guard guard)
    {
        string message = "";
        Ally ally = Allies.Find(a => a.ID1 == guard.EquippedAllyID);
        //ally.EquippedGuards.Remove(guard);
        Guard g = new Guard(guardType:guard.GuardType);
        if (guard.IsArmor())
        {
            ally.EquippedArmor = g;
        }
        else
        {
            ally.EquippedAccessory = g;
        }
        guard.EquippedAllyID = "";
        message += $"{ally.Name}は{guard.GetPlusedName()}を外した。";
        return message;
    }

    //public void OnClickGuard(Guard g)
    //{
    //    removedGuard = g;
    //}

    //TODO:アイテム整理
    //public void OnClick()
    //{
    //    infoPanel.gameObject.SetActive(false);
    //    isClicked = true;
    //}

    //public void OnClickLvUpDialog()
    //{
    //    infoPanel.gameObject.SetActive(false);
    //    isClicked = true;
    //}

    public void SpDamageEffect(Unit unit)
    {
        Ally ally = unit as Ally;
        Transform allyPanel;

        int index = Allies.IndexOf(ally);

        allyPanel = allyPanels[index].transform;

        Transform bar = allyPanels[index].transform.Find("Status/MaxSPBar/CurrentSPBar");

        float perSP = ally.GetPerSP();

        //ＨＰバーの動き
        Vector3 scale = new Vector3(perSP, 1, 1);
        bar.DOScale(scale, 0.3f);

    }

    public void DamageEffect(Unit unit, int damage, bool hit = true)
    {
        Ally ally = unit as Ally;
        Transform allyPanel;

        int index = Allies.IndexOf(ally);

        allyPanel = allyPanels[index].transform;

        TextMeshProUGUI text = allyPanels[index].transform.Find("Status/Damage").GetComponent<TextMeshProUGUI>();
        CanvasGroup canvas = text.GetComponent<CanvasGroup>();

        canvas.alpha = 1;
        Vector3 pos = text.transform.position;
        //text.transform.position = new Vector3(pos.x, 0, pos.y);
        Sequence seq = DOTween.Sequence();

        //TODO:直す
        Transform bar = allyPanels[index].transform.Find("Status/MaxHPBar/CurrentHPBar");

        float perHP = ally.GetPerHP();

        if (hit)
        {
            //ＨＰバーの動き
            Vector3 scale = new Vector3(perHP, 1, 1);
            bar.DOScale(scale, 0.3f);

            if (damage > 0)
            {
                text.text = "-" + (damage).ToString();
                allyPanel.DOShakePosition(0.5f, strength: 30, vibrato: 50);

                text.color = new Color(0.78f, 0.06f, 0f, 1f);
                seq.Append(text.transform.DOShakePosition(0.5f, strength: 30, vibrato: 50).SetEase(Ease.OutCirc)
                    .OnStart(() => { text.transform.position = new Vector3(pos.x, 0, pos.y); }));
                seq.Append(text.transform.DOLocalMoveY(200, 2f));
                seq.Join(canvas.DOFade(0, 0.5f));
            }
            else
            {
                text.text = "+" + (-damage).ToString();
                text.color = new Color(0.48f, 0.76f, 0f, 1f);
                seq.SetDelay(0.5f);
                seq.Append(text.transform.DOLocalMoveY(200, 2f));
                seq.Join(canvas.DOFade(0, 0.5f)).OnComplete(() => { text.transform.position = new Vector3(pos.x, 0, pos.y); });
            }
        }
        else
        {
            text.text = "miss!!";
            text.color = new Color(1f, 1f, 1f, 1f);
            seq.Append(text.transform.DOLocalMoveY(200, 2f).OnStart(() => { text.transform.position = new Vector3(pos.x, 0, pos.y); }));
            seq.Join(canvas.DOFade(0, 0.5f));
        }


    }

    public void AddAlly()
    {

    }

}
