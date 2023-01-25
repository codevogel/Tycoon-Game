using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject nextPopup;
    [SerializeField] GameObject ControlOrBuildingsUI;
    [SerializeField] GameObject EnemySpawner;


    public void RemovePopup()
    {
        // LeanTween.easeInCubic(0.5f, 2f, 2f);
        // LeanTween.easeOutQuint(0.5f, 10f, 20f);
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(DestroyMe);

        if (nextPopup != null) nextPopup.SetActive(true);
        if (ControlOrBuildingsUI != null) ControlOrBuildingsUI.SetActive(true);
        if (EnemySpawner != null) EnemySpawner.SetActive(true);
    }

    // Update is called once per frame
    void DestroyMe()
    {
        Destroy(gameObject);
    }
}
