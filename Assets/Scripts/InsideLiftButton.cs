using UnityEngine;
using UnityEngine.UI;

public class InsideLiftButton : MonoBehaviour
{
    public ElevatorController myElevator;
    public int targetFloor;
    
    private Image buttonImage;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.green;

    private bool isPressed = false;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.color = normalColor;
        
        // Subscribe to Observer (Specific to THIS elevator)
        myElevator.OnFloorReachedInside += OnFloorReached;
    }

    public void OnClick()
    {
        if (!isPressed && myElevator.currentFloor != targetFloor)
        {
            isPressed = true;
            buttonImage.color = pressedColor; // Light Up
            myElevator.AddDestination(targetFloor);
        }
    }

    // Observer Callback
    private void OnFloorReached(int floor)
    {
        if (floor == targetFloor)
        {
            isPressed = false;
            buttonImage.color = normalColor; // Light off
        }
    }

    private void OnDestroy()
    {
        if (myElevator != null)
        {
            myElevator.OnFloorReachedInside -= OnFloorReached;
        }
    }
}