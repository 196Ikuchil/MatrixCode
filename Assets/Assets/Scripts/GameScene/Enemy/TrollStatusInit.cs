using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrollStatusInit : MonoBehaviour
{

    EnemyStatus status;
    GameMasterScript gameMaster;
    public TrollData wData;
    AbilityList aList;
    AbilityReader aReader;

    private List<EnemyStatus.SkillData> sData = new List<EnemyStatus.SkillData>();
    private List<Ability> aData = new List<Ability>();
    int Lv = 1;


    int[] ability1 = new int[5] { 74, 79, 89,94,172 };
    int[] ability2 = new int[5] { 75, 80, 90, 95, 173 };
    int[] ability3 = new int[5] { 76, 81, 91, 96, 174 };
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
        status.SetHP((int)wData.sheets[0].list[0].value + 380 * Lv);
        status.SetPower((int)wData.sheets[0].list[1].value + 15 * Lv);
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
