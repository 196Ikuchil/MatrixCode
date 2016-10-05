using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectMatrixNum : MonoBehaviour {

    int matrixNum;
    GameObject matrixPanel;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetPanel(GameObject g)
    {
        matrixPanel = g;
    }
    //パネルに自分のナンバーを返す
    public void OnClick()
    {
        matrixPanel.GetComponent<MatrixCodeTable>().SetMatrixComponentView(matrixNum);
    }

    public void SetMatNum(int i)
    {
        matrixNum = i;
        string s;
        if (i - 10 < 0) s = "00" + i;
        else if (i - 100 < 0) s = "0" + i;
        else s = i.ToString();
        SetText(s);
    }
    public void SetText(string str) //自分のtextを編集
    {
        transform.FindChild("Text").GetComponent<Text>().text = str;
    }

}
