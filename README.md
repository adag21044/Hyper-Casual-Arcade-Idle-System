# Hyper Casual Arcade Idle System

This Unity project demonstrates player movement, paper collection, and paper processing mechanics within a game environment. The player can move around, collect papers, place them on a work desk, and generate money. The project follows SOLID principles and uses game design patterns to ensure clean, maintainable code.

## Features

- **Player Movement**: The player can move towards the mouse click position. The movement is smooth, and the player automatically faces the direction of movement.
- **Paper Collection**: The player can collect papers from tables and stack them. The papers follow a smooth stacking animation.
- **Paper Placement**: Papers can be placed on work desks. Once placed, the papers are processed to generate money.
- **Work Desk Processing**: The work desk processes papers placed on it, with a corresponding animation. As papers are processed, money is generated.
- **Printer Functionality**: A printer object continuously prints papers, which can be collected by the player.

## Code Overview

### PlayerManager.cs
This script controls the player's movement, paper collection, and interaction with objects in the game world.

- **Movement**: The player moves towards the mouse click position, and the camera follows the player with a fixed offset.
- **Paper Collection**: Papers are collected from tables when the player is near them. The collected papers are stacked and follow a smooth animation.
- **Paper Placement**: When the player is near a work desk, they can place the collected papers on it. The papers are then processed by the work desk to generate money.

### WorkDesk.cs
This script manages the work desk's functionality, including processing papers and generating money.

- **Work Animation**: The work desk animates when processing papers, indicating that it is working.
- **Money Generation**: As papers are processed, money is generated and displayed in the game world.

### Printer.cs
This script handles the printer's functionality, continuously printing papers that the player can collect.

- **Paper Printing**: The printer prints papers at regular intervals. The printed papers are placed in specific positions and can be collected by the player.
