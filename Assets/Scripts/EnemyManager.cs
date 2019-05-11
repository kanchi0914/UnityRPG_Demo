using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using static EnumHolder;
using System.IO;

public class EnemyManager : MonoBehaviour
{

    private GameController gameController;

    [HideInInspector]
    public string selectedEnemy = "";

    private string selectedEnemyID = "";

    [HideInInspector]
    private bool isSelectable = false;

    public GameObject damageText;

    private AllyManager allyManager;
    private List<Ally> allyList = new List<Ally>();


    public Camera camera_object; //カメラを取得
    private RaycastHit hit; //レイキャストが当たったものを取得する入れ物

    public List<Enemy> enemies = new List<Enemy>();

    EnemyGenerator enemyGenerator;

    private List<GameObject> gameObjectsOfEnemies = new List<GameObject>();

    //private List<Vector3> prePos = new List<Vector3>();
    private float prePosY = 0;

    GameObject clickedGameObject;

    private Transform enemyPanel;

    public int test = 0;

    private TextAsset csvFile; // CSVファイル

    //プレハブをとる
    //スクリプトから取得？
    public GameObject enemyObject;

    public bool IsSelectable { get => isSelectable; set => isSelectable = value; }
    public string SelectedEnemyID { get => selectedEnemyID; set => selectedEnemyID = value; }

