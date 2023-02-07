# uScriptEvents

Event: onBarricadeBuilded(barricade)
Event: onBarricadeDamaged(player, barricade, damage, cause, *cancel)
Event: onBarricadeSalvaged(player, barricade, *cancel)

Event: onPlayerBleedingUpdated(player, isBleeding)
Event: onPlayerBrokenUpdated(player, isBroken)
Event: onPlayerFoodUpdated(player, newFood)
Event: onPlayerHealthUpdated(player, newHealth)
Event: onPlayerJoinRequested(player)
Event: onPlayerOxygenUpdated(player, newOxygen)
Event: onPlayerRelayVoice(player, walkie, *cancel)
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

Event: onVehicleLockRequest(vehicle, *cancel)

barricade [Class]:
    +anglex                [get]       : single
    +angley                [get]       : single
    +anglez                [get]       : single
    +generator             [get]       : generator
    +fire                  [get]       : fire
    +oven                  [get]       : oven

consumeable [Class]:
    +food                  [get]       : float
    +water                 [get]       : float

effectspawn [Class]:
    +effect(string guid, vector3 position)
    +effectPlayer(string guid, vector3 position, player)
    +effectClear(string guid)
 
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

item [Class]:
    +consumeable           [get]           : consumeable
    +durability            [get/set]       : uInt16
    +gun                   [get]           : gun

oven [Class]:
    +lit                   [get/set]       : boolean
    +wired                 [get]           : boolean

player [Class]:
    +salvageTime           [get/set]       : uInt16
    +temperature           [get]           : string

playerClothing [Class]:
    +removeBackpack()
    +removeGlasses()
    +removeHat()
    +removeMask()
    +removePants()
    +removeShirt()
    +removeVest()

playerSteam [Class]:
    +id                    [get]           : string            
    +name                  [get/set]       : string

vehicle [Class]:
    enterVehicle(player)
