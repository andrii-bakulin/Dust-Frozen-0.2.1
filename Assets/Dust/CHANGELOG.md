## [InDev]

________________________________________________________________________________________________________________________

## [0.2.0] - 2021-03-22

### Added

- Added Actions/ActivateAction

### Fixed

- Removed prefix "Du" for all Actions
- Removed prefix "Du" for all Animations
- Removed prefix "Du" for all Factory & FactoryMachines
- Removed prefix "Du" for all Fields
- Removed prefix "Du" for all Gizmos
- Removed prefix "Du" for all Helpers
- Removed prefix "Du" for all Instances
- Removed prefix "Du" for core classes
- Renamed prefix "Du" to "On" for all Events
- Renamed class "DuDebug" to "Debugger"
- Fully reviewed Normalizer

________________________________________________________________________________________________________________________

## [0.1.3] - 2021-03-11

### Added

- Added Animations/DuScale
- Added Actions/DuAction [core]
- Added Actions/DuCallbackAction
- Added Actions/DuDelayAction
- Added Actions/DuDestroyAction
- Added Actions/DuFlipAction
- Added Actions/DuFlowRandomAction
- Added Actions/DuMoveByAction
- Added Actions/DuMoveToAction
- Added Actions/DuRotateByAction
- Added Actions/DuRotateToAction
- Added Actions/DuScaleByAction
- Added Actions/DuScaleToAction
- Added Actions/DuSpawnAction
- Added Actions/DuTintAction
- Added Actions/DuTransformCopyAction
- Added Actions/DuTransformRandomAction
- Added Actions/DuTransformSetAction
- Added Actions/DuUpdateHierarchyAction

### Fixed

- DuTranslate renamed to DuMove
- DuClampFactoryMachine: added flags to turn on/off PSR clamp for each or XYZ-axises 
- On add Du*Event component it correct append to Undo stack
- DuFactoryInstance: allow add only one component to object (Issue #24)
- DuFactory: forced rebuild factory instances on change any param of depended prefabs (Issue #18)
- DuRandomTransform: default activate is OnAwake now
- Fully review usage of Space enums. Sync diff values/types; Dropped WorkSpace enum


________________________________________________________________________________________________________________________

## [0.1.2] - 2021-01-12

Prepared Dust for open beta 


________________________________________________________________________________________________________________________

## [0.1.1] - 2021-01-05

### Added

- Added component DuKeyEvent
- Added component DuMouseEvent
- Added component DuDebug
- DuDestroyer: added new center mode, offset, events, disable colliders
- DuFollow: added speed mode, speed limit, offset, updateInEditor, etc
- DuPulsate: added sleep time value
- DuShake: added wakeUpTime value
- DuSpawner: added manual mode, multiple spawn, etc
- DuSpawner: added onSpawn event

### Fixed

- DustGUI: fixed when ExtraSlider & ExtraIntSlider broke values on multiple objects selection
- DuColliderEvent: fix error and draw inspector in Unity.2020.2.*

### Dropped

- Dropped DuCapture component (deprecated)


________________________________________________________________________________________________________________________

## [0.1.0] - 2020-12-29

Base alpha version
