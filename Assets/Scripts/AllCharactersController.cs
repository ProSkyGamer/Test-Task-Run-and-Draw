#region

using System;
using System.Collections.Generic;
using UnityEngine;

#endregion

public class AllCharactersController : MonoBehaviour
{
    public static event EventHandler OnAllPlayersDead;

    [SerializeField] private List<Transform> allPlayers = new List<Transform>();
    [SerializeField] private float maxXOffset;
    [SerializeField] private float minXOffset;
    [SerializeField] private float maxYOffset;
    [SerializeField] private float minYOffset;

    private void Start()
    {
        DrawingController.Instance.OnTouchEnded += DrawingController_OnTouchEnded;

        AdditionalCharacterController.OnPlayerCollided += AdditionalCharacterController_OnPlayerCollided;
        SpikesController.OnPlayerCollided += SpikesController_OnPlayerCollided;
    }

    private void SpikesController_OnPlayerCollided(object sender, SpikesController.OnPlayerCollidedEventArgs e)
    {
        allPlayers.Remove(e.collidedPlayer);

        if (allPlayers.Count <= 0)
        {
            Time.timeScale = 0;
            OnAllPlayersDead?.Invoke(this, EventArgs.Empty);
        }
    }


    private void AdditionalCharacterController_OnPlayerCollided(object sender, EventArgs e)
    {
        AddPlayer();
    }

    private void DrawingController_OnTouchEnded(object sender, DrawingController.OnTouchEndedEventArgs e)
    {
        Vector2 screenResolution = DrawingController.Instance.GetDrawingFieldSize() / 2;

        List<Vector2> usedTouchPositions = new List<Vector2>();
        int countingIndex = e.allTouchPosition.Count / allPlayers.Count;

        for (int i = 0; i < allPlayers.Count; i++)
            usedTouchPositions.Add(e.allTouchPosition[i * countingIndex]);

        for (int i = 0; i < usedTouchPositions.Count; i++)
        {
            Vector3 newPlayerPosition = Vector3.zero;

            float xAxisMultiplier = usedTouchPositions[i].x > 0 ? maxXOffset : minXOffset;
            newPlayerPosition.x += usedTouchPositions[i].x / screenResolution.x * xAxisMultiplier;

            float yAxisMultiplier = usedTouchPositions[i].y > 0 ? maxYOffset : minYOffset;
            newPlayerPosition.z += usedTouchPositions[i].y / screenResolution.y * yAxisMultiplier;

            allPlayers[i].position = transform.position + transform.TransformDirection(newPlayerPosition);
        }
    }

    private void AddPlayer()
    {
        Transform newPlayer = Instantiate(allPlayers[0], transform);
        newPlayer.position = transform.position;
        allPlayers.Add(newPlayer);
    }

    private void OnDestroy()
    {
        DrawingController.Instance.OnTouchEnded -= DrawingController_OnTouchEnded;

        AdditionalCharacterController.OnPlayerCollided -= AdditionalCharacterController_OnPlayerCollided;
        SpikesController.OnPlayerCollided -= SpikesController_OnPlayerCollided;
    }
}