<h4>This Report will cover Decoupling Patterns that are useful in Game Design</h4>
<h5>1. Component <br> 2. Event Queue <br></h5>
<h3>Component</h3>
<b>Primary Objective:</b><br>Allow a single entity to span multiple domains without coupling domains to each other<br>
<b>Motivation: </b> <br>
Different domains should be kept isolated from each other, we want to avoid
AI, physics, rendering, sound and other domains to know about each other
otherwise any programmer wanting to make a change to the code would need to know
something about all intertwined domains to make sure they don't break anything. <br>
We can do this by abstracting domains into separate classes, for example moving all of the code
for handling user input into a separate 'inputcomponent' class. Then the target class
will own an instance of this Component, this can be repeated for all domains the target class touches. <br>
Our component classes are now decoupled, and if any domains need to interact with each other
they can be handled on a case by case basis. This also turns our component classes into
reusable packages which can be used for other classes. This method of reusing code is now preferred
compared to inheritance which is often too cumbersome for simple code reuse. <br>
<b>How it works:</b><br>A single entity spans multiple domains. To keep the domains isolated, the code for each is placed
 in its own component class. The entity is reduced to a simple container of components. <br>
<b>When to use it:</b><br>
1. You have a class that touches multiple domains which you want to keep decoupled from each other <br>
2. A class is getting massive and hard to work with <br>
3. You want to be able to define a variety of objects that share different capabilites,
 but using inheritance doesn't let you pick the parts you want to reuse precisely enough <br>
<b>Design Decisions:</b><br>
<b>How does the object get its components?</b><br><br>
If the object creates its own components:<br>
 1. Ensures the object always has the components it needs.<br>
 2. Harder to reconfigure the objects<br><br>
 If outside code provides components:<br>
 1. Object becomes more flexible<br>
 2. Object can be decoupled from the concrete component types.<br>
<b>How do components communicate with each other?</b><br><br>
 By modifying the container object's state:<br>
 1. Keeps the components decoupled<br>
 2. It requires any information that components need to share to get pushed up into the
 container object<br>
 3. Makes communication implicit and dependant on the order that components are processed.
 Need to be careful with the order components are laid out in the update method<br><br>
 By referring directly to each other:<br>
 1.  simple and fast, communication is a direct method call from one object to another<br>
 2. The two components become tightly-coupled, although its not as bad as having all code in a single classes.<br><br>
 By sending messages:<br>
 1. Most complex option, can create a telegram system into our container object that lets components broadcast
 information t o each other. Can define a base component interface that all components will implement which has a
 single receive method that component classes implement in order to listen to a incoming message. Then
 can add a send method in our containerobject. now if a component has access to its container, it can send
 messages to the container, which will rebroadcast the message to all contained components.<br>
 2. Ensures the components are decoupled from each other, the only coupling being the message value.<br>
 3. Container object is simple, all it does is blindly pass messages along.<br>
<b>Summary</b><br>
The unity framework core GameObject class is designed entirely around the component pattern. This pattern is a good option to consider during production of Nexus, 
it can be used you decouple userinput code from a main class, for example the userinput with their fighter.<br><br>
<h3>Event Queue</h3>
<b>Primary Objective:</b><br>Decouple when a message or event is sent from when it is processed.<br>
<b>Motivation: </b> <br>
Game code is likely complex enough as it is. The last thing we want to do is stuff a bunch
of checks for triggering something like in-game tutorials in there. instead you could have an event queue where
any game system can send to it and receive events from it so the actual game and tutorial is decoupled from each other
and a certain event from the event queue triggers the tutorial.<br>
<b>How it works:</b><br>
A queue stores a series of notification or requests in FIFO order. sending a notification
enqueues the request and returns. The request processor then processes items from the queue at a
later time. Requests can be handled directly or routed to interested parties. This decouples the
sender from the receiver both statically and in time.<br>
<b>When to use it:</b><br>
Is good to use when you want to decouple something in time (during runtime)<br>
<b>Code Structure</b><br>
 Ring buffer queue implementation (circular queue) with the head of the Queue is where Requests
are read from. head of queue is the oldest pending request.
Tail is the slot in the array where the next enqueued request will be written<br>
<b>Design Decisions:</b><br>
<b>What goes in the queue?</b><br><br>
If you queue events:<br>
An event is something that has already happened<br>
1. Likely to allow multiple listeners<br>
2. Scope of the queue tends to be broader (more globally visible)<br><br>
If you queue messages:<br>
message describes an action that we want to happen in the future<br>
1. More likely to have a single listener<br><br>
<b>Who can read from the queue?</b><br><br>
A Single-cast Queue:<br>
1. Queue becomes an implementation details of the reader<br>
2. Queue is more encapsulated<br>
3. Don't have to worry about contention between listeners<br><br>
A Broadcast Queue:<br>
1. Events can be disregarded (if there are 0 listeners)<br>
2. May need to filter events to reduce amount of event handlers to invoke<br><br>
A Work Queue:<br>
1. You have to schedule, since an item only goes to one listener queue needs logic
to figure out the best one to choose.<br><br>
<b>Who can write to the Queue?</b><br><br>
One Writer:<br>
1. Implicitly know where the event is coming from<br>
2. Usually allow multiple readers otherwise it will just feel like a basic queue<br><br>
Multiple Writers:<br>
1. Have to be more careful of cycles because a feedback loop can occur<br>
2. Likely want to reference to the sender in the event itself, since there are multiple writers,
reader needs to know who sent the event.<br>
<b>Summary:</b><br>
Event Queue pattern is a good way to decouple when we want decoupling to happen. This can be useful in the creation of Nexus, for example for adding a tutorial where
the player is given a brief introduction on how to play the game during the game. This is common in alot of AAA games where user input triggers 2 events at once; the tutorial and the base game event.