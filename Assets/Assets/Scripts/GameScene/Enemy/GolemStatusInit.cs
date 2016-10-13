using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GolemStatusInit : MonoBehaviour
{

    EnemyStatus status;
    GameMasterScript gameMaster;
    public GolemData wData;
    AbilityList aList;
    AbilityReader aReader;

    private List<EnemyStatus.SkillData> sData = new List<EnemyStatus.SkillData>();
    private List<Ability> aData = new List<Ability>();
    int Lv = 1;

    int[] ability1 = new int[11] { 83,88,93,111,115,134,148,170,179,188,193};
    int[] ability2 = new int[11] { 83, 88, 93, 112, 115, 134, 148,171, 180,183, 193 };
    int[] ability3 = new int[14] { 83, 88, 93,101, 112, 115, 136, 149, 171, 180, 183, 189, 194,200 };
    //int[] ability3 = new int[6] { 88, 109, 148, 169, 180, 190 };
    int[] abilityNum;//Listの横列の番号-2を指定

    void Awake()
    {
        status = gameObject.GetComponent<EnemyStatus>();
        gameMaster = FindObjectOfType<GameMasterScript>();
        aReader = FindObjectOfType<AbilityReader>();
        Lv = gameMaster.GetEnemyLV();
        if (Lv > 50) abilityNum = ability3;
        else if (Lv > 40) abilityNum = ability2;
        else if (Lv > 10) abilityNum = ability1;
        else abilityNum = ability1;
    }
    void Start()
    {
        status.SetHP((int)wData.sheets[0].list[0].value + 470 * Lv);
        status.SetPower((int)wData.sheets[0].list[1].value + 18 * Lv);
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
