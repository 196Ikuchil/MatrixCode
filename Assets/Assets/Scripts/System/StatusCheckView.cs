using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusCheckView : MonoBehaviour {
    public Text hp;
    public Text power;
    public Text mpower;
    public Text sp;
    public Text spr;
    public Text def;
    public Text comNum;
    public Text openNum;
    public GameObject sdReaderPref;

    SaveDataReader sdReader;
    void Awake()
    {
        sdReader = Instantiate(sdReaderPref).GetComponent<SaveDataReader>();
        hp.text = sdReader.GetHP().ToString();
        power.text = sdReader.GetPOWER().ToString();
        mpower.text = sdReader.GetMAJICPOWER().ToString();
        sp.text=sdReader.GetSP().ToString();
        spr.text = sdReader.GetSPRECOVER().ToString();
        def.text = sdReader.GetDEFENCE().ToString();
        comNum.text = sdReader.GetCommandNum().ToString();
        openNum.text = sdReader.GetMatrixOpenNum().ToString();
       
    }

    void Start()
    {
        iTween.MoveFrom(this.gameObject, iTween.Hash("y", this.transform.position.y + 50f, "time", 1f));
    }

    public void DestroyView()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("y", this.transform.position.y + 120f, "time", 1f));
        sdReader.DestroyObject();
        Destroy(this.gameObject,1.5f);
        
    }
}
