---------------------------------------------readme.txt----------------------------------------------------------

Terrain:
Generated a gameobject, which is a flat plane mesh along the x and z axis. ItÅf made up with a number of triangles as standard polygons by a number of vertices. Using Diamond-square algorithm to calculate the z value of each vertex in the terrain. The method is in ÅgApplyDiamondSquare()Åh. That is the key point of making this Terrain. The detailed method is referenced from Wikipedia (Diamond-square algorithm): https://en.wikipedia.org/wiki/Diamond-square_algorithm.
A texture was downloaded, edited and overlaid onto each vertex to create the impression of a rocky detailed surface on the mountains.


Waves: 
Generated a flat plane mesh along the x and z axis with a custom number of vertices. This done was through the creation of clockwise triangles, generated 2 triangles at a time for each square of vertices in the mesh. The the position and scale of the plane is then transformed to fit the mountain landscape. Then a custom shader that uses perpendicular cosine and sine waves to generate waves in the plane was created so that amplitude, wavelength and speed were all adjustable using the formula;
sin((vertex + _Time.y * _Speed)numWaves) (_Amplitude/num_waves)
Where numWaves = 2 * UNITY_PI/_Wavelength;
A texture was downloaded, edited and overlaid onto each vertex to create the impression of a ripple/wave effect on the water.

Target FPS : Turn off vSync, then set target fps to 30 (update every frame). 

Phong illumination: 
Both the shader for the terrain and the waves are mainly based on and modified from the phong model shader obtained in week 5 tutorial. Textures are directly applied from scripts and rgb value are added to the rgb output of the phong illumination model. A terrain orbiting sphere is implemented as the sun (Emission effect applied for realisticity) and acts as the light source to objects. In which illumination effect is calculated based on the vertex normals and their direction towards the ÅesunÅf. Ambient, diffuse and specular variables were then adjusted and combined for realistic shading. A night-day life effect is created by manually darkening the color of the texture, and color interaction with lighting allows a visual ÅedayÅf effect. 

Collision detection:
Added the collider and the rigidbody components in Waves, Terrain and the camera. So that it can use the unity build-in function to detect the collision. Also, we are setting 2 as the radius of the camera. It will protect the camera from seeing inside of the terrain.

Camera:
The camera can detect keyboard and mouse behaviours. It can allow the mouse to control the pitch and yaw by using Ågtransform.eulerAnglesÅh. Moreover, when detecting the keyboard input, it will use Åg transform.translateÅh to move the object based on the correct cameraÅfs current orientation.



Sources:
TargetFrameRate code based off answer by Brogan89 - https://answers.unity.com/questions/1366716/how-to-liimit-fps.html
Waves shader based off workshop 4/5 solution
Terrain shader based off workshop 5 solution
Terrain texture from Unity Asset Store
Diamond-Square code was adapted from Ahter Omar: https://www.youtube.com/watch?v=1HV8GbFnCik


Material/ texture sources:
Wave texture from http://telias.free.fr/textures_tex/water_tex.html
Terrain texture and skybox from Unity Asset Store
