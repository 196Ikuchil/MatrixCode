using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MatrixOpenPanel : MonoBehaviour {

    public GameObject matrixPref;
    MatrixCodeReader matrix;

    public Text text;

    public int leftNum = 0;
    public int centerNum = 0;
    public int rightNum = 0;
    public GameObject checkPanelPref;
    CheckPanel checkPanel;

    public AudioClip decideSeClip;
    AudioSource decideSeSource;

    public int GetLeftNum()
    {
        return leftNum;
    }
    public void SetLeftNum(int i)
    {
        leftNum = i;
    }
    public int GetCenterNum()
    {
        return centerNum;
    }
    public void SetCenterNum(int i)
    {
        centerNum = i;
    }
    public int GetRightNum()
    {
        return rightNum;
    }
    public void SetRightNum(int i)
    {
        rightNum = i;
    }

    public void OpenMatrixBoard()
    {
        int i = leftNum * 100 + centerNum * 10+rightNum;

        //もしまだ空いていなかったら
        if (!matrix.isOpenMatrix(i)) {
            string s = matrix.GetMatrixBenefit(i);
            //確認パネル
            checkPanel = Instantiate(checkPanelPref).GetComponent<CheckPanel>();
            checkPanel.transform.SetParent(this.transform.root.transform, false);
            checkPanel.SetText(s+"\nを解放しました");
        }else
        {
            text.text = "そのコードはすでに解放済みです";
            text.color = Color.red;
        }
    }
    public void RundomOpen()
    {
        int k;
        do
        {
            k = Random.Range(0, 1000);
        } while (matrix.isOpenMatrix(k));
        string s=matrix.GetMatrixBenefit(k);
        //確認パネル
        checkPanel = Instantiate(checkPanelPref).GetComponent<CheckPanel>();
        checkPanel.transform.SetParent(this.transform.root.transform, false);
        checkPanel.SetText(s + "\nを解放しました");

    }
    public void FullOpenAbility()
    {
        matrix.DebugFullAbilityOpen();
    }
    

    // Use this for initialization
    void Start () {
        matrix = Instantiate(matrixPref).GetComponent<MatrixCodeReader>();
        decideSeSource = gameObject.AddComponent<AudioSource>();
        decideSeSource.clip = decideSeClip;
        decideSeSource.loop = false;
		decideSeSource.volume = 0.1f;
	}
    public void DecideSePlay()
    {
        decideSeSource.Play();
    }
    public void DestroyPanel()
    {
        Destroy(this.gameObject);
    }
	
}
