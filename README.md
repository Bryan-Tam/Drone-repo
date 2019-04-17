# Drone-repo
547 Project (accidentally deleted original README)

## Current Functionality
* Right click to create drone instance
* Left click on drones to open info-menu
* Click "Delete" to delete instance of drone
* WASD to pan around
* Q/E to yaw to the sides
* Ctrl to move down, space to move up
* Click play/pause to toggle time simulation (or P, or esc)
* Drone routing to clickable spot (click "route" then select spot)

## To Do
* Drone authenticate other drone (radius)
* Geofence policy enforcement
* Attacker
* (?) Centralized traffic management
* (?) Trusted entity network (indicated by color)

## Extras
* Project selection onto the ground
* Lines indicating routes

## Scripts
### drone.cs
Contains delete script, which will migrate to pause_script. To contain communications script and object info. Maybe Routing?
### drone_info_display.cs
Sits on canvas of drone prefab. This controls the info-menu display scripts
### faceCamera.cs
This script also sits in the canvas so that the info-menu always faces the camera.
### FreeCame.cs
Allows Camera movement.
### pause_script.cs
Cannot be easily refactored, but contains the simulation script for the overall program. Contains mouse click functionality and toggling of simulation playback.
