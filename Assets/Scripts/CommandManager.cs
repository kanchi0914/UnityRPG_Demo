using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using System;
using static EnumHolder;

public class CommandManager : MonoBehaviour {

    private GameController gameController;

    //private string selectedAllyID = "";
    //private bool isSelectingAlly = false;

    //アイテム、スキルの詳細
    private bool isInformationON = false;

    //private Unit toUnit = new Unit();
    //private Unit fromUnit = new Unit();

    //誰が
    //public Ally currentAlly;
    private int index = 0;

    //選択済みのコマンドを格納するリスト
    private List<Command> allyCommandList = new List<Command>();

    private Command currentCommand;

    public GameObject skillButton;
    public GameObject itemButton;


    private GameObject commandParentPanel;
    private GameObject allySelectPanel;

    //使わない？
    private GameObject commandMessagePanel;


    private GameObject skillDetailPanel;
    private GameObject itemDetailPanel;
    private TextMeshProUGUI commandText;

    private EnemyManager enemyManager;

    public bool isReady = false;

    private bool isBattle = false;

    private string commandStatus = "";
    bool isSelectingCommands = true;
    
    //private Ability selectedAbility = new Ability();
    //private string selectedItemID = "";


    void Start () {
        //Init();
    }

    void Update()
    {
        //battlemanagerから呼び出す
        if (isBattle && isSelectingCommands)
        {
            CheckCommand();
        }
    }

    public void Init(GameController gameController)

    {
        this.gameController = gameController;
        this.commandParentPanel = gameController.CommandParentPanel;

        InitComponents();

        isBattle = true;

        StartNewTurn();
    }

    public void InitComponents()
    {
        this.commandMessagePanel = commandParentPanel.transform.Find
            ("CommandMessagePanel").gameObject;
        this.allySelectPanel = commandParentPanel.transform.Find
            ("AllySelectPanel").gameObject;
        this.skillDetailPanel = commandParentPanel.transform.Find
            ("CommandPanel/SkillDetailPanel").gameObject;
        this.itemDetailPanel = commandParentPanel.transform.Find
            ("CommandPanel/ItemDetailPanel").gameObject;
        this.commandText = commandMessagePanel.transform.Find("Text").
            GetComponent<TextMeshProUGUI>();
    }

    public void StartNewTurn()
    {

        foreach (Ally a in gameController.AllyManager.Allies)
        {
            a.CurrentCommand = new Command();
        }

        isSelectingCommands = true;
        gameController.AllyManager.HideAllyPanel(isClear: true);

        allyCommandList = new List<Command>();
        index = 0;
        commandStatus = "";
        isReady = false;

        gameController.AllyManager.HideAllyPanel(isClear:false, index: index);

        //SetNextPanel();
        SetNextPanel2();
        ResetDetailPanel();
    }

    public void SetActive(bool b)
    {
        if (b)
        {
            this.commandParentPanel.SetActive(true);
        }
        else
        {
            this.commandParentPanel.SetActive(false);
        }
    }

    /// <summary>
    /// 右段のコマンドの選択状況を確認
    /// </summary>
    private void CheckCommand()
    {
        //攻撃
        if (commandStatus == "attack")
        {
            //gameController.WaitClick("Enemy");
            gameController.CallBackManager.SetNewCallBacks(
                onClick:gameController.CallBackManager.OnClickedEnemy,
                onCanceled:gameController.CallBackManager.OnCanceledEnemySelecting,
                "Enemy");
            //敵が選択されたら
            if (!string.IsNullOrEmpty(enemyManager.SelectedEnemyID))
            {
                currentCommand.Ability = gameController.SkillGenerator.Generate("攻撃");
                currentCommand.ToUnit = enemyManager.enemies.Find(e => e.ID1 == enemyManager.SelectedEnemyID);
                SetCommand();
            }
        }
        //スキル
        else if (commandStatus == "skill" || commandStatus == "item") 
        {
            if (currentCommand.Ability != null)
            {
                SetAbilityTarget();
            }
        }
        //防御
        else if (commandStatus == "defense" || commandStatus == "escape")
        {
            currentCommand.Ability = gameController.SkillGenerator.Generate("防御");
            SetCommand();
        }

        //全員分のコマンドを選び終わったら準備完了
        if (CheckReady())
        {
            gameController.AllyManager.HideAllyPanel(isClear:true);
            gameController.BattleManager.StartExecutingCommands();
            isSelectingCommands = false;
        }

    }

