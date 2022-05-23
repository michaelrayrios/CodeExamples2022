using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionObjective : MonoBehaviour
{
    //A timer option to end a section
    [Tooltip("If this objective is a timer, and not enemies, check this box.")]
    public bool isTimer;
    [Tooltip("If using a timer, input the amount of seconds for the timer here.")]
    public int timerLength;
    [Tooltip("Find prox spawner in parent automatically?")]
    public bool findProxSpawner = false;
    //Reference for the prox spawner is found automatically as long as it is a parent
    [Tooltip("Prox spawner reference, if not found automatically in parent.")]
    public LevelSectionProximitySpawner proxSpawner;
    [Tooltip("Trigged subtraction on disable of this object")]
    public bool triggerSubtractOnDisable = false;

    private void Start()
    {
        if(isTimer)
        {
            StartCoroutine("RunTimer");
        }
    }

    //Subtracts an objective from the prox spawner - this should be called
    //Ondisable for an individual ship OR
    //On wave destroyed for a single wave entity OR
    //On waves destroyed for a multi-wave entity
    public void SubtractObjective()
    {
        proxSpawner.SubtractObjectiveFromRemaining();
    }

    //This is called to find the prox spawner automatically
    private void GetProximitySpawner()
    {
        proxSpawner = GetComponentInParent<LevelSectionProximitySpawner>();
    }

    private void OnEnable()
    {
        if (findProxSpawner) { GetProximitySpawner(); }
    }

    private void OnDisable()
    {
        proxSpawner.SubtractObjectiveFromRemaining();
    }

    private void Tick()
    {
        timerLength--;
    }

    IEnumerator RunTimer()
    {
        if( timerLength > 0)
        {
            Tick();
            yield return new WaitForSeconds(1f);
            StartCoroutine("RunTimer");
        }
        else
        {
            SubtractObjective();
            yield return new WaitForSeconds(1f);
        }
        
    }
}
