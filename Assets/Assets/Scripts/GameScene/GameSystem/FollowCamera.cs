using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour {
	public float distance = 5.0f;
	public float horizontalAngle = 0.0f;
	public float rotAngle = 180.0f; // 画面の横幅分カーソルを移動させたとき何度回転するか.
	public float verticalAngle = 10.0f;
	public Vector3 offset = Vector3.zero;

    //プレイヤー
    public GameObject player;
    public Transform lookTarget;
    public GameObject targetCursor;

    bool shakeView = false;

    TargetCursor target;
    //ジョイスティックを取得
    InputManager inputManager;
	void Start()
	{
		inputManager = FindObjectOfType<InputManager>();
        target = targetCursor.GetComponent<TargetCursor>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // ドラッグ入力でカメラのアングルを更新する. ジョイスティックでない
        if (inputManager.Moved())
        {
            float anglePerPixel = rotAngle / (float)Screen.width;
            Vector2 delta = inputManager.GetDeltaPosition();
            horizontalAngle -= delta.x * anglePerPixel*0.1f;
            horizontalAngle = Mathf.Repeat(horizontalAngle, 360.0f);
            verticalAngle += delta.y * anglePerPixel*0.1f;
            verticalAngle = Mathf.Clamp(verticalAngle, -60.0f, 60.0f);
        }

        // カメラを位置と回転を更新する.
        if (lookTarget != null)
        {
            Vector3 lookPosition = lookTarget.position + offset;
            // 注視対象からの相対位置を求める.
            Vector3 relativePos = Quaternion.Euler(verticalAngle, horizontalAngle, 0)* new Vector3(0, 0, -distance);

            // 注視対象の位置にオフセット加算した位置に移動させる.
            transform.position = lookPosition + relativePos;

            //ダメージ時の画面の揺れ
            if (shakeView)
            {
                Vector3 f =new Vector3(Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f),  Random.Range(0.05f, 0.2f));
                transform.LookAt(lookPosition + f);
            }else
            {
                transform.LookAt(lookPosition);
            }
            // 障害物を避ける.
            RaycastHit hitInfo;
            if (Physics.Linecast(lookPosition, transform.position, out hitInfo, 1 << LayerMask.NameToLayer("Ground")))
                transform.position = hitInfo.point;
        }
    }

    public void LookForward()//正面を向かせる
    {
        //ターゲット中はターゲットの方向を向かせる  
        if (target.GetPlayerTarget())
        {
            horizontalAngle = Quaternion.LookRotation(target.getTargetObject().transform.position - player.transform.position).eulerAngles.y;
            if (horizontalAngle > 360)
            {
                horizontalAngle -= 360;
            }
        }
        else horizontalAngle = Mathf.Clamp(player.transform.localEulerAngles.y, 0, 360);
    }

    //ダメージ時画面の揺れを受け取るPlayerCtrlから呼ばれる
    public IEnumerator ShakeView()
    {
        shakeView = true;
        yield return new WaitForSeconds(0.1f);
        shakeView = false;
    }
}
