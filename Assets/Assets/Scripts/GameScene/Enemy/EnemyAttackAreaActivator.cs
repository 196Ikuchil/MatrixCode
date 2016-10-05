using UnityEngine;
using System.Collections;

public class EnemyAttackAreaActivator : MonoBehaviour
{
    public AudioClip attackSeClip;
    AudioSource attackSeAudio;
    Collider[] attackAreaColliders; // 攻撃判定コライダの配列.

    void Start()
    {
        // 子供のGameObjectからAttackAreaスクリプトがついているGameObjectを探す。
        EnemyAttackArea[] attackAreas = GetComponentsInChildren<EnemyAttackArea>();
        attackAreaColliders = new Collider[attackAreas.Length];

        Debug.Log("attackArea:" + attackAreas.Length);
        //オーディオの初期化
        attackSeAudio = gameObject.AddComponent<AudioSource>();
        attackSeAudio.clip = attackSeClip;
        attackSeAudio.loop = false;

        for (int attackAreaCnt = 0; attackAreaCnt < attackAreas.Length; attackAreaCnt++)
        {
            // AttackAreaスクリプトがついているGameObjectのコライダを配列に格納する.
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].GetComponent<Collider>();
            attackAreaColliders[attackAreaCnt].enabled = false;  // 初期はfalseにしておく.
        }
    }

    // アニメーションイベントのStartAttackHitを受け取ってコライダを有効にする
    void StartAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = true;

        //オーディオ再生
        attackSeAudio.Play();
    }

    // アニメーションイベントのEndAttackHitを受け取ってコライダを無効にする
    void EndAttackHit()
    {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = false;
    }
}
