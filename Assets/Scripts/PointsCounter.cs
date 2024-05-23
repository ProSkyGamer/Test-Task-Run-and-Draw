#region

using System;
using TMPro;
using UnityEngine;

#endregion

public class PointsCounter : MonoBehaviour
{
    private TextMeshProUGUI pointsTextValue;

    private int currentPointsCount;

    private void Awake()
    {
        pointsTextValue = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        GemController.OnPlayerCollided += GemController_OnPlayerCollided;

        pointsTextValue.text = currentPointsCount.ToString();
    }

    private void GemController_OnPlayerCollided(object sender, EventArgs e)
    {
        currentPointsCount++;

        pointsTextValue.text = currentPointsCount.ToString();
    }

    private void OnDestroy()
    {
        GemController.OnPlayerCollided -= GemController_OnPlayerCollided;
    }
}