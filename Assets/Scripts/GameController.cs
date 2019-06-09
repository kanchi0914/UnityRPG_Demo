using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public MapCreator mapCreator = new MapCreator();
    public Player Player = new Player();
    //public TaggedCallBack CallBackOnClicked;

    public delegate void CallBack();

    public GameObject playerObject;
    public GameObject darkZone;
    public GameObject mainCamera;

    public Camera camera;

    //============================================================
    //ジェネレータクラス
    //============================================================

    public EnemyGenerator EnemyGenerator;
    public ItemGenerator ItemGenerator;
    public SkillGenerator2 SkillGenerator;
    public AllyGenerator AllyGenerator;
    public EventHolder EventHolder = new EventHolder();

    //============================================================
    //マネージャーオブジェクト
    //============================================================

    public MenuManager MenuManager = new MenuManager();
    public MessageManager MessageManager = new MessageManager();
    public CommandManager CommandManager = new CommandManager();
    public EnemyManager EnemyManager = new EnemyManager();
    public AllyManager AllyManager = new AllyManager();
    public ItemWindowManager ItemWindowManager = new ItemWindowManager();
    public OptionsManager OptionManager = new OptionsManager();
    public BattleManager BattleManager = new BattleManager();
    public EventManager EventManager = new EventManager();
    public FloorInfo floorInfoManager = new FloorInfo();
    public CallBackManager CallBackManager = new CallBackManager();

    public SkillEffector SkillEffector;
    public ItemEffecor ItemEffecor;

    //============================================================
    //ゲームオブジェクト
    //============================================================

    public GameObject[] AllyPanels;
    public GameObject EnemyPanel;
    public GameObject MessagePanel;
    public GameObject InoperablePanel;
    public GameObject CommandParentPanel;

    public GameObject SelectWhoPanel;
    public GameObject EventImagePanel;
    public Transform InfoPanel;

    public Transform ConfirmationWindowPanel;
    Button okButton;
    Button noButton;

    public Transform InoperablePanelUnderAlly;

    //public GameObject floorInfoPanel;

    public GameObject arrowPanel;
    private Button up;
    private Button down;
    private Button right;
    private Button left;

    //============================================================
    //変数
    //============================================================

    private Event currentEvent;

    public Canvas canvas;

    //private string waitingTarget = "";


    private string situation;

    //このへんどこに置くかはあとで
    private int gold = 10000;
    private int maxItemNum = 50;

    private (int x, int y) initialPos = (0, 0);
    private int mapSize = 11;
    public Cell[,] Cells = new Cell[11, 11];
    private int width = 2;

    private (int x, int y) playerPos = (0, 0);

    private bool isClickedOK = false;

    private int soulNum = 0;
    private int karmaNum = 0;
    private int fuelNum = 100;

    private List<Ally> allies;
    private List<Unit> units;
    private List<Enemy> enemies;

    private string areaName = "アガレスの塔";

    private int floor = 1;

    private bool isBattle = false;

    private bool isExit = true;
    private int textsIndex = 0;

    bool once = false;

    private bool isMoved = false;

    //private bool isCommandSelectedInEvent = false;
    //private string selectedCommandInEvent = "";

    public bool IsBattle { get => isBattle; set => isBattle = value; }
    public int Floor { get => floor; private set => floor = value; }
    public string AreaName { get => areaName; private set => areaName = value; }
    public Event CurrentEvent { get => currentEvent; set => currentEvent = value; }
    public int Gold { get; set; } = 1000;
    public int MaxItemNum { get => maxItemNum; private set => maxItemNum = value; }
    public string Situation { get => situation; set => situation = value; }
    public int SoulNum { get => soulNum; set => soulNum = value; }
    public int KarmaNum { get => karmaNum; set => karmaNum = value; }
    public int FuelNum { get => fuelNum; set => fuelNum = value; }

    //現在のマス
    public Cell CurrentCell { get => Cells[-Player.playerCoord.y, Player.playerCoord.x]; }

    private bool isShowedOptionWindow = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        InitGenerators();
        LoadData();

        InitMapCreator();
        InitManagerObjects();
        InitWindows();
        InitComponents();

        StartGame();
    }

    void StartGame()
    {
        GoFirstFloor();
    }

    // Update is called once per frame
    void Update()
    {
        //クリック待ちでないなら、次の処理を行う
        if (string.IsNullOrEmpty(CallBackManager.WaitingTarget))
        {
            InoperablePanelUnderAlly.gameObject.SetActive(false);
            SelectWhoPanel.gameObject.SetActive(false);

            //AllyManager.CheckLvUpOfPlayer();

            //全滅確認
            //全滅していたら～～
            //AllyManager.CheckAllDead();

            AllyManager.CheckLvUpOfNPC();
            AllyManager.CheckIsFullOfItems();
            CheckEvent();
        }
        //クリック待ちのときは、場合に応じて他のオブジェクトの表示を切り替え
        else
        {
            switch (CallBackManager.WaitingTarget)
            {
                case "Enemy":
                    WaitClick();
                    break;
                case "Ally":
                    SelectWhoPanel.gameObject.SetActive(true);
                    InoperablePanelUnderAlly.gameObject.SetActive(true);
                    WaitClick();
                    break;
                case "MessageWindow":
                    WaitClick();
                    break;
                case "infoOK":
                    //infoPanel.gameObject.SetActive(true);
                    //inoperablePanelUnderAlly.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    //============================================================
    //初期設定
    //============================================================


    #region "init"

    void InitGenerators()
    {
        ItemGenerator = new ItemGenerator();
        SkillGenerator = new SkillGenerator2(this);
        EnemyGenerator = new EnemyGenerator(this);
        AllyGenerator = new AllyGenerator(ItemGenerator, SkillGenerator);
    }

    void LoadData()
    {
        ItemGenerator.Load();
        SkillGenerator.Load();
        EnemyGenerator.Load();
        AllyGenerator.Load();
    }

    void InitMapCreator()
    {
        mapCreator.Init(this, playerObject, mapSize, Cells, width);
    }

    void InitManagerObjects()
    {
        AllyManager.Init(this);
        EnemyManager.Init(this);
        MessageManager.Init(this, MessagePanel);
        MenuManager.Init(this);
        ItemWindowManager.Init(this);
        OptionManager.Init();
        floorInfoManager.Init(this);
        CallBackManager.Init(this);
        //EventManager.Load();

        EventHolder.Init(this);

        BattleManager.Init(this);

        SkillEffector = new SkillEffector(this);
        ItemEffecor = new ItemEffecor(this);
    }

    void InitWindows()
    {
        InitInfoPanel();
    }

    void InitComponents()
    {
        up = arrowPanel.transform.Find("Up").GetComponent<Button>();
        up.onClick.AddListener(() => Move("up"));
        down = arrowPanel.transform.Find("Down").GetComponent<Button>();
        down.onClick.AddListener(() => Move("down"));
        left = arrowPanel.transform.Find("Left").GetComponent<Button>();
        left.onClick.AddListener(() => Move("left"));
        right = arrowPanel.transform.Find("Right").GetComponent<Button>();
        right.onClick.AddListener(() => Move("right"));

        okButton = ConfirmationWindowPanel.Find("Panel/OKButton").GetComponent<Button>();
        //okButton.onClick.AddListener(() => OnClickOKInConfirmation());
        noButton = ConfirmationWindowPanel.Find("Panel/NGButton").GetComponent<Button>();
        noButton.onClick.AddListener(() => OnClickNOInConfirmation());
    }

    public void InitInfoPanel()
    {
        Button okButton = InfoPanel.Find("Panel/Button").GetComponent<Button>();
        okButton.onClick.AddListener(() => OnClickInfo());
    }

    #endregion

    //============================================================
    //基本メソッド
    //============================================================

    #region "basic_method"
    public void Move(string s)
    {
        if (s == "up" && Cells[-(Player.playerCoord.y + 1), Player.playerCoord.x].GetCellType() != Cell.CellType.wall)
        {
            Player.playerCoord.y += 1;
        }
        else if (s == "down"
             && Cells[-(Player.playerCoord.y - 1), Player.playerCoord.x].GetCellType() != Cell.CellType.wall)
        {
            Player.playerCoord.y -= 1;
        }
        else if (s == "right"
            && Cells[-Player.playerCoord.y, Player.playerCoord.x + 1].GetCellType() != Cell.CellType.wall)
        {
            Player.playerCoord.x += 1;

        }
        else if (s == "left"
             && Cells[-Player.playerCoord.y, Player.playerCoord.x - 1].GetCellType() != Cell.CellType.wall)
        {
            Player.playerCoord.x -= 1;
        }

        Player.playerCoord = (Player.playerCoord.x, Player.playerCoord.y);
        playerObject.transform.position = new Vector3(Player.playerCoord.x * width
            , Player.playerCoord.y * width + 1, 0);

        SetCameraPos();

        //足元のマスを確認
        CheckCell();

    }

    public void SetCameraPos()
    {
        Vector3 cameraPos = new Vector3(Player.playerCoord.x * width, Player.playerCoord.y * width, -10);
        mainCamera.transform.position = cameraPos;
    }

    void GoFirstFloor()
    {
        mapCreator.InitMap();
        this.Cells = mapCreator.GetCells();
        Player.playerCoord = mapCreator.GetInitialPlayerCoord();

        playerObject.transform.position = new Vector3(Player.playerCoord.x * width,
            Player.playerCoord.y * width + 1, 0);
        Vector3 cameraPos = new Vector3(Player.playerCoord.x * width,
            Player.playerCoord.y * width, -10);
        mainCamera.transform.position = cameraPos;

        floorInfoManager.Reset();
    }

    public void GoNextFloor()
    {

        floor += 1;

        mapCreator.InitMap();
        this.Cells = mapCreator.GetCells();
        Player.playerCoord = mapCreator.GetInitialPlayerCoord();

        playerObject.transform.position = new Vector3(Player.playerCoord.x * width,
            Player.playerCoord.y * width + 1, 0);
        Vector3 cameraPos = new Vector3(Player.playerCoord.x * width,
            Player.playerCoord.y * width, -10);
        mainCamera.transform.position = cameraPos;

        floorInfoManager.Reset();
    }

    #endregion


    /// <summary>
    /// なにかをクリックするまで待つ
    /// </summary>
    /// <param name="tagOfTarget"></param>
    public void WaitClick()
    {
        //クリックされた
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d)
            {
                //対象のオブジェクトをクリックした
                if (collition2d.transform.tag == CallBackManager.WaitingTarget)
                {
                    CallBackManager.CallBackOnClicked(collition2d.transform.gameObject.name);
                    CallBackManager.ClearCallBack();
                }
            }
            //別のものがクリックされた
            else
            {
                CallBackManager.CallBackOnCanceled?.Invoke();
            }
        }
    }

    IEnumerator Delay(string id)
    {
        yield return new WaitForSeconds(0.1f);
        CallBackManager.CallBackOnClicked(id);
    }

    public void SetKarma(int karma)
    {
        KarmaNum += karma;
    }

    public void SetFuel(int fuel)
    {
        FuelNum += fuel;
    }

    public void SetSelectedAllyID(string id)
    {
        string selectedId = id;
        AllyManager.SelectedAllyID = selectedId;
    }

    public void UseSkillInField()
    {
        List<Unit> units = Utility.CastAlliesToUnits(AllyManager.Allies);
        Skill skill = MenuManager.SelectedSkill;
        string message = SkillEffector.UseInField(skill, units, AllyManager.Allies[0]);
        ShowAutoErasedMessageWindow(message);
        CloseAll();
    }

    public void UseItemInField()
    {
        Item selectedItem = AllyManager.Allies[0].Items.Find
            (i => i.ID == ItemWindowManager.SelectedItem.ID);

        List<Unit> units = Utility.CastAlliesToUnits(AllyManager.Allies);
        string message = AllyManager.UseItemInField(
            ItemWindowManager.SelectedItem, units, AllyManager.Allies[0]);
        ShowAutoErasedMessageWindow(message);
        CloseAll();
    }

    //============================================================
    //Cellチェック
    //============================================================
    public Cell GetCurrentCell()
    {
        return Cells[-Player.playerCoord.y, Player.playerCoord.x];
    }

    public void CheckCell()
    {
        Cell.CellType cellType = Cells[-Player.playerCoord.y, Player.playerCoord.x].GetCellType();

        switch (cellType)
        {
            case Cell.CellType.goal:
                GoNextFloor();
                break;
            case Cell.CellType.enemy:
                ActivateEnemyCell();
                break;
            case Cell.CellType.Event:
                ActivateEventCell();
                break;
            case Cell.CellType.fountain:
                ActivateFountainCell();
                break;
            case Cell.CellType.human:
                ActivateHumanCell();
                break;
            case Cell.CellType.treasure:
                ActivateTreasureCell();
                break;
            case Cell.CellType.rareTreasure:
                ActivateTreasureCell();
                break;
            case Cell.CellType.shop:
                ActivateShopCell();
                break;
            case Cell.CellType.trap:
                ActivateTrapCell();
                break;
            default:
                break;
        }

    }

    //別のとこで処理？
    public void ActivateEventCell()
    {
        textsIndex = 0;
        CurrentEvent = EventManager.GetEventByID("000");
    }

    public void ActivateTreasureCell()
    {
        textsIndex = 0;
        CurrentEvent = EventManager.GetEventByID("000");
    }

    public void ActivateFountainCell()
    {
        textsIndex = 0;
        CurrentEvent = EventManager.GetEventByID("000");
    }

    public void ActivateHumanCell()
    {
        textsIndex = 0;
        //CurrentEvent = EventManager.GetEventByID("test");
        CurrentEvent = EventHolder.GetEventByID("holy_knights");
    }

    public void ActivateShopCell()
    {
        //CurrentEvent = EventManager.GetEventByID("shop_text");
        CurrentEvent = EventHolder.GetEventByID("normal_shop");
    }

    public void ActivateTrapCell()
    {
        Debug.Log("trap");
        List<string> message = new List<string> {
                "罠を踏んだ。",
                "毒矢の罠だった！",
                "ダメージを受けた！"
            };
        MessageManager.SetAutoText(message);
    }

    public void ActivateEnemyCell()
    {
        var Cell = GetCurrentCell();
        if (Cell.Type == Cell.CellType.enemy && Cell.Enemies.Count > 0)
        {
            SetArrowPanelErasing();
            IsBattle = true;
            EnemyManager.SetEnemies();
            BattleManager.StartBattle();
        }

    }



    //============================================================
    //イベント
    //============================================================

    public void StartEvent(Event ev)
    {

    }

    public void CheckEvent()
    {
        if (currentEvent != null)
        {
            EventImagePanel.SetActive(true);
            //文字送り中
            if (!(CurrentEvent.OptionIDs.Count > 0))
            {
                if (currentEvent.Texts.Count > 0)
                {
                    //最初に表示されるメッセージ
                    if (textsIndex == 0)
                    {
                        MessageManager.SetText(CurrentEvent.Texts[0]);
                        textsIndex++;
                    }
                    MessageManager.SetActive(true);
                    CheckNextMessageOfEvent();
                }
                else
                {
                    SendNextMessage();
                }

            }
            //選択肢の表示
            else
            {
                //optionウィンドウを表示
                if (!isShowedOptionWindow)
                {
                    SetOptionsWindow(CurrentEvent);
                    isShowedOptionWindow = true;
                }
                else
                {
                    //選択された
                    if (!string.IsNullOrEmpty(OptionManager.SelectedOption))
                    {
                        CurrentEvent = EventHolder.GetEventByID(OptionManager.SelectedOption);
                        EndEvent();
                    }
                }
            }
        }
    }

    public void CheckNextMessageOfEvent()
    {
        //WaitClick("MessageWindow");
        CallBackManager.SetNewCallBacks(
            onClick: CallBackManager.OnClickedMessageWindowInEvents,
            waitingTarget: "MessageWindow");
    }

    //テキスト送り
    public void SendNextMessage()
    {
        if (CurrentEvent.Texts.Count > textsIndex)
        {
            MessageManager.SetText(CurrentEvent.Texts[textsIndex]);
            textsIndex++;
        }
        //テキストの終わり
        else
        {
            //if (currentEvent.OnEnd != null)
            //{
            //    currentEvent.OnEnd();
            //}
            currentEvent.OnEnd?.Invoke();

            //次がない場合はnullが返る
            //CurrentEvent = EventHolder.GetEventByID(CurrentEvent.NextID);
            EndEvent();
        }
    }

    public void SetOptionsWindow(Event ev)
    {
        OptionManager.SetPanel(ev);
    }

    public void EndEvent()
    {

        //currentEvent.OnEnd?.Invoke();

        if (currentEvent == null)
        {
            EventImagePanel.SetActive(false);
        }
        MessageManager.SetActive(false);

        textsIndex = 0;
        isShowedOptionWindow = false;
        OptionManager.SelectedOption = "";
        OptionManager.ClosePanel();

        //CallBackManager.ClearCallBack();

    }

    //============================================================
    //インフォメーションウィンドウ
    //============================================================
    public void SetInfo(string header = "info", string message = "\n \n")
    {
        TextMeshProUGUI headerTextObj = InfoPanel.Find("Panel/TopText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI messageTextObj = InfoPanel.Find("Panel/InfoText").GetComponent<TextMeshProUGUI>();

        headerTextObj.text = header;
        messageTextObj.text = message;

        InfoPanel.gameObject.SetActive(true);
        CallBackManager.SetNewCallBacks(waitingTarget: "infoOk");
    }

    public void OnClickInfo()
    {
        Console.WriteLine(currentEvent);
        InfoPanel.gameObject.SetActive(false);
        CallBackManager.ClearCallBack();
        Console.WriteLine(currentEvent);
    }


    //============================================================
    //確認ウィンドウ
    //============================================================



    public void InitConfirmationWindow()
    {

    }

    //delegateメソッドにOKを押した後処理したい部分を代入
    public void SetConfirmationWindow(CallBack delegateMethod
        , string message1, string message2)
    {
        okButton.onClick.RemoveAllListeners();
        TextMeshProUGUI messageTextObj = ConfirmationWindowPanel
            .Find("Panel/InfoText").GetComponentInChildren<TextMeshProUGUI>();

        messageTextObj.text = message1;

        okButton.onClick.AddListener(() => OnClickOKInConfirmation(message2));
        okButton.onClick.AddListener(() => delegateMethod());
        ConfirmationWindowPanel.gameObject.SetActive(true);
    }

    public void OnClickOKInConfirmation(string message)
    {
        ConfirmationWindowPanel.gameObject.SetActive(false);
        SetInfo(message: message);
        ////OKボタンをクリックした後に行う処理
        //delegateMethod();
    }

    public void OnClickNOInConfirmation()
    {
        ConfirmationWindowPanel.gameObject.SetActive(false);
    }


    //============================================================
    //メッセージウィンドウ
    //============================================================

    public void ShowAutoErasedMessageWindow(string message)
    {
        MessageManager.SetActive(true);
        MessageManager.SetText(message);
    }

    //============================================================
    //その他
    //============================================================

    public void SetArrowPanelErasing()
    {
        Vector3 pos = Vector3.zero;
        var worldCamera = Camera.main;
        RectTransform t = arrowPanel.GetComponent<RectTransform>();
        var canvasRect = canvas.GetComponent<RectTransform>();
        var screenPos = RectTransformUtility.WorldToScreenPoint(worldCamera, new Vector3(1000f, -450f, 0f));
        RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPos, Camera.main, out pos);

        Sequence seq = DOTween.Sequence();
        seq.Append(t.DOLocalMoveX(1000f, 1.00f).SetEase(Ease.InOutBack));
    }

    public void ResetArroPanel()
    {
        RectTransform t = arrowPanel.GetComponent<RectTransform>();

        Sequence seq = DOTween.Sequence();
        seq.Append(t.DOLocalMoveX(240f, 1.00f).SetEase(Ease.InOutBack));
    }

    public void SetinoperablePanelUnderAlly(bool b)
    {
        if (b)
        {
            InoperablePanelUnderAlly.gameObject.SetActive(true);
        }
    }

    public void CloseAll()
    {
        SelectWhoPanel.gameObject.SetActive(false);
        InoperablePanel.SetActive(false);
        InoperablePanelUnderAlly.gameObject.SetActive(false);
        MenuManager.CloseMenu();
    }


}
