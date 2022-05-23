using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionProximitySpawner : MonoBehaviour
{
    [Tooltip("Is this section the first section?")]
    public bool isFirstSection;
    [Tooltip("Is this section the current section?")]
    public bool isCurrentSection;
    [Tooltip("Is the section started?")]
    public bool sectionStarted;
    [Tooltip("Has the section been completed?")]
    public bool sectionComplete;
    [Tooltip("The amount of objectives to be destroyed in this section. Note: If the section is a timer, input 1.")]
    public int objectiveCount;
    [Tooltip("The collider that will interact with the player and start the section.")]
    public Collider sectionTrigger;
    [Tooltip("The section objects, they should be inactive, the trigger activates them.")]
    public GameObject sectionObjects;
    [Tooltip("Additional objects to deactivate when this section is complete.")]
    public List<GameObject> additionalObjectsToDeactivate;
    [Tooltip("The amount of time after this section is complete that it will wait until it destroys the section obejcts.")]
    public float cleanupTimer = 20f;
    [Tooltip("Remaining objectives in this section.")]
    public int remainingObjectives;
    [Tooltip("The objective box.")]
    public ObjectiveWaypointTrigger objectiveWaypoint;
    [Tooltip("Audio source for Level Section audio.")]
    public AudioSource levelSectionAudioSource;
    [Tooltip("Audio clip list")]
    public List<AudioClip> levelSectionClips;
    [Tooltip("Delay time for opening audio")]
    public int openingAudioDelay;
    [Tooltip("The objective window hud object for this level section")]
    public GameObject objectiveWindow;
    [Tooltip("The amount of seconds the objective window will stay up.")]
    public int objectiveWindowSeconds = 10;

    //private void OnTriggerEnter(Collider other) - Old way, removed due to missing the entering event
    //{
    //    if (isCurrentSection && !isFirstSection)
    //    {
    //        StartSection();
    //    }
    //}

    private void OnTriggerStay(Collider other) //New way, checks for event every frame, but does not miss it
    {
        if(isCurrentSection && !isFirstSection && !sectionStarted)
        {
            StartSection();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(sectionComplete)
        {
            CleanUpSectionObjects();
            if(additionalObjectsToDeactivate.Count > 0)
            {
                CleanupAdditionalObjects();
            }
        }
    }

    private void Start()
    {
        remainingObjectives = objectiveCount;
        if (isFirstSection)
        {
            StartSection();
        }
    }

    public void CleanupAdditionalObjects()
    {
        foreach(GameObject go in additionalObjectsToDeactivate)
        {
            go.SetActive(false);
        }
    }

    public void SetCurrentSection()
    {
        if (objectiveWaypoint != null) { objectiveWaypoint.gameObject.SetActive(true); }
        isCurrentSection = true;
        StartCoroutine(PlayAudioFromListWithDelay(0, openingAudioDelay));
        ShowObjective(objectiveWindowSeconds);
    }

    private void StartSection()
    {
        if(!sectionStarted)
        {
            if(sectionObjects != null)
            {
                sectionObjects.SetActive(true);
            }
            sectionStarted = true;
            //PlayAudioFromList(1);
        }
    }

    private void CleanUpSectionObjects()
    {
        if (objectiveWaypoint != null) { objectiveWaypoint.gameObject.SetActive(false); }
        Destroy(sectionObjects, cleanupTimer);
    }

    private void Subtract()
    {
        remainingObjectives--;
    }

    private void SubtractAndCloseout()
    {
        remainingObjectives--;
        sectionComplete = true;
        isCurrentSection = false;
        PlayAudioFromList(2);
    }

    public void SubtractObjectiveFromRemaining()
    {
        if(remainingObjectives > 1)
        {
            Subtract();
        }
        else
        {
            SubtractAndCloseout();
        }
        
    }

    public void PlayAudioFromList(int index)
    {
        if(levelSectionClips.Count != 0)
        {
            if(index < levelSectionClips.Count)
            {
                levelSectionAudioSource.clip = levelSectionClips[index];
                levelSectionAudioSource.Play();
            }
        }
    }

    private void ShowObjective(int seconds)
    {
        if (objectiveWindow != null)
        {
            objectiveWindow.SetActive(true);
        }
        StartCoroutine(HideObjectiveAfterSeconds(seconds));
    }

    IEnumerator PlayAudioFromListWithDelay(int audioIndex, int delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlayAudioFromList(audioIndex);
    }

    IEnumerator HideObjectiveAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (objectiveWindow != null)
        {
            objectiveWindow.SetActive(false);
        }
    }
}
