# uScriptExtended [![](https://img.shields.io/github/downloads/MolyiEZ/uScriptExtended/total.svg)](https://github.com/MolyiEZ/uScriptExtended/releases)

## Documentation

```
Event: onAnimalDamaged(animal, killer, *cancel, *damage, limb, ragdoll)

Event: onBarricadeBuilded(barricade)
Event: onBarricadeDamaged(player, barricade, damage, cause, *cancel)
Event: onBarricadeSalvaged(player, barricade, *cancel)

Event: onDayNightUpdated(isDayTime)
Event: onMoonUpdated(isFullMoon)
Event: onRainUpdated(rain)
Event: onSnowUpdated(snow)

Event: onFarmHarvest(player, barricade, *cancel)

Event: onGunBarrelChanged(player, item, oldItem, newItem, *cancel)
Event: onGunBulletHit(player, item, hit, *cancel)
Event: onGunGripChanged(player, item, oldItem, newItem, *cancel)
Event: onGunMagazineChanged(player, item, oldItem, newItem, *cancel)
Event: onGunSightChanged(player, item, oldItem, newItem, *cancel)
Event: onGunShooted(player, item)
Event: onGunTacticalChanged(player, item, oldItem, newItem, *cancel)

Event: onPlayerBleedingUpdated(player, isBleeding)
Event: onPlayerBrokenUpdated(player, isBroken)
Event: onPlayerDamagedCustom(player, killer, *cancel, *damage, cause, limb, ragdoll)
Event: onPlayerFoodUpdated(player, newFood)
Event: onPlayerHealthUpdated(player, newHealth)
Event: onPlayerJoinRequested(playerSteam)
Event: onPlayerOxygenUpdated(player, newOxygen)
Event: onPlayerPositionUpdated(player)
Event: onPlayerRadiationUpdated(player, isRadiated)
Event: onPlayerRelayVoice(player, isWalkie, *cancel)
Event: onPlayerSafetyUpdated(player, isSafe)
Event: onPlayerStaminaUpdated(player, newStamina)
Event: onPlayerSwapSeats(player, vehicle, fromseat, toseat, *cancel)
Event: onPlayerTakingItem(player, itemId, *cancel)
Event: onPlayerTemperatureUpdated(player, temperature)
Event: onPlayerUnequipped(player, item, *cancel)
Event: onPlayerVirusUpdated(player, newVirus)
Event: onPlayerWaterUpdated(player, newWater)

Event: onPluginKeyTick(player, simulation, key, state)

Event: onResourceDamaged(player, damage, cause, *cancel)

Event: onStructureBuilded(structure)
Event: onStructureDamaged(player, structure, damage, cause, *cancel)
Event: onStructureSalvaged(player, structure, *cancel)

Event: onSiphonVehicleRequest(player, vehicle, amount, *cancel)

Event: onVehicleCarjack(player, vehicle, force, torque, *cancel)
Event: onVehicleLockpick(player, vehicle, *cancel)
Event: onVehicleLockRequest(vehicle, *cancel)
Event: onVehicleRepair(player, vehicle, totalHealing, *cancel)
Event: onVehicleTireDamaged(player, vehicle, cause, *cancel)

Event: onZombieDamaged(zombie, killer, *cancel, *damage, limb, ragdoll)


animal [Class]:
    +alertPlayer(player)
    +dropLoot()
    +kill(string ragdoll)
    +moveTo(vector3 position)
    +runFrom(vector3 position)
    +startle()
    +health		   [get]	    : float
    +id 		   [get]	    : ushort
    +isFleeing		   [get]	    : boolean
    +isHunting		   [get]	    : boolean
    +instanceId		   [get]	    : uInt32
    +position		   [get]	    : vector3
    +targetPlayer	   [get]	    : player
    +targetPoint	   [get]	    : vector3

barricade [Class]:
    +anglex                [get]	    : single
    +angley                [get]	    : single
    +anglez                [get]	    : single
    +farm                  [get]	    : farm
    +generator             [get]	    : generator
    +fire                  [get]	    : fire
    +oven                  [get]	    : oven

consumeable [Class]:
    +food                  [get]	    : float
    +water                 [get]	    : float

effectspawn [Class]:
    +effect(string guid, vector3 position)
    +effectPlayer(string guid, vector3 position, player)
    +effectClear(string guid)

farm [Class]:
    +canFertilize	   [get]	    : boolean
    +grow 		   [get]	    : ushort
    +growth		   [get]	    : ushort
    +harvestExperience	   [get/set]	    : uInt32
    +isFullyGrown	   [get]	    : boolean

fire [Class]:
    +lit                   [get/set]       : boolean
    +wired                 [get]           : boolean

generator [Class]:
    +powered               [get/set]       : boolean                   
    +wirerange             [get]           : generator
    +fuel                  [get]           : uInt16

gun [Class]:
    +ammoMax               [get]           : float
    +ammoMin               [get]           : float
    +damage                [get]           : float
    +fireDelay             [get]           : float
    +fireRate              [get]           : float
    +movementSpeed         [get]           : float
    +range                 [get]           : float
    +recoilAim             [get]           : float
    +recoilCrouch          [get]           : float
    +recoilMultiplier      [get]           : float
    +recoilProne           [get]           : float
    +recoilSprint          [get]           : float
    +reloadTime            [get]           : float
   
hit [Class]:
    +animal 		   [get]           : animal
    +barricade 		   [get]           : barricade
    +direction 		   [get]           : vector3
    +limb 		   [get]           : string
    +normal 		   [get]           : vector3
    +point 		   [get]           : vector3
    +player 		   [get]           : player
    +type 		   [get]           : string
    +vehicle 		   [get]           : vehicle
    +zombie 		   [get]           : zombie

item [Class]:
    +consumeable           [get]           : consumeable
    +description           [get]           : string
    +durability            [get/set]       : uInt16
    +gun                   [get]           : gun

oven [Class]:
    +lit                   [get/set]       : boolean
    +wired                 [get]           : boolean

player [Class]:
    +arrestCustom(ushort id, ushort strenght)
    +isGrounded		   [get]	   : boolean
    +isSafe		   [get]	   : boolean
    +isRadiated		   [get]	   : boolean
    +key(key)              [get]           : boolean               
    +salvageTime           [get/set]       : uInt16
    +stamina               [get/set]       : uInt16
    +temperature           [get]           : string

playerClothing [Class]:
    +removeBackpack()
    +removeGlasses()
    +removeHat()
    +removeMask()
    +removePants()
    +removeShirt()
    +removeVest()
    
playerLook [Class]:
    +getAnimal() : animal
    +getZombie() : zombie

playerSteam [Class]:
    +id                    [get]           : string            
    +name                  [get/set]       : string
    
serverExtended [Class]:
    +clearAllAnimals()
    +getAnimal(ushort instanceId) : animal
    +getAnimalsInRadius(vector3 position, single radius) : object
    +getPlayersInRadius(vector3 position, single radius) : object
    +getZombiesInRadius(vector3 position, single radius) : object

vehicle [Class]:
    +enterVehicle(player)
    
zombie [Class]:
    +acid(vector3 direction, vector3 origin)
    +boulder(vector3 direction, vector3 origin)
    +breath()
    +charge()
    +dropLoot()
    +throw()
    +spark(vector3 target)
    +spit()
    +stomp()
    +health 		   [get]	   : float
    +isBoss		   [get]	   : boolean
    +isCutesy		   [get]	   : boolean
    +isHunting		   [get/set]       : boolean
    +isHyper		   [get]	   : boolean
    +isMega		   [get]	   : boolean
    +isRadioactive	   [get]	   : boolean
    +id 	 	   [get]	   : ushort
    +maxHealth		   [get]	   : float
```
