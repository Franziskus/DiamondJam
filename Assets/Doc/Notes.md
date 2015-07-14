# Implementation Notes
I started with LevelData and GemFactory in order to enable some testing on it. IDs are a compact form in order to save the values but are unsuitable for the logical level. However, I had some test levels ready to use. To be flexible my structure is build based on components with LevelManager as a central administration unit. The LevelManager is responsible for the process of gems and rounds.
![UML Class diagramm](http://yuml.me/diagram/scruffy/class/[LevelManager%7Ccontext:IGemContext]+-%3E[%3C%3CILevelPhysics%3E%3E],%20[%3C%3CILevelPhysics%3E%3E]%5E-[FallDownRightPhysics],%20[LevelManager]+-%3E[%3C%3CILevelRules%3E%3E],%20[%3C%3CILevelRules%3E%3E]%5E-[RescueTargetRules],%20[LevelManager]+-%3E[%3C%3CILevelSource%3E%3E],%20[%3C%3CILevelSource%3E%3E]%5E-[LevelDataSource],%20[%3C%3CILevelSource%3E%3E]%5E-[RandomLevelSource],%20[VisualManager]-[LevelManager],%20[%3C%3CIGemContext%3E%3E]%5E-[Level%7Citems:%20Gem])(http://yuml.me/3ba853ca)
The components automatically register when the game starts. It enables the designer to regulate the level and to compose from the source.

By expending the ILevelRules new implementation of new rules can be added. For example: New Bonus rounds could be added where the player can pop unlimited gems in a certain timeframe. 

Same is true for ILevelSource were for example a level could be created a pre-defined or randomly. This can be realize with Unity by simply exchanging the components. 

Same thing for Physics although it seldom happens.

In order to be able to administrate  and re-use the Gems the following structure was used:
![UML Class diagramm](http://yuml.me/diagram/scruffy/class/[Gem%7C+group:int;+chain:bool]%5E-[ColorGem],%20[Gem]%5E-[Bomb],%20[Gem]%5E-[Firework],%20[Gem]%5E-[Target],%20[Gem]%5E-[Turn],%20[ColorGem%7C+color:GemColor]%5E-[IcedColorGem%7C+hitpoints:int],%20[ColorGem]%5E-[BonusColorGem%7C+points:int])(http://yuml.me/2b7852e7)
As you can see. It worked very well especially with Color Iced and Bonus Gems.

All components and Gems are sharing the same context (Level) and adjust the level.

The UI is kept simple and build as a facade so that only the LevelManager communicates to the VisualManager.

The VisualManager simply holds an array of the GameObjects that can be clicked on to show the level status. In addition it is also responsible for the rest of UI.
