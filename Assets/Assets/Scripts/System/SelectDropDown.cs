using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectDropDown : MonoBehaviour {

    public GameObject panel;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void SetLeftValue(Dropdown d)
    {
        panel.GetComponent<MatrixOpenPanel>().SetLeftNum(d.value);
    }
    public void SetCenterValue(Dropdown d)
    {
        panel.GetComponent<MatrixOpenPanel>().SetCenterNum(d.value);
    }
    public void SetrightValue(Dropdown d)
    {
        panel.GetComponent<MatrixOpenPanel>().SetRightNum(d.value);
    }
}
