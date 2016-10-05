using UnityEngine;
using System.Collections;

public class DelayMagicBoxCollider : MonoBehaviour
{
    BoxCollider c;
    public float delaytime = 0;
    // Use this for initialization
    void Awake()
    {
        c = GetComponent<BoxCollider>();
        c.isTrigger = false;
        StartCoroutine(delay());
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator delay()
    {
        yield return new WaitForSeconds(delaytime);
        c.isTrigger = true;
    }
}
