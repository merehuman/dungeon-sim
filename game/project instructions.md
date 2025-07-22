 Overall Goal: To create a simulation of a tabletop roleplaying game in the terminal, where the user types in start and the game rolls "dice" (generates a random number based off of specified tables) and follows the instructions corresponding to the "rolled" number in addition to randomly making choices it is faced with in the course of the simulation. The game displays each "choice" that the simulation makes and each dice roll number and the written outcome in the terminal. This is a simulation of a game, the only user interaction is when the press start at the very beginning, and otherwise all of the choices are made randomly.

 Detailed Steps:
 We want to create a prototype of the game first, following these steps. The prototype will just focus on character creation. It will use the character rules file in the "tabletop materials/rules" folder and the character tables file in the tabletop materials/character tables" folder. 

 1. Turn the tables in the "tabletop materials/tables" folder into .csv files.

 2. Create a system for "rolling dice" that can then read the tables in the tabletop materials folder and determine an outcome.

 3. Create a system that prints to the terminal all of the rolled dice outcomes and descriptions in the tables and puts together a character sheet. The printing will be iterative and then once the character sheet is finished it will print everything out again all together.