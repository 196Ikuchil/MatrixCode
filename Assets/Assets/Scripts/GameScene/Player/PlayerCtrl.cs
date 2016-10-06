/* 
 * キャラクターを操作するスクリプト
 * 主要なキャラの機能はここに入れたい
 * 数字管理はstatus系に
 * アビリティ管理も別にしたい
 * 参考資料あり
*/
using UnityEngine;
using UnityEngine.UI; //デバッグ用終わったら消す！！
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerCtrl : MonoBehaviour
{

    //デバッグ用
   // public Text text;



    GameRuleCtrl gameRuleCtrl;  //クエストを管理

    Joystick joystick;          //キャラクタの操作
    InputManager inputManager;  //それ以外の操作入力
    CharacterStatus status;     //キャラクタのステータスを管理している
    CharaAnimation charaAnimation;  //キャラのモーションを管理   
    FollowCamera camera; 
    
    TargetCursor targetCursor;  //ターゲットカーソル

    public AudioClip damageSeClip;
    AudioSource damageSeAudio;
    public AudioClip magicSeClip;
    AudioSource magicSeAudio;
    public AudioClip abilitySeClip;
    AudioSource abilitySeAudio;
    public AudioClip ability2SeClip;
    AudioSource ability2SeAudio;

    //キャラのステート
    enum State
    {
        Walking,
        Attacking,
        Dodging,
        Died,
        Reacting,
    };

    State state = State.Walking;		//現在のステート.
    State nextState = State.Walking;	//次のステート.

    bool isTargeting=false;         //敵がターゲッティングされているかどうか　
    GameObject targetObject;        //ターゲットのオブジェクト

    const float RayCastMaxDistance = 100.0f;
    public float ReactLine = 0.3f;      //キャラがのけぞるダメージの大きさ

    public Vector3 dodgingVector { get; set; }
    public bool canDodgingMove = false;

    float startReactTime;
 
    //パーティクル系
    public GameObject hitEffect;        //攻撃が当たった
    public GameObject chargeEffect;     //物理攻撃溜め技
    public GameObject magicChargeEffect;//魔法発動前
    public GameObject lastBeatEffect; //ラストビートのえふぇくと

    void Start()
    {
        status = this.GetComponent<CharacterStatus>();
        charaAnimation = this.GetComponent<CharaAnimation>();
        inputManager = FindObjectOfType<InputManager>();
        gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
        camera = FindObjectOfType<FollowCamera>();
        //ターゲットマーカー
        targetCursor = FindObjectOfType<TargetCursor>();
        targetCursor.SetPosition(transform.position);

        //ジョイスティック
        joystick = FindObjectOfType<Joystick>();

        damageSeAudio = gameObject.AddComponent<AudioSource>();
        damageSeAudio.clip = damageSeClip;
        damageSeAudio.loop = false;
        magicSeAudio = gameObject.AddComponent<AudioSource>();
        magicSeAudio.clip = magicSeClip;
        magicSeAudio.loop = false;
        abilitySeAudio = gameObject.AddComponent<AudioSource>();
        abilitySeAudio.clip = abilitySeClip;
        abilitySeAudio.loop = false;
        ability2SeAudio = gameObject.AddComponent<AudioSource>();
        ability2SeAudio.clip = abilitySeClip;
        ability2SeAudio.loop = false;

        ReactLine = status.MaxHP * ReactLine;
    }

    void Update()
    {
        switch (state)
        {
            case State.Walking:
                Walking();
                break;
            case State.Attacking:
                Attacking();
                break;
            case State.Dodging:
                Dodging();
                break;
            case State.Reacting:
                Reacting();
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
                case State.Attacking:
                    AttackStart();
                    break;
                case State.Died:
                    Died();
                    break;
                case State.Dodging:
                    StartDodge();
                    break;
                case State.Reacting:
                    StartReacting();
                    break;
            }
        }
    }

    // ステートを変更する.
    void ChangeState(State nextState)
    {
        this.nextState = nextState;
    }
    void StartReacting()
    {
        Debug.Log("startRecting");
        SetStateStart();
        status.Reacting = true;
        charaAnimation.SetReactBool(true);
        startReactTime = Time.time;
    }
    void Reacting()
    {
        
        charaAnimation.SetReactBool(false);
        if (Time.time - startReactTime > 0.4f)
        {
            status.Reacting = false;
            ChangeState(State.Walking);
        }
    }
    public void EndReact()
    {
        
    }

    void WalkStart()
    {
        SetStateStart();
    }

    void Walking()
    {
        //ジョイスティックにより移動をさせる
       if(state!=State.Reacting) SendMessage("SetDestinationWithJoyStick",inputManager.GetPlayerDeltaPosition());
        
        if (inputManager.Clicked())
        {
            Ray ray = Camera.main.ScreenPointToRay(inputManager.GetCursorPosition());   //ターゲットカーソルを操作する
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, RayCastMaxDistance, (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("PlayerHit")) | (1 << LayerMask.NameToLayer("EnemyHit"))))
            {
                if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("Ground") && !joystick.GetIsActiveStick()) //地面をクリック
                    targetCursor.SetPlayerTarget(false);
                
                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHit"))     //敵をクリック
                {
                    targetCursor.SetPlayerTarget(true);
                    targetCursor.SetTargetObject(hitInfo.collider.gameObject.transform.root.gameObject);
                }
                else if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("PlayerHit"))
                {
                    targetCursor.SetPlayerTarget(false);

                }
            }
        }
    }

    void AttackStart()
    {
        Debug.Log("AttackStart");
        SetStateStart();
        status.attacking = true;
        
        //技の情報を格納（コマンドを技の使用中に変えると変更してしまうから）
        status.SetCurrentUseAbilityInfo();
        //剣のエフェクト
        status.StartSwordEffect();
        
        // 敵の方向に振り向かせる(ターゲットしているなら)
        if (targetCursor.playerTarget)
        {
            Vector3 targetDirection = (targetCursor.transform.position - transform.position).normalized;
            SendMessage("SetDirection", targetDirection);
        }

        //パーティクル系の処理
        int motionNum = status.abilityList[status.currentAbilitySelectNumbe].motion;
        switch (motionNum) {
            case 1://少し遅い物理攻撃
                MakeParticle(chargeEffect,1f);
                abilitySeAudio.Play();
                break;
            case 2://遅い物理攻撃
                MakeParticle(chargeEffect, 1.5f);
                abilitySeAudio.Play();
                break;
            case 3://かなり遅い溜め攻撃
                MakeParticle(chargeEffect,3f);
                ability2SeAudio.Play();
                break;
            case 4://魔法の短いエフェクト
                MakeParticle(magicChargeEffect,0.5f);
                break;
            case 5://魔法のエフェクト
                MakeParticle(magicChargeEffect, 1f);
                break;
            case 6://魔法のエフェクト
                MakeParticle(magicChargeEffect, 1.5f);
                break;
            case 7://魔法の短いエフェクト
                MakeParticle(magicChargeEffect, 0.5f);
                break;
            case 8://魔法のエフェクト
                MakeParticle(magicChargeEffect, 1f);
                break;
            case 9://魔法の短いエフェクト
                MakeParticle(magicChargeEffect, 1.5f);
                break;
        }
        if(10<=motionNum && motionNum <= 15)
        {
            MakeParticle(magicChargeEffect, 0.5f);
        }
        if (motionNum == 24)
        {
            MakeParticle(lastBeatEffect, 2f);
        }

        if(4<=motionNum && motionNum <= 15)//魔法の音
        {
            magicSeAudio.Play();
        }if(16<=motionNum&& motionNum <= 24)
        {
            abilitySeAudio.Play();
        }
    }

    // 攻撃中の処理.
    void Attacking()
    {
        
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walking);
        }
    }

    //あたり判定をなくす
    void StartDodge()
    {
        Debug.Log("StartDodge");
        SetStateStart();
        dodgingVector = this.transform.forward;
        status.Dodging= true;
        this.canDodgingMove = true;
    }

    //キャラを動かす
    void Dodging()
    {
        if(canDodgingMove) SendMessage("DodgingMove", dodgingVector);
        if (charaAnimation.IsDodged())
                ChangeState(State.Walking);
    }

    //アニメーションプレハブから呼ばれる
    public void EndCanDodgingMove()
    {
        canDodgingMove = false;
    }

    //死んだ
    void Died()
    {
        status.died = true;
       StartCoroutine( gameRuleCtrl.GameOver());
    }

    void Damage(AttackArea.AttackInfo attackInfo)
    {
        //ヒットエフェクト発生
        MakeParticle(hitEffect,0.3f,new Vector3(0.0f, 0.5f, 0.0f));
        damageSeAudio.Play();

        int damage = attackInfo.attackPower;
        if (status.weakElement == attackInfo.element)//弱点属性なら
        {
            damage =(int)( attackInfo.attackPower * attackInfo.elementPower);
        }
        //ディフェンスの効果
        damage = (int)(damage - status.Defence * 0.07);
        //シールド
        if (status.DefOneBoost)
        {
            damage = (int)(damage*(1-status.DefOneBoostMig));
            status.StopDefOneBoost();
        }
        //バリア
        if (status.eternalDef)
        {
            damage = (int)(damage * (1 - status.eternalDefMig));
        }

        //ダメージが一定以上ならのけぞり
        if (damage >= ReactLine && !status.lastBeat)
        {
            ChangeState(State.Reacting);
            StartCoroutine(camera.ShakeView());//画面の揺れ
        }
        status.HP -= damage;
       // text.text = "プレイヤー:" + damage + "ダメージ"; //TODO
        
        if (status.HP <= 0)
        {
            status.HP = 0;
            status.stopAllStatusUp();
            ChangeState(State.Died);//体力０死亡
        }
    }

    // ステートが始まる前にステータスを初期化する.
    void SetStateStart()
    {
        
        status.StopSwordEffect();
        status.attacking = false;
        status.Dodging = false;
        status.died = false;
        status.Reacting = false;
        
    }

    //GUIアタックボタンが押された-＞ゲームマスターがこのメソッドを呼ぶ
    public void AttackButton()
    {
        float distance = Vector3.Distance(targetCursor.transform.position, transform.position);
        if (distance <= status.abilityList[status.currentAbilitySelectNumbe].range)
        {         
            if (!status.expendSP()) return; //SP不足なら
            ChangeState(State.Attacking);
        }
    }

    //回避ボタンが押された
    public void dodgeButton()
    {
        if (state!=State.Walking) return;//SP不足なら今歩いていないなら
        if (!status.expandSPWithKaihi()) return;//SP不足なら消費せず終了
        ChangeState(State.Dodging);
    }

    //パーティクル選択メソッド
    void MakeParticle(GameObject particle, float f) //消滅時間　発生させるパーティクル
    {
        MakeParticle(particle, f, new Vector3(0f, 0f, 0f));  //足元発生
    }
    void MakeParticle(GameObject particle,float f,Vector3 v)//v 発生場所
    {
        GameObject effect = Instantiate(particle, transform.position, Quaternion.identity) as GameObject;
        effect.transform.localPosition = transform.position+v;
        Destroy(effect, f);
    }
}

