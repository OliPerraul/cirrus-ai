# cirrus-ai
## Action rpg ai framework for Unity

### Cirrus.Broccoli
*  Customized version of [NPBehave](https://github.com/meniku/NPBehave) which is no longer supported

### Director
* The ai director is used to regulate shared access of abilities. This is used in cases where multiple enemies are teaming up against a single target. Instead of attacking all at once an order is predetermined.

### Steering
* Steering behaviors are defined along guidelines proposed by Andrew Fray in his article titled
[Steering Behaviours are Doing it Wrong](https://andrewfray.wordpress.com/2013/02/20/steering-behaviours-are-doing-it-wrong/).
* Context steering enables multiple competing forces to be used simultaneously.

### Content
* Include notable example of behaviour tree node, and steering.
    * `HostileNodeBase` demonstrates strafing around a target and usage of the AI director.
    * `WanderNode` demonstrates natural looking wandering motion using a noise map and incremental adjustments.
