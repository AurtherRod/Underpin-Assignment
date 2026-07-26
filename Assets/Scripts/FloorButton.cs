using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour
{
    public int floorNumber;
    public LiftDirection buttonDirection;
    
    private Image buttonImage;
    public Color normalColor = Color.white;
    public Color pressedColor = Color.yellow;

    private bool isPressed = false;

    private void Start()
    {
        buttonImage = GetComponent<Image>();
        buttonImage.color = normalColor;
        
        // Subscribe to Observer
        ElevatorManager.Instance.OnElevatorArrivedAtFloor += OnElevatorArrived;
    }

    public void OnClick()
    {
        if (!isPressed)
        {
            isPressed = true;
            buttonImage.color = pressedColor; // Light Up
            ElevatorManager.Instance.RequestElevator(floorNumber, buttonDirection);
        }
    }

    // Observer Callback
    private void OnElevatorArrived(int arrivedFloor, LiftDirection dir)
    {
        // Turn off light if an elevator arrives at this floor going our desired direction
        if (arrivedFloor == floorNumber)
        {
            isPressed = false;
            buttonImage.color = normalColor;
        }
    }

    private void OnDestroy()
    {
        if (ElevatorManager.Instance != null)
        {
            ElevatorManager.Instance.OnElevatorArrivedAtFloor -= OnElevatorArrived;
        }
    }
}