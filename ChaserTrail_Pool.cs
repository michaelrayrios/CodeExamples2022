using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserTrail_Pool : MonoBehaviour
{
    public int poolSize= 20; //Pool size, default to 20
    public int returnDelay = 5; //Return delay to disable object, default to 5 seconds
    public List<ChaserTrail_TrailHead> trailHeads_free;
    public List<ChaserTrail_TrailHead> trailHeads_inUse;

    public ChaserTrail_TrailHead trailHeadReference;

    //Onenable Setup
    private void OnEnable()
    {
        FillPool(trailHeadReference);
    }

    //Create several objects and add to the free list
    private void FillPool(ChaserTrail_TrailHead pooledTrail)
    {
        for(int i = 0; i < poolSize; i++)
        {
            ChaserTrail_TrailHead aTrail = Instantiate(trailHeadReference);
            aTrail.gameObject.SetActive(false);
            trailHeads_free.Add(aTrail);
        }
    }

    //assign an object from the free list to the requestee
    public ChaserTrail_TrailHead GetTrail()
    {
        CheckTopOff();
        ChaserTrail_TrailHead openTrail = trailHeads_free[0];
        trailHeads_free.Remove(openTrail);
        trailHeads_inUse.Add(openTrail);
        openTrail.gameObject.SetActive(true);
        return openTrail;
    }

    //receive an object from the requestee after use
    public void ReturnTrail(ChaserTrail_TrailHead returningTrail)
    {
        StartCoroutine(ReturnTrailWithDelay(returningTrail));
    }

    //create more objects if there are none available
    public void CheckTopOff()
    {
        if(trailHeads_free.Count == 0)
        {
            ChaserTrail_TrailHead aTrail = Instantiate(trailHeadReference);
            trailHeads_free.Add(aTrail);
        }
    }

    IEnumerator ReturnTrailWithDelay(ChaserTrail_TrailHead returningTrail)
    {
        trailHeads_inUse.Remove(returningTrail);
        yield return new WaitForSeconds(returnDelay);
        trailHeads_free.Add(returningTrail);
        returningTrail.gameObject.SetActive(false);
    }
}
