using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyStatus : MonoBehaviour
{
    // 体力.
    public int HP = 100;
    public int MaxHP = 100;

    // 攻撃力.
    public int Power = 10;

    //攻撃の倍率
    public float mag = 1;

    //防御力
    public float Defence = 0;

    //弱点属性
    public int weakElement = 0;

    //攻撃属性
    public int attackElement = 0;

    public string characterName;

    //状態.
    public bool attacking = false;
    public bool Dodging = false;
    public bool died = false;

    public GameObject lastAttackTarget;

    //アビリティデータ
    private List<Ability> aData = new List<Ability>();

    //技の格納
    public class SkillData
    {
        public SkillData(float f,int m)
        {
            mag = f;
            motion = m;
        }
        public int motion;
        public float mag;
    }
    //スキルデータを保存
    private List<SkillData> sData = new List<EnemyStatus.SkillData>();

    void Start()
    {

    }

    void Update()
    {
       
    }

    public void SetHP(int h)
    {
        MaxHP = h;
        HP = h;
    }

 
    public void SetPower(int p)
    {
        Power = p;
    }

    public void SetDefence(int p)
    {
        Defence = p;
    }

    public void SetSkills(List<SkillData> s)
    {
        sData = s;
    }
    public void SetMag(float f)
    {
        mag = f;
    }
    public SkillData GetSkillData()//ランダムにスキルを返す
    {
        int i=Random.Range(0, sData.Count);
        return sData[i];
    }
    public void SetAbility(List<Ability> a)
    {
        aData = a;
    }
    public Ability GetAbility()
    {
        int i = Random.Range(0, aData.Count);
        return aData[i];
    }

}
