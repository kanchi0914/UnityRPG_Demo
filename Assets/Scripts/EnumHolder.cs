using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumHolder
{
    public enum EventScriptType { text, option }

    //種族
    public enum Race { 人間, ドワーフ, エルフ, オーガ, コボルト, デビル }
    //クラス
    public enum Job { nothing, warrior, paladin
            , mage, cleric, thief, ranger
            ,alchemist, shaman}


    public static string GetJobName(this Job job)
    {
        string[] names = { "無し", "ウォリアー", "パラディン", "メイジ", "クレリック"
                , "シーフ", "レンジャー", "アルケミスト", "シャーマン" };
        return names[(int)job];
    }


    public enum Personality { normal, offensive, defensive, intellectual }

    //ステータス
    public enum Status {Lv, MaxHP, MaxSP, currentHP, currentSP, STR, DEF
            , INT, MNT, TEC, AGI, LUK}

    public static string GetStatusName(this Status status)
    {
        string[] names = { "Lv", "最大HP", "最大SP", "HP"
                , "SP", "筋力", "耐久", "知性", "精神"
                , "技術", "敏捷", "運"};
        return names[(int)status];
    }

    public enum AbilityType { item, skill }

    //アイテムの種類
    public enum ItemType { weapon, guard, accessary, gem, material, drug, book }

    //スキルの発動タイプ
    public enum SkillType { active, passive }
    //アビリティのタイプ
    public enum DamageType { nothing, damage, hpHeal, spHeal, cure, ailment, resurrection, buff, other }
    //ターゲット
    public enum Target { nothing, opponent, ally, self }
    //単体か、全体か
    public enum Scope { nothing, single, random, entire }

    public enum EnemySkillName { 攻撃, 殴りつける }

    public enum SkillName { 無し, 何もしない, 混乱, 眠り状態, 攻撃, 防御, 逃走,
        //敵のスキル
        金切り声, 
        //ウォリアー
        ソニックバット, なぎ払い, テンションアップ, パワースマッシュ, メテオストライク
            , 疾風の舞, 怒涛の舞, ハイボルテージ, チャージ, 狂戦士の心, 捨て身,
            //パラディン
        岩石割り, 挑発, 防御の心得, かばう, 狙いすまし, セイントブレイカー, 魔法ガード, 防御陣形,
        深呼吸, 呼吸法, 静かなる怒り, スロースターター, パリィ, 対魔の構え, 反撃の構え,
        カウンター, 感覚共有, 不屈の闘志, リフレッシュ,
        //クレリック
        ヒール, ヒールオール, キュア, キュアオール, リザレクション, リフレッシュオール,
        ホーリーアロー, ホーリーライト, 魔法バリア, 戦後治療, SP自然回復上昇, 
        聖職者の矜持, 天啓, 加護の光,  
        //メイジ
        採集, 精製, 蒸留, 道具知識, 植物知識, SP回復攻撃, 二刀流, ポイズン, スリープ,
        状態異常の心得,ファイアボール, フロストウェーブ, サンダーブレイク,
        フレア, テンペスト, ブリザード, シェルアーマー, 弱点知識, 詠唱,
        集中, 炎の極意, 雷の極意, 氷の極意, メテオバーン, アルティメットバーン, 魔法フィールド,
        オーラ, 魔導を極めし者, テレポート, ワープホール,
        //レンジャー
        ウィングショット, 乱れ打ち, ファストステップ, オートコントロール, 生物知識,
        警戒, 倹約家, 商売上手, 閃き, アイテム守り, テクニシャン, 罠解除, 種族マスター, 

        //シャーマン
        ダークネス, ナイトメア, テラーボイス, ファントムボイス, サイレンス, ホワイトノイズ, 
        蘇生術, 呪殺, エナジードレイン, デッドパーティ, 悪魔の契約, 血の契約, 呪いの儀式,
        封魔の儀式, 呪詛返し, 黄泉返り, 未来蘇生, 死の呪い, 復活者, 呪解, 追い打ち,
        異常吸収, 免疫力, 
        //シーフ
        闇に生きる者,
        粘液, 殴りつける
            , 毒攻撃, 呪い攻撃, 氷攻撃, 即死攻撃, 噛みつき
            , 痺れる粉, 毒の粉, 眠りの歌, 叫び声
            , 炎のブレス, 地ならし,
    };

    public static string ConvertSkillNameToString(SkillName e)
    {
        string s = Enum.GetName(typeof(SkillName), e);
        return s;
    }

    //アイテムID
    public enum ItemName { 無し, 粗悪な油, 薬草, 上薬草, 特薬草, 特効薬, 毒消し草, 万能薬, 魔力の粉, 魔力の丸薬, 魔力の源, 輝く粉
            , 毒ビン, 火炎ビン, 電撃ビン, 氷結ビン }


    //スキル

    //物理攻撃か、魔法攻撃か
    public enum AttackType { nothing, physics, magic }
    //属性
    public enum Attribution { nothing, fire, thunder, ice, holy, dark }
    public static string GetAttributionName(this Attribution attribution)
    {
        string[] names = { "無", "火炎", "電撃", "氷結", "聖", "闇"};
        return names[(int)attribution];
    }



    //装備の種類
    public enum EquipType { weapon, guard }

    public enum GuardType { armor, accessory }

    public enum WeaponAbility
    {
        magic_damage_up,
        ailment_per_up,
        critial_per_up,
        hpHeal_on_attack,
        spHeal_on_attack,
        down_sp_consumption
    }
    public static string GetWeaponAbilityName(this WeaponAbility weaponAbility)
    {
        string[] names = {
            "魔法威力アップ",
            "回復効果アップ",
            "アイテム効果アップ",
            "状態異常付与率アップ",
            "クリティカル率アップ",
            "攻撃時にHP回復",
            "通常攻撃時にSP回復",
            "消費SPダウン"
        };
        return names[(int)weaponAbility];
    }

    public enum GuardAbility
    {
        
    }
    public static string GetGuardAbilityName(this GuardAbility guardAbility)
    {
        string[] names = {
            "被クリティカル率ダウン"
            ,"戦闘終了時にHP回復"
            ,"移動時にHP回復"
        };
        return names[(int)guardAbility];
    }

    //特効
    public enum EnemyType { normal, undead, dragon, human, flying, nature, magic, beast, water }
    public static string GetEffectivenessName(this EnemyType effectiveness)
    {
        string[] names = { "無し", "アンデッド系", "ドラゴン系", "人型系", "飛行系"
                , "自然系", "魔法系", "獣", "水棲系"};
        return names[(int)effectiveness];
    }

    //状態異常の種類
    public enum Ailment
    {
        nothing, poison, sleep, paralysis, confusion
            , seal, terror, stun, death
    }

    public static string GetAilmentName(this Ailment ailment)
    {
        string[] names = { "無し", "毒", "睡眠", "麻痺"
                , "混乱", "封印", "恐怖", "スタン", "即死" };
        return names[(int)ailment];
    }
    //public static string GetAilmentName(this Ailment ailment)
    //{
    //    string[] names = { "無し", "毒", "睡眠", "麻痺"
    //            , "混乱", "封印", "恐怖", "呪い", "出血"
    //            , "転倒", "凍結", "火傷", "スタン", "即死" };
    //    return names[(int)ailment];
    //}

    public static int ChangeMinusValueInNeed(int value)
    {
        if (value < 0) return 0;
        else return value;
    }

    public static string GetRaceName(this Race race)
    {
        string[] names = { "人間", "ドワーフ", "エルフ", "オーガ"
                , "コボルト", "デビル" };
        return names[(int)race];
    }



}
