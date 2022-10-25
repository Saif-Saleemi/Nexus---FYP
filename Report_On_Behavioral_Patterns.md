<h4>This Report will cover Behavioral Patterns that are useful in Game Design</h4>
<h5>1. Subclass Sandbox <br> 2. Type Object</h5>
<h3>Subclass Sandbox</h3>
<b>Primary Objective:</b><br>Define behavior in a subclass using a set of operations provided by its base class<br>
<b>Motivation: </b> <br>Define a sandbox method (an abstract protected method) that subclasses must implement. Given that, to implement a behavior: <br>
 <b>1.</b> Create a new class that inherits its base class<br>
 <b>2.</b> Override activate(), the sandbox method<br>
 <b>3.</b> Implement the body of that by calling the protected methods that the new class provides<br>
 - This fixes redundent code and constrains coupling to one place. <br>
<b>How it works:</b><br>A base class defines an abstract sandbox method and several provided operations. Marking them protected makes it clear
               that they are  for use by derived classes. Each derived sandbox subclass implements the sandbox method using provided
               operations.<br>
<b>When to use it:</b><br>
        - You have a base class with a number of derived classes. <br>
        - The base class is able to provide all of the operations that a derived class may need to perform. <br>
        - There is behavioral overlap in the subclasses and you want to make it easier to share code between them. <br>
        - You want to minimize coupling between those derived classes and the rest of the program. <br>
<b>Design Decisions:</b><br>
<b>What operations should be provided?</b><br>
If the provided operations is only used by one or a few subclasses you are adding complexity to the base class which affects everyone, 
but only a couple of classes benefit. <br> <br>
If the implementation of a provided operation only forwards a call to some outside system, 
then it isn't adding much value may just be simpler to call the outside method directly <br> <br>
<b>Should methods be provided directly, or through objects that contain them?</b><br>
The main problem with this pattern is that you can end up with a 
painfully large number of methods crammed into your base class. You can mitigate that by moving some of those methods over to other classes. 
The provided operations in the base class then return just one of those objects.
This reduces the number of methods in the base class and makes the code easier to maintain. It also lowers coupling between the base class and other systems. <br>
<b>How does the base class get the state that it needs?</b><br>
Do Two-state initialization: <br><br>
The constructor will take no parameters and just create an object.
Call a seperate method defined directly on the base class to pass in the rest of the data that it needs
The issue with this is that you need to make sure you always call init(), if u ever forget code will fail.
This can be fixed by encapsulating the entire process into a single function that will create an object and initialize it with data <br>
<b>Summary:</b><br>
This behavioral pattern is a good way to remove redunant code especially in a game design scenario when you are dealing with alot of entities.<br><br> In relation to Nexus this pattern can prove to be useful as we are dealing with alot of moves (Objects) that will derive from a base class (Moves) which can provide the operations the specific move needs.
<h3>Type Object</h3>
<b>Primary Objective:</b><br>Allow the flexible creation of new classes by creating a single class, each instance of which represents a different type of object.<br>
<b>Motivation: </b> <br> We want to create objects that have a similar behaviour type but varying personal data.
The basic OOP approach seems reasonable, making a base class and multiple classes inheriting it but as more and more subclasses
are introduced and as we want to fine tune previous subclasses it gets too time consuming.
We want to change data without having to recompile the whole game every time.<br>
<b>How it works:</b><br>Define a type object and a typed object class. Each type object instance represents a different logical type.
 Each typed object stores a reference to the type object that describes its type.<br>
<b>When to use it:</b><br>
<b>1.</b> You don't know what types you will need up front<br>
<b>2.</b> You want to be able to modify or add new types without having to recompile or change code<br>
<b>Problems</b><br>
Type objects have to be tracked manually, we are responsible for managing not only the classes in memory but also their types. have to make sure
all of the type objects are instantiated and kept in memory aslong as our classes need them. Whenever we create a new object, we need to
ensure that its correctly instantiated with a refernece to a valid type. <br> <br>
Harder to define behavior for each type because we replace an overidden method with a member variable, makes it easy to use type objects
to define type specific data but hard to define type specific behavior. <br>
<b>Design Decisions:</b><br>
<b>Is the type object encapsulated or exposed?</b><br>
If Encapsulated :<br>
The typed object can selectively override behavior from the type object.
Have to write forwarding methods for everything the type object exposes. If our type object class has a large number of methods, the object class will have
its own methods for each of the ones we want to be publicly visible. <br>
If Exposed:<br> Outside code can interact with type objects without having an instance of the typed class.<br>
<b>How are typed objects created?</b><br>
<b>1.</b> Construct the object and pass in its type object. <br>
<b>2.</b> Call a 'constructor' function on the type object <br>
<b>Summary:</b><br> The Type Object pattern is extremely useful in game design as it can accomodate additions fairly easily down the line, 
for example it would prove to be useful if I wanted to add more attacks or fighters to my game down the line. <br><br>
However it comes at the cost of needing more managment. It could have some use in the development of Nexus however it will not be implemented if not needed as it is a fairly complex pattern to implement and maintain in runtime.

