using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowPopUp : MonoBehaviour
{
    #region Popup vars
    public GameObject popup;
    public TMP_Text popuptext;
    private Tile _popupTile;
    #endregion

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ShowPopup();
        }
    }

    // Start is called before the first frame update
    /// <summary>
    /// doet popup laten zien on right click. 
    /// popup geeft informatie over de building/road en buttons om te upgraden/destroyen.
    /// </summary>
    void ShowPopup()
    {
        _popupTile = GridManager.Instance.HoverTile;
        if (_popupTile == null) return;
        if (_popupTile.HasContent)
        {
            if (_popupTile.Content is Building)
            {
                string output = "";
                popup.SetActive(true);
                Building currentBuilding = (Building)_popupTile.Content;
                Debug.Log(currentBuilding.output.Contents.Count);
                foreach (KeyValuePair<ResourceType, int> kvp in currentBuilding.output.Contents)
                {
                    output += string.Format("Resource = {0}, Amount  = {1}", kvp.Key, kvp.Value);
                    output += "\n";
                }

                popuptext.text = $"{output}";
                Console.WriteLine(output);
            }
            else if (_popupTile.Content is Road)
            {

            }
        }
        else
        {
            return;
        }
    }
    
    /// <summary>
    /// Removes the contents at targetTile.
    /// </summary>
    /// <param name="popupTile">The tile at which to remove the contents.</param>
    public void RemoveObjectAt()
    {
        _popupTile.RemoveContent();
        popup.SetActive(false);
    }
}
