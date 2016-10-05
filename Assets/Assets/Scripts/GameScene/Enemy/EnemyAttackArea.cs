
using UnityEngine;
using System.Collections;

public class EnemyAttackArea : MonoBehaviour
{
    EnemyStatus status;

    void Start()
    {
        status = transform.root.GetComponent<EnemyStatus>();
    }

    /*
    public class AttackInfo
    {
        public int attackPower; // この攻撃の攻撃力.
        public Transform attacker; // 攻撃者.
    }*/


    // 攻撃情報を取得する.
     AttackArea.AttackInfo GetAttackInfo()
    {
        AttackArea.AttackInfo attackInfo = new AttackArea.AttackInfo();
        attackInfo.attackerName = "Enemy";
        // 攻撃力の計算.
        attackInfo.attackPower = (int)(status.Power*status.mag);
        attackInfo.element = status.attackElement;
        attackInfo.elementPower = 1.5f;
        attackInfo.attacker = transform.root;

        return attackInfo;
    }

    // 当たった.
    void OnTriggerEnter(Collider other)
    {
        // 攻撃が当たった相手のDamageメッセージをおくる.
        other.SendMessage("Damage", GetAttackInfo());
        // 攻撃した対象を保存.
        status.lastAttackTarget = other.transform.root.gameObject;
    }


    // 攻撃判定を有効にする.
    void OnAttack()
    {
        GetComponent<Collider>().enabled = true;
    }


    // 攻撃判定を無効にする.
    void OnAttackTermination()
    {
        GetComponent<Collider>().enabled = false;
    }
}
