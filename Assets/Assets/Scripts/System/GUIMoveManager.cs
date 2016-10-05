using UnityEngine;
using System.Collections;

public class GUIMoveManager : MonoBehaviour
{
    public GameObject[] Left;
    public GameObject[] Right;

    Vector3[] LeftPositions;
    Vector3[] RightPositions;

    float EDGE = 60f;
    float TIME = 1f;

    // Use this for initialization
    void Start()
    {
        this.fadeIn(); //プレハブが生成されたらフェードイン処理を実行
    }

    void fadeIn()
    {
        for (int i = 0; i < Left.Length; i++)
        {
            this.moveAction(Left[i], true, true);
        }
        for (int i = 0; i < Right.Length; i++)
        {
            this.moveAction(Right[i], true, false);
        }
    }

    public void fadeOut()
    {
        for (int i = 0; i < Left.Length; i++)
        {
            this.moveAction(Left[i], false, true);
        }
        for (int i = 0; i < Right.Length; i++)
        {
            this.moveAction(Right[i], false, false);
        }
    }

    void moveAction(GameObject pObj, bool isIn, bool isLeft)
    {
        if (isIn)
        {
            if (isLeft)
            {
                iTween.MoveFrom(pObj, iTween.Hash(
                    "x", pObj.transform.position.x - EDGE
                    , "time", TIME
                    ));
            }
            else
            {
                iTween.MoveFrom(pObj, iTween.Hash(
                    "x", pObj.transform.position.x + EDGE
                    , "time", TIME
                    ));
            }
        }
        else
        {
            if (isLeft)
            {
                iTween.MoveTo(pObj, iTween.Hash(
                    "x", pObj.transform.position.x-EDGE
                    , "time", TIME
                    ));
            }
            else
            {
                iTween.MoveTo(pObj, iTween.Hash(
                    "x", pObj.transform.position.x+EDGE
                    , "time", TIME
                    ));
            }
        }
    }
}