# UrbanPathMaker

Welcome!
This repo is meant to walk you through the process of creating your own urban path maker based on our case study for NYC. We have divided the process into 4 different sections, and for each we have provided the files and datasets we have used. You are welcome to download and use them, however we believe that you will have to edit and update the documents based on your specific needs. This is meant to be a roadmap with clear examples rather than a copy/paste repo - a few basic things will be needed to follow the path we took to make this project happen:

1. A set of available pedestrian data for your location
2. Access to URBANO for Grasshopper (can find in food4rhino.com)
3. Access to google colab
4. Knowledge of Unity

----------------------------------------------------------------------------------
A – Data Gathering

1.	Collection of pedestrian data from city sources: NYC Department of Transit Data (NYCDOT)

The NYCDOT has gathered for the past 10 years information on pedestrian data throughout the city. This excel spreadsheet is taken directly from the nyc.gov website and can be used for public purposes.
2.	Collection of amenity and public transport values for each NYCDOT location

The grasshopper plug-in Urbano contains data from OSM and Google Places for two cities (as of 6/26/2020) New York and Paris. With this grasshopper script one can collect the data needed for each geolocation. To have it work, you must input the coordinates data extracted from the OSM website onto the script and download a map file for it that will provide all the information needed.

3.	Combined dataset

This csv file shows the collection of data on step 1 and two, and how we combined them in order to have a concise dataset that is formatted for training our AI

------------------------------------------------------------------------------------

B – AI training: ANN Regression Model
1.	Tensorflow Keras Training through Google Colab

The ANN Regression Model we used was tailored to our dataset. This google colab shows our process, and it is likely that if you have a different dataset the hyper-parameters of the model would need to be altered for your needs. Elements to alter are things such as activation layers, number of hidden layers, or number of neurons per layer to name a few.

2.	Trained Model (.h5 file and pickled scalers)

The .h5 and saved scalers are our trained model, from which you can run predictions for pedestrian numbers throughout NYC. Please note that the prediction accuracy values change depending on the location, our model is the most precise in high density areas, such as Manhattan, Brooklyn, and some areas of Queens, and it shows higher discrepancies in areas such as the Bronx, Staten Island, and the Rockaways. This is due to the lack of pedestrian collection points in some areas of the city by NYCDOT.

------------------------------------------------------------------------------------

C – Preparation for UNITY

1.	Converting .h5 model into .ONNX model (Krishna??)

This colab notebook shows the needed steps to transform a .h5 trained model into an open source format called .ONNX which interacts with the Unity interface Barracuda.

2.	Producing Tiles: Grasshopper Urbano FBX script.

With this script you can generate a FBX file with NYC geometry. The script separates amenities into layers as well as creating entry and exit gates for pedestrians to appear in the game.

------------------------------------------------------------------------------------

D – UNITY Game Generator Scripts

3.	Producing Pedestrians: spawn_source.cs

With this script, the unity game is interacting with the .ONNX trained model and generating predictions for the number of pedestrians at a given scene.

4.	Creating a navigation plane:  NavMeshComponents > NavMeshSurface.cs

With this script, one creates the areas in which pedestrians can navigate. Those areas that are excluded act as barriers and pedestrians can’t access them. 

5.	Dragging buildings during the game: Drag3D.cs

With this script the FBX building elements can be moved and dragged on the scene during run time.

6.	Adding amenities to the scene: OnClickInLayer.cs

With this script, one can select an amenity type from a dropdown menu and add a new amenity to the scene.

7.	Navigating the scene with keyboard and mouse: PlayerMovement.cs and Scroll.cs

With this script one can use the keyboard to navigate through the scene as well as zoom in and out by scrolling the wheel of a mouse.
