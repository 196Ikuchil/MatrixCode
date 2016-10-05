using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleSceneCtrl : MonoBehaviour {

    public GameObject panel;
    public GameObject credit;
    public GameObject canvas;
    public AudioClip decide;
    AudioSource decideSource;
    public AudioClip cancel;
    AudioSource cancelSource;
    void Awake()
    {
        //fpsの設定
        Application.targetFrameRate = 45;
        //画面の解像度
        float screenRate = (float)1024 * 0.7f / Screen.width;
        if (screenRate > 1) screenRate = 1;
        int width = (int)(Screen.width * screenRate);
        int height = (int)(Screen.height * screenRate);
        Screen.SetResolution(width, height, true, 15);
    }
	// Use this for initialization
	void Start () {
        decideSource = gameObject.AddComponent<AudioSource>();
        decideSource.clip = decide;
        decideSource.loop = false;
        cancelSource = gameObject.AddComponent<AudioSource>();
        cancelSource.clip = cancel;
        cancelSource.loop = false;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
        decideSource.Play();
        FadeManager.Instance.LoadLevel("SaveDataSelectScene", 2f);

    }
    public void DeleteSaveData(int i)
    {
        panel.GetComponent<DeleteDataPanel>().SetDeleteNum(i);
        panel.SetActive(true);
    }
    public void ToMatrixCodeRoom()
    {
        decideSource.Play();
        FadeManager.Instance.LoadLevel("MatrixCodeRoomScene", 1.5f);
    }
    public void PlayClickSe()
    {
        decideSource.Play();
    }
    public void CancelSe()
    {
        cancelSource.Play();
    }
    public void CreditOpen()
    {
        Instantiate(credit).transform.SetParent(canvas.transform,false);
        
    }
}
