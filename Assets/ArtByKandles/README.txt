RPG Wolf Guide

Hi and  thank you for purchasing RPG Wolf!! As with most of my assets, I update them based on demand and feedback. If you have any ideas for additional skins or animations, feel free to email me at artbykandles@gmail.com. You can also send me any questions or errors you might find. I will response ASAP. 

If you'd like to recieve monthly assets from me, check out my patreon: https://www.patreon.com/Kandles

This pack is pretty straight forward, just pull the prefabs onto the scene and  they should be ready to go. 

All of the wolves share one controller and animations.

If you need the wolves to have their own controllers and seperate animations: 
Copy the animations and the controller 
Drag the new controller into the "animator" component of desired prefab. Open the controller and replace all of the animations in the "Animator Window" of Unity with the new copied animations. 

If you would like to reskin these wolves with your own art:
Copy the PNG textures that you are replacing
Save the new art on the SAME size canvas and in the SAME place as the old art.
Go into the "Parts" folder in the prefab
Select the part you'd like to change
On the top right of the inspector, go from "normal" mode to "debug" mode
Go to Saved Properties > Tex Envs >  _MainTex > Second
Under "Texture" Select the new PNG file you'd like to use

If you're having trouble with a sprite moving once an animation plays:
Create an Empty Game Object
Drag the prefab into that empty game object
Make sure any movement and positioning of the sprite is using the Empty Game Object as an anchor, and now the prefabs themselves. 

