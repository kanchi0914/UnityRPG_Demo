using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static Utility;
using static EnumHolder;

public class ItemWindowManager : MonoBehaviour
{

    private GameController gameController;
    private AllyManager allyManager;

    public ContentSetter contentSetter;

    public Transform ItemWindowPanel;

    public Transform itemButtonObject;

    public Transform Button;

    //public Transform yesNoPanel;
    //public Transform infoPanel;


    private List<Item> items;

    private TextMeshProUGUI topText; 

    private Transform multiSelectingButtonObject;
    private Transform multiProcessingButtonObject;
    private Transform sortButtonObject;
    private Transform itemContentPanel;
    private Transform subWindowPanel;
    private TextMeshProUGUI[] itemTexts;
    private List<Transform> itemButtonObjects = new List<Transform>();

    Button useItemButton;
    Button itemDescriptionButton;
    Button discardItemButton;

    Button multiSelectingButton;
    Button multiProcessingButton;
    Button SortButton;

    TextMeshProUGUI multiProcessingTextObj;

    private bool isMultiSelecting = false;

    private string situation = "";
    private string clickedCommand = "";

    //Item selectedItem = new Item();
    private Item selectedItem = new Item();
    private List<Item> selectedItems = new List<Item>();
    //string selectedItemID = "";
    // private List<string> selectedItemIDs = new List<string>();

    public string ClickedCommand { get => clickedCommand; set => clickedCommand = value; }
    public List<Item> SelectedItems { get => selectedItems; private set => selectedItems = value; }
    public Item SelectedItem { get => selectedItem; private set => selectedItem = value; }

    private void Start()
    {

    }


    private void Update()
    {
        if (isMultiSelecting && SelectedItems.Count > 0)
        {
            multiProcessingButton.interactable = true;
            multiProcessingTextObj.color = Color.white;
        }
        else
        {
            multiProcessingButton.interactable = false;
            multiProcessingTextObj.color = Color.gray;
        }
    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        allyManager = gameController.AllyManager;
        InitComponents();
    }

    public void InitComponents()
    {
        itemContentPanel = ItemWindowPanel.Find("ScrollView/Viewport/Content");
        itemTexts = itemContentPanel.GetComponentsInChildren<TextMeshProUGUI>();

        topText = ItemWindowPanel.Find("TopText").GetComponent<TextMeshProUGUI>();

        subWindowPanel = ItemWindowPanel.Find("SubWindow");

        //複数選択ボタンのリスナーを設定
        multiSelectingButtonObject = ItemWindowPanel.Find("MultiSelecting");
        multiSelectingButton = multiSelectingButtonObject.GetComponent<Button>();
        multiSelectingButton.onClick.AddListener(() => OnClickMultiSelecting());

        multiProcessingButtonObject = ItemWindowPanel.Find("MultiProcessing");
        multiProcessingButton = multiProcessingButtonObject.GetComponent<Button>();
        multiProcessingButton.onClick.AddListener(() => OnClickMultiProcessing());
        multiProcessingTextObj = multiProcessingButtonObject.GetComponentInChildren<TextMeshProUGUI>();

        sortButtonObject = ItemWindowPanel.Find("Sort");
        SortButton = sortButtonObject.GetComponent<Button>();
        SortButton.onClick.AddListener(() => OnClickSort());


        //サブウィンドウに表示される文字の設定
        useItemButton = subWindowPanel.Find("Panel/SelectButtonPanel/Use").GetComponent<Button>();
        itemDescriptionButton = subWindowPanel.Find("Panel/SelectButtonPanel/Description").GetComponent<Button>();
        discardItemButton = subWindowPanel.Find("Panel/SelectButtonPanel/Discard").GetComponent<Button>();

        //サブウィンドウに表示される文字のリスナーを設定
        useItemButton.onClick.AddListener(() => OnClickUseItem());
        discardItemButton.onClick.AddListener(() => OnClickDiscardItem());

    }

    public void SetTwoColumnItemWindow(string situation, List<Item> items)
    {
        this.situation = situation;
        //参照私
        this.items = items;
        InitMainWindow(items);
        InitSubWindow();
        this.ItemWindowPanel.gameObject.SetActive(true);
    }


