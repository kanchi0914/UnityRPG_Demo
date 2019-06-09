using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CallBackManager
{
    private GameController gameController;

    public delegate void VoidCallBack();
    public delegate void TaggedCallBack(string tag);

    private Action<string> callBackOnClicked;
    private Action callBackOnCanceled;
    private string waitingTarget = "";

    public Action<string> CallBackOnClicked { get => callBackOnClicked; private set => callBackOnClicked = value; }
    public Action CallBackOnCanceled { get => callBackOnCanceled; private set => callBackOnCanceled = value; }
    public string WaitingTarget { get => waitingTarget; private set => waitingTarget = value; }

    public void Init(GameController gameController)
    {
        this.gameController = gameController;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="onClick"></param>
    /// <param name="onCanceled"></param>
    /// <param name="waitingTarget">クリック対象オブジェクトのタグ名</param>
    public void SetNewCallBacks(Action<string> onClick = null, Action onCanceled = null, string waitingTarget = "")
    {
        CallBackOnClicked = onClick;
        CallBackOnCanceled = onCanceled;
        this.WaitingTarget = waitingTarget;
    }

    public void ClearCallBack()
    {
        CallBackOnClicked = null;
        CallBackOnCanceled = null;
        WaitingTarget = null;
    }


    //============================================================
    //対象のtagをクリック後に呼ばれるメソッド
    //============================================================

    //public void OnClickedAllyBySkillTargettingInField(string id)
    //{
    //    gameController.SetSelectedAllyID(id);
    //    gameController.UseSkillInField();
    //}

    public void OnClickedAllyByItemTargettingInField(string id)
    {
        gameController.SetSelectedAllyID(id);
        gameController.UseItemInField();
    }

    public void OnClickedAlly(string id)
    {
        gameController.AllyManager.SelectedAllyID = id;
    }

    public void OnClickedEnemy(string id)
    {
        gameController.EnemyManager.SelectedEnemyID = id;
    }

    public void OnClickedMessageWindowInBattle(string id)
    {
        gameController.BattleManager.ExecuteNextCommand();
    }

    public void OnClickedMessageWindowInEndOfBattle(string id)
    {
        gameController.BattleManager.ExitBattle();
    }

    public void OnClickedMessageWindowInEvents(string id)
    {
        gameController.SendNextMessage();
    }

    //============================================================
    //対象のtagがクリックされなかったときに呼ばれるメソッド
    //============================================================

    public void OnCanceledAllySelecting()
    {
        gameController.SelectWhoPanel.gameObject.SetActive(false);
        gameController.InoperablePanelUnderAlly.gameObject.SetActive(false);
        ClearCallBack();
    }

    public void OnCanceledEnemySelecting()
    {
        ClearCallBack();
    }



}