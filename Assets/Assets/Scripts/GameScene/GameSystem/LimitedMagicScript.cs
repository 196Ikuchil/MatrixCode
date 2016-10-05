using UnityEngine;
using System.Collections;

public class LimitedMagicScript : MonoBehaviour {
    MagicMaster mMaster;
    MagicScript mScript;
	// Use this for initialization
	void Start () {
        mMaster = FindObjectOfType<MagicMaster>();
        mMaster.AddToLimitList(this.gameObject);
        mScript = GetComponent<MagicScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<HitArea>().receiverName ==mScript.attackerName) return;
        Destroy(this.gameObject); //MagicScriptはあたり判定しか削除しない

    }

    void OnDestroy()
    {
        mMaster.RemoveListObject(this.gameObject);
    }
}
