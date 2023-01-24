using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeNonInteractable : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Awake()
    {
        slider.interactable = false;
    }
}
