﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using static EnumHolder;
using System;
using UnityEngine;
using System.Linq;

public class SkillGenerator2
{

    private GameController gameController;

    private TextAsset csvFile; // CSVファイル
    private List<string[]> csvData = new List<string[]>(); // CSVの中身を入れるリスト

    private List<Skill> skillInfos = new List<Skill>();

    private int height = 0; // CSVの行数

    public List<Skill> SkillInfos { get => skillInfos; set => skillInfos = value; }

    public SkillGenerator2(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void Load()
    {
        Convert.ToBoolean("true");

        csvFile = Resources.Load("CsvData/skillData_temp") as TextAsset; /* Resouces/CSV下のCSV読み込み */
        StringReader reader = new StringReader(csvFile.text);

        reader.ReadLine();
        reader.ReadLine();

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            csvData.Add(line.Split(',')); // リストに入れる
        }

        AddSkillInfos();

        Debug.Log("");
    }

    public void AddSkillInfos()
    {
        foreach (string[] s in csvData)
        {
            bool exists = Enum.IsDefined(typeof(SkillName), s[0]);
            if (exists)
            {
                Skill skill = Generate(s[0]);
                skillInfos.Add(skill);
            }
        }
    }

    public Skill Generate(string skillName)
    {
        Item item = new Item();
        Skill skill = new Skill();
        string[] data = csvData.Find(s => s[0] == skillName);

        //IDは使わない
        skill.ID = "data[0]";

        skill.SkillName = (SkillName)Enum.Parse(typeof(SkillName), skillName, true);
        skill.Name = skillName;

        //skill.SpConsumptions = int.Parse(data[1].Split('/'));

        if (data[1].Contains("/"))
        {
            skill.SpConsumptions = Array.ConvertAll(data[1].Split('/'), x => int.Parse(x));
        }
        else
        {
            skill.SpConsumptions = Enumerable.Repeat<int>(int.Parse(data[1]), 3).ToArray();
        }

        if (data[2].Contains("/"))
        {
            skill.ValueByLv = Array.ConvertAll(data[2].Split('/'), x => int.Parse(x));
        }
        else
        {
            skill.ValueByLv = Enumerable.Repeat<int>(int.Parse(data[2]), 3).ToArray();
        }

        //物理か魔法か
        skill.AttackType = (AttackType)Enum.Parse(typeof(AttackType), data[3], true);
        //属性
        if (string.IsNullOrEmpty(data[4]))
        {
            skill.Attribution = Attribution.nothing;
        }
        else
        {
            skill.Attribution = (Attribution)Enum.Parse(typeof(Attribution), data[4], true);
        }
        //追加効果
        if (string.IsNullOrEmpty(data[5]))
        {
            skill.AdditionalAilment = Ailment.nothing;
        }
        else
        {
            skill.AdditionalAilment = (Ailment)Enum.Parse(typeof(Ailment), data[5], true);
        }

        //説明文
        skill.Description = data[6];

        //使用メッセージ
        skill.Message1 = data[7];

        //スキルの発動タイプ
        skill.SkillType = (SkillType)Enum.Parse(typeof(SkillType), data[8], true);

        //発動可能な場面
        skill.IsAvairableOnBattle = Convert.ToBoolean(data[9]);
        skill.IsAvairableOnFloor = Convert.ToBoolean(data[10]);

        //対象
        skill.Target = (Target)Enum.Parse(typeof(Target), data[11], true);

        //範囲
        skill.Scope = (Scope)Enum.Parse(typeof(Scope), data[12], true);

        //ダメージタイプ？

        //職業
        skill.JobType = (Job)Enum.Parse(typeof(Job), data[14], true);

        return skill;

    }

}