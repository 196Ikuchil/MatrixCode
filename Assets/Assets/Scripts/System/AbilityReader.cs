using UnityEngine;
using System.Collections;

public class Ability {
    public string Name { get; set; }
    public int Id { get; set; }
    public float Attack { get; set; }
    public float Sp { get; set; }
    public float benefit { get; set; }
    public float value { get; set; }
    public float element { get; set; }
    public bool isPhysic { get; set; }
    public float range { get; set; }
    public int motion { get; set; }
    public string memo { get; set; }
    public int ability { get; set; }
}


public class AbilityReader : MonoBehaviour {

    public AbilityList abilityListPref;
    AbilityList.Sheet abilityList;

	// Use this for initialization
	void Awake () {
        abilityList = abilityListPref.sheets[0];
	}

    public string GetAbilityName(int i)
    {
        return abilityList.list[i].Name;
    }
    public int GetAbilityId(int i)
    {
        return abilityList.list[i].id;
    }
    public float GetAbilityAttack(int i)
    {
        return abilityList.list[i].Attack;
    }
    public float GetAbilitySp(int i)
    {
        return abilityList.list[i].Sp;
    }
    public float GetAbilityValue(int i) //効果値を帰す
    {
        return abilityList.list[i].value;
    }
    public float GetAbilitybenefit(int i) //効果がある系ならその値をないのなら0を返す
    {
        if (abilityList.list[i].benefit == 0)
            return 0;
        else
            return abilityList.list[i].benefit;
    }
    public int GetAbilityAbility(int i)
    {
        return (int)abilityList.list[i].ability;
    }
    public float GetAbilityelement(int i) //エレメント番号を返す
    {
        return abilityList.list[i].element;
    }
    public float GetAbilityRange(int i)
    {
        return abilityList.list[i].range;
    }
    public int GetAbilityMotion(int i)
    {
        return (int)abilityList.list[i].motion;
    }
    public bool GetAbilityIsPhysic(int i)
    {
        if (abilityList.list[i].ability == 0)
            return true;
        else
            return false;
    }
    public string GetMemo(int i)
    {
        return abilityList.list[i].memo;
    }
    public Color GetAbilityColor(int i) //アビリティのコマンドにセットするカラーを返す
    {
        if (i == 0 || i == 1) return Color.white; //空、またはたたかうコマンド
        else if (this.GetAbilityIsPhysic(i)) return Color.red; //物理攻撃なら赤
        else if (this.GetAbilityelement(i) == 4) return Color.green; //回復なら緑
        else return Color.blue; //魔法系なら青
    }


    public Ability GetAbility(int i) //アビリティ情報一式を返す
    {
        Ability a=new Ability();
        a.Name = this.GetAbilityName(i);
        a.Id = this.GetAbilityId(i);
        a.Attack = this.GetAbilityAttack(i);
        a.Sp = this.GetAbilitySp(i);
        a.benefit = this.GetAbilitybenefit(i);
        a.value = this.GetAbilityValue(i);
        a.element = this.GetAbilityelement(i);
        a.isPhysic = this.GetAbilityIsPhysic(i);
        a.range = this.GetAbilityRange(i);
        a.motion = this.GetAbilityMotion(i);
        a.memo = this.GetMemo(i);
        a.ability = this.GetAbilityAbility(i);

        return a;
    }
}
