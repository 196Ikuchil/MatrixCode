using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour
{
    Animator animator;
    EnemyStatus status;
    EnemyCtrl enemyctrl;
    Vector3 prePosition;
    MagicMaster mMaster;
    bool isDown = false;
    bool attacked = false;
    bool kaihied = false;
    bool attackFrame = false;

    public float waitTime = 2.0f;
    public GameObject magicCircle;

    public AudioClip magicSeClip;
    AudioSource magicSeSource;

    public bool IsAttacked()
    {
        return attacked;
    }
    public bool IsDodged()
    {
        return kaihied;
    }
    void EndKaihi()
    {
        kaihied = true;
    }

    void EndAttack()
    {
        attacked = true;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        enemyctrl = GetComponent<EnemyCtrl>();
        prePosition = transform.position;
        mMaster = FindObjectOfType<MagicMaster>();

        magicSeSource = gameObject.AddComponent<AudioSource>();
        magicSeSource.clip= magicSeClip;
        magicSeSource.loop = false;
		magicSeSource.volume = 0.2f;
    }

    void Update()
    {
        Vector3 delta_position = transform.position - prePosition;
        animator.SetFloat("Speed", delta_position.magnitude / Time.deltaTime);

        if (attacked && !status.attacking)
        {
            attacked = false;
            attackFrame = false;
        }
        if (kaihied && !status.Dodging)
        {
            kaihied = false;
        }

        animator.SetBool("Attacking", (!attacked && status.attacking));
        if ((!attacked && status.attacking) && !attackFrame)//攻撃がtrueなら発動スキルを選択しセット,攻撃モーションへ
        {
            EnemyStatus.SkillData s = status.GetSkillData();
            animator.SetInteger("AnimationNum",s.motion);
            status.SetMag(s.mag);
            //魔法
            if (s.motion == 99)
            {
                Ability a = status.GetAbility();
                Vector3 v = enemyctrl.attackTarget.position;
                GameObject m = (GameObject)Instantiate(magicCircle, transform.position, Quaternion.identity);
                Destroy(m, 3f);
                magicSeSource.Play();
                if (7 <= a.motion && a.motion <= 9)
                {
                    StartCoroutine(DelayMagicV(a, v));
                    
                }
                else
                {
                    StartCoroutine(DelayMagic(a, v));
                }
                
            }
            attackFrame = true;
        }

        if (!isDown && status.died)
        {
            isDown = true;
            animator.SetTrigger("Down");
        }

        prePosition = transform.position;
    }

    private IEnumerator DelayMagicV(Ability a,Vector3 v)
    {
        yield return new WaitForSeconds(waitTime);
        mMaster.Magic(false, 1, a, (int)(status.Power*status.mag), this.transform.position, "Enemy", v);
    }
    private IEnumerator DelayMagic(Ability a, Vector3 v)
    {
        yield return new WaitForSeconds(waitTime);
        mMaster.Magic(false, 1, a, (int)(status.Power*status.mag), v, "Enemy");
    }
}