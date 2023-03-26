# uScriptExtended [![](https://img.shields.io/github/downloads/MolyiEZ/uScriptExtended/total.svg)](https://github.com/MolyiEZ/uScriptExtended/releases)

## Documentation

```
Event: onAnimalAttackingPlayer(animal, player, *cancel)
Event: onAnimalAttackingPoint(animal, point, *cancel)
Event: onAnimalDamaged(animal, killer, *cancel, *damage, limb, ragdoll)
Event: onAnimalFleeing(animal, direction, *cancel)
Event: onAnimalSpawned(animal)
Event: onAnimalRevived(animal)

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
Event: onPlayerClothingEquipping(player, item, slot, *cancel)
Event: onPlayerClothingUnequipping(player, item, slot, *cancel)
Event: onPlayerDamagedCustom(player, killer, *cancel, *damage, cause, limb, ragdoll)
Event: onPlayerFlagUpdated(player, flagId, flagValue)
Event: onPlayerFoodUpdated(player, newFood)
Event: onPlayerGroupUpdated(player, oldGroupId, oldGroupRank, newGroupId, newGroupRank)
Event: onPlayerHealthUpdated(player, newHealth)
Event: onPlayerJoinRequested(playerSteam, rejectionReason)
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

Event: onResourceDamaged(player, damage, cause, *cancel)

Event: onStructureBuilded(structure)
Event: onStructureDamaged(player, structure, damage, cause, *cancel)
Event: onStructureSalvaged(player, structure, *cancel)

Event: onSiphonVehicleRequest(player, vehicle, amount, *cancel)

Event: onVehicleCarjack(player, vehicle, force, torque, *cancel)
Event: onVehicleHeadLightsUpdated(player, vehicle, *cancel)
Event: onVehicleHorn(player, vehicle, *cancel)
Event: onVehicleLockpick(player, vehicle, *cancel)
Event: onVehicleLockRequest(vehicle, *cancel)
Event: onVehicleRepair(player, vehicle, totalHealing, *cancel)
Event: onVehicleTireDamaged(player, vehicle, cause, *cancel)

Event: onZombieDamaged(zombie, killer, *cancel, *damage, limb, ragdoll)


animal [Class]:
    +alertPlayer(player)
    +damage(uInt16 amount, [bool dropLoot], [string ragdoll])
    +dropLoot()
    +kill([string ragdoll])
    +moveTo(vector3 position)
    +runFrom(vector3 position)
    +startle()
    +look(vector3 position)[get] 	: animalLook
    +backward              [get] 	: vector3
    +down                  [get] 	: vector3
    +forward               [get] 	: vector3
    +health		   [get/set]	: uInt16
    +id 		   [get]	: uInt16
    +isAttacking           [get/set]	: boolean
    +isFleeing		   [get/set]	: boolean
    +isHunting		   [get/set]	: boolean
    +isMoving              [get/set]	: boolean
    +isRunning         	   [get/set]	: boolean
    +isWandering       	   [get/set]	: boolean
    +instanceId		   [get]	: uInt32
    +left                  [get] 	: vector3
    +position		   [get/set]	: vector3
    +right                 [get] 	: vector3
    +targetPlayer	   [get]	: player
    +targetPoint	   [get]	: vector3
    +up                    [get] 	: vector3
    
animalLook [Class]:
    +getBarricade()               	: barricade
    +getPlayer()                  	: player
    +getPoint()                   	: vector3
    +getStructure()              	: structure
    +getVehicle()                 	: vehicle

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
    +canFertilize	       [get]	    : boolean
    +grow 		           [get]	    : uInt16
    +growth		           [get]	    : uInt16
    +harvestExperience	   [get/set]	    : uInt32
    +isFullyGrown	       [get]	    : boolean

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
    +rarity	           [get]	   : string
    +quality		   [get]	   : ushort
    
map [Class]:
    +getKey(object value)  [get]	   : object

oven [Class]:
    +lit                   [get/set]       : boolean
    +wired                 [get]           : boolean

player [Class]:
    +arrestCustom(uInt16 id, uInt16 strenght)
    +hasEarpiece	   [get]	   : boolean
    +isGrounded		   [get]	   : boolean
    +isSafe		   [get]	   : boolean
    +isRadiated		   [get]	   : boolean
    +oxygen	           [get/set]	   : uInt16
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
    
playerInventoy [Class]:
    +addItemAuto(ushort itemId, [byte amount], [bool autoEquipWeapon], [bool autoEquipUseable], [bool autoEquipClothing])
    
playerLook [Class]:
    +getAnimal() : animal
    +getZombie() : zombie

playerSteam [Class]:
    +id                    [get]           : string            
    +name                  [get/set]       : string
    
serverExtended [Class]:
    +clearAllAnimals()
    +getAnimal(uInt16 instanceId) : animal
    +getAnimalsInRadius(vector3 position, single radius) : object
    +getPlayersInRadius(vector3 position, single radius) : object
    +getZombiesInRadius(vector3 position, single radius) : object

String [Base Type]:
    +isMatch(string value) [get]	   : boolean
    
structure [Class]:
    +anglex   		   [get]	   : single
    +angley   		   [get] 	   : single
    +anglez   		   [get]	   : single
    +isWired  		   [get]	   : boolean

vehicle [Class]:
    +enterVehicle(player)
    +look(vector3 position)[get]               : vehicleLook
    +backward              [get] 	       : vector3
    +down                  [get] 	       : vector3
    +forward               [get] 	       : vector3
    +isBatteryFull	   [get]	       : boolean
    +isBatteryReplaceable  [get]	       : boolean
    +isDrowned	           [get]	       : boolean
    +isEmpty	           [get]	       : boolean
    +isEngineOn	           [get]	       : boolean
    +isEnginePowered	   [get]	       : boolean
    +isExitable	           [get]	       : boolean
    +isInsideNoDamageZone  [get]	       : boolean
    +isInsideSafezone	   [get]	       : boolean
    +isRefillable	   [get]	       : boolean
    +isRepaired	           [get]	       : boolean
    +isSiphonable	   [get]	       : boolean
    +isSkinned	           [get]	       : boolean
    +isTireReplaceable	   [get]	       : boolean
    +isUnderwater	   [get]	       : boolean
    +left                  [get] 	       : vector3
    +right                 [get] 	       : vector3
    +up                    [get] 	       : vector3
	
vehicleLook [Class]:
    +getBarricade()               : barricade
    +getPlayer()                  : player
    +getPoint()                   : vector3
    +getStructure()               : structure
    +getVehicle()                 : vehicle
    
zombie [Class]:
    +acid(vector3 direction, vector3 origin)
    +boulder(vector3 direction, vector3 origin)
    +breath()
    +charge()
    +damage(uInt16 amount, [bool dropLoot], [string ragdoll])
    +dropLoot()
    +kill([string ragdoll])
    +throw()
    +spark(vector3 target)
    +spawnZombie(vector3 position, [string speciality], [single angle])
    +spit()
    +stomp()
    +health 		   [get/set]   : uInt16
    +isBoss		       [get]	   : boolean
    +isCutesy		   [get]	   : boolean
    +isHunting		   [get/set]   : boolean
    +isHyper		   [get]	   : boolean
    +isMega		       [get]	   : boolean
    +isRadioactive	   [get]	   : boolean
    +id 	 	       [get]	   : uInt16
    +maxHealth		   [get]	   : float
    +position		   [get/set]   : vector3
    +speciality		   [get/set]   : string
    
    
Explanation:
Ragdoll must be one of these: "NONE", "BRONZE", "SILVER", "GOLD" or "ZERO".

Speciality must be one of these: "NONE", "NORMAL", "MEGA", "CRAWLER", "SPRINTER", "FLANKER_FRIENDLY", "FLANKER_STALK", "BURNER", "ACID", "BOSS_ELECTRIC", "BOSS_WIND", "BOSS_FIRE", "BOSS_ALL", "BOSS_MAGMA", "SPIRIT", "BOSS_SPIRIT", "BOSS_NUCLEAR", "DL_RED_VOLATILE", "DL_BLUE_VOLATILE", "BOSS_ELVER_STOMPER", "BOSS_KUWAIT".

RejectionReason must be one of these: "SERVER_FULL", "WRONG_HASH_LEVEL", "WRONG_HASH_ASSEMBLY", "WRONG_VERSION", "WRONG_PASSWORD", "NAME_PLAYER_SHORT", "NAME_PLAYER_LONG", "NAME_PLAYER_INVALID", "NAME_PLAYER_NUMBER", "NAME_CHARACTER_SHORT", "NAME_CHARACTER_LONG", "NAME_CHARACTER_INVALID", "NAME_CHARACTER_NUMBER", "PRO_SERVER", "PRO_CHARACTER", "PRO_DESYNC", "PRO_APPEARANCE", "ALREADY_PENDING", "ALREADY_CONNECTED", "NOT_PENDING", "LATE_PENDING", "WHITELISTED", "AUTH_VERIFICATION", "AUTH_NO_STEAM", "AUTH_LICENSE_EXPIRED", "AUTH_VAC_BAN", "AUTH_ELSEWHERE", "AUTH_TIMED_OUT", "AUTH_USED", "AUTH_NO_USER", "AUTH_PUB_BAN", "AUTH_ECON_DESERIALIZE", "AUTH_ECON_VERIFY", "PING", "PLUGIN", "CLIENT_MODULE_DESYNC", "SERVER_MODULE_DESYNC", "WRONG_LEVEL_VERSION", "WRONG_HASH_ECON", "WRONG_HASH_MASTER_BUNDLE", "LATE_PENDING_STEAM_AUTH", "LATE_PENDING_STEAM_ECON", "LATE_PENDING_STEAM_GROUPS", "NAME_PRIVATE_LONG", "NAME_PRIVATE_INVALID", "NAME_PRIVATE_NUMBER"

If something is inside [] means that is optional

```
