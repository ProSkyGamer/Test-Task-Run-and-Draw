#region

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#endregion

public class DrawingController : MonoBehaviour
{
    public static DrawingController Instance { get; private set; }

    public event EventHandler<OnTouchEndedEventArgs> OnTouchEnded;

    public class OnTouchEndedEventArgs : EventArgs
    {
        public List<Vector2> allTouchPosition;
    }

    [SerializeField] private Image pointImage;
    [SerializeField] private Transform pointsContainer;
    [SerializeField] private RectTransform drawingFieldBoundsRectTransform;

    private readonly List<Vector2> allTouchPositions = new List<Vector2>();

    private Bounds drawingBounds;
    private Vector2 drawingFieldSize;

    private Vector2 lastTouchPosition;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        drawingFieldSize = new Vector2(drawingFieldBoundsRectTransform.rect.width, drawingFieldBoundsRectTransform.rect.height);

        drawingBounds = new Bounds
        {
            center = drawingFieldBoundsRectTransform.position,
            size = drawingFieldSize
        };
    }

    private void Start()
    {
        AllCharactersController.OnAllPlayersDead += AllCharactersController_OnAllPlayersDead;
    }

    private void AllCharactersController_OnAllPlayersDead(object sender, EventArgs e)
    {
        Hide();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector2 newTouchPosition = Input.GetTouch(0).position;
            if (lastTouchPosition != newTouchPosition && drawingBounds.Contains(newTouchPosition))
            {
                lastTouchPosition = newTouchPosition;
                allTouchPositions.Add(newTouchPosition);
                Instantiate(pointImage, newTouchPosition, Quaternion.identity, pointsContainer);
            }
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 newTouchPosition = Input.mousePosition;
            if (lastTouchPosition != newTouchPosition && drawingBounds.Contains(newTouchPosition))
            {
                lastTouchPosition = newTouchPosition;
                allTouchPositions.Add(newTouchPosition);
                Instantiate(pointImage, newTouchPosition, Quaternion.identity, pointsContainer);
            }
        }
        else
        {
            if (allTouchPositions.Count != 0)
            {
                Debug.Log("Touch ended");
                OnTouchEnded?.Invoke(this, new OnTouchEndedEventArgs
                {
                    allTouchPosition = allTouchPositions
                });
                allTouchPositions.Clear();
                Transform[] allPointImages = pointsContainer.GetComponentsInChildren<Transform>();
                foreach (Transform foundPointImage in allPointImages)
                {
                    if (foundPointImage != pointsContainer && foundPointImage != pointImage.transform)
                        Destroy(foundPointImage.gameObject);
                }
            }
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public Vector2 GetDrawingFieldSize()
    {
        return drawingFieldSize;
    }
}