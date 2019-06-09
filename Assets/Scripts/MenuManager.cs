using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;
using UnityEngine.EventSystems;
using static EnumHolder;
using System;
using static Utility;

public class MenuManager : MonoBehaviour
{

    private AllyManager allyManager;
    private MessageManager messageManager;
    private GameController gameController;

    private ItemEffecor itemEffector;

    public Transform mainMenu;
    public Transform detailedMenu;
    public Transform itemButtonObject;

    public Transform infoPanel;

    //新メニュー
    public Transform menuPanel;
    public Transform itemPanel;
    public Transform allyPanel;
    public Transform bookPanel;
    public Transform settingPanel;

    //public Transform itemSelectingPanel;

    public Sprite emptySlot;
    public Sprite plusSlot;
    public Sprite minusSlot;

    private Button selectStatusButton;
    private Button selectSkillButton;
    private Button selectItemButton;
    private Button selectSettingsButton;
    private Button closeButton;

    private Transform statusPanel;

    private Transform skillPanel;
    private Transform skillContentPanel;
    private Transform skillDetail;

    private Transform underToggles;
    private Transform allyTogglesParentInSkill;
    private Transform allyTogglesParentInEquipment;

    private Transform equipmentPanel;
    private Transform equipContentPanel;


    private Transform gemDescription;
    //private TextMeshProUGUI[] skillTexts;

    private Transform multiSelecting;
    private Transform itemContentPanel;
    private Transform itemDetail;
    private TextMeshProUGUI[] itemTexts;
    private List<Transform> itemButtonObjects = new List<Transform>();


    private List<Transform> skillButtonPanels = new List<Transform>();

    private string preSelectedToggle = "";
    private string currentSelectedToggle = "";
    //private Button[] itemButtons;
    //private Transform settingPanel;

    List<Ally> allies;
    private int allyNum = 0;

    private bool isOrderingItems = false;

    private bool isMultiSelecting = false;

    private bool isUsingItem = false;

    private bool isDiscardingItem = false;

    private string yesNo = "";

    private string selectedItemID = "";
    Item selectedItem = new Item();
    private List<string> selectedItemIDs = new List<string>();
    private enum ItemDetailCommand { nothing, use, description, pass, discard };
    private ItemDetailCommand itemDetailCommand = ItemDetailCommand.nothing;

    private Skill selectedSkill;

    private enum CurrentWindow { status, skill, item, book, settings }
    private CurrentWindow currentWindow = CurrentWindow.status;

    private int selectedMenuNo = 0;

    private ItemDetailCommand ItemDetailCommand1 { get => itemDetailCommand; set => itemDetailCommand = value; }
    public string SelectedItemID { get => selectedItemID; set => selectedItemID = value; }
    public Item SelectedItem { get => selectedItem; set => selectedItem = value; }
    public Skill SelectedSkill { get => selectedSkill; set => selectedSkill = value; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        this.allyManager = gameController.AllyManager;
        this.messageManager = gameController.MessageManager;
        itemEffector = gameController.ItemEffecor;

        this.InitComponents();

        SetListeners();

        allies = allyManager.Allies;


        //gemDescription = statusPanel.Find("GemDescription");

    }



    //============================================================
    //初期設定
    //============================================================


    public void InitComponents()
    {
        statusPanel = allyPanel.Find("StatusPanel");
        skillPanel = allyPanel.Find("SkillPanel");
        equipmentPanel = allyPanel.Find("EquipmentPanel");

        allyTogglesParentInEquipment = equipmentPanel.Find("ParentPanel/AllyToggles");
        allyTogglesParentInSkill = skillPanel.Find("ParentPanel/AllyToggles");

        itemContentPanel = itemPanel.Find("ScrollView/Viewport/Content");
        skillContentPanel = skillPanel.Find("ParentPanel/Skills/ScrollView/Viewport/Content");
        equipContentPanel = equipmentPanel.Find("ParentPanel/Equipments/ScrollView/Viewport/Content");

        //ステータス、スキル、装備のtoggle
        underToggles = allyPanel.Find("Toggles");
        //toggleのリスナー設定
        ResetUnderToggles(true);

        foreach (Transform t in skillContentPanel)
        {
            skillButtonPanels.Add(t);
        }

        foreach (Transform t in itemContentPanel)
        {
            itemButtonObjects.Add(t);
        }

        skillDetail = skillPanel.Find("SkillDetail");

        Button useSkillButton = skillDetail.Find("Panel/SelectButtonPanel/Use").GetComponent<Button>();
        useSkillButton.onClick.AddListener(() => OnClickUseSkill());
    }

