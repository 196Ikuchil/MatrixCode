using UnityEngine;
using System.Collections;

public class PlayerAttackAreaActivator : MonoBehaviour
{
    public AudioClip dodgeSeClip;
    AudioSource dodgeSeAudio;
    Collider[] attackAreaColliders; // 攻撃判定コライダの配列.
    GameObject[] footObjects;
    GameObject[] swordObjects;
    GameObject[] playerHits;
    Collider[] footColliders;  //脚用攻撃範囲
    Collider[] swordColliders; //剣用攻撃範囲

    Collider[] playerColliders; //受ける判定

    PlayerCtrl playerCtrl;

    //ヒット＆アウェイ時
    bool hitAndAway = false;

    public const string foot = "Foot";    //タグ
    public const string sword = "Sword";
    public const string playerHit = "PlayerHit";

    void Start()
    {

        footObjects = GameObject.FindGameObjectsWithTag(foot);
        swordObjects = GameObject.FindGameObjectsWithTag(sword);
        playerHits = GameObject.FindGameObjectsWithTag(playerHit);

        playerCtrl = GetComponent<PlayerCtrl>();

        //剣の初期化
        swordColliders = new Collider[swordObjects.Length];
        for (int i = 0; i < footObjects.Length; i++)
        {
            swordColliders[i] = swordObjects[i].GetComponent<AttackArea>().GetComponent<Collider>();
            swordColliders[i].enabled = false;
        }

        //足の初期化
        footColliders = new Collider[footObjects.Length];
        for (int i = 0; i < footObjects.Length; i++)
        {
            footColliders[i] = footObjects[i].GetComponent<AttackArea>().GetComponent<Collider>();
            footColliders[i].enabled = false;
        }

        //受け部分の初期化
        playerColliders = new Collider[playerHits.Length];
        for (int i = 0; i < playerHits.Length; i++)
        {
            playerColliders[i] = playerHits[i].GetComponent<Collider>();
            playerColliders[i].enabled = true;
        }

        //オーディオの初期化
        dodgeSeAudio = gameObject.AddComponent<AudioSource>();
        dodgeSeAudio.clip = dodgeSeClip;
        dodgeSeAudio.loop = false;
		dodgeSeAudio.volume = 0.1f;

    }

    //とりあえず剣用の関数
    // アニメーションイベントのStartAttackHitを受け取ってコライダを有効にする
    void StartAttackHit()
    {
        foreach (Collider swordCollider in swordColliders)
            swordCollider.enabled = true;

    }

    // アニメーションイベントのEndAttackHitを受け取ってコライダを無効にする
    void EndAttackHit()
    {
        foreach (Collider swordCollider in swordColliders)
            swordCollider.enabled = false;
    }
    // アニメーションイベントのStartAttackHitを受け取ってコライダを有効にする
    void StartFootAttackHit()
    {
        foreach (Collider swordCollider in swordColliders)
            swordCollider.enabled = true;

    }

    // アニメーションイベントのEndAttackHitを受け取ってコライダを無効にする
    void EndFootAttackHit()
    {
        foreach (Collider swordCollider in swordColliders)
            swordCollider.enabled = false;
    }

    //回避モーション
    void StartKaihiAction()
    {
        foreach (Collider playerCollider in playerColliders)
            playerCollider.enabled = false;
        dodgeSeAudio.Play();
    }
    void EndKaihiAction()
    {
        foreach (Collider playerCollider in playerColliders)
            playerCollider.enabled = true;
    }

    void StartHitWay()
    {
        playerCtrl.dodgingVector = this.transform.forward;
        hitAndAway = true;
    }
    void EndHitWay()
    {
        hitAndAway = false;
    }

    void Update()
    {
        if (hitAndAway)          
            SendMessage("DodgAndRunMove", playerCtrl.dodgingVector);
    }
}