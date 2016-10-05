using UnityEngine;
using System.Collections;

public class TargetCursor : MonoBehaviour {
	// 半径
	public float radius = 1.0f;
	// 回転速度
	public float angularVelocity = 480.0f;
	// 目的地
	public Vector3 destination = new Vector3( 0.0f, 0.5f, 0.0f );
	// 位置
	Vector3 position = new Vector3( 0.0f, 0.5f, 0.0f );
	// 角度
	float angle = 0.0f;

    //パーティクル
    ParticleSystem particle;
    //プレイヤーのターゲットが有効かどうか
    public bool playerTarget=false;
    //ターゲットのオブジェクト(ノーターゲットならplayerをターゲット)
    GameObject targetObject;

    public GameObject player;
    CharacterStatus status;


    // 位置を設定する
	public void SetPosition(Vector3 iPosition)
	{
		destination = iPosition;
		// 高さは固定
		destination.y = 0.5f;
	}
	
	void Start()
	{
        //自分のパーティクル取得
        particle = gameObject.GetComponent<ParticleSystem>();

        //プレイヤーのステータス取得　ターゲットレンジ取得のため
        status = player.GetComponent<CharacterStatus>();
		// 初期位置をプレイヤーに設定
		SetPosition( player.transform.position);
        SetTargetObject(player);
		position = destination;
	}

	// Update is called once per frame
	void Update () {
        //カーソル先が倒されてなくなってしまった。
        if (targetObject == null) SetPlayerTarget(false);

        SetPosition(targetObject.transform.position);

		position += ( destination - position ) / 10.0f;
		// 回転角度
		angle += angularVelocity * Time.deltaTime;
		// オフセット位置
		Vector3 offset = Quaternion.Euler( 0.0f, angle, 0.0f ) * new Vector3( 0.0f, 0.0f, radius );
		// エフェクトの位置
		transform.localPosition =  position + offset;

        Debug.Log((Vector3.Distance(this.transform.position, player.transform.position))+"  "+status.abilityList[status.currentAbilitySelectNumbe].range);
        //現在レンジ範囲内なら黄色に、自分なら黄緑 残りはデフォルト水色
        if (playerTarget == false) SetColor(new Color(0, 255, 0));
        else if (Vector3.Distance(this.transform.position, player.transform.position) < status.abilityList[status.currentAbilitySelectNumbe].range) SetColor(Color.yellow);
        else SetColor(new Color(0, 192, 255));
	}

    public bool GetPlayerTarget()
    {
        return playerTarget;
    }
    public void SetPlayerTarget(bool isTarget)
    {
        if (isTarget == false) SetTargetObject(player);
        playerTarget = isTarget;
    }

    public GameObject getTargetObject()
    {
        return targetObject;
    }
    public void SetTargetObject(GameObject g)
    {
        targetObject = g;
    }

    public void SetColor(Color c)
    {
        this.particle.startColor = c;
    }
}