using UnityEngine;

public class CharaAnimation : MonoBehaviour
{

    //魔法発動エフェクト
    public GameObject[] MagicEffects = new GameObject[6];

    Animator animator;
	CharacterStatus status;
    PlayerCtrl playerctrl;
	Vector3 prePosition;
	bool isDown = false;
	public bool attacked = false;
    bool beforeAttack = false;
    bool kaihied = false;
	
	public bool IsAttacked()
	{
		return attacked;
	}
    public bool IsDodged()
    {
        return kaihied;
    }
    void EndDodging()
    {
        kaihied = true;
    }

	void EndAttack()
	{
        Debug.Log("endAttack");
        status.StopSwordEffect();
		attacked = true;
	}
	
	void Start ()
	{
		animator = GetComponent<Animator>();
		status = GetComponent<CharacterStatus>();
        playerctrl = GetComponent<PlayerCtrl>();
		prePosition = transform.position;
	}
	
	void Update ()
	{
		Vector3 delta_position = transform.position - prePosition;
		animator.SetFloat("Speed", delta_position.magnitude / Time.deltaTime);
		
		if(attacked && !status.attacking)
		{
			attacked = false;
		}
        if(kaihied && !status.Dodging)
        {
            kaihied = false;
        }

        animator.SetInteger("AnimationNumber", status.abilityList[status.currentAbilitySelectNumbe].motion);
        animator.SetBool("Attacking", (!attacked && status.attacking));
        animator.SetBool("Dodging", (!kaihied && status.Dodging));
        if((!attacked && status.attacking) && !beforeAttack)//このフレームでアタックに移ったならぱーひくる出現
        {
            ParticleEffect((int)status.abilityList[status.currentAbilitySelectNumbe].element, status.abilityList[status.currentAbilitySelectNumbe].motion);
        }
        //現在のアタックの状態を保存する
        beforeAttack = (!attacked && status.attacking);

        if(!isDown && status.died)
		        {
			        isDown = true;
			        animator.SetTrigger("Down");
		        }

        prePosition = transform.position;
	}

    //発射のエフェクト
    void ParticleEffect(int element, int motion) //属性により分ける モーションによっても時間を帰る
    {
        if (motion == 0) return; //たたかうモーションならパーティクルなし
        GameObject effect = (GameObject)Instantiate(MagicEffects[element], this.transform.position, Quaternion.identity);
        effect.transform.SetParent(this.transform);
        if (motion == 1 || motion == 4 || motion == 7 || motion == 10)//でがはやい
            Destroy(effect.gameObject, 1f);
        else if (motion == 2 || motion == 5 || motion == 8)//普通
            Destroy(effect.gameObject, 1.7f);
        else if (motion == 3 || motion == 6 || motion == 9)//おそめ
            Destroy(effect.gameObject, 2.5f);
        else Destroy(effect.gameObject, 1f);
    }

    public void SetReactBool(bool b)
    {
        animator.SetBool("React", b);
    }

}