    public void SetAbilityTarget()
    {
        if (currentCommand.Ability.Target == Target.opponent)
        {
            //単体
            if (currentCommand.Ability.Scope == Scope.single)
            {
                //SetText($"{abilityType}：{ability.Name}の対象を選んでください");
                gameController.CallBackManager.SetNewCallBacks(
                onClick: gameController.CallBackManager.OnClickedEnemy,
                onCanceled: gameController.CallBackManager.OnCanceledEnemySelecting,
                waitingTarget:"Enemy");
                //敵を選択したら
                if (!string.IsNullOrEmpty(enemyManager.SelectedEnemyID))
                {
                    currentCommand.ToUnit = enemyManager.enemies.Find(e => e.ID1 == enemyManager.SelectedEnemyID);
                    SetCommand();
                }
            }
            //敵全体
            else if (currentCommand.Ability.Scope == Scope.entire)
            {
                SetCommand();
            }
        }

        //味方に発動
        else if (currentCommand.Ability.Target == Target.ally)
        {
            //単体
            if (currentCommand.Ability.Scope == Scope.single)
            {
                SetText($"{currentCommand.Ability.Name}を使う相手を選んでください");
                gameController.CallBackManager.SetNewCallBacks(
                onClick: gameController.CallBackManager.OnClickedAlly,
                onCanceled: gameController.CallBackManager.OnCanceledAllySelecting,
                waitingTarget: "Ally");
                //選択したら
                if (!string.IsNullOrEmpty(gameController.AllyManager.SelectedAllyID))
                {
                    currentCommand.ToUnit = gameController.AllyManager.Allies
                        .Find(a => a.ID1 == gameController.AllyManager.SelectedAllyID);
                    SetCommand();
                }
                //味方全体
                if (currentCommand.Ability.Scope == Scope.entire)
                {
                    SetCommand();
                }
            }
        }
        //自分に発動
        else if (currentCommand.Ability.Target == Target.self)
        {
            currentCommand.ToUnit = gameController.AllyManager.Allies[index];
            SetCommand();
        }
    }
    
    private bool CheckReady()
    {
        if (allyCommandList.Count == gameController.AllyManager.Allies.Count(a => a.IsDeath == false)) return true;
        else return false;
    }

    //一人のコマンドが選択完了したら、Commandクラスをセット
    private void SetCommand()
    {
        allyCommandList.Add(currentCommand);
        gameController.AllyManager.Allies[index].CurrentCommand = currentCommand;

        //SetText("");
        commandStatus = "";
        ResetDetailPanel();

        //まだ残っていたら
        //死亡しているやつはカウントしない
        if (!CheckReady())
        {
            index += 1;
            SetNextPanel2();
        }

    }

    //コマンドキャンセル時の処理
    private void CancelCommand()
    {
        index -= 1;
        SetNextPanel2();
    }

    //Allyクラスが持つスキル、アイテムに応じて表示内容を設定
    //ボタンの設定もここで
    private void SetNextPanel2()
    {
        gameController.AllyManager.SelectedAllyID = "";
        gameController.EnemyManager.SelectedEnemyID = "";

        currentCommand = new Command() { FromUnit = gameController.AllyManager.Allies[index]};
        //コマンドが選択できない場合は飛ばす
        //コマンド自体を登録しない
        if (gameController.AllyManager.Allies[index].IsDeath)
        {
            index++;
            SetNextPanel2();
        }
        else if (gameController.AllyManager.Allies[index].Ailments.ContainsKey(Ailment.sleep))
        {
            Skill skill = gameController.SkillGenerator.Generate("眠り状態");
            currentCommand.Ability = skill;
            SetCommand();
        }
        else
        {
            SetText($"{gameController.AllyManager.Allies[index].Name}のコマンドを選択してください");
            gameController.AllyManager.HideAllyPanel(index:index);

            //SetAllySelectPanel();
            SetSkillPanel();
            SetItemPanel();
        }
    }

    //仲間を選択
    //void SetAllySelectPanel()
    //{
    //    for (int i = 0; i < allies.Count; i++)
    //    {
    //        string buttonName = "Button" + (i + 1).ToString();
    //        Transform buttonObj = allySelectPanel.transform.Find
    //            ("Panel/SelectButtonPanel/" + buttonName);
    //        Button button = buttonObj.transform.GetComponent<Button>();
    //        TextMeshProUGUI text = buttonObj.transform.Find("Text").
    //            GetComponent<TextMeshProUGUI>();

    //        text.text = allies[i].GetName();
    //        if (allies[i].IsDeath)
    //        {
    //            button.interactable = false;
    //        }
    //        else
    //        {
    //            button.interactable = true;
    //            button.onClick.AddListener(() => OnClickTest());
    //            string allyID = allies[i].GetID();
    //            button.onClick.AddListener(() => OnclickAllySellect(allyID));
    //        }
    //    }
    //}

