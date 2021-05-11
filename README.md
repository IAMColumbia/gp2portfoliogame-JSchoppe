# Brute Drive

Genre: Arcade Racer | Smash'em Up <br>
Platform: Android

The heist went according to plan, now it is up to you to get the team back to headquarters by evading the police at all costs. Drive along procedurally generated routes in real world locations while using your wits or simply brute force to escape the long arm of the law.

<details>
<summary>Long Description</summary>
<br>
 
In Brute Drive you drive a fortified escape van through local streets to get to a final destination. The route you are given to drive will reflect a real world
location with different environments calling for different paces of gameplay. Use the weight of your vehicle to smash through cruisers. Be careful not to get cornered though, take too much damage and its game over. Utilize your knowledge of the local roads to choose between forks in the route, but watch out for police barricades that will impede
certain streets. Reach safety by any means necessary.

</details>

## Contributors
<div align="center">
  <div style="display: flex;">
    <img src="https://avatars1.githubusercontent.com/u/38409262?s=460&u=f0e7f13731979e49c5f6240ee56c6556bb88a5fa&v=4" width="50px">
    <img src="https://avatars1.githubusercontent.com/u/1069059?s=460&u=d7795dacb9505f51922adc41b66e728f3fa54cd1&v=4" width="50px">
    <img src="https://avatars.githubusercontent.com/u/36770934?v=4" width="50px">
  </div>
</div>

 - Programming
   - [Jethro Schoppenhorst](https://github.com/JSchoppe)
 - Feedback/Code Review
   - [Jeff Meyers](https://github.com/dsp56001)
   - [Ian Jeannin](https://github.com/IanJeannin)
 - Software Used
   - [Paint.NET](https://github.com/paintdotnet)
   - [Unity](https://github.com/Unity-Technologies)
   - [Visual Studio](https://visualstudio.microsoft.com)
   - [GitHub](https://github.com/github)


## Proposal Details
This section outlines the initial proposal for this project. Each milestone is estimated to be about 3 weeks in duration. With a total project duration of about 2 months.

<details>
<summary>Development Value + Risks</summary>
<br>

Value:
* Demonstrates FSM using state pattern
* Demonstrates strategy pattern with Car/AI controller
* Demonstrates object pool, observer pattern, with AI manager
* Implements Google Maps API
* Reactive generation algorithm, geometry and graph analysis
* Implements physics system using forces + torques

Risks:
* Android input may be a time sink
* Car physics parameter tuning is a lot of design work
* Flocking behaviour will be hard to manage with steering
* Potential unforeseen edge cases given strange map data
* Requires an upfront investment to create mock assets

</details>

<details>
<summary>Speculative UML</summary>
<br>

While this does not cover the entirety of all necessary scripts, I speculate the following UML will represent a majority of the work done in code.
One of the key components will be the vehicle controller. This will not use the wheel colliders provided by Unity, and will instead simulate
wheel and collision forces relative to 2D space. Initially damage will be applied without location context based on collision speed. Upon incurring
some amount of damage the vehicles will destruct into a collection of free falling rigidbodies.

![car controller uml](https://github.com/JSchoppe/Brute-Drive/blob/master/docs/Proposal/Images/UML_CarController.png)

The player's car controller will take input from the device. Multiple controller configurations will need to be setup for testing and gameplay on the android device. Virtual touch controls will need to be created to support the android platform.

![car controller uml](https://github.com/JSchoppe/Brute-Drive/blob/master/docs/Proposal/Images/UML_Input.png)

The AI will derive from the same base controller class as the player. They will implement a finite state machine that reacts to the current environment and other actors. The states shown here are tentative and more states might be required to ensure proper flocking behaviour.

![car controller uml](https://github.com/JSchoppe/Brute-Drive/blob/master/docs/Proposal/Images/UML_AIController.png)

One of the largest challenges will be meaningfully processing the google maps data into a stage. The data provides movement graphs for the road segments that can be used to find potential paths. Parameters will have to be added to ensure the generation algorithm can account for varied environments and outliers. The generator should be able to reliably partition off routes on the map such that the player cannot escape the level bounds. The AI manager and stage sequence scripts show the basic way the gameplay will be sequenced.

![car controller uml](https://github.com/JSchoppe/Brute-Drive/blob/master/docs/Proposal/Images/UML_Sequencing.png)

</details>

<details>
<summary>Milestones</summary>
<br>

### Proof of Concept
This milestone will feature modeling of the driver controller for the player and AI, such that vehicles can interact with each other and the world in a very basic way. An algorithm will be developed to randomly choose a non-intersecting route through the recieved graph from google maps. Basic modifications will be made to the map to add colliders around the chosen route.
### Vertical Slice
In this milestone the gameplay loop will be more fleshed out with basic vehicle damage implemented. Wrecks will be given a lightened weight so that they can be pushed out of the way by the player. Player will be able to complete a run from start to finish (on a prechosen map), and will also be able to fail a run if their vehicle recieves too much damage.
### First Build
Player will be able to choose the coordinates, or use current coordinates, to generate a stage. Generation should be tested against numerous different street layout archetypes to locate generation and behavioural bugs. AI will be fine tuned to be more reactive and predictive of the player's actions.
### Cleaned Up and Documented
Fix lingering bugs and improve the gameplay polish. Clean up technical debt as time permits.

</details>
