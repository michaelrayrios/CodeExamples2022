using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSectionHandler : MonoBehaviour
{
    public List<LevelSectionProximitySpawner> levelSections;
    public int currentLevelSection;

    private void Start()
    {
        currentLevelSection = 0;
        SetCurrentSection(currentLevelSection);
        StartCoroutine("CheckSectionCycle");
    }

    IEnumerator CheckSectionCycle()
    {
        if(CheckCurrentLevelSectionForComplete())
        {
            SetCurrentSection(currentLevelSection);
        }
        yield return new WaitForSeconds(3);
        StartCoroutine("CheckSectionCycle");
    }

    private void SetCurrentSection(int section)
    {
        levelSections[currentLevelSection].SetCurrentSection();
    }

    private bool CheckCurrentLevelSectionForComplete()
    {
        if(levelSections[currentLevelSection].sectionComplete)
        {
            currentLevelSection++; //Move to next section
            return true;
        }
        else
        {
            return false;
        }
    }
}