    void SetSkillPanel(bool isSelectingAlly = false)
    {
        Transform buttonPanel = skillDetailPanel.transform.Find("ButtonPanel");
        int size = gameController.AllyManager.Allies[index].skills.Count;

        for (int i = 0; i < 8; i++)
        {
            string buttonName = "Button" + (i + 1).ToString();
            Transform buttonObject = buttonPanel.transform.Find(buttonName);
            Button button = buttonObject.GetComponent<Button>();
            Transform textObject = buttonObject.Find("Text");
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

            if (i < size)
            {
                Skill skill = gameController.AllyManager.Allies[index].skills[i];

                string skillText = skill.Name;

                //戦闘中使用可能なスキルのみ
                if (skill.IsAvairableOnBattle)
                {
                    skillText += " - SP" + skill.SpConsumptions[skill.SkillLevel].ToString();
                    text.text = skillText;

                    //SPが足りていれば
                    if (skill.SpConsumptions[skill.SkillLevel] < gameController.AllyManager.Allies[index].Statuses[Status.currentSP])
                    {
                        button.interactable = true;
                        button.onClick.AddListener(() => { currentCommand.Ability = skill; });
                    }
                    else
                    {
                        button.interactable = false;
                    }
                }
                else
                {
                    text.text = skillText;
                    button.interactable = false;
                }

            }

            else
            {
                text.text = "";
                button.interactable = false;
            }
        }
    }

    void SetItemPanel(bool isSelectingAlly = false)
    {
        Transform buttonPanel = itemDetailPanel.transform.Find("ButtonPanel");
        int size = gameController.AllyManager.Allies[index].Items.Count;

        for (int i = 0; i < 8; i++)
        {
            string buttonName = "Button" + (i + 1).ToString();
            Transform buttonObject = buttonPanel.transform.Find(buttonName);
            Button button = buttonObject.GetComponent<Button>();
            Transform textObject = buttonObject.Find("Text");
            TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

            if (i < size)
            {
                Item item = gameController.AllyManager.GetItems()[i];
                string itemText = item.Name;
                text.text = itemText;

                if (item.IsAvairableOnBattle)
                {
                    button.interactable = true;
                    button.onClick.AddListener(() => { currentCommand.Ability = item; });
                }
                else
                {
                    text.text = itemText;
                    button.interactable = false;
                }

                //if (!isSelectingAlly)
                //{
                //    if (item.IsAvairableOnBattle)
                //    {
                //        button.interactable = true;
                //        button.onClick.AddListener(() => OnClickDetail(item, item.ID));
                //    }
                //    else
                //    {
                //        text.text = itemText;
                //        button.interactable = false;
                //    }
                //}
                ////仲間を選択中
                //else
                //{
                //    text.text = itemText;
                //    button.interactable = false;
                //}

            }
            else
            {
                //image.color = new Color(1, 1, 1, 0.4f);
                text.text = "";
                //button.enabled = false;
                button.interactable = false;
            }
        }
    }

    void ResetDetailPanel()
    {

        skillDetailPanel.SetActive(false);
        itemDetailPanel.SetActive(false);

        if (commandStatus == "skill")
        {
            skillDetailPanel.SetActive(true);
        }
        else if (commandStatus == "item")
        {
            itemDetailPanel.SetActive(true);
        }
    }

    void DestroyPanel()
    {
        Transform buttonPanel = skillDetailPanel.transform.Find("ButtonPanel");
        foreach (Transform t in buttonPanel)
        {
            if (t != null) Destroy(t.gameObject);
        }
        buttonPanel = itemDetailPanel.transform.Find("ButtonPanel");
        foreach (Transform t in buttonPanel)
        {
            if (t != null) Destroy(t.gameObject);
        }
    }


    public void SetEnemyManager(EnemyManager e)
    {
        this.enemyManager = e;
    }

    public List<Command> getCommandList()
    {
        return this.allyCommandList;
    }

    public void OnClickTest()
    {
        Debug.Log("test");
    }

    //仲間選択をキャンセルし、スキル選択に戻る
    //public void OnclickCanceAllySellect(string id)
    //{
    //    isSelectingAlly = false;
    //    SetSkillPanel();
    //}

    //public void OnclickAllySellect(string id)
    //{
    //    selectedAllyID = id;
    //    Debug.Log(selectedAllyID);
    //}


    public void SetText(string str)
    {
        this.commandText.text = str;
    }

    //スキル、アイテム名をクリックしたとき
    public void OnClickDetail(Ability ability)
    {
        //仲間選択ウィンドウの表示
        //if (ability.Target == Target.ally)
        //{
        //    if (ability.GetType() == AbilityType.item)
        //    {
        //        SetItemPanel(isSelectingAlly:true);
        //    }
        //    else
        //    {
        //        SetSkillPanel(isSelectingAlly:true);
        //    }
        //}

        if (isInformationON)
        {

        }

        currentCommand.Ability = ability;
        //selectedItemID = id;

    }

    public void OnClickAttack()
    {
        commandStatus = "attack";
        ResetDetailPanel();
    }

    public void OnClickDeffense()
    {
        commandStatus = "defense";
        ResetDetailPanel();
    }

    public void OnclickSkill()
    {
        commandStatus = "skill";
        ResetDetailPanel();
    }

    public void OnclickItem()
    {
        commandStatus = "item";
        ResetDetailPanel();
    }

    public void OnclickEscape()
    {
        commandStatus = "escape";
        ResetDetailPanel();
    }


}
