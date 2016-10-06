using UnityEngine;
using System.Collections;

public class DelayMagicSphereCollider : MonoBehaviour
{
    SphereCollider c;
    public float delaytime = 0;
    // Use this for initialization
    void Awake()
    {
        c = GetComponent<SphereCollider>();
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
