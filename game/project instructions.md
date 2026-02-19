 Overall Goal: To create a simulation of a tabletop roleplaying game in the terminal, where the user types in start and the game rolls "dice" (generates a random number based off of specified tables) and follows the instructions corresponding to the "rolled" number in addition to randomly making choices it is faced with in the course of the simulation. The game displays each "choice" that the simulation makes and each dice roll number and the written outcome in the terminal. This is a simulation of a game, the only user interaction is when the press start at the very beginning, and otherwise all of the choices are made randomly.

 Detailed Steps:
 We want to create a prototype of the game first, following these steps. The prototype will just focus on character creation. It will use the character rules file in the "tabletop materials/rules" folder and the character tables file in the tabletop materials/character tables" folder. 

 1. Turn the tables in the "tabletop materials/tables" folder into .csv files.

 2. Create a system for "rolling dice" that can then read the tables in the tabletop materials folder and determine an outcome.

 3. Create a system that prints to the terminal all of the rolled dice outcomes and descriptions in the tables and puts together a character sheet. The printing will be iterative and then once the character sheet is finished it will print everything out again all together.

To-Do (Next Steps):
1. Implement advanced name modifier logic to generate more varied and meaningful character names.
2. Handle 'stat of choice' modifiers interactively or randomly for non-interactive simulation.
3. Expand HP calculation to handle special cases (e.g., reroll, N/A, or race/class-specific rules).
4. Implement detailed attacks/spells generation, including random spell selection for Wizards/Clerics and martial attack details for Fighters.
5. Add logic to restrict weapon selection for Rogues, Clerics, and Wizards as per rules.
6. Add support for multiple personality traits and their activation rules.
7. Refactor code for modularity and maintainability (e.g., separate character, item, and rolling logic into classes).
8. Add logging of all dice rolls and choices to a session log for debugging and review.
9. Implement automated tests for character creation and table parsing.
10. Format the console output so that the generated character sheet and all rolls/choices are displayed in a clear, readable, and visually appealing way.
11. Prepare for the next phase: simulating dungeon exploration and encounters.