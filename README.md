# Underpin Assignment - 2D Elevator Simulation

## Overview
This is a Unity project for the Underpin Assignment, featuring a fully functional 2D Elevator Simulation. The system handles 3 distinct elevators servicing 6 floors (Ground through 5), built using Unity Editor version `6000.4.6f1`.

## Technical Architecture & Logic
This project was built with clean code practices and a scalable architecture in mind:
- **Observer Pattern (C# Events):** Utilized for decoupled communication between the elevators and UI components, ensuring buttons accurately light up and turn off based on specific elevator arrivals.
- **Singleton Pattern:** The `ElevatorManager` acts as the central dispatcher to evaluate elevator availability, calculate distances, and penalize busy elevators.
- **Directional SCAN Algorithm:** Elevators are directionally aware. They intelligently pick up queued passengers along their current path of travel before switching directions, rather than blindly following a chronological first-come, first-served queue.

## Key Unity Features
- Universal Render Pipeline (URP)
- Canvas-based UI with RectTransform movement logic
- Unity Animator for state-driven door sequences
- New Input System 
- Visual Scripting support
- 2D Animation / Tilemap tooling
- Multiplayer Center package included

## Requirements
- Unity Editor `6000.4.6f1`
- Git (if cloning from version control)

## How to Open the Project
1. Launch Unity Hub.
2. Click **Add** and select the `Underpin Assignment` folder if it is not already in your projects list.
3. Open the project from the Hub using the matching Unity version.

## Project Files
- `Assets/Scripts/` - Contains the core simulation logic (`ElevatorManager.cs`, `ElevatorController.cs`, `ElevatorInsidePanel.cs`, etc.)
- `Assets/Scenes/` - Contains the main playable simulation scene.
- `Packages/manifest.json` - Package dependencies.
- `ProjectSettings/` - Project configuration.

## Notes
- Keep the `Library/` and `Temp/` folders out of source control.
- Please ensure you use the exact Unity Hub version (`6000.4.6f1`) to avoid any compatibility or package resolution issues.

## Contact
Developed by Jainish. 
If you need help running the project or want to understand how the assignment is structured, open the project in Unity and inspect the `Assets/Scenes` and `Assets/Scripts` folders.