    // Use this for initialization
    void Awake()
    {

    }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
        enemyPanel = gameController.EnemyPanel.transform;
        allyManager = gameController.AllyManager;
        enemyGenerator = gameController.EnemyGenerator;
    }


    // Update is called once per frame
    void Update()
    {
        //対象の敵を選ぶときだけクリック狩野
        if (IsSelectable)
        {
            selectObject();
        }
    }

    //============================================================
    //基本メソッド
    //============================================================

    public void SetEnemies()
    {
        gameObjectsOfEnemies = new List<GameObject>();
        enemies = new List<Enemy>();
        enemies = gameController.GetCurrentCell().Enemies;
        CreateEnemyObjects();
        UpdateEnemyPanel();
    }

    public void SetSelectedEnemyID(string enemyID)
    {
        selectedEnemyID = enemyID;
    }

    public bool CheckAllDead()
    {
        bool isAllDead = true;
        foreach (Enemy enemy in enemies)
        {
            if (!enemy.IsDeath)
            {
                isAllDead = false;
                break;
            }
        }
        return isAllDead;
    }


    public List<Command> GetEnemyCommands()
    {
        List<Command> enemyCommandList = new List<Command>();

        //味方のリストを更新
        allyList = allyManager.Allies;

        foreach (Enemy enemy in enemies)
        {
            //行動確率に応じてランダムで選択
            var sum = 0;
            foreach (var key in enemy.EnemyActions.Keys)
            {
                sum += enemy.EnemyActions[key];
            }

            int currentSum = Random.Range(0, sum);

            Ability selectedAbility = new Ability();
            string commandType;

            foreach (var key in enemy.EnemyActions.Keys)
            {
                currentSum -= enemy.EnemyActions[key];
                selectedAbility = key;
                if (currentSum < 0)
                {
                    break;
                }
            }

            commandType = "skill";

            Ally selectedAlly = allyList[Random.Range(0, allyList.Count)];

            Command command = new Command()
            { FromUnit = enemy, ToUnit = selectedAlly, Ability = selectedAbility };

            enemy.CurrentCommand = command;
            enemyCommandList.Add(command);

        }
        return enemyCommandList;
    }



    //============================================================
    //コンポーネント関連
    //============================================================

    public void CreateEnemyObjects()
    {
        foreach (Enemy e in enemies)
        {
            AddEnemyObject(e);
        }
    }

    /// <summary>
    /// 
    /// 敵のオブジェクトをInstantiateして追加
    /// </summary>
    /// <param name="enemy"></param>
    public void AddEnemyObject(Enemy enemy)
    {
        GameObject obj = Instantiate(enemyObject);
        Image image = obj.transform.Find("Panel/Image").GetComponent<Image>();
        TextMeshProUGUI hpText = obj.transform.Find("Panel/HP").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI nameText = obj.transform.Find("Panel/Name").GetComponent<TextMeshProUGUI>();
        hpText.text = enemy.Statuses[Status.MaxHP].ToString();
        nameText.text = enemy.Name;

        image.sprite = enemy.Image;
        obj.transform.SetParent(this.enemyPanel, false);
        obj.name = enemy.ID1;

        gameObjectsOfEnemies.Add(obj);
    }

    public void ClearAll()
    {
        enemies = new List<Enemy>();
        foreach (GameObject g in gameObjectsOfEnemies)
        {
            Destroy(g);
        }
    }

    public void UpdateEnemyPanel()
    {
        foreach (GameObject g in gameObjectsOfEnemies)
        {
            Enemy enemy = enemies.Find(e => e.ID1 == g.name);
            if (!enemy.IsDeath)
            {
                GameObject panelObject = g.transform.Find("Panel").gameObject;
                GameObject imageObject = panelObject.transform.Find("Image").gameObject;
                TextMeshProUGUI hpText = panelObject.transform.Find("HP").GetComponent<TextMeshProUGUI>();

                RectTransform rect = hpText.rectTransform;
                Vector3 pos = hpText.transform.position;
                hpText.text = "HP " + enemy.Statuses[Status.currentHP].ToString();
            }
            else
            {
                SetEraseEffect(enemy);
            }
        }
    }

    public void SetDamageEffect(Unit unit, int damage, bool hit = true)
    {

        GameObject obj = GameObject.Find(unit.ID1);
        GameObject panelObject = obj.transform.Find("Panel").gameObject;

        GameObject textObject = Instantiate(damageText);
        textObject.transform.SetParent(panelObject.transform, false);
        Transform transform = textObject.transform;
        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();

        Transform bar = panelObject.transform.Find("MaxHPBar/CurrentHPBar");

        if (hit)
        {
            float perHP = unit.GetPerHP();

            //ＨＰバーの動き
            Vector3 scale = new Vector3(perHP, 1, 1);
            bar.DOScale(scale, 0.3f);

            GameObject obj3 = panelObject.transform.Find("Image").gameObject;
            Transform rect2 = obj3.transform;
            CanvasGroup imageCanvas = obj3.GetComponent<CanvasGroup>();

            CanvasGroup canvas = panelObject.transform.Find("Damage").GetComponent<CanvasGroup>();
            canvas.alpha = 1;

            Sequence seq = DOTween.Sequence();
            seq.Append(imageCanvas.DOFade(0.0f, 0.1f).SetLoops(5));
            seq.Append(imageCanvas.DOFade(1.0f, 0.0f));

            transform.DOLocalMoveY(200, 1.0f).SetEase(Ease.OutCirc).OnComplete(() => Destroy(transform.gameObject));

            int minusDamage = (-damage);
            text.text = minusDamage.ToString();
        }
        else
        {
            transform.DOLocalMoveY(200, 1.0f).SetEase(Ease.OutCirc).OnComplete(() => Destroy(transform.gameObject));
            text.text = "miss!!";
        }

    }

    //消滅時のエフェクト
    public void SetEraseEffect(Unit unit)
    {
        GameObject obj = GameObject.Find(unit.ID1);
        GameObject obj2 = obj.transform.Find("Panel").gameObject;
        CanvasGroup panelCanvas = obj2.GetComponent<CanvasGroup>();
        panelCanvas.DOFade(0, 1.0f);
    }


    // 左クリックしたオブジェクトを取得する関数(2D)
    private void selectObject()
    {
        GameObject result = null;
        // 左クリックされた場所のオブジェクトを取得
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 tapPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collition2d = Physics2D.OverlapPoint(tapPoint);
            if (collition2d && collition2d.transform.tag == "Enemy")
            {
                string selectedId = collition2d.transform.gameObject.
                    transform.parent.gameObject.name;
                //Debug.Log(enemyList.Find(e => e.id == selectedId).IsDeath);
                //選択可能な状況かどうか
                if (!enemies.Find(e => e.ID1 == selectedId).IsDeath)
                {
                    this.selectedEnemy = selectedId;
                    //Debug.Log(this.selectedEnemy);
                    this.IsSelectable = false;
                }
            }
        }
    }

    public void ResetSelectCondition()
    {
        selectedEnemy = null;
        IsSelectable = false;
    }

    public void SetActive(bool b)
    {
        this.enemyPanel.parent.gameObject.SetActive(b);
    }

}
