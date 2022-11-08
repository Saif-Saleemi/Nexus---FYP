<h4>This Report will cover Goal-Driven Agent Behavior and will answer the following questions: </h4>
1. How does it work? <br>
2. How can it be implemented?
<h4>We will then look at the advantantages of implementing Goal-Driven Agent Behvaior</h4>
<h2>How does it work?</h2>
In this pattern, an agent's behavior is defined as a collection of hierarchical goals which are either atomic or composite.  <br>
Atomic goals are goals that define a single task, behavior or action whereas composite goals are comprised of several subgoals which in turn may be atomic or composite creating a nested hierarchy. <br> <br>
Each update the agent examines the game state and selects, from a set of predefined high-level goals, the one it belives will most likely enable it to satisfy its strongest desire.
It will then attempt to follow this goal by decomposing it into any constituent subgoals, satisfying each one in turn doing this until the goal is either satisfied or has failed, or until
the game state necessitates a change of strategy. <br> <br>

<h2>How can it be implemented?</h2>
Firstly we must make use of the composite design pattern which works by defining an abstract base class that represents both composite and atomic objects which enables agents to manipulate goals identically, no matter how simple or complex they are.
<br><br>
Goal objects are similar to the state class, having a method for handling messages and the Activate, Process and Terminate methods are similar to the Enter, Execute, and Exit methods of state. 
<br><br>The Activate method initializes logic and repersents the planning phase of the goal
 which can be called any number of times to replan if the situation demands it. <br><br>
The Process method is executed each update step, returning an enumerated value indicating the status of the goal which can be one of four values: <br>
Inactive - goal is waiting to be activated<br>
Active - the goal has been activated and will be processed each update step<br>
Completed - The goal has completed and will be removed on next update <br>
Failed - Goal has failed and will either replan or be removed on next update.<br><br>
The Terminate method undertakes necessary tidying up before a goal is exited and is called just before a goal is destroyed. <br><br>
To tidy up implementation, alot of the logic can be abstracted out into a Goal_Composite class, which all concrete composite goals can inherit from.
All composite goals will call a function 'ProcessSubgoals' each update step to process their subgoals, ensuring that completed and failed goals are removed from the list beofre processing
the next subgoal in line and returning its status. <br><br>

<h3>Goal Arbitration:</h3>
Using the highest-level goal Goal_Think, which each agent owns a persistent instance of, an agent can arbitrate between available strategies (goals), choosing the most appropriate to be pursued. <br><br>
Every think update each of these strategies is evaluated and given a score representing the desirability of pursuing it. The strategy with the highest score is assigned to be the one the agent will attempt to satisfy. <br><br>
To calculate a score, each Goal_Think aggregates several Goal_Evaluator instances, one for each strategy. These objects have methods for calculating the desirability of the strategy they represent
and for adding that goal to Goal_Think's subgoal list. Each CalculateDesirability method is a hand-crafted algorithm that returns a value indicating desirability of a bot pursuing that strategy. Algorithms can be hard to create so making helper functions that map feature specific information from the game to a numerical value in the range 0 to 1 which are then utilized in the formulation
of the desirability algorithms. <br><br>
Create your own formula for specific strategies, for example, a goal GetHealth can have a formula of desirability -> (D = k * 1 - Health / DistToHealth) where k is a constant used to tweak the result.
This relationship makes sense because the farther you have to go to retrieve an item the less you desire it, whereas the lower your health level, the greater your desire. 
Look for similar relationships in strategies to find a good formula to calculate desirability.
Algorithms can be improved in this case, as right now it is a linear function which isn't realistic. To create an non-linear function divide by the square or even cube of DistToHealth.<br><br>
<h4>Goal_Think</h4>
Goal_Think iterates through each evaluator each think update and select the highest to be the strategy a bot will pursue. 
Goal_Think is the arbiter of strategy goals. You can even switch in and out entire sets of strategy goals to provide an agent with a whole new suite of behaviors to select from. This is used 
to good effect in AAA games like Far Cry. <br><br>

<h2>Benefits of using Goal-based Arbitration Design</h2>
Goal Arbitration is an algorithmic process defined by a handful of numbers. It is driven by data instead of logic like in FSM. This is very beneficial as all you have to do to tweak behavior is change numbers. <br><br>
Another advantage of hierarchical goal-based arbitration design is that extra features are provided with little additional effort from the programmer.<br><br>
<h3>Personalities:</h3>
We can create agents with different personality traits by multiplying desirability scores with a constant that biases it in the required direction (Aggressive agent that prioritizes attacking over defending). <br><br>
To facilitate this, the Goal_Evaluator base class contains a member variable Char_Bias, which is assigned a value by the client in the constructor, this is then used in the CalculateDesirability method to adjust the score. 
To make personalities persist between games we can create a seperate script file for each bot containing the biases. <br><br>
<h3>State Memory:</h3>
The stack like nature of composite goals automatically endows agents with a memory, enabling them to temporarily change behavior by pushing a new goal (or goals) onto the front of the current goal's subgoal list. <br><br>
As soon as the new goal is satisfied it will be popped from the list and the agent will resume whatever it was doing previously.
Even if the agent strays off path the built-in logic for detecting failure and replanning allows the design to move backward up through the hierarchy until a parent is found that is capable of replanning the goal and putting the agent back on track. <br><br>
<h3>Command Queueing:</h3>
Players can order agents to do multiple things one after the other, for example they can order an NPC to Patrol between two points. This is heavily used in RTS games where players can think ahead and send NPCs to do multiple goals. <br><br>

<h2>Summary:</h2>
Goal-Driven agent design  is a flexible and powerful architecture allowing agent behavior to be modeled as a set of high-level strategies, each of which is comprised of a nested hierarchy of composite and atomic goals. <br><br>
All agents will have a goal Goal_Think which is their highest level goal that will help them arbitrate between other strategies/goals using desirability scores and bias. <br><br>
Although it shares many similarities, this type of architecture is far more sophisticated than a state-based design providing alot of advantages at the cost of being more complex to code and implement compared to its State-design counterpart. <br><br>
<h3>How can it be applied to Nexus?</h3>
This design can have many benefits in the production of Nexus, The AI the player will play against can have a series of goals it wants to satisfy such as keep fighter alive or kill enemy fighter. It can also possess different personality traits by adding bias to desirability scores. For example it could be an aggressive AI that focuses on using damage based attacks instead of defense providing more diversity for the player to fight against. <br><br>
We can also change the difficulty of the AI by creating entire sets of strategy goals to provide the agent with completely new behavior. For example, an advanced AI may have goals and subgoals that decide to use an attack move that is strong against the enemy fighter type and will make more intuitive decisions such as using a heal item when low on health or defense moves. A novice AI will just have atomic goals such as kill fighter which can be done by using high damage abilities disregarding whether they are effective or not against that specific fighter.<br><br>

<h5>Reference:</h5> Chapter 9 of Programming Game AI by Example - Mat Buckland