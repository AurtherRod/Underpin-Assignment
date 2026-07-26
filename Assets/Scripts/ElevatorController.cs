using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public int liftID;
    public int currentFloor = 0;

    public float speed = 500f;
    public float doorWaitTime = 2f;

    public float[] floorHeights = new float[6];

    private List<int> destinationQueue = new List<int>();
    private bool isMoving = false;
    private Animator animator;
    private RectTransform rectTransform;

    // NEW: Track the current direction of travel
    private LiftDirection currentDirection = LiftDirection.None;

    public event Action<int> OnFloorReachedInside;

    public bool IsBusy => isMoving || destinationQueue.Count > 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void AddDestination(int floor)
    {
        if (currentFloor == floor && !isMoving)
        {
            StartCoroutine(OpenDoorsInstantly());
            return;
        }

        if (!destinationQueue.Contains(floor))
        {
            destinationQueue.Add(floor);
            // REMOVED: destinationQueue.Sort(); 

            if (!isMoving)
            {
                StartCoroutine(ProcessQueue());
            }
        }
    }

    private IEnumerator OpenDoorsInstantly()
    {
        isMoving = true;
        ElevatorManager.Instance.NotifyElevatorArrival(currentFloor, LiftDirection.None);
        OnFloorReachedInside?.Invoke(currentFloor);
        yield return StartCoroutine(OperateDoors());
        isMoving = false;
    }

    // NEW: The Elevator Algorithm Logic
    private int GetNextLogicalFloor()
    {
        if (destinationQueue.Count == 0) return currentFloor;

        int bestFloor = -1;
        float minDistance = float.MaxValue;

        // Step 1: Try to find the closest requested floor in the CURRENT direction
        foreach (int floor in destinationQueue)
        {
            if ((currentDirection == LiftDirection.Up && floor > currentFloor) ||
                (currentDirection == LiftDirection.Down && floor < currentFloor))
            {
                float dist = Mathf.Abs(floor - currentFloor);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestFloor = floor;
                }
            }
        }

        // Step 2: If no floors are in the current direction (or we are idle), find the absolute closest floor
        if (bestFloor == -1)
        {
            foreach (int floor in destinationQueue)
            {
                float dist = Mathf.Abs(floor - currentFloor);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestFloor = floor;
                }
            }
        }

        return bestFloor;
    }

    private IEnumerator ProcessQueue()
    {
        isMoving = true;

        while (destinationQueue.Count > 0)
        {
            // Calculate the smart destination instead of just grabbing index 0
            int targetFloor = GetNextLogicalFloor();

            // Set our new current direction
            currentDirection = targetFloor > currentFloor ? LiftDirection.Up : LiftDirection.Down;

            float targetY = floorHeights[targetFloor];

            while (Mathf.Abs(rectTransform.anchoredPosition.y - targetY) > 0.1f)
            {
                Vector2 newPos = rectTransform.anchoredPosition;
                newPos.y = Mathf.MoveTowards(rectTransform.anchoredPosition.y, targetY, speed * Time.deltaTime);
                rectTransform.anchoredPosition = newPos;
                yield return null;
            }

            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, targetY);
            currentFloor = targetFloor;

            // Remove the specific floor we just reached, not just index 0
            destinationQueue.Remove(targetFloor);

            ElevatorManager.Instance.NotifyElevatorArrival(currentFloor, currentDirection);
            OnFloorReachedInside?.Invoke(currentFloor);

            yield return StartCoroutine(OperateDoors());
        }

        isMoving = false;
        currentDirection = LiftDirection.None; // Reset direction when idle
    }

    private IEnumerator OperateDoors()
    {
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(doorWaitTime);
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
    }
}