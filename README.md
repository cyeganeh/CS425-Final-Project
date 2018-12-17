# Maze Rocky

### by christian Yeganeh
A CS425 Game Final Project

[TOC]

## Game Play:

This game isn't crazy complex or difficult to play. You start out in a mazey looking dungeon. Your goal is to find the hidden treasure chest that is somewhere in the maze. You can traverse the dungeon by moving with either WASD or the Arrow keys. However, beware of the skelentons, because while you are an almost invinsible zombie, skelentons know how to cast magic that can slowly wear down your health. Becare and fight with your rocks, papers, and scissors to out best the skelentons and get to their precious treasure.

## Technical Components: 

The main focus of this game was to be able to design and implement a procedurely generated maze, along with turn based combat, and artificial inteligence for the enemies. The game takes play with you exploring the randomly generated world in a real-time realm, however, if you get too close to an enemy, it starts the turn-based combat. The enemies normally are just randomly moving, however, if they see you, they will make a beeline for you and try to attack you. 

## The Process:

There was a lot of research into methods for creating mazes. In the end, I implemented a simple algorithm that can be adjusted to create more rooms or more corridors. The artificial intelligence came next and was difficult to establish how they would react or move when they see you with walls. I almost implemented an A\* approach for searching around corners, but the process kept ending up with bugs, so it was eventually ripped out. 
    
## Additional features

-Sound/music
-A\* searching for the artificial intelligence
-better more throughly designed UI, possibly including health bars instead of just text. 

[Video introduction](https://youtu.be/FbCJBwZ2hrA)
