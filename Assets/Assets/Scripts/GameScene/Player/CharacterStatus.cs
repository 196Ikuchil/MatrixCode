using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour
{
    // 体力.
    public int HP = 100;
    public int MaxHP = 100;

    // 攻撃力.
    public int Power = 10;
    public int MPower = 10;

    //SP
    public float SP = 100;
    public float MaxSP = 100;
    //SP回復力
    public float SPRecoverPower = 1;    //一秒間に回復する両　後で、deltaTimeで割るから。

    //防御力
    public float Defence = 0;

    //弱点
    public int weakElement = 0;

    public string characterName = "Player";

    //状態.
    public bool attacking = false;
    public bool Dodging = false;
    public bool died = false;
    public bool Reacting = false;

    //攻撃威力ブースと
    public GameObject powerBoostPref;
    public GameObject powerBoostP;
    public bool powerBoost = false;
    float powerBoostTime = 0.0f;    // 強化時間
    float powerBoostMig = 0f;       //強化倍率
    //継続回復
    public GameObject eternalHealPref;
    public bool eternalHeal = false;
    float eternalHealTime=0f;
    float eternalHealMig = 0f;
    public GameObject eternalHealP;
    float nextTime;
    //属性倍率アップ
    public GameObject elementBoostPref;
    public bool elementBoost = false;
    float elementBoostTime = 0f;
    public float elementBoostMig = 0f;
    public GameObject elementBoostP;
    //SP継続回復
    public GameObject eternalSpPref;
    public bool eternalSp = false;
    float eternalSpTime = 0f;
    float eternalSpMig = 0f;
    public GameObject eternalSpP;
    public float GetSpboostMag()//GUI用に使う
    {
        if (eternalSp)
            return eternalSpMig*0.3f;
        else return 1f*0.3f;
    }
    //属性指定
    public GameObject changeElementP;
    public bool iselementChange = false;
    public int ChangeElement = 0;
    float changeElementTime = 0;
    public GameObject changeElementR;
    public GameObject changeElementB;
    public GameObject changeElementY;
    //一時攻撃力アップ
    public GameObject powerOneBoostPref;
    public bool powerOneBoost = false;
    float powerOneBoostMig=0f;
    public GameObject powerOneBoostP;
    //継続バリア
    public GameObject eternalDefPref;
    public bool eternalDef = false;
    float eternalDefTime = 0f;
    public float eternalDefMig = 0f;
    public GameObject eternalDefP;
    //一時防御アップ
    public GameObject DefOneBoostPref;
    public bool DefOneBoost = false;
    public  float DefOneBoostMig = 0f;
    public GameObject DefOneBoostP;

    //ラストビート中
    public bool lastBeat = false;
    float defaultCameraDist;
    void StartLastBeat()
    {
        defaultCameraDist=camera.distance;
        lastBeat = true;
        camera.distance = 3.5f;
    }
    void EndLastBeat()
    {
        lastBeat = false;
        camera.distance = defaultCameraDist;
    }
    FollowCamera camera;

    //使用した技の情報
    public float abilityPower;
    public int abilityElement;
    public float abilityElementPower;
    public int abilityAbility;
    public int abilityRecoverSP;
    

    //剣エフェクト
    public float basicAttackEffect=1.6f;
    public SwordEffect effectObject;

    public GameObject lastAttackTarget;

    MagicMaster magicMaster;  //魔法の仕様管理
    TargetCursor targetCursor; //ターゲットカーソル

    //アビリティ一覧
    public Ability[] abilityList;

    public CommandPanel currentCommand=new CommandPanel();
    public int currentAbilitySelectNumbe=0;
    

    public void SetAbility(Ability[] a)
    {
        abilityList = a;
    }

    // アイテム取得
    public void GetItem(DropItem.ItemKind itemKind)
    {
        switch (itemKind)
        {
            case DropItem.ItemKind.Attack:
                powerBoostTime = 5.0f;
                break;
            case DropItem.ItemKind.Heal:
                // MaxHPの半分回復
                HP = Mathf.Min(HP + MaxHP / 2, MaxHP);
                break;
        }
    }

    void Start()
    {
        //魔法管理
        magicMaster = FindObjectOfType<MagicMaster>();
        targetCursor = FindObjectOfType<TargetCursor>();
        camera = FindObjectOfType<FollowCamera>();
    }

    void Update()
    {
        if (powerBoost)
            EternalPowerBoost();
        if (eternalHeal)//継続回復
            EternalRecoverHP();
        if (elementBoost)
            EternalElementBoost();
        if (iselementChange)
            ElementChangeEffect();
        if (eternalSp)
            EternalSp();
        if (eternalDef)
            EternalDef();

        if(!attacking) //現在攻撃中でなければ（攻撃中変更されると発動魔法が変更されてしまう）
            currentAbilitySelectNumbe = currentCommand.GetCurrentAbility();//現在の選択アビリティを毎フレーム保存
    }

    public void RecoverSP(int i)//敵にダメージが入ったら呼ばれる
    {

        SP += (SPRecoverPower*GetSpboostMag()*i);//基礎回復力*倍率*技の回復力
        SP=Mathf.Clamp(SP, 0f, MaxSP);
    }

    //現在選択しているコマンドのSPを消費
    public bool expendSP()
    {
        if (abilityList[currentAbilitySelectNumbe].ability ==0) //物理攻撃ならSPは使わない
        {        
            return true;
        }

        //SP不足
        if(SP < abilityList[currentAbilitySelectNumbe].Sp)
        {
            return false;
        }
        //trueなら消費しない
        if (attacking == false)
        {
            SP -= abilityList[currentAbilitySelectNumbe].Sp;
        }
        return true;
    }

    public bool expandSPWithKaihi()
    {

        //SP不足
        if (this.SP <(this.MaxSP*0.05))
        {
            return false;
        }
        //trueなら消費しない
        if (Dodging == false)
        {
            this.SP =this.SP- (MaxSP * 0.1f);
            Debug.Log("kaihi");
        }
        return true;
    }

    public void SetHP(int h)
    {
        MaxHP = h;
        HP = h;
    }

    public void SetSP(int s)
    {
        MaxSP = s;
        SP = s;
    }
    public void SetSPRe(int i)
    {
        SPRecoverPower = i;
    }
    public void SetPower(int p)
    {
        Power = p;
    }
    public float GetPower()
    {
        float i = 1;
        if (powerOneBoost)
        {
            i *= powerOneBoostMig;
            Destroy(powerOneBoostP);
            powerOneBoost = false;
        }
        if (powerBoost) i*=powerBoostMig;

        return Power*i;
    }
    public void SetMPower(int p)
    {
        MPower = p;
    }
    public float GetMPower()
    {
        float i = 1;
        if (powerOneBoost)
        {
            i *= powerOneBoostMig;
            Destroy(powerOneBoostP);
            powerOneBoost = false;
        }
        if (powerBoost) i *= powerBoostMig;

        return MPower * i;
    }
    public void SetDefence(int p)
    {
        Defence = p;
    }

    //魔法の仕様
    public void StartMagic()
    {
        if (iselementChange)
        {
            magicMaster.Magic(elementBoost, elementBoostMig, abilityList[currentAbilitySelectNumbe], (int)GetMPower(), targetCursor.transform.position, "Player", ChangeElement);
            
        }
        else magicMaster.Magic(elementBoost, elementBoostMig, abilityList[currentAbilitySelectNumbe], (int)GetMPower(), targetCursor.transform.position, "Player");
    }

    //発射型魔法
    public void StartVectorMagic()
    {
        if (iselementChange)
        {
            magicMaster.Magic(elementBoost, elementBoostMig, abilityList[currentAbilitySelectNumbe], (int)GetMPower(), this.transform.position, "Player", targetCursor.transform.position, ChangeElement);
        }
        else magicMaster.Magic(elementBoost, elementBoostMig, abilityList[currentAbilitySelectNumbe], (int)GetMPower(), this.transform.position, "Player", targetCursor.transform.position);
    }

    //回復系魔法
    public void StartHealMagic()
    {
        RecoverHP(abilityList[currentAbilitySelectNumbe].Attack);
    }

    //継続回復系
    public void StartEternalHealMagic()
    {
        if (!eternalHeal) //現在起動中でなかったら
        {
            eternalHealP = (GameObject)Instantiate(eternalHealPref);
            eternalHealP.transform.SetParent(this.transform,false);
        }
        eternalHeal = true;
        eternalHealTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        eternalHealMig = abilityList[currentAbilitySelectNumbe].Attack; //倍率
    }
    //HP回復
    public void RecoverHP(float i)
    {
        this.HP += (int)(i * this.GetMPower());
        this.HP = Mathf.Clamp(this.HP,0,this.MaxHP+1);
    }
    //継続回復
    public void EternalRecoverHP()
    {
        nextTime=eternalHealTime;
        nextTime-= Time.deltaTime;
        if(((int)(eternalHealTime)-(int)(nextTime))>0)
            RecoverHP(eternalHealMig);
        eternalHealTime = nextTime;
        if (eternalHealTime < 0)
        {
            StopEternalRecHP();
        }
    }
    void StopEternalRecHP()
    {
        eternalHealTime = 0f;
        eternalHealMig = 0f;
        Destroy(eternalHealP);
        eternalHeal = false;
    }
    //攻撃力ブースト
    public void StartPowerBoost()
    {
        if (!powerBoost) //現在起動中でなかったら
        {
            powerBoostP = (GameObject)Instantiate(powerBoostPref);
            powerBoostP.transform.SetParent(this.transform,false);
        }
        powerBoost = true;
        powerBoostTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        powerBoostMig = abilityList[currentAbilitySelectNumbe].Attack; //倍率
    }
    //ブースト時間中
    public void EternalPowerBoost()
    {
        powerBoostTime -= Time.deltaTime;
        if (powerBoostTime < 0)
        {
            StopEternalPowerBoost();
        }
    }
    void StopEternalPowerBoost()
    {
        powerBoostTime = 0f;
        powerBoostMig = 0f;
        Destroy(powerBoostP);
        powerBoost = false;
    }
    //属性倍率アップ
    public void StartElementBoost()
    {
        if (!elementBoost) //現在起動中でなかったら
        {
            elementBoostP = (GameObject)Instantiate(elementBoostPref);
            elementBoostP.transform.SetParent(this.transform,false);
        }
        elementBoost = true;
        elementBoostTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        elementBoostMig = abilityList[currentAbilitySelectNumbe].Attack; //倍率
    }
    //ブースト時間中
    public void EternalElementBoost()
    {
        elementBoostTime -= Time.deltaTime;
        if (elementBoostTime < 0)
        {
            StopEternalElementBoost();
        }
    }
    void StopEternalElementBoost()
    {
        elementBoostTime = 0f;
        elementBoostMig = 0f;
        Destroy(elementBoostP);
        elementBoost = false;
    }
    //SP継続回復
    public void StartSpHeal()
    {
        if (!eternalSp) //現在起動中でなかったら
        {
            eternalSpP = (GameObject)Instantiate(eternalSpPref);
            eternalSpP.transform.SetParent(this.transform,false);
        }
        eternalSp = true;
        eternalSpTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        eternalSpMig = abilityList[currentAbilitySelectNumbe].Attack; //倍率
    }
    //ブースト時間中
    public void EternalSp()
    {
        eternalSpTime -= Time.deltaTime;
        if (eternalSpTime < 0)
        {
            StopEternalSp();
        }
    }
    void StopEternalSp()
    {
        eternalSpTime = 0f;
        eternalSpMig = 0f;
        Destroy(eternalSpP);
        eternalSp = false;
    }
    //エレメントチェンジ
    public void StartElementChange()
    {
        ChangeElement = (int)abilityList[currentAbilitySelectNumbe].element;
        if (iselementChange) //現在起動中なら
            ElementChangeEffect();
        switch (ChangeElement) {
            case 1:
                changeElementP= (GameObject)Instantiate(changeElementR);
                break;
            case 2:
                changeElementP = (GameObject)Instantiate(changeElementB);
                break;
            case 3:
                changeElementP = (GameObject)Instantiate(changeElementY);
                break;
        }
        changeElementP.transform.SetParent(this.transform,false);
        changeElementTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        iselementChange = true;
    }
    public void ElementChangeEffect()
    {
        changeElementTime -= Time.deltaTime;
        if (changeElementTime < 0)
        {
            StopElementChangeEffect();
        }
    }
    void StopElementChangeEffect()
    {
        changeElementTime = 0f;
        ChangeElement = 0;
        Destroy(changeElementP);
        iselementChange = false;
    }
    //一回のみパワーブースと
    public void StartPowerOneBoost()
    {
        if (!powerOneBoost) //現在起動中でなかったら
        {
            powerOneBoostP = (GameObject)Instantiate(powerOneBoostPref);
            powerOneBoostP.transform.SetParent(this.transform,false);
        }
        powerOneBoost = true;
        powerOneBoostMig= abilityList[currentAbilitySelectNumbe].Attack;
    }

    //使用したスキルの情報を格納
    public void SetCurrentUseAbilityInfo()
    {
        abilityPower = abilityList[currentAbilitySelectNumbe].Attack;
        abilityElement = (int)abilityList[currentAbilitySelectNumbe].element;
        abilityElementPower = abilityList[currentAbilitySelectNumbe].value;
        abilityAbility = abilityList[currentAbilitySelectNumbe].ability;
        abilityRecoverSP = (int)abilityList[currentAbilitySelectNumbe].Sp;
    }
    //バリア
    public void StartDef()
    {
        if (!eternalDef) //現在起動中でなかったら
        {
            eternalDefP = (GameObject)Instantiate(eternalDefPref);
            eternalDefP.transform.SetParent(this.transform,false);
        }
        eternalDef = true;
        eternalDefTime = abilityList[currentAbilitySelectNumbe].benefit; //継続時間
        eternalDefMig = abilityList[currentAbilitySelectNumbe].Attack; //倍率        
    }
    //ブースト時間中
    public void EternalDef()
    {
        eternalDefTime -= Time.deltaTime;
        if (eternalDefTime < 0)
        {
            StopEternalDef();
        }
    }
    void StopEternalDef()
    {
        eternalDefTime = 0f;
        eternalDefMig = 0f;
        Destroy(eternalDefP);
        eternalDef = false;
    }
    //一回のみ防御ブースと
    public void StartSheildOneBoost()
    {
        if (!DefOneBoost)
        {
            DefOneBoostP = changeElementP = (GameObject)Instantiate(DefOneBoostPref);
            DefOneBoostP.transform.SetParent(this.transform,false);
        }
        DefOneBoost = true;
        DefOneBoostMig = abilityList[currentAbilitySelectNumbe].Attack;
    }
    public void StopDefOneBoost()
    {
        DefOneBoost = false;
        Destroy(DefOneBoostP);
    }
    public void stopAllStatusUp()
    {
        StopEternalRecHP();
        StopEternalPowerBoost();
        StopEternalElementBoost();
        StopEternalSp();
        StopElementChangeEffect();
        StopEternalDef();
        StopDefOneBoost();
    }


    //attackが一定以上なら県にエフェクトをかける　PlayerCtrlから呼ばれる
    public void StartSwordEffect()
    {
        if (abilityPower*abilityElementPower>=basicAttackEffect)
        {
            effectObject.StartEffect(abilityElement);
        }
    }
    //キャラアニメーションから呼ばれる
    public void StopSwordEffect()
    {
        effectObject.StopEffect();
    }
    
}
