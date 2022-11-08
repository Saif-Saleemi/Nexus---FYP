<h4>This Report will cover State-Driven Agent Design and will answer The following questions:</h4>

1. Why are Finite State Machines Useful In Game Design?
2. How Can Finite State Machines be Implemented in Game Design?

<h4>After These questions have been answered we will delve deeper into the code structure of Finite State Machines in games and apply this knowledge to Nexus.</h4>

<h2>Why are Finite State Machines (FSMs) Useful in Game Design?</h2>

Firstly to answer this question we must know what a Finite State Machine (FSM) is. A FSM is a device which has a finite number of states it can be in at any given time and can operate on input to either make transitions from one state to another or to cause an output or action to take place. Another thing to note is that FSMs can only be in one state at any moment in time. The objective of a FSM is to decompose an objects behavior into easily manageable chunks or 'states'.

With this being said we can now identify how FSMs can be useful in game design. By decomposing object behavior it makes actions easy to code/debug and adds flexibility. For these main reasons FSMs are seen as the general backbone of AI programming.

<h2>How Can Finite State Machines be Implemented in Game Design?</h2>

The basic approach to implementing a FSM is to use multiple If-statements or a switch statement, this proves to be infeasible for more complicated programs as these programs will consist of far too many states. Therefore this method has a lack of flexibility as adding states down the line will prove to be challenging.

A more effective approach would be to use State Transition Tables (STTs) which can be queried by an agent (object) at regular intervals and react based on the stimulus it recieves. Each state can be modelled as a seperate object or function existing external to the agent making it clean and flexible. All rules in the table are tested each time interval and if needed the state object is inputted into the agent to update the agents behaviour.

<h2>Additional Information</h2>

Before any FSM is coded it is imperative that a *BaseGameEntity* class is made. This is a simple class with a private member for storing the ID number which makes every entity/agent unique. All agent classes will derive from this base class.

<h4>There are two State types that will be useful in the creation of Nexus:  </h4>

1. Current State - The state the object is currently in
2. Global State - A state which can be triggered by every other state
3. Previous State -  Can save the previous state while changing state

<h4>General Code Structure</h4>

Each State object will have an Entry method and an Exit method which are called when the object is entering or leaving that state. Create a State base class *State* which all state objects will derive from

Have a *State Machine Class* which keeps the design alot cleaner by encapsulating all the state related data and methods, seperating them from agent classes. This allows an agent to own an instance of a state machine and delegate management of current states, global states and previous states

The State Change Process is as follows : 

 1. Record Previous State
 2. Call Exit Method of Current State
 3. Change to New State
 4. Call Entry Method of New State

One Final thing to note is that we can use multiple FSMs working in parallel, for example one to control a characters movement and one to control the weapon. This Structure is known as Hierarchical State Machine and will prove to be useful during game development

<h4>Use of Messaging</h4>

A great way to indicate a change of state is for an agent to send/recieve a message. This enhances the illusion of intelligence a great deal. How to do this is shown below:

<h5>Telegram Structure</h5>

To deal with Message Dispatch and Management a Class *MessageDispatcher* will have a function *DispatchMessage* which creates a message. 

Before a message can be dispatched, the *MessageDispatcher* must obtain a pointer to the entity specified by the sender. To do this there needs to be a database of instantiated entities provided for the *MessageDispatcher* to refer to. The *EntityManager* contains a map in which pointers to entities are cross-referenced by their ID.


<h5>Message Handling</h5>

Steps needed to incorporate Message Handling:

1. Edit *BaseGameEntity* so any subclass can receive messages, done by declaring a pure virtual function *HandleMessage*
2. Edit *State* so *BaseGameEntity* states can choose to accept and handle messages, done by declaring pure virtual function *OnMessage*
3. Edit *StateMachine* so it contains a *HandleMessage* method, When an entity recieves a telegram it is sent to the entities current state and then the global state if needed

*HandleMessage* and *OnMessage* are bools to indicate that the message has been recieved successfully. 

<h2>How this all ties in with Game Design and Nexus</h2>

Overall we have gained some insight into FSMs and can see how to implement them and the general code structure needed to maintain a FSM. We can now start to model some prototype FSMs for Nexus which will be exapanded and improved later down the line with the use of State-Transition Diagrams.

- FSM #1 *Menu* - Can have different states for menu options (Settings, NewGame,etc.)
- FSM #2 *Game* - Can have different states for the game (Start, Finish)
- FSM #3 *Turn* - Can have different states for the turn (PlayerTurn, OpponentTurn)
- FSM #4 *Fighter* - Can have different states for the fighter (Alive, Dead, InUse, WaitingForMove, MakingMove, FinishedMove, etc.)
- FSM #5 *Attack* - Can have different states for the Attack (Usable, NotUsuable, etc.)


<h5>Reference:</h5> Chapter 2 of Programming Game AI by Example - Mat Buckland


