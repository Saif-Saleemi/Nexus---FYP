<h4>This Report will cover Optimisation Patterns that are useful in Game Design</h4>
<h5>1. Dirty Flag <br>2. Object Pool<br> 3. Spatial Partition <br></h5>
<h3>Dirty Flag</h3>
<b>Primary Objective:</b><br>
Avoid unnecessary work by deferring it until the result is needed <br>
<b>Motivation: </b> <br>
For example, we want to collapse modifications to multiple local transforms along an objects parent chain
into a single recalculation on the object. Also avoid recalculation on objects that didn't
move. And if an object gets removed before its rendered, doesn't have to calculate its world transform
at all. Essentially we can speed up computation by not performing operations on objects we know will not change. <br>
<b>How it works:</b><br>
A set of primary data changes over time. A set of derived data is determined from this using some
expensive process. A “dirty” flag tracks when the derived data is out of sync with the primary data.
It is set when the primary data changes. If the flag is set when the derived data is needed, then it is
reprocessed and the flag is cleared. Otherwise, the previous cached derived data is used.<br>
<b>When to use it:</b><br>
This pattern solves a pretty specific problem.
Should only reach for it when you have a performance problem big enough to
justify the added code complexity. <br>
<b>Design Decisions:</b><br>
<b>When is the dirty flag cleaned?</b><br><br>
When the result is needed:<br>
1. It avoids doing calculation entirely if the result is never used.<br>
2. If the calculation is time-consuming, it can cause a noticeable pause.<br><br>
At well-defined checkpoints:<br>
1. Doing the work doesn't impact user experience, can give something to distract the player
while the game is busy processing.<br>
2. Lose control over when the work happens.<br><br>
In the Background:<br>
1. Can tune how often the work if performed<br>
2. Can do more redundant framework<br>
3. Need to support for doing work asynchronously.<br><br>
<b>How fine-grained is your dirty tracking?</b><br><br>
If its more fine-grained:<br>
1. Only process data that actually changed<br><br>
If its more coarse-grained:<br>
1. End up processing unchanged data<br>
2. Less memory used for storing dirty flags<br>
3. Less time is spent on fixed overhead.<br><br>
<b>Summary:</b><br>
Although fairly complex to implement, this pattern can provide a very good way to speed up the efficiency of the game. This type of pattern works fairly well with FPS games where physics plays a part. <br><br>
<h3>Object Pool</h3>
<b>Primary Objective:</b><br>
Improve performance and memory use by reusing objects from a fixed pool instead of allocating
and freeing them individually. <br>
<b>Motivation: </b> <br>
Programming games is similar to programming embedded systems, memory is scarce.
We want to make sure that creating and destroying objects doesn't cause memory fragmentation
An object pool can solve this problem. To the memory manager, we’re just allocating one
big hunk of memory up front and not freeing it while the game is playing. To the users of the pool, we
can freely allocate and deallocate objects to our heart’s content.<br>
<b>How it works:</b><br>
Define a pool class that maintains a collection of reusable objects. Each object supports an “in use”
query to tell if it is currently “alive”. When the pool is initialized, it creates the entire collection of
objects up front (usually in a single contiguous allocation) and initializes them all to the “not in use”
state.<br>
When you want a new object, ask the pool for one. It finds an available object, initializes it to “in
use”, and returns it. When the object is no longer needed, it is set back to the “not in use” state. This
way, objects can be freely created and destroyed without needing to allocate memory or other
resources.<br>
<b>When to use it:</b><br>
1.You need to frequently create and destroy objects<br>
2.Objects are similar in size<br>
3.Allocating objects on the heap is slow or could lead to memory fragmentation<br>
4.Each object encapsulates a resource such as a database or network connection that is expensive
to acquire and could be reused.<br>
<b>Design Decisions:</b><br>
<b>Are objects coupled to the pool?</b><br><br>
If objects are coupled to the pool:<br>
1.Implemenation is simpler<br>
2.You can ensure that the objects can only be created by the pool.<br><br>
If objects are not coupled to the pool:<br>
1.Objects of any type can be pooled. can create a generic reusable pool class<br>
2.The “in use” state must be tracked outside the objects<br><br>
<b>What is responsible for initializing the reused objects?</b><br><br>
If the pool reinitializes internally:<br>
1.The pool can completely encapsulate its objects.<br>
2.The pool is tied to how objects are initialized.<br><br>
If outside code initializes the object:<br>
1.The pool’s interface can be simpler.<br>
2.Outside code may need to handle the failure to create a new object.<br><br>
<b>Summary:</b><br>
This pattern can improve performance of a game greatly and can work fairly well with Nexus. For example when we have a large amount of objects to deal we can look to implement this pattern if it makes things more efficient. This will likely be the case as we will have alot of objects in the game including the environment, the enemy fighters and the player's fighters.<br><br>
<h3>Spatial Partition</h3>
<b>Primary Objective:</b><br>
Efficiently locate objects by storing them in a data structure organized by their positions.<br>
<b>Motivation: </b> <br>
If we store our objects in a data structure organized by their locations, we can find
them much more quickly. This pattern is about applying that idea to spaces that have more than one
dimension.<br>
Spatial partitions exist to knock an O(n) or O(n²) operation down to something more manageable. The
more objects you have, the more valuable that becomes. Conversely, if your n is small enough, it may
not be worth the bother.<br>
<b>How it works:</b><br>
For a set of objects, each has a position in space. Store them in a spatial data structure that
organizes the objects by their positions. This data structure lets you efficiently query for objects at
or near a location. When an object’s position changes, update the spatial data structure so that it
can continue to find the object.<br>
<b>When to use it:</b><br>
You have a set of objects that each have some kind of
position and you are doing enough queries to find objects by location that your performance is
suffering.<br>
<b>Design Decisions:</b><br>
<b>Is the partition hierarchical or flat?</b><br><br>
If it’s a flat partition:<br>
1.It's simpler (flat data structure)<br>
2.Memory usage is constant<br>
3.Can be faster to update when objects change their positions<br><br>
If it’s hierarchical:<br>
1.It handles empty space more efficiently.<br>
2.It handles densely populated areas more efficiently<br><br>
<b>Summary:</b><br>
This pattern trades memory for speed. If you’re shorter on memory than you are on clock cycles,
that may be a losing proposition. We can make use of this pattern if we ever get into a situation where we need to keep track of the location of objects instead of using a naive approach like a detection system.
