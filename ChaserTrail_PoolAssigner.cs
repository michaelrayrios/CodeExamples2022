using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserTrail_PoolAssigner : MonoBehaviour
{
    private ChaserTrail_Pool ctp;
    private ChaserTrail_Anchor cta_Cursor;

    private void OnEnable()
    {
        ctp = FindObjectOfType<ChaserTrail_Pool>();
        StartCoroutine("AssignPools");
    }

    IEnumerator AssignPools()
    {
        yield return new WaitForSeconds(1);
        foreach(Transform t in transform)
        {
            //Debug.Log("I see " + t.gameObject.name);
            cta_Cursor = t.gameObject.GetComponentInChildren<ChaserTrail_Anchor>();
            if(cta_Cursor != null)
            {
                cta_Cursor.chaserPool = ctp;
            }
        }
    }
}
