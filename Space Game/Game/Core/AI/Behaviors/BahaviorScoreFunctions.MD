# Behavior Score Functions

## Patrol Behavior: 
    f = hasNoAimingShip * shielHhullScore
## Intercept Behavior:
    f = shielHhullScore * (distanceToAimingShip * 50 / environment.ShortRangeScanDistance);
## Battle Behavior:
    f = inAttacDistance * shielHhullScore + (1 - targetShielHullScore)
## Flee Behavior:
    f = (targetShielHullScore * 1.5) + (shielHhullScore * 1.5)
## Resource Gathering Behavior:
    f = (1 - inventoryLevel) * (1 - distanceToRecourceScore)
