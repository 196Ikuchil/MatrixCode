
using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour
{
    CharacterStatus status;
    public AudioClip attackSeClip;
    AudioSource attackSeAudio;

    void Start()
    {
        attackSeAudio = gameObject.AddComponent<AudioSource>();
        attackSeAudio.clip = attackSeClip;
        attackSeAudio.loop = false;
        status = transform.root.GetComponent<CharacterStatus>();
    }

    public enum Elements
    {
        WHITE,
        RED,
        BLUE,
        YELLOW,
        GREEN,
        BLACK
    }

    public class AttackInfo
    {
        public string attackerName="Player";
        public int attackPower; // この攻撃の攻撃力.
        public Transform attacker; // 攻撃者.
        public int element;  //属性
        public float elementPower; //属性有効時の倍率
    }


    // 攻撃情報を取得する.
    AttackInfo GetAttackInfo()
    {
        AttackInfo attackInfo = new AttackInfo();
        // 攻撃力の計算.
        attackInfo.attackPower = (int)(status.GetPower()*status.abilityPower);
        if (status.iselementChange)
        {
            attackInfo.element = status.ChangeElement;
        }
        else attackInfo.element = status.abilityElement;

        if (status.elementBoost)
        {
            attackInfo.elementPower = status.abilityElementPower * status.elementBoostMig;
        }
        else attackInfo.elementPower = status.abilityElementPower;
        attackInfo.attacker = transform.root;

        return attackInfo;
    }

    // 当たった.
    void OnTriggerEnter(Collider other)
    {
        attackSeAudio.Play();
        // 攻撃が当たった相手のDamageメッセージをおくる.
        other.SendMessage("Damage", GetAttackInfo());
        // 攻撃した対象を保存.
        status.lastAttackTarget = other.transform.root.gameObject;
        //物理攻撃なのでSP回復
        status.RecoverSP(status.abilityRecoverSP);
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
