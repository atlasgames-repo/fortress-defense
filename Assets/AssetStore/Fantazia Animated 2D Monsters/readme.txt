In order for this asset to work, you need to download spine-unity package V3.8:
http://esotericsoftware.com/spine-unity-download#Older-Versions
To use the asset just drag one of the prefabs from "Prefabs" folder and put it on the scene (you might need to change the x and y scale if the monster is too small for your game)

You can easily manipulate the prefabs and skeletons. Things you can do by using spine:
-On animation complete function (Example: give the player loot and gold when the death animation is completed)
-On event function (All the monsters have "OnDamaging" event that is triggered when the attack animation reaches the damaging position. For example, when the skeleton sword reaches the player, you can trigger damage taking function)
-Generate transforms that follow the skeleton of the monster. (Example: attaching a collider on the sword of the skeleton and call OnTriggerEnter2D when it hits an object)
-And much much more (check this page for all the functionality of Spine in Unity http://esotericsoftware.com/spine-unity)



If you notice some weird gray lines in the monsters then that means your Color Space is not set to Gamma, to change it then do the following:
Edit > Project Settings > Player > Other Settings > Rendering > Color Space to Gamma


If you have made a game using this asset then we will be glad if you sent us its name so we can show it to everyone (if you give us the permission)


**** Please consider leaving a review on the assetstore page (https://assetstore.unity.com/packages/slug/174782). This will greatly help us updating the package with more monsters.
**** Thanks for purchasing and good luck on your game.




-Asset Page: https://assetstore.unity.com/packages/slug/174782
Warriors: Animated 2D Characters: https://assetstore.unity.com/packages/2d/characters/warriors-animated-2d-characters-178121
Fantazia: Character Editor: https://assetstore.unity.com/packages/2d/characters/fantazia-character-editor-181572
-Here are our other assets: https://assetstore.unity.com/publishers/19300 send us an email when you review Fantazia: Animated 2D Monsters.
-Contact us: yazun.shn@gmail.com




