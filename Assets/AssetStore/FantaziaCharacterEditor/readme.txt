
In order for this asset to work, you need to download spine-unity package V3.8:
http://en.esotericsoftware.com/spine-unity-download


You can easily manipulate the prefabs and skeletons. Things you can do by using spine:
-On animation complete function (Example: show Game Over screen when the death animation is completed)
-On event function. There are multiple events in the characters which can be used to do certain function (Example: fire arrow when OnArrowLeftBow is triggered)
-And much much more (check this page for all the functionality of Spine in Unity http://esotericsoftware.com/spine-unity)

The events in the animations are:
OnDamaging: For Attack1 and Attack2. Occurs when the weapon reaches the target to hit.
OnFinishingCasting: For Cast1, Cast2 and Cast3: Occurs when the casting animation is reaching the state where the character can fire a skill
OnArrowLeftBow: For Shoot1, Shoot2 and Shoot3: Occurs when shooting animation reaches the state where the arrow disappears (so you can make your own arrow to be fired)

****The gears are divided into multiple skins in Spine. For more information about Spine Skins in runtime read: 
http://esotericsoftware.com/blog/Spine-3-8-released#Improved-skin-API Section: Improved skin API
http://esotericsoftware.com/forum/Combining-Skins-12528
http://en.esotericsoftware.com/spine-runtime-skins

The way skin combination used in this asset is: (Full implementaion is found in GearEquipper.cs script)
-Get the skeleton and its data from the character gameobject
	var skeleton = characterAnimator.Skeleton;
	var skeletonData = skeleton.Data;
-Create a custom skin using:
	var NewCustomSkin = new Skin("CustomCharacter");
Then add the gear pieces you want (The names of the gear sprites in Resources/GearIcons is the name of the corrsponding skin):
	NewCustomSkin.AddSkin(skeletonData.FindSkin("MELEE 1")); //This will equip the character with the melee weapon #1
-Add all the gears you want, then to apply the skin write:
	skeleton.SetSkin(NewCustomSkin);
	skeleton.SetSlotsToSetupPose();


Applying any animation is very easy, simply write: (Full implementaion is found in CharacterAnimator.cs script)
	characterAnimator.AnimationState.SetAnimation(0, ANIMATION_NAME, Is_Loop);

-The animation names are:
Attack1
Attack2
Attack 1 DUELIST
Attack 2 DUELIST
Attack 3 DUELIST
Buff
Cast1
Cast2
Cast3
Death
Defence
Fly
Hurt
Idle
Idle Archer
Jump	//Full Jump
Jump1	//Take off
Jump1 ARCHER
Jump2	//Idle Flying
Jump3	//Landing
Jump3 ARCHER
Run
Run ARCHER
Run DUELIST
Shoot1
Shoot2
Shoot3
Walk

****IMPORTANT NOTE: Sometimes when you drag a prefab, it appears as the basic skin. It is on visual bug from Spine itself, when you play the game the character will have its full gear.


If you notice some weird gray lines in the characters then that means your Color Space is not set to Gamma, to change it then do the following:
Edit > Project Settings > Player > Other Settings > Rendering > Color Space to Gamma




**** Please consider leaving a review on the assetstore page (https://assetstore.unity.com/packages/slug/174782). This will greatly help us updating the package with more monsters.
**** Thanks for purchasing and good luck on your game.


-Fantazia: Character Editor: https://assetstore.unity.com/packages/slug/181572
-Fantazia: Animated 2D Monsters: https://assetstore.unity.com/packages/2d/characters/fantazia-animated-2d-monsters-174782
-Here are our other assets: https://assetstore.unity.com/publishers/19300 
-Contact us: yazun.shn@gmail.com




