using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{


    void Start()
    {
        iTween.MoveFrom(this.gameObject, iTween.Hash("y", this.transform.position.y + 50f, "time", 1f));
    }

    public void DestroyView()
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("y", this.transform.position.y + 120f, "time", 1f));
        Destroy(this.gameObject, 1.5f);

    }
}
