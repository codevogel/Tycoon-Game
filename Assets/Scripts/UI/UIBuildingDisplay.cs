using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class UIBuildingDisplay : MonoBehaviour
{
    [SerializeField] [CanBeNull] private BuildingPreset buildingInfo;

    [SerializeField] private TMP_Text buildingName;
    [SerializeField] private TMP_Text buildingInformationText;

    private readonly List<string> _buildingInformation = new List<string>();
    private void Start()
    {
        if (buildingInfo == null) return;

        buildingName.text = buildingInfo.name;
        buildingInformationText.text = null;

        if (buildingInfo.BuildCost.Length != 0)
        {
            _buildingInformation.Add("<b>Build Cost:</b> \n");
            GetText(buildingInfo.BuildCost);
        }

        if (buildingInfo.ProductionCost.Length != 0)
        {
            _buildingInformation.Add("<b>Production cost:</b> \n");
            GetText(buildingInfo.ProductionCost);
        }

        if (buildingInfo.Produces.Length != 0)
        {
            _buildingInformation.Add("<b>Produces:</b> \n");
            GetText(buildingInfo.Produces);
        }

        foreach (var informationString in _buildingInformation)
        {
            buildingInformationText.text += informationString;
        }
    }

    /// <summary>
    /// Get information from the scriptable object and turn it into text
    /// This text gets added to list in a certain order so it can later be printed
    /// </summary>
    /// <param name="resource">Resource gotten from the scriptable object</param>
    private void GetText(Resource[] resource)
    {
        if (resource.Length != 0)
        {
            foreach (var textString in resource)
            {
                _buildingInformation.Add($"{textString.Type.ToString()} : {textString.Amount}\n");
            }
        }
    }
}