    public void UpdateWindow()
    {
        InitMainWindow(this.items);
        InitSubWindow();
    }
    /// <summary>
    /// メインウィンドウの初期設定
    /// </summary>
    /// <param name="items"></param>
    public void InitMainWindow(List<Item> items)
    {

        if (this.situation == "buying")
        {
            topText.text = "購入アイテムを選択";
            multiProcessingTextObj.text = "まとめて購入";
        }
        else if (this.situation == "selling")
        {
            topText.text = "売却アイテムを選択";
            multiProcessingTextObj.text = "まとめて売却";
        }
        else
        {
            topText.text = "所持アイテム一覧";
            multiProcessingTextObj.text = "捨てる";
        }
       
        //ボタンをすべて削除
        foreach (Transform child in itemContentPanel)
        {
            Destroy(child.gameObject);
        }

        TextMeshProUGUI text = multiSelectingButtonObject.Find("Text").GetComponent<TextMeshProUGUI>();
        contentSetter.SetTextColor(text, Color.gray);
        text.text = "複数選択　Off";

        itemButtonObjects = new List<Transform>();

        //ボタンを追加
        for (int i = 0; i < items.Count; i++)
        {
            Item item = items[i];
            CreateItemButton(item);
        }
    }

    /// <summary>
    /// サブウィンドウの初期設定(ボタン数)
    /// </summary>
    public void InitSubWindow()
    {
        subWindowPanel.gameObject.SetActive(false);

        //menu時以外は、捨てるボタンを非表示
        if (situation == "menu")
        {
            discardItemButton.gameObject.SetActive(true);
        }
        else
        {
            discardItemButton.gameObject.SetActive(false);
        }
    }

    public void CreateItemButton(Item item)
    {
        Transform itemTransform = Instantiate(itemButtonObject);
        Button button = itemTransform.GetComponent<Button>();

        //2カラムか1カラムか
        if (situation == "menu")
        {
            itemTransform.SetParent(itemContentPanel, false);
        }
        else
        {
            itemTransform.SetParent(itemContentPanel, false);
            //throw Utility.ItemWindowError;
        }

        itemTransform.gameObject.SetActive(true);
        TextMeshProUGUI text = itemTransform.GetComponentInChildren<TextMeshProUGUI>();

        string equipped = "";
        if (item.ItemType == ItemType.weapon || item.ItemType == ItemType.guard)
        {
            Equipment e = item as Equipment;
            if (!string.IsNullOrEmpty(e.EquippedAllyID))
            {
                equipped = "E ";
            }
        }
        text.text = equipped + item.Name;

        if (situation == "buying")
        {
            text.text += $" {item.PurchasePrice}";
        }
        else if (situation == "selling")
        {
            text.text += $" {item.SellingPrice}";
        }

        text.color = Color.white;
        itemTransform.name = item.ID;
        button.interactable = true;
        button.onClick.AddListener(() => OnClickItemButton(item));

        itemButtonObjects.Add(itemTransform);

    }

