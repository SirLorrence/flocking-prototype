# Flocking Prototype

With this project I took my time to learn how to do an simple [flocking simulation](https://www.red3d.com/cwr/boids/). My knowledge of this is still elementary, but I'm planning on implementing this as an game mechanic in one of my up coming projects. For now...look at the little fishies.
<!--How the hell html works in markdown?!?! I dont care if just works-->
<p align = "center">
 <a href="https://github.com/SirLorrence/flocking-prototype/tree/main/Assets/Scripts">Source Code</a>
</p>

<p align="center">
  <img src="https://github.com/SirLorrence/flocking-prototype/blob/main/flockimg-readme.gif?raw=true">
</p>

## The Rules

### 1. Move towards the average position of the flock - Cohesion
This is done by each entity (Craig Reynolds calls them boids) calculating the other entities position and creating an average center position
Getting the other's entities positions - ``  centerVector += neighbor.transform.position;``\
After dividing the sum of the positions by the size of the flock - ``centerVector /= flockSize;`` 

<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/cohesion.gif">
</p>

### 2. Align with forward heading of the flock - Alignment
This is done by adding all the Forward Vectors (facing directions) and dividing them by the size of the flock. Since the entities are just going forward I used the same ``centerVector`` variable to calculate the foward direction:\
`` var desiredFacingDirection = (centerVector + avoidanceVector) - transform.position;``
<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/alignment.gif">
</p>

### 3. Avoid crowding together - Separation
When another entity is too close to one another, you'll need to calculate a new forward direction by using the flock forward heading (vector) + 
the direction of the where to avoid the other entity + the flocks position.\
Getting the avoidance direction vector: \
``if (closetNeighbor < _manager.NeighborDist) avoidanceVector += transform.position - neighbor.transform.position;``


<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/separation.gif">
</p>

