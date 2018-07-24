Project 6
Author: Zhen Tian, Drake Addis

Drake- Gun Mechanics and systems and Player Health system
Zhen- Maze Design, Enemies, Player Movement, and Shield system

User Input: 
	Movement- W,A,S,D
	Sprint- Left Shift
	Jump- Space
	Pause- Tab
	Fire Weapon- Left Click
	Switch Weapon- Mouse Scroll
	Reset Level- R
	
Summary:
	This is a first person shooting game.  A maze is randomly generated
	each time the game is played and the player must move and jump through map 
	and find the exit.  There are two different enemies and two different
	weapons for the player to use. The map include various items throughout
	for the player to collect. If enemies detect the player, they will follow
	the player and attempt to kill them. Overall, the team accomplished most 
	parts in the proposal. 
	
	The game implementation result is slightly different with the proposal the team 
	provided at early stage of this project. 
	
	First, in the proposal the team would like to have the enemy path preseted, 
	that is, the enemy will follow a certain path to search for player. However, 
	the team member found this very hard to do because they must figure out many 
	problems such as how to generate a path in a randomly generated scene instead 
	of a static scene, how to implement the pathfinding algorithm so that the enemy 
	is able to follow the path, be interrupted by the player, and return to its own 
	path(this part will be very much time-consuming), how to avoid inconsistency of 
	AI movement(for example, if the enemy collides with the wall, another enemy or 
	some other obstacles, how to get the enemy back to its own path). Hence, the team 
	decided that the enemy should be able to search for the player in the scene freely. 
	
	Second, the proposal said the player should be able to crouch or crawl in order to 
	avoid damage. The team found this setting useless because all enemies in the game 
	are not able to attack the player from a certain distance, they must get very close 
	to to hit the player. Therefore, this design was replaced with jump so that the 
	player can do a "rocket jump" with the help of the rocket launcher. This is much more 
	useful than crouching or crawling. 
	
	Additionally, at early stage, the team wanted to use bumping map, particle system and 
	lighting system to enhance the graphics of the game. However, since the team members 
	are not experienced game designers, and because of the limited time, they were not able 
	to achieve this goal. 
	
P.S. 
	The first person shooting camera movement is implemented based on the Holistic3d's tutorial 
	"How to construct a simple First Person Controller with Camera Mouse Look in Unity 5"
	Retrieved from: https://www.youtube.com/watch?v=blO039OzUZc&t=314s
	