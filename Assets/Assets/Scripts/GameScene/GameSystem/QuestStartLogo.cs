using UnityEngine;
using System.Collections;

public class QuestStartLogo : MonoBehaviour {
    AudioSource startSeSource;
    public AudioClip startAudioSeClip;
	// Use this for initialization
	IEnumerator Start () {
        startSeSource = gameObject.AddComponent<AudioSource>();
        startSeSource.clip = startAudioSeClip;
        startSeSource.loop=false;

        startSeSource.Play();
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(this.gameObject, iTween.Hash("x", this.transform.position.x + 100f, "time", 1f));
        Destroy(this.gameObject,1.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
