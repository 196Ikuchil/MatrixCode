using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllosaurusStatusInit : MonoBehaviour
{

    EnemyStatus status;
    GameMasterScript gameMaster;
    public AllosaurusData wData;
    AbilityList aList;
    AbilityReader aReader;

    private List<EnemyStatus.SkillData> sData = new List<EnemyStatus.SkillData>();
    private List<Ability> aData = new List<Ability>();
    int Lv = 1;

    int[] abilityNum = new int[3] { 74, 79, 84 };//Listの横列の番号-2を指定
                                                 // Use this for initialization

    void Awake()
    {
        status = gameObject.GetComponent<EnemyStatus>();
        gameMaster = FindObjectOfType<GameMasterScript>();
        aReader = FindObjectOfType<AbilityReader>();
        Lv = gameMaster.GetEnemyLV();
    }
    void Start()
    {
        status.SetHP((int)wData.sheets[0].list[0].value + 100 * Lv);
        status.SetPower((int)wData.sheets[0].list[1].value + 5 * Lv);
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
