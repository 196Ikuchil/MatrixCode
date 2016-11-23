using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MagicScript : MonoBehaviour {

    public string attackerName; //使用者
    public string name; //魔法の名前
    public int CharaPower=1;      //使用者のパワー
    public float MagicPower=1;      //魔法の倍率
    public int Element=1;         //魔法の属性
    public float elementPower = 1; //有効時の属性の倍率
    public GameObject[] particle;   //パーティクル
    public float DeathTime=30f;         //魔法が消える時間
    public bool IsEternal = false;      //永続かどうか（複数ヒット可能か）
    public Vector3[] particlePosition;
    AudioSource effectSeSource;
    public AudioClip effectSeClip;
    public float SeStartDelay = 0;
    public bool loop;

    void Awake()
    {
    }
	// Use this for initializatio
	void Start () {

        //パーティクル開始
        for(int i=0;i<particle.Length;i++)
        {
            GameObject effect = Instantiate(particle[i], particlePosition[i], particle[i].transform.rotation) as GameObject;
            effect.transform.SetParent(this.transform,false);
            Destroy(effect.gameObject, DeathTime);
        }
        StartCoroutine(SePlay());
        Destroy(this.gameObject,DeathTime);

        
        
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    // 攻撃情報を取得する.
    AttackArea.AttackInfo GetAttackInfo()
    {
        AttackArea.AttackInfo attackInfo = new AttackArea.AttackInfo();
        // 攻撃力の計算.
        attackInfo.attackerName= attackerName;
        attackInfo.attackPower = (int)(this.CharaPower*this.MagicPower);
        attackInfo.element = this.Element;
        attackInfo.elementPower = this.elementPower;

        return attackInfo;
    }

    // 当たった.
    void OnTriggerEnter(Collider other)
    {
        AttackArea.AttackInfo info = GetAttackInfo();
        //使用者に魔法が当たったら無視する。
        if (other.gameObject.GetComponent<HitArea>().receiverName == info.attackerName) return;
        // 攻撃が当たった相手のDamageメッセージをおくる.
        other.SendMessage("Damage", GetAttackInfo());
        
        //永続でないならあたり判定を削除する
        if (IsEternal != true)
        {
            Destroy(this.gameObject.GetComponent<Rigidbody>());
        }
    }
    IEnumerator SePlay()
    {
        effectSeSource = gameObject.AddComponent<AudioSource>();
        effectSeSource.clip = effectSeClip;
        effectSeSource.loop = this.loop;
		effectSeSource.volume = 0.1f;
        yield return new WaitForSeconds(SeStartDelay);
        effectSeSource.Play(); 
    }
}