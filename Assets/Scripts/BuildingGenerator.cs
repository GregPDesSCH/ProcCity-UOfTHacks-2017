using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () { 
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /*
    
        Steps to procedurally generate a city building:

        Like in any other building construction, we start from the bottom with the foundation / base,
        then work on our way up adding floors of modules as we go, and finally the roof on top.

        1. We select either one, two, or three building entrance modules.
        2. Fill the rest of the floor with mostly outer solid walls.
        3. Start adding the floors on top with a for loop with perhaps intervals or random values to add intricate features.
        4. On each level, select a type of module to use for the straight sides of the building.
        5. At each corner, add a specific module to make the corners look neat, unless the module used for each floor is completely glass.
        6. Add a roof on top of the building (which can be just a plane at a low-level perspective)
     
    */



}
