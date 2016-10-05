using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagicMaster : MonoBehaviour {

    string magic = "Prefabs/Magic/";
    public int StartMagicNumber = 75;
   
    //制限魔法
    const int maxLimitedMagicNum = 5;
    public List<GameObject> limitedMagicList;
    // Use this for initialization
	void Start () {
        limitedMagicList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void Magic(bool boost, float mig, Ability a, int power, Vector3 target, string attackerName)
    {
        Magic(boost, mig, a, power, target, attackerName, -1);
    }

    public void Magic(bool boost,float mig ,Ability a,int power,Vector3 target,string attackerName,int element)//指定位置に発生する系 
    {
        int id = a.Id;
         id-= StartMagicNumber;
        GameObject m= (GameObject)Instantiate(Resources.Load(magic+id.ToString()),target,Quaternion.identity);
        MagicScript ma = m.GetComponent<MagicScript>();
        ma.attackerName = attackerName;
        ma.CharaPower = power;
        ma.MagicPower = a.Attack;
        if (element != -1) ma.Element = element;
        else ma.Element = (int)a.element;
        if (boost) ma.elementPower = a.value * mig;
        else ma.elementPower = a.value;
    }

    public void Magic(bool boost,float mig, Ability a, int power, Vector3 user,string attackerName,Vector3 vector)
    {
        Magic(boost, mig, a, power, user,attackerName, vector, -1);
    }
    public void Magic(bool boost,float mig, Ability a, int power, Vector3 user,string attackerName,Vector3 vector,int element)//使用者から発射される系
    {
        int id = a.Id;
        id -= StartMagicNumber;
        GameObject m = (GameObject)Instantiate(Resources.Load(magic + id.ToString()), user,Quaternion.LookRotation((vector-user).normalized));
        MagicScript ma = m.GetComponent<MagicScript>();
        ma.attackerName= attackerName;
        ma.CharaPower = power;
        ma.MagicPower = a.Attack;
        if (element != -1) ma.Element = element;
        else ma.Element = (int)a.element;
        if (boost) ma.elementPower = a.value * mig;
        else ma.elementPower = a.value;
    }

    public void AddToLimitList(GameObject g)
    {
        if (limitedMagicList.Count >= maxLimitedMagicNum)//上限以上になったら古いのから消す
        {
            GameObject a=limitedMagicList[0];
            Destroy(a);
        }
        limitedMagicList.Add(g);
    }
    public void RemoveListObject(GameObject g)
    {
        limitedMagicList.RemoveAt(limitedMagicList.IndexOf(g));
    }
}
