<h4>This Report will cover Sequencing Patterns that are useful in Game Design</h4>
<h5>1. Double Buffer <br> 2. Game Loop <br> 3. Update Method</h5>

<h3>Double Buffer</h3>
<b>Primary Objective:</b><br> To cause a series of operations to appear instantaneous or simultaneous <br>
<b>Motivation: </b> <br>Makes things like rendering seem instantaneous to create the illusion of a coherent game world<br>
<b>How it works:</b><br>Two framebuffers, one represents the current frame (the one the video hardware is reading from) and the other is where our rendering code is writing to .
When rendering is done a switch() function is used to swap the framebuffers making the other framebuffer available to use.<br>
<b>When to use it:</b><br>
<b>1.</b> We have some state that is being modified incrementally<br>
<b>2.</b> That same state may be accessed in the middle of modification<br>
<b>3.</b> We want to prevent the code that's accessing the state from seeing the work in progress<br>
<b>4.</b> We want to be able to read the state and we don't want to have to wait while its being written<br>
<b>Summary</b><br> The core problem double buffering solves is the state being accessed while it is being modified. This has 2 causes : <br>
<b>1.</b> The state is directly accessed from code on another thread or interrupt.<br>
<b>2.</b> When the code doing the modification is accessing the same state that its modifying. This can occur in alot of situations such as in AI where entities are interacting with each other

<h3>Game Loop</h3>
<b>Primary Objective:</b><br>Decouple the progression of game time from user input and processor speed<br>
<b>Motivation: </b> <br>Provide immediate feedback instead of having to wait for results creating an interactive program.<br>
<b>How it works:</b><br>The game loop processes user input but doesn't wait for it, the loop always keeps going, and is usually implemented as follows: <br>
while(true){ <br>
processInput(), <br> 
Update(),  <br>
Render()} <br>
<b>ProcessInput()</b> - handles user input that has happened since the last call <br>
<b>Update()</b> - advances the game simulation one step (runs AI and physics <br>
<b>Render()</b> - Draws the game so the player can see what happened <br>
Overall the Game Loop does two main things: <br>
<b>1.</b> Run the loop shown above <br>
<b>2.</b> Tracks the passage of time to control the rate of gameplay <br>
<b>When to use it:</b><br>
This pattern is fairly special because it is the core of every game and most game engies such as the one being used to create Nexus (Unity) already have the game loop hardcoded into their engine.

<h3>Update Method</h3>
<b>Primary Objective:</b><br>Simulate a collection of independant objects by telling each object to process one frame of behavior at a time<br>
<b>Motivation: </b> <br>Each entity in a game should encapsulate its own behavior. 
This will keep the game loop uncluttered and make it easy to add or remove entities. 
To do this we add the update() method, the game loop maintains a collection of objects but doesn't know their concrete types. 
All it knows is that they can be updated seperating each objects behavior both from the game loop and from the other objects.
Game loop goes through the collection of entites and calls update() on each giving each a chance to perform one frame worth of behavior. 
The game loop has a dynamic collection of objects making it easy to add or remove them from collection<br>
<b>How it works:</b><br>The game world maintains a collection of objects. 
Each object implements an update method that stimulates one frame of the object's behavior. 
Each frame the game updates every object in the collection.<br>
<b>When to use it:</b><br>Good to use when a game has a wide range of live entites that the player interacts with, 
but bad to use when the game is more abstract and the moving pieces are less like living actors and more like pieces on a chessboard.<br>
<b>What to do if the order in which objects are updated is important:</b><br>
The order in which objects are updated are important. For example, if A comes before B in the list of objects, then when A updates, it will see B's previous state. 
But when B updates it will see A's new state since A has already been updated. <br> <br>
Updating sequentially each update incrementally changes the world from one valid state to the next with no period of time where things are ambiguous and need to be reconciled.
To stop new objects acting during the frame it was spawned can cache the number of objects in the list at the beginning of the update loop and only update that many before stopping:
This is done by incrementing an integer that represents the length of the array of objects and cache the length in numObjectsThisTurn at the beginning of the loop so the iteration stops before we get to new objects.
<b>Design Decisions:</b><br>
<b>What class to put the update() method in?</b> <br>
<b>1. Entity Class -  </b> This is the simplest option and works if you don't have too many kinds of entites, but having to subclass Entity evey time you want a new behavior can be painful when you have a large number of different kinds. <br>
<b>2. Component Class -  </b> This lets each component update itself independently, lets you decouple parts of a single entity from each other. More on this will be researched in Chapter 5 (Decoupling Patterns) <br>
<b>3. A Delegate Class - </b> Other patterns involve delegating part of a class's behaviour to another object. The State pattern does this and the Type Object pattern. <br>
<b>How do you deal with dormant objects?</b><br>
To deal with dormant objects you can maintain a collection of live objects that need updating, when an object is disabled it's removed from the collection. <br><br>
The problem with this is that using seperate collection for active objects takes extra memory, and there is still the master collection. Works ok when speed is more important than memory. 
This can be improved further by having the other collection only contain inactive entities instead of all of them, requires collections to be kept in sync. <br><br>
Overall the more inactive objects you have the more useful it is to have a seperate collection that avoids them during game loop
<h3>Summary</h3>
In this report we have researched 3 useful sequencing patterns used in game design: Double buffer, The Game loop and the Update() method. In relation to Nexus, we can already expect not to be coding the game loop ourselves as it is already hardcoded in the unity game engine but we can apply the knowledge learnt about the double buffer pattern and the update method to aid us in creating a well coded game.

