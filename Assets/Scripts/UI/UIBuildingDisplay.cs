using System;
using System.Collections.Generic;
using Architect.Placeables.Presets;
using Buildings.Resources;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBuildingDisplay : MonoBehaviour
    {
        [SerializeField] [CanBeNull] private BuildingPreset buildingInfo;

        [SerializeField] private TMP_Text buildingName;
        [SerializeField] private TMP_Text buildingInformationText;

        private readonly List<string> _buildingInformation = new();
        private void Start()
        {
            if (buildingInfo == null) return;

            buildingName.text = buildingInfo.name;
            buildingInformationText.text = null;

            if (buildingInfo.buildCost.Length != 0)
            {
                _buildingInformation.Add("<b>Build Cost:</b> \n");
                GetText(buildingInfo.buildCost);
            }

            if (buildingInfo.productionCost.Length != 0)
            {
                _buildingInformation.Add("<b>Production cost:</b> \n");
                GetText(buildingInfo.productionCost);
            }

            if (buildingInfo.produces.Length != 0)
            {
                _buildingInformation.Add("<b>Produces:</b> \n");
                GetText(buildingInfo.produces);
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
                    _buildingInformation.Add($"{textString.type.ToString()} : {textString.amount}\n");
                }
            }
        }
    }
}
