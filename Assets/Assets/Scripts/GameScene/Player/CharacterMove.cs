using UnityEngine;
using System.Collections;

// キャラクターを移動させる。
// Chapter3
public class CharacterMove : MonoBehaviour
{
    // 重力値.
    const float GravityPower = 9.8f;

    // キャラクターコントローラーのキャッシュ.
    CharacterController characterController;
    CharacterStatus status;
    // 到着したか（到着した true/到着していない false)
    public bool arrived = false;

    // 向きを強制的に指示するか.
    bool forceRotate = false;

    // 強制的に向かせたい方向.
    Vector3 forceRotateDirection;


    // 移動速度.
    public float walkSpeed = 1.0f;
    public float dodgingSpeed = 1.0f;
    // 回転速度.
    public float rotationSpeed = 360.0f;


    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        status = GetComponent<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (arrived)
        {
            // 強制向き指定.
            Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            arrived = true;
        }
        // 強制的に向きを変えるを解除.
        if (forceRotate && Vector3.Dot(transform.forward, forceRotateDirection) > 0.99f)
            forceRotate = false;
        //重力
        characterController.Move(Vector3.down * GravityPower * Time.deltaTime);

    }

    public void SetDestinationWithJoyStick(Vector2 destination)
    {
        if (destination != Vector2.zero && !status.attacking)
        {
            arrived = false;
            //カメラの向く方向へ移動するベクトルを生成
            Vector3 delta3 = destination;
            delta3 = delta3.normalized;
            delta3.z = delta3.y;
            var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            delta3 = cameraForward * delta3.z * 100 + Camera.main.transform.right * delta3.x * 100;

            delta3 = delta3.normalized;
            //向き
            Quaternion characterTargetRotation = Quaternion.LookRotation(delta3);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);

            //重力
            delta3 += Vector3.down * GravityPower * Time.deltaTime;
            //移動
            characterController.Move(delta3 *walkSpeed* Time.deltaTime);

            //動いているとき向く方向を変更
            if (delta3.x != 0 && delta3.z != 0)
                SetDirection(delta3);
        }
    }

    //上とほぼ一緒　回避時の移動
    public void DodgingMove(Vector3 destination)
    {
        if (destination != Vector3.zero && !status.attacking)
        {
            arrived = false;
            //カメラの向く方向へ移動するベクトルを生成
            Vector3 delta3 = destination;
          /*  var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            delta3 = cameraForward * delta3.z * 100 + Camera.main.transform.right * delta3.x * 100;
            */
            delta3 = delta3.normalized;
            //向き
            Quaternion characterTargetRotation = Quaternion.LookRotation(delta3);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);

            //重力
            delta3 += Vector3.down * GravityPower * Time.deltaTime;
            //移動
            characterController.Move(delta3 * dodgingSpeed * Time.deltaTime);

            //動いているとき向く方向を変更
            if (delta3.x != 0 && delta3.z != 0)
                SetDirection(delta3);
        }
    }
    public void DodgAndRunMove(Vector3 destination)
    {
        if (destination != Vector3.zero)
        {
            arrived = false;
            //カメラの向く方向へ移動するベクトルを生成
            Vector3 delta3 = destination;
            /*  var cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
              delta3 = cameraForward * delta3.z * 100 + Camera.main.transform.right * delta3.x * 100;
              */
            delta3 = delta3.normalized;
            //向き
            Quaternion characterTargetRotation = Quaternion.LookRotation(delta3);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, characterTargetRotation, rotationSpeed * Time.deltaTime);

            //重力
            delta3 += Vector3.down * GravityPower * Time.deltaTime;
            //移動
            characterController.Move(delta3 * dodgingSpeed * Time.deltaTime);

            //動いているとき向く方向を変更
            if (delta3.x != 0 && delta3.z != 0)
                SetDirection(delta3);
        }
    }

    // 指定した向きを向かせる.
    public void SetDirection(Vector3 direction)
    {
        forceRotateDirection = direction;
        forceRotateDirection.y = 0;
        forceRotate = true;
    }

    // 目的地に到着したかを調べる. true　到着した/ false 到着していない.
    public bool Arrived()
    {
        return arrived;
    }
}
