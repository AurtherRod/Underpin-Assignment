using UnityEngine;
using UnityEngine.UI;

public class ElevatorInsidePanel : MonoBehaviour
{
    [Header("Setup")]
    public ElevatorController myElevator;

    [Header("Button References")]
    [Tooltip("Drag your button Image components here in order, from Floor 0 to 5")]
    public Image[] buttonImages = new Image[6];

    [Header("Visuals")]
    public Color normalColor = Color.white;
    public Color pressedColor = Color.green;

    private void Start()
    {
        // Subscribe once for the whole panel
        if (myElevator != null)
        {
            myElevator.OnFloorReachedInside += TurnOffButtonLight;
        }
    }

    // THIS is the method we call from Unity's On Click event
    public void OnFloorButtonPressed(int floorNumber)
    {
        if (myElevator.currentFloor != floorNumber)
        {
            // Turn on the specific button's light using the array index
            if (floorNumber >= 0 && floorNumber < buttonImages.Length)
            {
                buttonImages[floorNumber].color = pressedColor;
            }

            // Tell the elevator to move
            myElevator.AddDestination(floorNumber);
        }
    }

    // Observer Callback
    private void TurnOffButtonLight(int floorNumber)
    {
        // Turn off the specific button's light when the elevator arrives
        if (floorNumber >= 0 && floorNumber < buttonImages.Length)
        {
            buttonImages[floorNumber].color = normalColor;
        }
    }

    private void OnDestroy()
    {
        if (myElevator != null)
        {
            myElevator.OnFloorReachedInside -= TurnOffButtonLight;
        }
    }
}