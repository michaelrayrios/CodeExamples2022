using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VSX.UniversalVehicleCombat; //Reference VSX for finding health

public class ChaserTrail_Anchor : MonoBehaviour
{
    private ChaserTrail_TrailHead trailHead;
    public float smokeCheckInterval = 1f;
    public float relocationInterval = 0.1f; //Relocation interval, default to .1sec
    [Range(0, 100)]
    public int healthPercentToSmoke = 70;
    public HealthType healthType;
    public ChaserTrail_Pool chaserPool;
    private bool trailing;
    private bool checkingForSmoke;
    private Health healthRef;
    public bool projectile = false;

    //On enable setup
    private void OnEnable()
    {
        StartCoroutine("WaitThenSetup");
    }

    private void OnDisable()
    {
        if(!projectile) { StopCheckingForSmoke(); }
        if(trailing == true)
        {
            StopTrailing();
        }
    }

    private void StartCheckingForSmoke()
    {
        CheckingForSmoke(true);
        StartCoroutine("CheckForSmoke");
    }   
    
    private void StopCheckingForSmoke()
    {
        CheckingForSmoke(false);
    }

    private void StartTrailing()
    {
        GetTrailHead();
        PlayTrailAnimations();
        Trailing(true);
        StartCoroutine("RelocateTrail");
    }

    private void StopTrailing()
    {
        TrailheadCollection(true);
        //StopTrailAnimations();
        ReturnTrailHead();
        Trailing(false);
    }

    //Find Health if it exists
    private void FindHealth() //This script requires VSX
    {
        healthRef = GetComponentInParent<Health>();
    }

    private void SmokeCheck()
    {
        if((healthRef.GetCurrentHealthFractionByType(healthType)*100) <= healthPercentToSmoke)
        {
            if(trailing != true)
            {
                StartTrailing();
            }
        }
        else
        {
            if(trailing != false)
            {
                StopTrailing();
            }
        }
    }

    //Find the pool object
    private void FindPoolObject()
    {
        chaserPool = FindObjectOfType<ChaserTrail_Pool>();
    }

    //Get a trailhead from the Pool object
    public void GetTrailHead()
    {
        trailHead = chaserPool.GetTrail();
    }


    //Release the trailhead back to the pool for reuse
    public void ReturnTrailHead()
    {
        if(chaserPool != null) { chaserPool.ReturnTrail(trailHead); }
        trailHead = null;
    }

    //Play Trail Animations
    public void PlayTrailAnimations()
    {
        trailHead.PlayAllAnimations();
    }

    //Stop Trail Animations
    public void StopTrailAnimations()
    {
        trailHead.StopPlayingAnimations();
    }

    //Set Trailing Bool
    public void Trailing(bool isTrailing)
    {
        trailing = isTrailing;
    }

    //Set Smoking Bool
    public void CheckingForSmoke(bool isChecking)
    {
        checkingForSmoke = isChecking;
    }

    //Set Chaser TrailHead Collection bool
    public void TrailheadCollection(bool requiresCollection)
    {
        trailHead.requiresCollection = requiresCollection;
    }

    IEnumerator CheckForSmoke()
    {
        SmokeCheck();
        yield return new WaitForSeconds(smokeCheckInterval);
        if(checkingForSmoke)
        {
            StartCoroutine("CheckForSmoke");
        }
        else
        {
            //Do nothing
        }
    }

    IEnumerator RelocateTrail()
    {
        trailHead.transform.position = transform.position;
        yield return new WaitForSeconds(relocationInterval);
        if(trailing)
        {
            StartCoroutine("RelocateTrail");
        }
        else
        {
            //Do nothing
        }
    }

    IEnumerator WaitThenSetup()
    {
        yield return new WaitForSeconds(0.5f);
        if(chaserPool == null)
        {
            //Debug.Log("Chaser pool was null on object " + transform.parent.transform.gameObject.name);
            FindPoolObject();
        }
        else
        {
            //Debug.Log("Chase pool info was prefilled on object " + transform.parent.transform.gameObject.name);
        }
        if (projectile) { StartTrailing(); }
        else
        {
            FindHealth();
            StartCheckingForSmoke();
        }
    }
}
