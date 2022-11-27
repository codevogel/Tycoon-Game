using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Slide_UI_Menu : MonoBehaviour
{
    [SerializeField] private Image resourceBackground;
    [SerializeField] private Button slideButton;
    public bool isExpanded;

    private float _resourceMenuPosition;

    private GameObject _buttonGo;
    [SerializeField]private Vector3 _buttonScale;

    private void Start()
    {
        isExpanded = true;

        _resourceMenuPosition = resourceBackground.gameObject.transform.position.x;

        _buttonGo = slideButton.gameObject;
        _buttonScale = _buttonGo.transform.localScale;
    }

    /// <summary>
    /// On Button press check if the resource menu is expanded or not.
    /// If expanded, collapse the menu else expand menu
    /// </summary>
    public void OnButtonPress()
    {
        if (isExpanded) CollapseField();
        else ExpandField();
    }
    
    /// <summary>
    /// Expand the resource menu
    /// </summary>
    private void ExpandField()
    {
        //Position is multiplied by -1 to reverse the position and then by 2 to move it to the correct position
        resourceBackground.gameObject.transform.position -=
            new Vector3(_resourceMenuPosition * -1 * 2, 0, 0);

        //Position is multiplied by -1 to reverse the position and then by 2 to move it to the correct position
        _buttonGo.transform.position -=
            new Vector3(_resourceMenuPosition * -1 * 2, 0, 0);
        
        _buttonScale = new Vector3(
            _buttonScale.x,
            _buttonScale.y * -1,
            _buttonScale.z);
        _buttonGo.transform.localScale = _buttonScale;
        
        isExpanded = true;
    }
    
    /// <summary>
    /// Collapse the resource menu
    /// </summary>
    private void CollapseField()
    {
        //Position is multiplied by -1 to reverse the position and then by 2 to move it to the correct position
        resourceBackground.gameObject.transform.position +=
            new Vector3(_resourceMenuPosition * -1 * 2, 0, 0);
        
        //Position is multiplied by -1 to reverse the position and then by 2 to move it to the correct position
        slideButton.gameObject.transform.position +=
            new Vector3(_resourceMenuPosition * -1 * 2, 0, 0);

        _buttonScale = new Vector3(
            _buttonScale.x,
            _buttonScale.y * -1,
            _buttonScale.z);
        _buttonGo.transform.localScale = _buttonScale;

        isExpanded = false;
    }
}