    public void SetListeners()
    {

        foreach (Transform t in allyTogglesParentInEquipment)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((x) => OnClickToggle(toggle));
        }

        foreach (Transform t in allyTogglesParentInSkill)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((x) => OnClickToggle(toggle));
        }
    }


    //============================================================
    //基本メソッド
    //============================================================
    //アイテム使用後
    public void CloseMenu()
    {
        allyPanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
        ResetUnderToggles();
    }

    public void ResetUnderToggles(bool isInit = false)
    {
        foreach (Transform t in menuPanel)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            TextMeshProUGUI text = t.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.white;
            toggle.isOn = false;
            if (isInit)
            {
                toggle.onValueChanged.AddListener(DisplayMenu);
            }
        }

        foreach (Transform t in underToggles)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            if (isInit)
            {
                toggle.isOn = false;
                toggle.onValueChanged.AddListener((x) => OnClickToggle(toggle));
            }
        }
    }

    public void OnClickEmpty()
    {

    }

    public void OnClickToggle(Toggle toggle)
    {
        SetAllyPanel();
    }




    //============================================================
    //ウィンドウの挙動
    //============================================================


    //メニューをタップ
    public void DisplayMenu(bool b)
    {

        itemPanel.gameObject.SetActive(false);
        allyPanel.gameObject.SetActive(false);
        bookPanel.gameObject.SetActive(false);
        settingPanel.gameObject.SetActive(false);

        foreach (Transform t in menuPanel)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            TextMeshProUGUI text = toggle.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.white;
        }

        foreach (Transform t in menuPanel)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            TextMeshProUGUI text = toggle.transform.GetComponentInChildren<TextMeshProUGUI>();
            if (toggle.isOn)
            {
                text.color = Color.yellow;
                if (t.name == "Ally")
                {
                    Debug.Log("ally");
                    SetAllyPanel();
                    allyPanel.gameObject.SetActive(true);
                }
                else if (t.name == "Item")
                {
                    Debug.Log("item");
                    SetItemWindow();
                    itemPanel.gameObject.SetActive(true);
                }
                else if (t.name == "Book")
                {
                    Debug.Log("book");
                    bookPanel.gameObject.SetActive(true);
                }
                else if (t.name == "Setting")
                {
                    Debug.Log("setting");
                    settingPanel.gameObject.SetActive(true);
                }
            }

        }

    }

    public void SetAllyPanel()
    {

        currentWindow = CurrentWindow.status;

        bool isInitial = true;

        statusPanel.gameObject.SetActive(false);
        skillPanel.gameObject.SetActive(false);
        equipmentPanel.gameObject.SetActive(false);

        TextMeshProUGUI topText = allyPanel.Find("TopText").GetComponent<TextMeshProUGUI>();

        string currentPanel = "Status";

        foreach (Transform t in underToggles)
        {
            Toggle toggle = t.GetComponent<Toggle>();
            TextMeshProUGUI text = toggle.GetComponentInChildren<TextMeshProUGUI>();
            if (toggle.isOn)
            {
                currentPanel = toggle.name;
                text.color = Color.white;
                isInitial = false;
            }
            else
            {
                text.color = Color.gray;
            }
        }

        if (isInitial)
        {
            Toggle initialToggle = underToggles.Find("Status").GetComponent<Toggle>();
            TextMeshProUGUI initialText = initialToggle.GetComponentInChildren<TextMeshProUGUI>();
            initialToggle.isOn = true;
            initialText.color = Color.white;
        }

        if (currentPanel == "Status")
        {
            statusPanel.gameObject.SetActive(true);
            topText.text = "ステータス一覧";
            SetStatusPanel();
        }
        else if (currentPanel == "Skill")
        {
            skillPanel.gameObject.SetActive(true);
            topText.text = "スキル一覧";
            SetSkillPanel();
        }
        else if (currentPanel == "Equipment")
        {
            equipmentPanel.gameObject.SetActive(true);
            topText.text = "装備品一覧";
            SetWeaponPanel();
        }

    }

    public void SetStatusPanel()
    {

        //currentWindow = CurrentWindow.status;

        Ally ally;
        List<Ally> allies = allyManager.Allies;

        Debug.Log(allies.Count());

        Transform contents = statusPanel.Find("ScrollView/Viewport/Content");

        //ステータス画面の設定
        for (int i = 0; i < 3; i++)
        {
            string name = "Status" + (i + 1).ToString();
            Transform statusContent = contents.Find(name);

            if (allies.Count() < i + 1)
            {
                statusContent.gameObject.SetActive(false);
                break;
            }
            else
            {
                ally = allies[i];

                statusContent.gameObject.SetActive(true);

                TextMeshProUGUI nameEtc = statusContent.Find("Name").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI level = statusContent.Find("Lv").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI exp = statusContent.Find("Exp").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI status = statusContent.Find("Status").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI equipments = statusContent.Find("Equipments").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI equippedStatus = statusContent.Find("EquippedStatus").GetComponent<TextMeshProUGUI>();

                nameEtc.text = $"{ally.Name}\n"
                    + $"{ally.Race.GetRaceName()}\n"
                    + $"{ally.Job.GetJobName()}";

                level.text = $"{ally.Statuses[Status.Lv]}";

                exp.text = $"{ally.ExpSum}\n{ally.NextLvExp}";

                status.text = $"{ally.Statuses[Status.currentHP]}/{ally.Statuses[Status.MaxHP]}\n"
                    + $"{ally.Statuses[Status.currentSP]}/{ally.Statuses[Status.MaxSP]}\n"
                    + $"{ally.Statuses[Status.STR]}\n"
                    + $"{ally.Statuses[Status.DEF]}\n"
                    + $"{ally.Statuses[Status.INT]}\n"
                    //+ $"{ally.Statuses[Status.MNT]}\n"
                    + $"{ally.Statuses[Status.TEC]}\n"
                    + $"{ally.Statuses[Status.AGI]}\n";
                    //+ $"{ally.Statuses[Status.currentHP]}\n";

                string equipment = "";

                if (ally.EquippedWeapon.ID != "nothing")
                {
                    equipment += $"武器:{ally.EquippedWeapon.Name}\n";
                }
                else
                {
                    equipment += $"武器:無し\n";
                }

                if (ally.EquippedArmor.ID != "nothing")
                {
                    equipment += $"防具:{ally.EquippedArmor.Name}\n";
                }
                else
                {
                    equipment += $"防具:無し\n";
                }

                if (ally.EquippedAccessory.ID != "nothing")
                {
                    equipment += $"装飾品:{ally.EquippedAccessory.Name}\n";
                }
                else
                {
                    equipment += $"装飾品:無し\n";
                }
                equipments.text = equipment;

                equippedStatus.text = $"{ally.GetOffensivePower()}\n"
                    + $"{ally.GetPhysicalDefensivePower()}\n"
                    //+ $"{ally.GetMagicalDefensivePower()}\n"
                    + $"{ally.GetHitProb()}\n"
                    + $"{ally.GetAvoidProb()}\n";
            }
        }
    }

    //>仲間>スキル 選択時
    public void SetSkillPanel()
    {
        skillDetail.gameObject.SetActive(false); ;

        currentWindow = CurrentWindow.skill;

        Ally ally;
        bool isInitialSkill = true;

        SetAllyToggle(allyTogglesParentInSkill);

        for (int i = 0; i < 3; i++)
        {
            string name = "Ally" + (i + 1).ToString();
            Transform transform = allyTogglesParentInSkill.Find(name);
            Toggle toggle = transform.GetComponent<Toggle>();
            TextMeshProUGUI text = transform.GetComponentInChildren<TextMeshProUGUI>();

            if (toggle.isOn)
            {
                ally = allies[i];
                //スキル一覧をセット
                for (int j = 0; j < 8; j++)
                {
                    Button button = skillButtonPanels[j].Find("Button").GetComponent<Button>();
                    Image backGround = skillButtonPanels[j].Find("Button").GetComponent<Image>();
                    TextMeshProUGUI skillNameText = skillButtonPanels[j]
                        .Find("Button/SkillName").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI description = skillButtonPanels[j]
                        .Find("Button/Description").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI sp = skillButtonPanels[j]
                        .Find("Button/SP").GetComponent<TextMeshProUGUI>();
                    Transform whiteLine = skillButtonPanels[j].Find("Button/WhiteLine");

                    backGround.color = new Color(0.0f, 0.0f, 0.0f, 0.5f);
                    skillNameText.text = "";
                    description.text = "";
                    sp.text = "";
                    whiteLine.gameObject.SetActive(false);
                    button.interactable = false;
                    //すでについているリスナーを全て削除(大事)
                    button.onClick.RemoveAllListeners();

                    if (j < ally.skills.Count)
                    {
                        Color c = Color.white;
                        c.a = 0.3f;
                        backGround.color = c;
                        Skill skill = ally.skills[j];
                        //string skillName = Enum.GetName(typeof(SkillName), skill.SkillName);
                        string skillName = skill.DisplayedSkillName;
                        //skillButtonPanels[j].name = skill.Name;
                        skillButtonPanels[j].name = Enum.GetName(typeof(SkillName), skill.SkillName);
                        skillNameText.text = skillName;
                        description.text = skill.Description;
                        if (skill.SkillType == SkillType.active)
                        {
                            sp.text = $"消費SP: {skill.SpConsumptions[skill.SkillLevel]}";
                        }
                        else
                        {
                            sp.text = $"消費SP: -";
                        }
                        whiteLine.gameObject.SetActive(true);
                        button.interactable = true;
                        //リスナー追加
                        button.onClick.AddListener(() => OnClickSkillNameButton(skill, ally));
                    }
                    else
                    {

                    }

                }
            }
            else
            {
                text.color = Color.gray;
            }
        }
    }

    // >仲間>装備 タブを選択時
    public void SetWeaponPanel()
    {
        //currentWindow = CurrentWindow.status;

        Ally ally;
        Transform equipmentPanel = allyPanel.Find("EquipmentPanel");
        Transform allyToggles = equipmentPanel.Find("ParentPanel/AllyToggles");
        Transform equipDetail = equipmentPanel.Find("EquipDetail");

        equipDetail.gameObject.SetActive(false);

        SetAllyToggle(allyTogglesParentInEquipment);

        List<Transform> equipContentPanels = new List<Transform>();

        for (int i = 0; i < 3; i++)
        {
            string name = "Ally" + (i + 1).ToString();
            Transform transform = allyTogglesParentInEquipment.Find(name);
            Toggle toggle = transform.GetComponent<Toggle>();
            TextMeshProUGUI text = transform.GetComponentInChildren<TextMeshProUGUI>();

            if (toggle.isOn)
            {

                ally = allies[i];
                //text.color = Color.white;

                Transform weaponPanel = equipContentPanel.Find("Weapon");
                //装備一覧をセット
                for (int j = 0; j < 3; j++)
                {
                    Transform equipPanel;

                    if (j == 0)
                    {
                        equipPanel = equipContentPanel.Find("Weapon");
                    }
                    else if (j == 1)
                    {
                        equipPanel = equipContentPanel.Find("Guard1");
                    }
                    else
                    {
                        equipPanel = equipContentPanel.Find("Guard2");
                    }

                    Button button;
                    button = equipPanel.Find("Button").GetComponent<Button>();
                    TextMeshProUGUI nameLabel = equipPanel.Find("Button/Name").GetComponent<TextMeshProUGUI>();
                    TextMeshProUGUI descriptionLabel = equipPanel.Find("Button/Description").GetComponent<TextMeshProUGUI>();
                    string nameText = "";
                    string descriptionText = "";

                    Image backGround = equipPanel.GetComponent<Image>();
                    Color c = Color.black;
                    button.interactable = false;
                    button.onClick.RemoveAllListeners();

                    //武器
                    if (j == 0)
                    {
                        Weapon weapon = ally.EquippedWeapon;
                        nameText += "武器:";
                        if (weapon == null || weapon.ID == "nothing")
                        {
                            nameText += "装備なし";
                        }
                        else
                        {
                            c = Color.white;
                            nameText += weapon.Name;
                            if (weapon.AdditionalPoint > 0)
                            {
                                nameText += "+";
                                nameText += weapon.AdditionalPoint.ToString();
                            }

                            descriptionText += $"攻撃力+{weapon.NormalValue} ";
                            //descriptionText += $"命中+{weapon.HitAboidProb}\n";

                            foreach (KeyValuePair<WeaponAbility, int> kvp in weapon.WeaponAbilities)
                            {
                                descriptionText += $"{kvp.Key.GetWeaponAbilityName()}+{kvp.Value}";
                            }

                            button.interactable = true;
                        
                            if (ally.EquippedWeapon.IsUnique)
                            {

                            }
                        }
                    }
                    //防具
                    else if (j == 1)
                    {
                        Guard guard = ally.EquippedArmor;
                        nameText += "防具:";
                        if (guard == null || guard.ID == "nothing")
                        {
                            nameText += "装備なし";
                        }
                        else
                        {
                            c = Color.white;
                            nameText += guard.Name;
                            if (guard.AdditionalPoint > 0)
                            {
                                nameText += "+";
                                nameText += guard.AdditionalPoint.ToString();
                            }

                            descriptionText += $"防御力+{guard.NormalValue} ";
                            //descriptionText += $"回避+{guard.NormalValue}\n";

                            foreach (KeyValuePair<GuardAbility, int> kvp in guard.GuardAbilities)
                            {
                                descriptionText += $"{kvp.Key.GetGuardAbilityName()}+{kvp.Value}";
                            }

                            button.interactable = true;
                            //リスナー追加
                            //button.onClick.AddListener(() => OnClickSkillDetail(skill, ally));

                            if (ally.EquippedWeapon.IsUnique)
                            {

                            }
                        }
                    }
                    else
                    {
                        Guard guard = ally.EquippedAccessory;
                        nameText += "装飾品:";
                        if (guard == null || guard.ID == "nothing")
                        {
                            nameText += "装備なし";
                        }
                        else
                        {
                            c = Color.white;
                            nameText += guard.Name;
                            if (guard.AdditionalPoint > 0)
                            {
                                nameText += "+";
                                nameText += guard.AdditionalPoint.ToString();
                            }

                            descriptionText += $"防御力+{guard.NormalValue} ";

                            foreach (KeyValuePair<GuardAbility, int> kvp in guard.GuardAbilities)
                            {
                                descriptionText += $"{kvp.Key.GetGuardAbilityName()}{ConvertPlusOrMinusString(kvp.Value)}";
                            }

                            button.interactable = true;
                            //リスナー追加
                            //button.onClick.AddListener(() => OnClickSkillDetail(skill, ally));

                            if (ally.EquippedWeapon.IsUnique)
                            {

                            }
                        }
                    }

                    c.a = 0.3f;
                    backGround.color = c;
                    nameLabel.text = nameText;
                    descriptionLabel.text = descriptionText;

                }
            }
        }
    }

    //スキル名タップ時
    public void OnClickSkillNameButton(Skill skill, Ally ally)
    {
        for (int j = 0; j < 8; j++)
        {
            Color c = Color.black;
            if (j < ally.skills.Count)
            {
                c = Color.white;
                if (skillButtonPanels[j].name == Enum.GetName(typeof(SkillName), skill.SkillName))
                {
                    c = Color.yellow;
                }
            }
            Transform button1 = skillButtonPanels[j].Find("Button");
            Utility.SetColorOfButtonObject(button1, c, 0.6f);
        }

        Vector3 mousePosition = Input.mousePosition;
        Transform buttonObject
                = skillDetail.Find("Panel/SelectButtonPanel/Use");
        Button button = buttonObject.GetComponent<Button>();

        TextMeshProUGUI text = buttonObject.Find("Text").
        GetComponent<TextMeshProUGUI>();

        Vector3 w_position = Camera.main.ScreenToWorldPoint(mousePosition);
        w_position.z = 1.0f;
        w_position.x = w_position.x - 3.0f;
        skillDetail.position = w_position;

        //フロアで使えるかどうか
        if (!skill.IsAvairableOnFloor ||
            skill.SpConsumptions[skill.SkillLevel] > allies[allyNum].Statuses[Status.currentSP])
        {
            button.interactable = false;
            text.color = new Color(1f, 1f, 1f, 0.3f);
        }
        else
        {
            button.interactable = true;
            text.color = new Color(1f, 1f, 1f, 1.0f);
        }

        SelectedSkill = skill;
        skillDetail.gameObject.SetActive(true);
    }

    public void OnClickUseSkill()
    {
        if (SelectedSkill.Target == Target.ally)
        {
            //gameController.Situation = "use_skill";
            //gameController.WaitClick("Ally");
            //gameController.CallBackManager.SetNewCallBacks(gameController.CallBackManager
            //    .OnClickedAllyBySkillTargettingInField,
            //    gameController.CallBackManager.OnCanceledAllySelecting, "Ally");

            gameController.CallBackManager.SetNewCallBacks((string id) => 
            {
                gameController.SetSelectedAllyID(id);
                gameController.UseSkillInField();
            },
            () =>
            {
                //gameController.SelectWhoPanel.gameObject.SetActive(false);
                //gameController.InoperablePanelUnderAlly.gameObject.SetActive(false);
                gameController.CallBackManager.ClearCallBack();
            },
            "Ally"
            );


        }
        else
        {
            gameController.UseSkillInField();
        }
    }


    public void SetAllyToggle(Transform allyTogglesParentPanel)
    {
        bool isInitial = true;

        for (int i = 0; i < 3; i++)
        {
            string name = "Ally" + (i + 1).ToString();
            Toggle toggle = allyTogglesParentPanel.Find(name).GetComponent<Toggle>();
            TextMeshProUGUI text = allyTogglesParentPanel.Find(name).GetComponentInChildren<TextMeshProUGUI>();
            text.color = Color.gray;
            if (allies.Count() < i + 1)
            {
                toggle.interactable = false;
                text.text = "";
            }
            else
            {
                toggle.interactable = true;
                text.text = allies[i].Name;
            }

            if (toggle.isOn)
            {
                text.color = Color.white;
                isInitial = false;
            }
        }

        //初期値
        if (isInitial)
        {
            Toggle initialToggle = allyTogglesParentPanel.Find("Ally1").GetComponent<Toggle>();
            TextMeshProUGUI initialText = initialToggle.GetComponentInChildren<TextMeshProUGUI>();
            initialToggle.isOn = true;
            initialText.color = Color.white;
        }
    }


    // >アイテム　選択時
    public void SetItemWindow()
    {
        currentWindow = CurrentWindow.item;
        gameController.ItemWindowManager.SetTwoColumnItemWindow("menu", allies[0].Items);
    }



    //============================================================
    //
    //============================================================


    //public void OnClickRightPanel()
    //{
    //    if (allyNum + 1 == allies.Count)
    //    {
    //        allyNum = 0;
    //    }
    //    else
    //    {
    //        allyNum += 1;
    //    }
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickLeftPanel()
    //{
    //    if (allyNum == 0)
    //    {
    //        allyNum = allies.Count - 1;
    //    }
    //    else
    //    {
    //        allyNum -= 1;
    //    }
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickMenu()
    //{
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickStatus()
    //{
    //    currentWindow = CurrentWindow.status;
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickSkill()
    //{
    //    currentWindow = CurrentWindow.skill;
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickItem()
    //{
    //    currentWindow = CurrentWindow.item;
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickSettings()
    //{
    //    currentWindow = CurrentWindow.settings;
    //    OpenDetailWindow(allyNum);
    //}

    //public void OnClickCancel()
    //{
    //    GameObject parent;
    //    selectedMenuNo = 0;
    //    parent = transform.parent.gameObject;
    //    parent.gameObject.SetActive(false);
    //}


}
