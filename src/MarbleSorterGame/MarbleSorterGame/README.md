#Game Configuration Instructions

The game is configured by a file located in assets/Config/game.json

##Sample game.json

```
{
    "ScreenWidth": 1280,
    "ScreenHeight": 720,
    "MarblePeriod": 9.0,
    "GatePeriod": 0.5,
    "TrapDoorPeriod": 0.5,
    "Driver": "Keyboard",
    "DriverOptions": {
        "SimulationName":  "test",
        "UpdateInterval": 5
    },
    "Preset": {
        "Marbles": [
            { "Color": "Red", "Weight": "Small" },
            { "Color": "Blue", "Weight": "Medium" },
            { "Color": "Green", "Weight": "Large" },
            { "Color": "Random", "Weight": "Random" }
        ],
        "Buckets": [
            { "Capacity": 1, "Weight": "Red", "Color": null },
            { "Capacity": 1, "Weight": null, "Color": "Medium" },
            { "Capacity": 1, "Weight": "Random", "Color": "Random" }
        ]
    }
}
```
##ScreenWidth/ScreenHeight

The width and height of the game screen

##MarblePeriod/GatePeriod/TrapDoorPeriod

How fast each of their respective objects open/close

##Driver

Can be either:
* "Keyboard" for controlling game with keyboard (numbers 1-4 control the gate, conveyor, and each trapdoor in order)
* "Simulation" for controlling game with PLC sim

##Driver Options

SimulationName must be the name of the PLC sim you are using if controlling by simulation

##Marbles

n amount of marbles can be added. Each marble must resemble:

>{ "Color": c, "Weight": w }

Where:
* c can be "Red", "Blue", "Green", or "Random"
* w can be "Small", "Medium", "Large", or "Random"

##Buckets

3 buckets must be configured which much resemble:

>{ "Capacity": x, "Weight": w, "Color": c }

Where:
* x is the amount of marbles required inside the bucket and is a whole number
* w can be "Small", "Medium", "Large", "Random", or null
* c can be "Red", "Blue", "Green", "Random", or null

**null bucket requirements indicate that any marble of that weight/color can go in**