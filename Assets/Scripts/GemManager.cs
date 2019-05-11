using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts
{
    class GemManager
    {

        //public void SetWeaponGemPanel(Ally ally)
        //{
        //    Transform equipmentPanel = allyPanel.Find("EquipmentPanel");
        //    Transform weaponPanel = equipmentPanel.Find("ParentPanel/Equipments/Weapon");
        //    Image backGroundColor = weaponPanel.GetComponent<Image>();
        //    TextMeshProUGUI weaponName = weaponPanel.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        //    Transform slotsPanel = weaponPanel.Find("SlotsPanel");
        //    //TextMeshProUGUI statusText = weaponPanel.Find("Status").GetComponent<TextMeshProUGUI>();

        //    if (ally.EquippedWeapon != null || ally.EquippedWeapon.ID != "nothing")
        //    {
        //        backGroundColor.color = Color.white;
        //        Weapon weapon = ally.EquippedWeapon;
        //        //ユニーク武器
        //        if (weapon.IsUnique)
        //        {
        //            weaponName.text = $"武器1:<color=yellow>{weapon.Name}+{weapon.AdditionalPoint}";
        //        }
        //        else
        //        {
        //            weaponName.text = $"武器1:{weapon.Name}+{weapon.AdditionalPoint}";
        //        }

        //        for (int j = 0; j < 8; j++)
        //        {
        //            Transform slot = slotsPanel.Find("Slot" + (j + 1).ToString());

        //            string gemAdditionalPoint = "";
        //            int gemAdditionalPointValue = 0;
        //            string gemStatusUp = "";
        //            Dictionary<Status, int> gemStatusUpDict = new Dictionary<Status, int>
        //                    {
        //                        {Status.STR, 0}
        //                        ,{Status.DEF, 0}
        //                        ,{Status.INT, 0}
        //                        ,{Status.MNT, 0}
        //                        ,{Status.TEC, 0}
        //                        ,{Status.AGI, 0}
        //                        ,{Status.LUK, 0}
        //                    };
        //            string gemEffectiveness = "";
        //            string gemAilments = "";
        //            //string gemBattleStart = "";
        //            //e.eventID = EventTriggerType.PointerClick;

        //            if (j < weapon.GemSlotsNum)
        //            {
        //                Image image = slot.GetComponent<Image>();
        //                TextMeshProUGUI text = slot.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
        //                slot.gameObject.SetActive(true);
        //                if (j < weapon.Gems.Count)
        //                {
        //                    //スロットに表示される文字
        //                    if (weapon.Gems[j].PlusValue != 0)
        //                    {
        //                        text.text = "攻";
        //                        gemAdditionalPointValue += weapon.Gems[j].PlusValue;
        //                    }
        //                    else if (weapon.Gems[j].PlusStatus.value != 0)
        //                    {
        //                        gemStatusUpDict[weapon.Gems[j].PlusStatus.status]
        //                            += weapon.Gems[j].PlusStatus.value;
        //                        switch (weapon.Gems[j].PlusStatus.status)
        //                        {
        //                            case Status.STR:
        //                                text.text = "筋";
        //                                break;
        //                            case Status.DEF:
        //                                text.text = "耐";
        //                                break;
        //                            case Status.INT:
        //                                text.text = "知";
        //                                break;
        //                            case Status.MNT:
        //                                text.text = "精";
        //                                break;
        //                            case Status.TEC:
        //                                text.text = "技";
        //                                break;
        //                            case Status.AGI:
        //                                text.text = "敏";
        //                                break;
        //                            case Status.LUK:
        //                                text.text = "運";
        //                                break;
        //                        }
        //                    }
        //                    else if (weapon.Gems[j].Ailment.ailment != Ailment.nothing)
        //                    {
        //                        gemAilments += weapon.Gems[j].Ailment.ailment.GetAilmentName();
        //                        switch (weapon.Gems[j].Ailment.ailment)
        //                        {
        //                            case Ailment.bleeding:
        //                                text.text = "血";
        //                                break;
        //                            case Ailment.burn:
        //                                text.text = "火";
        //                                break;
        //                            case Ailment.confusion:
        //                                text.text = "混";
        //                                break;
        //                            case Ailment.curse:
        //                                text.text = "呪";
        //                                break;
        //                            case Ailment.frost:
        //                                text.text = "氷";
        //                                break;
        //                            case Ailment.paralysis:
        //                                text.text = "痺";
        //                                break;
        //                            case Ailment.poison:
        //                                text.text = "毒";
        //                                break;
        //                            case Ailment.seal:
        //                                text.text = "封";
        //                                break;
        //                            case Ailment.sleep:
        //                                text.text = "眠";
        //                                break;
        //                            case Ailment.stun:
        //                                text.text = "絶";
        //                                break;
        //                            case Ailment.terror:
        //                                text.text = "怖";
        //                                break;
        //                            case Ailment.turnorver:
        //                                text.text = "転";
        //                                break;
        //                        }
        //                    }
        //                    else if (weapon.Gems[j].Effectiveness.effectiveness != EnemyType.normal)
        //                    {
        //                        gemEffectiveness += weapon.Gems[j].Effectiveness.effectiveness.GetEffectivenessName();
        //                        switch (weapon.Gems[j].Effectiveness.effectiveness)
        //                        {
        //                            case EnemyType.beast:
        //                                text.text = "獣";
        //                                break;
        //                            case EnemyType.dragon:
        //                                text.text = "龍";
        //                                break;
        //                            case EnemyType.flying:
        //                                text.text = "飛";
        //                                break;
        //                            case EnemyType.human:
        //                                text.text = "人";
        //                                break;
        //                            case EnemyType.magic:
        //                                text.text = "魔";
        //                                break;
        //                            case EnemyType.nature:
        //                                text.text = "自";
        //                                break;
        //                            case EnemyType.undead:
        //                                text.text = "屍";
        //                                break;
        //                        }
        //                    }

        //                    //呪われていなければ青色に
        //                    if (weapon.Gems[j].PlusStatus.value > 0 ||
        //                        weapon.Gems[j].Ailment.ailment != Ailment.nothing ||
        //                        weapon.Gems[j].Effectiveness.effectiveness != EnemyType.normal)
        //                    {
        //                        image.sprite = plusSlot;
        //                    }
        //                    else
        //                    {
        //                        image.sprite = minusSlot;
        //                    }

        //                    string mergedGemText = "";
        //                    mergedGemText += gemAdditionalPoint;
        //                    if (gemAdditionalPointValue != 0) mergedGemText += gemAdditionalPointValue.ToString();
        //                    mergedGemText += gemStatusUp;
        //                    foreach (KeyValuePair<Status, int> kvp in gemStatusUpDict)
        //                    {
        //                        if (kvp.Value != 0) mergedGemText += kvp.Key.GetStatusName() + "　";
        //                    }
        //                    if (gemEffectiveness != "")
        //                    {
        //                        mergedGemText += gemEffectiveness + "特効";
        //                    }
        //                    else if (gemAilments != "")
        //                    {
        //                        mergedGemText += gemAilments + "攻撃";
        //                    }

        //                    Debug.Log(mergedGemText);
        //                    EventTrigger e = slot.GetComponent<EventTrigger>();
        //                    EventTrigger.Entry entry = new EventTrigger.Entry();
        //                    entry.eventID = EventTriggerType.PointerClick;
        //                    entry.callback.AddListener((x) => OnClickGem(mergedGemText));
        //                    e.triggers.Add(entry);

        //                }
        //                //スロットが空いている場合
        //                else
        //                {
        //                    text.text = "";
        //                    image.sprite = emptySlot;
        //                }
        //            }
        //            else
        //            {
        //                slot.gameObject.SetActive(false);
        //            }
        //        }

        //    }
        //    //武器なし
        //    else
        //    {
        //        weaponName.text = "武器1:無し";
        //        backGroundColor.color = Color.black;
        //    }
        //}

        //public void SetGuardGemPanel(Ally ally)
        //{
        //    Transform equipmentPanel = allyPanel.Find("EquipmentPanel");

        //    for (int j = 0; j < 3; j++)
        //    {
        //        TextMeshProUGUI guardName;
        //        Transform guardPanel = equipmentPanel.Find("ParentPanel/Equipments/Guard" + (j + 1).ToString());
        //        Image backGroundColor = guardPanel.GetComponent<Image>();
        //        TextMeshProUGUI weaponName = guardPanel.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        //        Transform slotsPanel = guardPanel.Find("SlotsPanel");

        //        backGroundColor = guardPanel.GetComponent<Image>();
        //        guardName = guardPanel.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        //        slotsPanel = guardPanel.Find("SlotsPanel");

        //        //装備があれば
        //        if (ally.EquippedGuards.Count > j + 1)
        //        {
        //            backGroundColor.color = Color.white;
        //            Guard guard = ally.EquippedGuards[j];
        //            //ユニーク
        //            if (guard.IsUnique)
        //            {
        //                guardName.text = $"防具1:　<color=yellow>{guard.Name}+{guard.AdditionalPoint}";
        //            }
        //            else
        //            {
        //                guardName.text = $"防具1:　{guard.Name}+{guard.AdditionalPoint}";
        //            }

        //            for (int k = 0; k < 8; k++)
        //            {
        //                Transform slot = slotsPanel.Find("Slot" + (k + 1).ToString());
        //                if (k < guard.GemSlotsNum)
        //                {
        //                    slot.gameObject.SetActive(true);
        //                    Image image = slot.GetComponent<Image>();
        //                    TextMeshProUGUI text = slot.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
        //                    if (k < guard.Gems.Count)
        //                    {
        //                        //スロットに表示される文字
        //                        if (guard.Gems[k].PlusValue != 0)
        //                        {
        //                            text.text = "守";
        //                        }
        //                        else if (guard.Gems[k].PlusStatus.value != 0)
        //                        {
        //                            switch (guard.Gems[k].PlusStatus.status)
        //                            {
        //                                case Status.STR:
        //                                    text.text = "筋";
        //                                    break;
        //                                case Status.DEF:
        //                                    text.text = "耐";
        //                                    break;
        //                                case Status.INT:
        //                                    text.text = "知";
        //                                    break;
        //                                case Status.MNT:
        //                                    text.text = "精";
        //                                    break;
        //                                case Status.TEC:
        //                                    text.text = "技";
        //                                    break;
        //                                case Status.AGI:
        //                                    text.text = "敏";
        //                                    break;
        //                                case Status.LUK:
        //                                    text.text = "運";
        //                                    break;
        //                            }
        //                        }
        //                        else if (guard.Gems[k].Ailment.ailment != Ailment.nothing)
        //                        {
        //                            switch (guard.Gems[k].Ailment.ailment)
        //                            {
        //                                case Ailment.bleeding:
        //                                    text.text = "血";
        //                                    break;
        //                                case Ailment.burn:
        //                                    text.text = "火";
        //                                    break;
        //                                case Ailment.confusion:
        //                                    text.text = "混";
        //                                    break;
        //                                case Ailment.curse:
        //                                    text.text = "呪";
        //                                    break;
        //                                case Ailment.frost:
        //                                    text.text = "凍";
        //                                    break;
        //                                case Ailment.paralysis:
        //                                    text.text = "痺";
        //                                    break;
        //                                case Ailment.poison:
        //                                    text.text = "毒";
        //                                    break;
        //                                case Ailment.seal:
        //                                    text.text = "封";
        //                                    break;
        //                                case Ailment.sleep:
        //                                    text.text = "眠";
        //                                    break;
        //                                case Ailment.stun:
        //                                    text.text = "絶";
        //                                    break;
        //                                case Ailment.terror:
        //                                    text.text = "怖";
        //                                    break;
        //                                case Ailment.turnorver:
        //                                    text.text = "転";
        //                                    break;
        //                            }
        //                        }
        //                        else if (guard.Gems[k].Attribution.attribution != Attribution.nothing)
        //                        {
        //                            switch (guard.Gems[k].Attribution.attribution)
        //                            {
        //                                case Attribution.fire:
        //                                    text.text = "炎";
        //                                    break;
        //                                case Attribution.ice:
        //                                    text.text = "氷";
        //                                    break;
        //                                case Attribution.thunder:
        //                                    text.text = "雷";
        //                                    break;
        //                                case Attribution.dark:
        //                                    text.text = "闇";
        //                                    break;
        //                                case Attribution.holy:
        //                                    text.text = "聖";
        //                                    break;
        //                            }
        //                        }

        //                        //呪われていなければ青色に
        //                        if (guard.Gems[k].PlusStatus.value > 0 ||
        //                            guard.Gems[k].Ailment.ailment != Ailment.nothing ||
        //                            guard.Gems[k].Attribution.attribution != Attribution.nothing)
        //                        {
        //                            image.sprite = plusSlot;
        //                        }
        //                        else
        //                        {
        //                            image.sprite = minusSlot;
        //                        }
        //                    }
        //                    //スロットが空いている場合
        //                    else
        //                    {
        //                        text.text = "";
        //                        image.sprite = emptySlot;
        //                    }
        //                }
        //                else
        //                {
        //                    slot.gameObject.SetActive(false);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            guardName.text = "防具" + (j + 1).ToString() + ":無し";
        //            backGroundColor.color = Color.black;
        //        }

        //    }
        //}

        //public void SetWeaponGems2(Ally ally)
        //{

        //    Transform weaponPanel = statusPanel.Find("ScrollView/Viewport/Content/Weapon");
        //    Image backGroundColor = weaponPanel.GetComponent<Image>();
        //    TextMeshProUGUI weaponName = weaponPanel.Find("NameLabel").GetComponent<TextMeshProUGUI>();
        //    Transform slotsPanel = weaponPanel.Find("SlotsPanel");
        //    TextMeshProUGUI statusText = weaponPanel.Find("Status").GetComponent<TextMeshProUGUI>();

        //    //武器情報
        //    if (ally.EquippedWeapon != null)
        //    {
        //        backGroundColor.color = new Color(1f, 1f, 1f, 0.15f);
        //        Weapon weapon = ally.EquippedWeapon;
        //        //ユニーク武器
        //        if (weapon.IsUnique)
        //        {
        //            weaponName.text = $"武器1:　<color=yellow>{weapon.Name}+{weapon.AdditionalPoint}";
        //        }
        //        else
        //        {
        //            weaponName.text = $"武器1:　{weapon.Name}+{weapon.AdditionalPoint}";
        //        }

        //        statusText.text = "攻撃力+" + weapon.GetAddedValue();

        //        if (weapon.HitAboidProb != 0)
        //        {
        //            statusText.text += $"　命中+{weapon.HitAboidProb}";
        //        }

        //        //ステータス上昇値
        //        foreach (KeyValuePair<Status, int> kvp in weapon.UppedStatuses)
        //        {
        //            if (kvp.Value != 0)
        //            {
        //                statusText.text += $"　{kvp.Key.GetStatusName()}+{kvp.Value}";
        //            }
        //        }
        //        //特攻
        //        foreach (EnemyType e in weapon.Effectivenesses)
        //        {
        //            statusText.text += $"　{e.GetEffectivenessName()}特攻";
        //        }

        //        //追加攻撃
        //        foreach (Ailment a in weapon.AdditionalAilments)
        //        {
        //            statusText.text += $"　{a.GetAilmentName()}攻撃";
        //        }

        //        for (int i = 0; i < 12; i++)
        //        {
        //            Transform slot = slotsPanel.Find("Slot" + (i + 1).ToString());

        //            string gemAdditionalPoint = "";
        //            int gemAdditionalPointValue = 0;
        //            string gemStatusUp = "";
        //            Dictionary<Status, int> gemStatusUpDict = new Dictionary<Status, int>
        //        {
        //                {Status.STR, 0},
        //                {Status.DEF, 0},
        //                {Status.INT, 0},
        //                {Status.MNT, 0},
        //                {Status.TEC, 0},
        //                {Status.AGI, 0},
        //                {Status.LUK, 0}
        //        };
        //            string gemEffectiveness = "";
        //            string gemAilments = "";
        //            string gemBattleStart = "";
        //            //e.eventID = EventTriggerType.PointerClick;

        //            if (i < weapon.GemSlotsNum)
        //            {
        //                Image image = slot.GetComponent<Image>();
        //                TextMeshProUGUI text = slot.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
        //                slot.gameObject.SetActive(true);
        //                if (i < weapon.Gems.Count)
        //                {
        //                    //スロットに表示される文字
        //                    if (weapon.Gems[i].PlusValue != 0)
        //                    {
        //                        text.text = "攻";
        //                        gemAdditionalPointValue += weapon.Gems[i].PlusValue;
        //                    }
        //                    else if (weapon.Gems[i].PlusStatus.value != 0)
        //                    {
        //                        gemStatusUpDict[weapon.Gems[i].PlusStatus.status]
        //                            += weapon.Gems[i].PlusStatus.value;
        //                        switch (weapon.Gems[i].PlusStatus.status)
        //                        {
        //                            case Status.STR:
        //                                text.text = "筋";
        //                                break;
        //                            case Status.DEF:
        //                                text.text = "耐";
        //                                break;
        //                            case Status.INT:
        //                                text.text = "知";
        //                                break;
        //                            case Status.MNT:
        //                                text.text = "精";
        //                                break;
        //                            case Status.TEC:
        //                                text.text = "技";
        //                                break;
        //                            case Status.AGI:
        //                                text.text = "敏";
        //                                break;
        //                            case Status.LUK:
        //                                text.text = "運";
        //                                break;
        //                        }
        //                    }
        //                    else if (weapon.Gems[i].Ailment.ailment != Ailment.nothing)
        //                    {
        //                        gemAilments += weapon.Gems[i].Ailment.ailment.GetAilmentName();
        //                        switch (weapon.Gems[i].Ailment.ailment)
        //                        {
        //                            case Ailment.bleeding:
        //                                text.text = "血";
        //                                break;
        //                            case Ailment.burn:
        //                                text.text = "火";
        //                                break;
        //                            case Ailment.confusion:
        //                                text.text = "混";
        //                                break;
        //                            case Ailment.curse:
        //                                text.text = "呪";
        //                                break;
        //                            case Ailment.frost:
        //                                text.text = "氷";
        //                                break;
        //                            case Ailment.paralysis:
        //                                text.text = "痺";
        //                                break;
        //                            case Ailment.poison:
        //                                text.text = "毒";
        //                                break;
        //                            case Ailment.seal:
        //                                text.text = "封";
        //                                break;
        //                            case Ailment.sleep:
        //                                text.text = "眠";
        //                                break;
        //                            case Ailment.stun:
        //                                text.text = "絶";
        //                                break;
        //                            case Ailment.terror:
        //                                text.text = "怖";
        //                                break;
        //                            case Ailment.turnorver:
        //                                text.text = "転";
        //                                break;
        //                        }
        //                    }
        //                    else if (weapon.Gems[i].Effectiveness.effectiveness != EnemyType.normal)
        //                    {
        //                        gemEffectiveness += weapon.Gems[i].Effectiveness.effectiveness.GetEffectivenessName();
        //                        switch (weapon.Gems[i].Effectiveness.effectiveness)
        //                        {
        //                            case EnemyType.beast:
        //                                text.text = "獣";
        //                                break;
        //                            case EnemyType.dragon:
        //                                text.text = "龍";
        //                                break;
        //                            case EnemyType.flying:
        //                                text.text = "飛";
        //                                break;
        //                            case EnemyType.human:
        //                                text.text = "人";
        //                                break;
        //                            case EnemyType.magic:
        //                                text.text = "魔";
        //                                break;
        //                            case EnemyType.nature:
        //                                text.text = "自";
        //                                break;
        //                            case EnemyType.undead:
        //                                text.text = "屍";
        //                                break;
        //                        }
        //                    }

        //                    //呪われていなければ青色に
        //                    if (weapon.Gems[i].PlusStatus.value > 0 ||
        //                        weapon.Gems[i].Ailment.ailment != Ailment.nothing ||
        //                        weapon.Gems[i].Effectiveness.effectiveness != EnemyType.normal)
        //                    {
        //                        image.sprite = plusSlot;
        //                    }
        //                    else
        //                    {
        //                        image.sprite = minusSlot;
        //                    }

        //                    string mergedGemText = "";
        //                    mergedGemText += gemAdditionalPoint;
        //                    if (gemAdditionalPointValue != 0) mergedGemText += gemAdditionalPointValue.ToString();
        //                    mergedGemText += gemStatusUp;
        //                    foreach (KeyValuePair<Status, int> kvp in gemStatusUpDict)
        //                    {
        //                        if (kvp.Value != 0) mergedGemText += kvp.Key.GetStatusName() + "　";
        //                    }
        //                    if (gemEffectiveness != "")
        //                    {
        //                        mergedGemText += gemEffectiveness + "特効";
        //                    }
        //                    else if (gemAilments != "")
        //                    {
        //                        mergedGemText += gemAilments + "攻撃";
        //                    }


        //                    Debug.Log(mergedGemText);

        //                    EventTrigger e = slot.GetComponent<EventTrigger>();
        //                    EventTrigger.Entry entry = new EventTrigger.Entry();
        //                    entry.eventID = EventTriggerType.PointerClick;
        //                    entry.callback.AddListener((x) => OnClickGem(mergedGemText));
        //                    e.triggers.Add(entry);

        //                }
        //                //スロットが空いている場合
        //                else
        //                {
        //                    text.text = "";
        //                    image.sprite = emptySlot;
        //                }
        //            }
        //            else
        //            {
        //                slot.gameObject.SetActive(false);
        //            }
        //        }

        //    }
        //    //武器なし
        //    else
        //    {
        //        weaponName.text = "武器1:";
        //        statusText.text = "";
        //        backGroundColor.color = new Color(0f, 0f, 0f, 0.15f);
        //    }
        //}

        //public void SetGuardsGems2(Ally ally)
        //{

        //    Transform guardPanel;
        //    Image backGroundColor;
        //    TextMeshProUGUI guardName;
        //    Transform slotsPanel;
        //    TextMeshProUGUI statusText;

        //    for (int i = 0; i < 3; i++)
        //    {
        //        guardPanel = statusPanel.Find("ScrollView/Viewport/Content/Guard" + (i + 1).ToString());
        //        backGroundColor = guardPanel.GetComponent<Image>();
        //        guardName = guardPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        //        slotsPanel = guardPanel.Find("SlotsPanel");
        //        statusText = guardPanel.Find("Status").GetComponent<TextMeshProUGUI>();

        //        //装備があれば
        //        if (i < ally.EquippedGuards.Count)
        //        {
        //            backGroundColor.color = new Color(1f, 1f, 1f, 0.15f);
        //            Guard guard = ally.EquippedGuards[i];
        //            //ユニーク
        //            if (guard.IsUnique)
        //            {
        //                guardName.text = $"防具1:　<color=yellow>{guard.Name}+{guard.AdditionalPoint}";
        //            }
        //            else
        //            {
        //                guardName.text = $"防具1:　{guard.Name}+{guard.AdditionalPoint}";
        //            }

        //            statusText.text = "防御力+" + guard.GetAddedValue();

        //            if (guard.HitAboidProb != 0)
        //            {
        //                statusText.text += $"　回避+{guard.HitAboidProb}";
        //            }

        //            //ステータス上昇値
        //            foreach (KeyValuePair<Status, int> kvp in guard.UppedStatuses)
        //            {
        //                if (kvp.Value != 0)
        //                {
        //                    statusText.text += $"　{kvp.Key.GetStatusName()}+{kvp.Value}";
        //                }
        //            }
        //            ////特攻
        //            //foreach (EffectivenessType e in guard.Effectivenesses)
        //            //{
        //            //    statusText.text += $"　{e.GetEffectivenessName()}特攻";
        //            //}

        //            //追加攻撃
        //            foreach (Ailment r in guard.Ailments)
        //            {
        //                statusText.text += $"　{r.GetAilmentName()}無効";
        //            }
        //            foreach (Attribution a in guard.Attributions)
        //            {
        //                statusText.text += $"　{a.GetAttributionName()}耐性";
        //            }

        //            for (int j = 0; j < 12; j++)
        //            {
        //                Transform slot = slotsPanel.Find("Slot" + (j + 1).ToString());
        //                if (j < guard.GemSlotsNum)
        //                {
        //                    slot.gameObject.SetActive(true);
        //                    Image image = slot.GetComponent<Image>();
        //                    TextMeshProUGUI text = slot.Find("TextMeshPro Text").GetComponent<TextMeshProUGUI>();
        //                    if (j < guard.Gems.Count)
        //                    {
        //                        //スロットに表示される文字
        //                        if (guard.Gems[j].PlusValue != 0)
        //                        {
        //                            text.text = "守";
        //                        }
        //                        else if (guard.Gems[j].PlusStatus.value != 0)
        //                        {
        //                            switch (guard.Gems[j].PlusStatus.status)
        //                            {
        //                                case Status.STR:
        //                                    text.text = "筋";
        //                                    break;
        //                                case Status.DEF:
        //                                    text.text = "耐";
        //                                    break;
        //                                case Status.INT:
        //                                    text.text = "知";
        //                                    break;
        //                                case Status.MNT:
        //                                    text.text = "精";
        //                                    break;
        //                                case Status.TEC:
        //                                    text.text = "技";
        //                                    break;
        //                                case Status.AGI:
        //                                    text.text = "敏";
        //                                    break;
        //                                case Status.LUK:
        //                                    text.text = "運";
        //                                    break;
        //                            }
        //                        }
        //                        else if (guard.Gems[j].Ailment.ailment != Ailment.nothing)
        //                        {
        //                            switch (guard.Gems[j].Ailment.ailment)
        //                            {
        //                                case Ailment.bleeding:
        //                                    text.text = "血";
        //                                    break;
        //                                case Ailment.burn:
        //                                    text.text = "火";
        //                                    break;
        //                                case Ailment.confusion:
        //                                    text.text = "混";
        //                                    break;
        //                                case Ailment.curse:
        //                                    text.text = "呪";
        //                                    break;
        //                                case Ailment.frost:
        //                                    text.text = "凍";
        //                                    break;
        //                                case Ailment.paralysis:
        //                                    text.text = "痺";
        //                                    break;
        //                                case Ailment.poison:
        //                                    text.text = "毒";
        //                                    break;
        //                                case Ailment.seal:
        //                                    text.text = "封";
        //                                    break;
        //                                case Ailment.sleep:
        //                                    text.text = "眠";
        //                                    break;
        //                                case Ailment.stun:
        //                                    text.text = "絶";
        //                                    break;
        //                                case Ailment.terror:
        //                                    text.text = "怖";
        //                                    break;
        //                                case Ailment.turnorver:
        //                                    text.text = "転";
        //                                    break;
        //                            }
        //                        }
        //                        else if (guard.Gems[j].Attribution.attribution != Attribution.nothing)
        //                        {
        //                            switch (guard.Gems[j].Attribution.attribution)
        //                            {
        //                                case Attribution.fire:
        //                                    text.text = "炎";
        //                                    break;
        //                                case Attribution.ice:
        //                                    text.text = "氷";
        //                                    break;
        //                                case Attribution.thunder:
        //                                    text.text = "雷";
        //                                    break;
        //                                case Attribution.dark:
        //                                    text.text = "闇";
        //                                    break;
        //                                case Attribution.holy:
        //                                    text.text = "聖";
        //                                    break;
        //                            }
        //                        }

        //                        //呪われていなければ青色に
        //                        if (guard.Gems[j].PlusStatus.value > 0 ||
        //                            guard.Gems[j].Ailment.ailment != Ailment.nothing ||
        //                            guard.Gems[j].Attribution.attribution != Attribution.nothing)
        //                        {
        //                            image.sprite = plusSlot;
        //                        }
        //                        else
        //                        {
        //                            image.sprite = minusSlot;
        //                        }
        //                    }
        //                    //スロットが空いている場合
        //                    else
        //                    {
        //                        text.text = "";
        //                        image.sprite = emptySlot;
        //                    }
        //                }
        //                else
        //                {
        //                    slot.gameObject.SetActive(false);
        //                }
        //            }

        //        }
        //        else
        //        {
        //            guardName.text = "防具:" + (i + 1).ToString();
        //            statusText.text = "";
        //            backGroundColor.color = new Color(0f, 0f, 0f, 0.15f);
        //        }
        //    }
        //}
    }
}
