using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static EnumHolder;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    //private allyList[]
    //味方のリストがいっぱいになったらコマンド実行

    public Transform resultPanel;
    private Transform buttonObject;

    //public bool isOnBattle = false;
    //private bool isWaitingOK = false;

    //初期化する
    private int requiredExp = 0;
    private int requiredGold = 0;
    private List<Item> requiredItems = new List<Item>();

    private GameController gameController;
    private MessageManager messageManager;
    private CommandManager commandManager;
    private EnemyManager enemyManager;
    private AllyManager allyManager;
    private SkillGenerator2 skillGenerator;


    private GameObject commandParentPanel;

    private List<Ally> allies;
    private List<Unit> units;
    private List<Enemy> enemies;

    private GameObject inoperablePanel;


    List<Command> tempCommands = new List<Command>();

    private bool isStartedTurn = false;
    private bool isClicked = false;

    private bool isWaitingExit = false;
    public bool isStart = false;

    public bool isWaitingClick = false;

    public bool isDone = false;

    private int turn;



    // Use this for initialization
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
        this.messageManager = gameController.MessageManager;
        this.commandManager = gameController.CommandManager;
        this.allyManager = gameController.AllyManager;
        this.skillGenerator = gameController.SkillGenerator;
        this.enemyManager = gameController.EnemyManager;
        this.inoperablePanel = gameController.InoperablePanel.gameObject;
        this.commandParentPanel = gameController.CommandParentPanel;

        InitComponets();
    }

    public void InitComponets()
    {
        buttonObject = resultPanel.Find("OK");
        Button button = this.buttonObject.GetComponent<Button>();
        button.onClick.AddListener(() => OnClickResultOK());
    }

    public void StartBattle()
    {
        commandManager.Init(gameController);
        commandManager.SetEnemyManager(enemyManager);

        turn = 1;
        tempCommands = new List<Command>();

        allies = allyManager.Allies;
        //enemies = enemyManager.GetEnemyList();
        enemies = gameController.GetCurrentCell().Enemies;

        messageManager.SetActive(true);
        commandManager.SetActive(false);
        enemyManager.SetActive(true);

        units = new List<Unit>();
        foreach (Ally ally in allies)
        {
            units.Add(ally);
        }
        foreach (Enemy enemy in enemies)
        {
            units.Add(enemy);
        }

        messageManager.SetText("敵が<b> 現れた </b>");

        inoperablePanel.SetActive(true);
        StartCoroutine(Delay());

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        inoperablePanel.SetActive(false);
        messageManager.SetActive(false);

        //最初のターンのコマンド選択画面
        commandManager.SetActive(true);
    }

    //全てのコマンドを実行
    public void StartExecutingCommands()
    {
        tempCommands = SortBySpeed();
        tempCommands = CheckPoison(tempCommands);
        commandManager.SetActive(false);
        messageManager.SetActive(true);
        messageManager.Reset();
        gameController.Situation = "battle";
        ExecuteNextCommand();
    }

    //敏捷で並び替え
    private List<Command> SortBySpeed()
    {
        tempCommands = new List<Command>();
        //実体のコピー？
        tempCommands.AddRange(commandManager.getCommandList());
        tempCommands.AddRange(enemyManager.GetEnemyCommands());

        //優先される行動
        List<Command> priCommands = new List<Command>();

        //優先される行動を書く
        for (int i = 0; i < tempCommands.Count; i++)
        {
            var ability = tempCommands[i].Ability;
            if (tempCommands[i].FromUnit.Buffs.Exists(b => b.Skill.SkillName == SkillName.疾風の舞))
            {
                priCommands.Add(tempCommands[i]);
                tempCommands.Remove(tempCommands[i]);
            }
            else if (ability.GetType() == typeof(Skill))
            {
                var skill = (Skill)ability;
                if (skill.SkillName == SkillName.ソニックバット) { }

                priCommands.Add(tempCommands[i]);
                tempCommands.Remove(tempCommands[i]);

            }
        }

        float random = UnityEngine.Random.Range(0.8f, 1.2f);
        foreach (Command c in tempCommands)
        {
            c.FromUnit.TempAGI = (int)(c.FromUnit.Statuses[Status.AGI] * random);
        }


        //速度順にソート
        IOrderedEnumerable<Command> orderedParameters = priCommands.OrderBy(c => c.FromUnit.Statuses[Status.AGI]);
        List<Command> orderedPri = priCommands.OrderByDescending(c => c.FromUnit.Statuses[Status.AGI]).ToList();

        List<Command> orderedTemp = tempCommands.OrderByDescending(c => c.FromUnit.Statuses[Status.AGI]).ToList();

        orderedPri.AddRange(orderedTemp);

        return orderedPri;
    }

    //メッセージウィンドウクリック時に、次のコマンドを実行する
    public void ExecuteNextCommand()
    {
        //コマンドがまだ残っていたら
        if (tempCommands.Count > 0)
        {
            //コマンドの処理
            Command c = tempCommands[0];
            tempCommands.RemoveAt(0);
            ExecuteCommand(c);
        }
        //全員の行動が完了したら次のターンへ
        else
        {
            EndTurn();
        }
    }


    public void EndTurn()
    {
        turn += 1;
        isStartedTurn = false;
        messageManager.SetActive(false);
        commandManager.SetActive(true);
        commandManager.StartNewTurn();
    }

    public List<Command> CheckPoison(List<Command> commands)
    {
        allies.ForEach(a =>
        {
            if (a.Ailments.ContainsKey(Ailment.poison))
            {
                Skill poison = skillGenerator.GenerateAilmentSkill(Ailment.poison);
                Command command = new Command()
                {
                    Ability = poison,
                    FromUnit = a,
                    ToUnit = a
                };
            }
        });
        return commands;
    }

    public void ExecuteCommand(Command command)
    {
        var message = "";

        Unit fromUnit = command.FromUnit;
        Unit toUnit = command.ToUnit;

        string fromUnitName = fromUnit.Name;
        //対象をとらないときどうするか
        //string toUnitName = toUnit.GetName();

        //混乱時、対象をランダムにする

        //行動できる状態か、状態異常の確認
        //死んでいたらスキップ
        if (!fromUnit.IsDeath)
        {
            //攻撃対象が死亡していたら、生きている相手の中から選びなおす
            if (toUnit != null && toUnit.IsDeath)
            {
                //if (command.Ability.Target == Target.ally && fromUnit.GetType() == typeof(Ally))
                //{

                //}
                toUnit = fromUnit.ChooseOpponentRandomly(units);
                //toUnitName = toUnit.GetName();
            }
            if (command.Ability.GetType() == typeof(Skill))
            {
                Skill skill = command.Ability as Skill;
                //状態異常により、コマンドがキャンセルされた場合
                //if (skill.SkillType == SkillType.物理攻撃 || skill.SkillType == SkillType.物理補助 &&
                //    fromUnit.Ailments.ContainsKey(Ailment.curse))
                //{
                //    Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.curse);
                //    message += gameController.SkillEffector.Use(ailmentSkill, units, fromUnit, toUnit);
                //}
                //else if (skill.SkillType == SkillType.魔法攻撃 || skill.SkillType == SkillType.魔法補助 &&
                //    fromUnit.Ailments.ContainsKey(Ailment.seal))
                //{
                //    Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.seal);
                //    message += gameController.SkillEffector.Use(ailmentSkill, units, fromUnit, toUnit);
                //}
                string ailmentMessage = CheckAilment(fromUnit, skill);
                if (!string.IsNullOrEmpty(ailmentMessage))
                {
                    message += gameController.SkillEffector.Use(skill, units, fromUnit, toUnit);
                }
                else
                {
                    message += ailmentMessage;
                }
            }
            else if (command.Ability.GetType() == typeof(Item))
            {
                //List<Item> items = allyManager.GetItems();
                Item item = command.Ability as Item;
                message += gameController.ItemEffecor.Use(item, units, fromUnit, toUnit);
            }

            //パネルの表示を更新
            messageManager.AddText(message);
            allyManager.UpdatePanels();
            enemyManager.UpdateEnemyPanel();
        }
        else
        {
            isWaitingClick = false;
        }

        //戦闘終了の確認
        //CheckDone();

        if (allyManager.CheckAllDead())
        {
            messageManager.AddText("冒険者たちは全滅した");
            isDone = true;
        }
        //敵が全滅
        else if (enemyManager.CheckAllDead())
        {
            StartCoroutine(OnClickInEndOfBattle());
        }
        else
        {
            StartCoroutine(Click());
        }
    }

    //状態異常によるコマンド変更の処理
    //コマンド実行の直前に呼び出される
    //特になにもない場合は空文字列が返る
    private string CheckAilment(Unit fromUnit, Skill skill)
    {
        string message = "";
        if (fromUnit.Ailments.ContainsKey(Ailment.sleep))
        {
            Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.sleep);
            message += gameController.SkillEffector.Use(ailmentSkill, null, null, null);
        }
        else if (fromUnit.Ailments.ContainsKey(Ailment.confusion))
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random > 0)
            {
                Unit toUnit = fromUnit.ChooseMateRandomly(units);
                Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.confusion);
                message += gameController.SkillEffector.Use(ailmentSkill, units, fromUnit, toUnit);
            }
        }
        else if (skill.SkillType == SkillType.物理攻撃 || skill.SkillType == SkillType.物理補助 &&
            fromUnit.Ailments.ContainsKey(Ailment.curse))
        {
            Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.curse);
            message += gameController.SkillEffector.Use(ailmentSkill, null, null, null);
        }
        else if (skill.SkillType == SkillType.魔法攻撃 || skill.SkillType == SkillType.特殊 &&
            fromUnit.Ailments.ContainsKey(Ailment.seal))
        {
            Skill ailmentSkill = skillGenerator.GenerateAilmentSkill(Ailment.seal);
            message += gameController.SkillEffector.Use(ailmentSkill, null, null, null);
        }
        return message;
    }

    //メッセージウィンドウをクリックするのを待ち、クリックされたら次のコマンドを実行
    IEnumerator Click()
    {
        yield return new WaitForSeconds(0.1f);
        gameController.CallBackManager.SetNewCallBacks(
        //onClick: gameController.CallBackManager.OnClickedMessageWindowInBattle,
        onClick:( (id) => 
        {
            ExecuteNextCommand();
        }),
        onCanceled: null,
        waitingTarget: "MessageWindow"
        );
    }

    IEnumerator OnClickInEndOfBattle()
    {
        yield return new WaitForSeconds(0.1f);
        gameController.CallBackManager.SetNewCallBacks(
            onClick: gameController.CallBackManager.OnClickedMessageWindowInEndOfBattle,
            onCanceled: null,
            waitingTarget: "MessageWindow"
            );
    }




    public void ExitBattle()
    {
        StartCoroutine(WinBattle());
        gameController.ResetArroPanel();
    }

    IEnumerator WinBattle()
    {
        messageManager.SetText("戦闘に勝利した");

        yield return new WaitForSeconds(2.5f);

        messageManager.SetActive(false);
        commandManager.SetActive(false);
        enemyManager.SetActive(false);

        gameController.IsBattle = false;
        gameController.GetCurrentCell().ConvertCellType(Cell.CellType.plane);

        enemyManager.ClearAll();
        SetResult();
    }

    /// <summary>
    /// 結果を表示する
    /// </summary>
    public void SetResult()
    {
        TextMeshProUGUI expText = resultPanel.Find("ExpText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI goldText = resultPanel.Find("GoldText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI itemText = resultPanel.Find("ItemText").GetComponent<TextMeshProUGUI>();

        expText.text = "";
        goldText.text = "";
        itemText.text = "";

        requiredItems = new List<Item>();
        requiredExp = 0;
        requiredGold = 0;

        //経験値、ゴールド
        foreach (Enemy e in enemies)
        {
            requiredExp += e.GetExp();
            requiredGold += e.GetGold();
        }

        //仲間の人数に応じて経験値を減少(ゴールドは減少しない)
        requiredExp = (int)(requiredExp * (((allies.Count - 1) * -0.25) + 1.0));

        expText.text = requiredExp.ToString();
        goldText.text = requiredGold.ToString();

        //ドロップアイテム
        foreach (Enemy e in enemies)
        {
            int prob = UnityEngine.Random.Range(0, 100);
            //TODO:順番を直す？
            foreach (var key in e.DropItems)
            {
                if (key.Value > prob)
                {
                    requiredItems.Add(key.Key);
                    break;
                }

            }
        }

        foreach (Item item in requiredItems)
        {
            itemText.text += item.Name + "\n";
        }

        resultPanel.gameObject.SetActive(true);

    }

    public void OnClickResultOK()
    {
        resultPanel.gameObject.SetActive(false);
        allyManager.AddExps(requiredExp);
        allyManager.AddItems(requiredItems);
    }

}