    /// <summary>
    /// アイテム名をタップしたときに表示されるサブウィンドウの位置を設定
    /// 後で直す
    /// </summary>
    public void SetPositionOfItemDetailWindow()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 w_position = Camera.main.ScreenToWorldPoint(mousePosition);
        w_position.z = 1.0f;
        w_position.x = w_position.x - 3.0f;
        subWindowPanel.position = w_position;
    }

    /// <summary>
    /// サブウィンドウの一番上(決定ボタン)のテキストを設定
    /// 
    /// </summary>
    /// <param name="item"></param>
    public void SetTextOfTopButtonInSubWindow(Item item)
    {
        Transform buttonObject
                = subWindowPanel.Find("Panel/SelectButtonPanel/Use");
        Button button = buttonObject.GetComponent<Button>();
        TextMeshProUGUI text = buttonObject.Find("Text").GetComponent<TextMeshProUGUI>();

        if (situation == "menu")
        {
            //装備品なら
            if (item.ItemType == ItemType.weapon || item.ItemType == ItemType.guard ||
                item.ItemType == ItemType.accessary)
            {
                Equipment eq = item as Equipment;
                if (string.IsNullOrEmpty(eq.EquippedAllyID))
                {
                    text.text = "装備";
                }
                else
                {
                    text.text = "外す";
                }
            }
            //装備品以外
            else
            {
                text.text = "使う";
                if (item.IsAvairableOnFloor)
                {
                    button.interactable = true;
                    contentSetter.SetTextColor(text, Color.white);
                }
                else
                {
                    button.interactable = false;
                    contentSetter.SetTextColor(text, Color.gray);
                }

            }
        }
        else if (situation == "buying")
        {
            text.text = "購入";
            //所持金不足
            if (item.PurchasePrice > gameController.Gold)
            {
                button.interactable = false;
                contentSetter.SetTextColor(text, Color.gray);
            }
        }
        else if (situation == "selling")
        {
            text.text = "売却";
        }
        else
        {
            throw Utility.ItemWindowError;
        }

    }

    //============================================================
    //リスナー設定
    //============================================================

    /// <summary>
    /// メインウィンドウ＞アイテム名をクリック時に呼ばれる
    /// サブウィンドウの設定を行い表示
    /// </summary>
    /// <param name="item"></param>
    public void OnClickItemButton(Item item)
    {
        //複数選択時
        if (isMultiSelecting)
        {
            Debug.Log(itemButtonObjects);
            Transform buttonObject = itemButtonObjects.Find(t => t.name == item.ID);
            //選択されたアイテム一覧になければ追加
            if (!SelectedItems.Contains(item))
            {
                Item selected = items.Find(i => i.ID == item.ID);
                SelectedItems.Add(item);
                contentSetter.SetColorOfButtonObject(buttonObject, Color.cyan);
            }
            else
            {
                Item selected = items.Find(i => i.ID == item.ID);
                SelectedItems.Remove(item);
                contentSetter.SetColorOfButtonObject(buttonObject, Color.white);
            }
        }
        //通常時
        else
        {
            SetPositionOfItemDetailWindow();
            SetTextOfTopButtonInSubWindow(item);
            SelectedItem = items.Find(i => i.ID == item.ID);
            subWindowPanel.gameObject.SetActive(true);
        }
    }

    //itemDetailの各項目がクリックされたとき
    public void OnClickUseItem()
    {
        clickedCommand = "use";

        if (this.situation == "buying")
        {
            if (gameController.AllyManager.Allies[0].Items.Count() >=
                gameController.MaxItemNum)
            {
                gameController.SetInfo(message: "アイテムが一杯で購入できません");
            }
            else
            {
                items.Remove(selectedItem);
                gameController.AllyManager.AddItem(selectedItem);
                gameController.SetInfo(message: $"{selectedItem.Name}を購入しました");
            }
        }
        else if (this.situation == "selling")
        {
            items.Remove(selectedItem);
            //gameController.AllyManager.RemoveItem(selectedItem);
            gameController.SetInfo(message: $"{selectedItem.Name}を売却しました");
        }

        else
        {
            if (SelectedItem.IsEquipment())
            {
                Equipment eq = SelectedItem as Equipment;
                //装備する対象を選ぶ
                if (string.IsNullOrEmpty(eq.EquippedAllyID))
                {
                    //gameController.Situation = "use_item";
                    //gameController.WaitClick("Ally");
                    gameController.CallBackManager.SetNewCallBacks(
                        gameController.CallBackManager.OnClickedAllyByItemTargettingInField,
                        gameController.CallBackManager.OnCanceledAllySelecting,
                        "Ally");
                }
                //外す
                else
                {
                    gameController.UseItemInField();
                }
            }
            else if (SelectedItem.Target == Target.ally)
            {
                //gameController.Situation = "use_item";
                //gameController.WaitClick("Ally");
                gameController.CallBackManager.SetNewCallBacks(
                    gameController.CallBackManager.OnClickedAllyByItemTargettingInField,
                    gameController.CallBackManager.OnCanceledAllySelecting,
                    "Ally");
            }
            else
            {
                gameController.UseItemInField();
            }

        }

        UpdateWindow();

    }


    public void OnClickMultiProcessing()
    {
        //複数アイテムを捨てる
        if (situation == "menu")
        {
            gameController.SetConfirmationWindow(DiscardItems
            , "選択されたアイテムを\n捨ててよろしいですか？"
            , $"{selectedItems.Count}個のアイテムを捨てました。");
        }
        else if (situation == "buying")
        {
            gameController.SetConfirmationWindow(PurchaseItems,
                "選択されたアイテムを\n購入してよろしいですか？",
                $"{selectedItems.Count}個のアイテムを購入しました。");
        }
        else if (situation == "selling")
        {
            gameController.SetConfirmationWindow(CellItems,
            "選択されたアイテムを\n売却してよろしいですか？",
            $"{selectedItems.Count}個のアイテムを売却しました。");
        }

    }

    /// <summary>
    /// サブウィンドウ＞捨てる　を選択時に呼ばれる
    /// </summary>
    public void OnClickDiscardItem()
    {
        gameController.SetConfirmationWindow(DiscardItems
            , $"{Utility.GetStringOfEnum(selectedItem.ItemName)}を\n捨ててよろしいですか？"
            , $"アイテムを捨てました。");
    }

    /// <summary>
    /// アイテムを捨てる
    /// </summary>
    public void DiscardItems()
    {
        if (selectedItems.Count > 1)
        {
            foreach (Item item in SelectedItems)
            {
                items.RemoveAll(i => i.ID == item.ID);
            }
        }
        else
        {
            items.Remove(selectedItem);
        }
        SelectedItems = new List<Item>();
        selectedItem = new Item();
        gameController.CloseAll();
    }

    public void PurchaseItems()
    {
        int sumPrice = 0;

        selectedItems.ForEach(e => sumPrice += e.PurchasePrice);
        if (sumPrice > gameController.Gold)
        {
            gameController.CallBackManager.ClearCallBack();
            gameController.SetInfo(message: "所持金が足りません");
        }
        else
        {
            if (selectedItems.Count > 1)
            {
                foreach (Item item in SelectedItems)
                {
                    items.RemoveAll(i => i.ID == item.ID);
                }
            }
            else
            {
                items.Remove(selectedItem);
            }
            SelectedItems = new List<Item>();
            selectedItem = new Item();
        }
    }

    public void CellItems()
    {
        if (selectedItems.Count > 1)
        {
            foreach (Item item in SelectedItems)
            {
                items.RemoveAll(i => i.ID == item.ID);
            }
        }
        else
        {
            items.Remove(selectedItem);
        }
        SelectedItems = new List<Item>();
        selectedItem = new Item();
        gameController.CloseAll();
    }

    public void Clicked()
    {

    }

    public void OnClickSort()
    {
        Debug.Log("");
    }


    public void OnClickMultiSelecting()
    {
        TextMeshProUGUI text = multiSelectingButtonObject.Find("Text").GetComponent<TextMeshProUGUI>();
        if (!isMultiSelecting)
        {
            isMultiSelecting = true;
            contentSetter.SetTextColor(text, Color.white);
            text.text = "複数選択　On";
        }
        else
        {
            for (int i = 0; i < itemButtonObjects.Count; i++)
            {
                Debug.Log(itemButtonObjects.Count);
                Image buttonImage = itemButtonObjects[i].transform.GetComponent<Image>();
                contentSetter.SetColorOfButtonObject(itemButtonObjects[i].transform, Color.white);

            }
            isMultiSelecting = false;
            contentSetter.SetTextColor(text, Color.gray);
            text.text = "複数選択　Off";
        }
    }




    //public void OnClickOK(GameObject g)
    //{
    //    yesNo = "yes";
    //    if (isDiscardingItem)
    //    {
    //        SetInfoPanel("アイテムを捨てました");

    //        List<Item> discardedItems = new List<Item>();
    //        if (isMultiSelecting)
    //        {
    //            foreach (string id in selectedItemIDs)
    //            {
    //                discardedItems.Add(allies[allyNum].Items.Find(item => item.ID == id));
    //            }
    //        }
    //        else
    //        {
    //            Item item = allies[allyNum].Items.Find(i => i.ID == SelectedItemID);
    //            Debug.Log(allies[allyNum].Items.Exists(i => i.ID == SelectedItemID));
    //            discardedItems.Add(item);
    //            Debug.Log(item.Name);
    //        }
    //        allies[allyNum].DiscardItems(discardedItems);
    //        isDiscardingItem = false;
    //    }

    //    UpdateItemWindow(allyNum);

    //    g.SetActive(false);
    //}

    //public void OnClickNo(GameObject g)
    //{
    //    yesNo = "false";
    //    g.SetActive(false);
    //}

    ////アイテム欄がいっぱいのときに呼ばれる
    //public void OrderItems(List<Item> restItems)
    //{
    //    //UpdateItemPanel();
    //    foreach (Item item in restItems)
    //    {
    //        Transform restItemButton = Instantiate(itemButtonObject);
    //        Button button = restItemButton.GetComponent<Button>();
    //        TextMeshProUGUI text = restItemButton.Find("Text").GetComponent<TextMeshProUGUI>();
    //        text.text = item.Name;
    //        text.color = new Color(0.75f, 0.20f, 0, 20);
    //        button.interactable = false;

    //        restItemButton.SetParent(itemContentPanel, false);
    //        itemButtonObjects.Add(restItemButton);
    //    }

    //    SetAllyPanel();

    //    //UpdateItemWindow(allyNum);
    //    //OnClickItem();
    //}




}

