using System;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager Instance { get; private set; }

    public List<ElevatorController> elevators;

    public event Action<int, LiftDirection> OnElevatorArrivedAtFloor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RequestElevator(int floor, LiftDirection direction)
    {
        ElevatorController bestElevator = FindBestElevator(floor, direction);
        if (bestElevator != null)
        {
            bestElevator.AddDestination(floor);
        }
    }

    private ElevatorController FindBestElevator(int floor, LiftDirection dir)
    {
        ElevatorController closest = null;
        float lowestScore = Mathf.Infinity;

        foreach (var lift in elevators)
        {
            // Base score is distance to the requested floor
            float score = Mathf.Abs(lift.currentFloor - floor);

            // NEW: Add a heavy penalty if the lift is already moving or has a queue!
            if (lift.IsBusy)
            {
                score += 100f; // This forces the algorithm to prefer idle lifts
            }

            if (score < lowestScore)
            {
                lowestScore = score;
                closest = lift;
            }
        }
        return closest;
    }

    public void NotifyElevatorArrival(int floor, LiftDirection direction)
    {
        OnElevatorArrivedAtFloor?.Invoke(floor, direction);
    }
}