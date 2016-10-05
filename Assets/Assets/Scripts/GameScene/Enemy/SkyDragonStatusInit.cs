using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkyDragonStatusInit : MonoBehaviour
{

    EnemyStatus status;
    GameMasterScript gameMaster;
    public SkyDragonData wData;
    AbilityList aList;
    AbilityReader aReader;

    private List<EnemyStatus.SkillData> sData = new List<EnemyStatus.SkillData>();
    private List<Ability> aData = new List<Ability>();
    int Lv = 1;

    int[] ability1 = new int[9] { 77,89, 90, 96, 110, 119,127,146,182 };
    int[] ability2 = new int[9] { 78, 90, 91, 97, 111, 120, 128, 147, 183 };
    int[] ability3 = new int[10] { 78, 92, 93, 98, 112, 119, 121, 167, 183,185 };
    int[] abilityNum;//Listの横列の番号-2を指定

    void Awake()
    {
        status = gameObject.GetComponent<EnemyStatus>();
        gameMaster = FindObjectOfType<GameMasterScript>();
        aReader = FindObjectOfType<AbilityReader>();
        Lv = gameMaster.GetEnemyLV();
        if (Lv > 20) abilityNum = ability3;
        else if (Lv > 10) abilityNum = ability2;
        else abilityNum = ability1;
    }
    void Start()
    {
        status.SetHP((int)wData.sheets[0].list[0].value + 415 * Lv);
        status.SetPower((int)wData.sheets[0].list[1].value + 13 * Lv);
        status.SetDefence((int)wData.sheets[0].list[2].value + 1 * Lv);

        //スキルを保存
        for (int i = 4; i < wData.sheets[0].list[3].value; i++)
        {
            EnemyStatus.SkillData s = new EnemyStatus.SkillData(wData.sheets[0].list[i].value, wData.sheets[0].list[i].motion);
            sData.Add(s);
        }
        status.SetSkills(sData);

        //アビリティを保存 //ここでレベル指定すれば使う技を変更できる
        for (int i = 0; i < abilityNum.Length; i++)
        {
            Ability a = aReader.GetAbility(abilityNum[i]);
            aData.Add(a);
        }
        status.SetAbility(aData);
    }
}
