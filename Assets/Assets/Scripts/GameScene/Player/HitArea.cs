using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {
    public string receiverName="Enemy";//攻撃のあたり判定に使用 Playerはここを書き換える
	void Damage(AttackArea.AttackInfo attackInfo)
	{
		transform.root.SendMessage ("Damage",attackInfo);
	}
}
