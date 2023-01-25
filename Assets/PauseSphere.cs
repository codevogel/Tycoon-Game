using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSphere : MonoBehaviour
{
    [SerializeField] GameObject sphere;

    public void SphereAnim()
    {
        sphere.SetActive(true);
        LeanTween.scale(sphere, new Vector3(150f, 150f, 150f), 0.1f).setOnComplete(ResetMe);
    }

    private void ResetMe()
    {
        LeanTween.scale(sphere, Vector3.zero, 0f);
        sphere.SetActive(false);
        Time.timeScale = 0f;
    }
}