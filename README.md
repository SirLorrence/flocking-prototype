# Flocking Prototype

With this project I took my time to learn how to do an simple flocking simulation. The rules are actually pretty simple: 
<p align="center">
  <img src="flockimg-readme.gif">
</p>

### 1. Move towards the average position of the flock - Cohesion
This is done by each entity (Craig Reynolds calls them boids) calculating the others entities position can creating an average center position
Getting the other's entities positions - ``  centerVector += neighbor.transform.position;``\
After dividing the sum of the positions by the size of the flock - ``centerVector /= flockSize;`` 

<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/cohesion.gif">
</p>

### 2. Align with forward heading of the flock - Alignment
This is done by adding all the Forward Vectors (facing directions) and dividing it but the size of the flock 
<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/alignment.gif">
</p>

### 3. Avoid crowding together - Separation
When another entity is to close to one another, you'll need to calculate an new forward direction by using the flock forward heading (vector) + 
the direction of the where to avoid the other entity + the flocks position.\
So something like this - `` var desiredFacingDirection = (centerVector + avoidanceVector) - transform.position;`` \
Getting the avoidance direction: \
``if (closetNeighbor < _manager.NeighborDist) avoidanceVector += transform.position - neighbor.transform.position;``


<p align="center">
  <img src="https://www.red3d.com/cwr/boids/images/separation.gif">
</p>

