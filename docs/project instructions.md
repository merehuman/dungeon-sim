 Overall Goal: To create a simulation of a tabletop roleplaying game in the terminal, where the user types in start and the game rolls "dice" (generates a random number based off of specified tables) and follows the instructions corresponding to the "rolled" number in addition to randomly making choices it is faced with in the course of the simulation. The game displays each "choice" that the simulation makes and each dice roll number and the written outcome in the terminal. This is a simulation of a game, the only user interaction is when the press start at the very beginning, and otherwise all of the choices are made randomly.

 Detailed Steps:
 We want to create a prototype of the game first, following these steps. The prototype will just focus on character creation. It will use the character rules file in the "tabletop materials/rules" folder and the character tables file in the tabletop materials/character tables" folder. 

 1. Turn the tables in the "tabletop materials/tables" folder into .csv files.

 2. Create a system for "rolling dice" that can then read the tables in the tabletop materials folder and determine an outcome.

 3. Create a system that prints to the terminal all of the rolled dice outcomes and descriptions in the tables and puts together a character sheet. The printing will be iterative and then once the character sheet is finished it will print everything out again all together.

To-Do (Next Steps):
4. Clean up UI. Create 3 columns, the far left being the menu button items the user can click, the middle being the output from character creation and hex exploration or any data output generated, and the right hand side being the hex data. 
    - Also, we want to limit repeated data being shown. So when displaying character or hex information first give a list of the options that the user can choose from. There should be a line along the bottom of this section that you can type in. Here you can type in the number from the list that you want to display more information about.
    - We also want a visualization of the hex elements in the righthand side column. This will take on the form of a hexagonal grid where explored hexes will show minimal summary information about each one.
