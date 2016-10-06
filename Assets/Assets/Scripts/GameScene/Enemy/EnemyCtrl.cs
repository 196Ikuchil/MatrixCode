using UnityEngine;
using System.Collections;
using UnityEngine.UI; /// デバッグ用　消すこと！！！

public class EnemyCtrl : MonoBehaviour
{
    //デバッグ用
    public Text text;
    Text MaxHPText;
    Text HPText;




    EnemyStatus status;
    EnemyAnimation charaAnimation;
    EnemyMove characterMove;
    public Transform attackTarget;
    GameRuleCtrl gameRuleCtrl;
    GameMasterScript gameMaster;

    //オーディオ系
    public AudioClip deathSeClip;
    AudioSource deathSeAudio;
    public AudioClip damageSeClip;
    AudioSource damageSeAudio;

    // 待機時間は２秒とする
    public float waitBaseTime = 2.0f;
    // 残り待機時間
    float waitTime;
    // 移動範囲５メートル
    public float walkRange = 5.0f;
    // 初期位置を保存しておく変数
    public Vector3 basePosition;

    public float AttackRange = 2.0f;
    public GameObject[] hitEffect;

    // ステートの種類.
    enum State
    {
        Walking,	// 探索
        Chasing,	// 追跡
        Attacking,	// 攻撃
        Died,       // 死亡
    };

    State state = State.Walking;        // 現在のステート.
    State nextState = State.Walking;    // 次のステート.


    // Use this for initialization
    void Start()
    {
        //text = GameObject.Find("DebugDamageView").GetComponent<Text>();
        MaxHPText = GameObject.Find("DebugMaxHPView").GetComponent<Text>();
        HPText = GameObject.Find("DebugHPView").GetComponent<Text>();
        status = GetComponent<EnemyStatus>();
        charaAnimation = GetComponent<EnemyAnimation>();
        characterMove = GetComponent<EnemyMove>();
        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
        gameMaster = FindObjectOfType<GameMasterScript>();
        // 初期位置を保持
        basePosition = transform.position;
        // 待機時間
        waitTime = waitBaseTime;

        damageSeAudio = gameObject.AddComponent<AudioSource>();
        damageSeAudio.clip = damageSeClip;
        damageSeAudio.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Walking:
                Walking();
                break;
            case State.Chasing:
                Chasing();
                break;
            case State.Attacking:
                Attacking();
                break;
        }

        if (state != nextState)
        {
            state = nextState;
            switch (state)
            {
                case State.Walking:
                    WalkStart();
                    break;
                case State.Chasing:
                    ChaseStart();
                    break;
                case State.Attacking:
                    AttackStart();
                    break;
                case State.Died:
                    Died();
                    break;
            }
        }
    }


    // ステートを変更する.
    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }

    void WalkStart()
    {
        StateStartCommon();
    }

    void Walking()
    {
        // 待機時間がまだあったら
        if (waitTime > 0.0f)
        {
            // 待機時間を減らす
            waitTime -= Time.deltaTime;
            // 待機時間が無くなったら
            if (waitTime <= 0.0f)
            {
                // 範囲内の何処か
                Vector2 randomValue = Random.insideUnitCircle * walkRange;
                // 移動先の設定
                Vector3 destinationPosition = basePosition + new Vector3(randomValue.x, 0.0f, randomValue.y);
                //　目的地の指定.
                SendMessage("SetDestination", destinationPosition);
            }
        }
        else
        {
            // 目的地へ到着
            if (characterMove.Arrived())
            {
                // 待機状態へ
                waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
            }
            // ターゲットを発見したら追跡
            if (attackTarget)
            {
                ChangeState(State.Chasing);
            }
        }
    }
    // 追跡開始
    void ChaseStart()
    {
        StateStartCommon();
    }
    // 追跡中
    void Chasing()
    {
        // 移動先をプレイヤーに設定
        SendMessage("SetDestination", attackTarget.position);
        // 2m以内に近づいたら攻撃
        if (Vector3.Distance(attackTarget.position, transform.position) <= AttackRange)
        {
            ChangeState(State.Attacking);
        }
    }

    // 攻撃ステートが始まる前に呼び出される.
    void AttackStart()
    {
        StateStartCommon();
        status.attacking = true;

        // 敵の方向に振り向かせる.
        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        SendMessage("SetDirection", targetDirection);

        // 移動を止める.
        SendMessage("StopMove");
    }

    // 攻撃中の処理.
    void Attacking()
    {
        if (charaAnimation.IsAttacked())
            ChangeState(State.Walking);
        // 待機時間を再設定
        waitTime = Random.Range(waitBaseTime, waitBaseTime * 2.0f);
        // ターゲットをリセットする
        attackTarget = null;
    }



    void Died()
    {
        status.died = true;
        gameMaster.killEnemy(transform.tag);
        Destroy(gameObject,0.4f);

        //オーディオ
        AudioSource.PlayClipAtPoint(deathSeClip, transform.position);
    }

    void Damage(AttackArea.AttackInfo attackInfo)
    {
        //パーティクル生成
        GameObject effect;
        switch (attackInfo.element) {
            case 0:
                effect = (GameObject)Instantiate(hitEffect[0], transform.position, Quaternion.identity);
                break;
            case 1:
                effect = (GameObject)Instantiate(hitEffect[1], transform.position, Quaternion.identity);
                break;
            case 2:
                effect = (GameObject)Instantiate(hitEffect[2], transform.position, Quaternion.identity);
                break;
            case 3:
                effect = (GameObject)Instantiate(hitEffect[3], transform.position, Quaternion.identity);
                break;
            case 4:
                effect = (GameObject)Instantiate(hitEffect[4], transform.position, Quaternion.identity);
                break;
            case 5:
                effect = (GameObject)Instantiate(hitEffect[5], transform.position, Quaternion.identity);
                break;
            case 6:
                effect = (GameObject)Instantiate(hitEffect[6], transform.position, Quaternion.identity);
                break;
            default:
                effect = (GameObject)Instantiate(hitEffect[0], transform.position, Quaternion.identity);
                break;
        }

        effect.transform.localPosition = transform.position + new Vector3(0.0f, 0.5f, 0.3f);
        Destroy(effect, 0.3f);
        damageSeAudio.Play();

        int damage = attackInfo.attackPower;
        if (status.weakElement == attackInfo.element)//弱点属性なら
        {
            damage = (int)(attackInfo.attackPower * attackInfo.elementPower);
        }
        //ディフェンスの効果
        damage = (int)(damage - status.Defence * 0.1);
        status.HP -= (int)(damage*Random.Range(0.9f,1.1f));

        //text.text = "敵：" + damage + "ダメージ";
        HPText.text = status.HP.ToString();
        MaxHPText.text = "/" + status.MaxHP.ToString();
        if (status.HP <= 0)
        {
            status.HP = 0;
            // 体力０なので死亡
            ChangeState(State.Died);
        }
    }

    // ステートが始まる前にステータスを初期化する.
    void StateStartCommon()
    {
        status.attacking = false;
        status.died = false;
    }
    // 攻撃対象を設定する
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
