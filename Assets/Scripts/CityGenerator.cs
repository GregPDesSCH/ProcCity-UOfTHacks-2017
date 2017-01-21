using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
*/


public class CityGenerator : MonoBehaviour
{
    // Singleton Instance
    public static CityGenerator cityGenerator;

    // Global Variables
    public float cityGridSquareSize = 1.25f;
    public int numberOfRows = 20;
    public int numberOfColumns = 20;

    // Prefab Assets
    public Transform initialBuilding;
    public Transform initialRoad;

    // Properties of the city

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /* 
        Steps to generating a city:

        1. Define an appropriate grid side to build the city out of.
        2. Draw out all of the roads in the grid, making sure that it satisfies the following constraints:
            a. Four pieces of roads can't be placed in a square.
            b. You can have a turn at the border, but it has to be a turn so that it does not look like it's passing through the limits.
            c. You can't have two intersections placed together.
            d. These are the different pieces that can be used to build the road network:
                                         |      ---     ---    --- 
                -----    |-    -|-       ---    |       |        |
                                                      ---        ---
        3. With the remaining spaces, add unique buildings to each block, or a set of blocks. Instantiate a GameObject
           and then use the appropriate functions in BuildingGenerator to make up the building out of a network of prefabs.
        4. Add outer walls to each side of the grid appropriately.


    */

    //
    void createRoads ()
    {
        /* 
            Steps:
            1. Lay out cross intersections.
            2. Lay out T intersections.
            3. Lay out left chicanes.  (complicated)
            4. Lay out right chicanes. (complicated)
            5. Lay out straight roads.
        */


    }

    // This function creates a straight line of road.
    void createStreets(int numberOfRoadPieces, bool streetIsNorthAndSouth, float initialX, float initialZ)
    {
        Vector3 positionVector;

        for (int roadPieceIndex = 0; roadPieceIndex < numberOfRoadPieces; roadPieceIndex++)
        {
            if (streetIsNorthAndSouth)
                positionVector = new Vector3(initialX, initialZ + cityGridSquareSize * roadPieceIndex, 0.0f);
            else
                positionVector = new Vector3(initialX + cityGridSquareSize * roadPieceIndex, initialZ, 0.0f);
            Instantiate(initialRoad, positionVector, Quaternion.identity);
        }
    }

    // This function creates a cross intersection.
    void createCrossIntersection(float originXCoordinate, float originZCoordinate)
    {
        createStreets(3, false, originXCoordinate - cityGridSquareSize, originZCoordinate);
        createStreets(1, false, originXCoordinate, originZCoordinate - cityGridSquareSize);
        createStreets(1, false, originXCoordinate, originZCoordinate + cityGridSquareSize);
    }

    void createTIntersection(byte orientation, float originXCoordinate, float originZCoordinate)
    {
        if (orientation == 0 || orientation == 2)
        {
            createStreets(3, false, originXCoordinate - cityGridSquareSize, originZCoordinate);
            createStreets(3, false, originXCoordinate, 
                originZCoordinate + orientation == 0 ? -cityGridSquareSize : cityGridSquareSize);
        }
        else
        {
            createStreets(3, true, originXCoordinate, originZCoordinate - cityGridSquareSize);
            createStreets(3, true, originXCoordinate + orientation == 1 ? -cityGridSquareSize : cityGridSquareSize, 
                originZCoordinate);
        }
    }

    void createLeftChicane(bool chicaneIsNorthAndSouth, float originXCoordinate, float originZCoordinate)
    {
        if (chicaneIsNorthAndSouth)
        {
            createStreets(2, false, originXCoordinate - cityGridSquareSize, originZCoordinate);
            createStreets(1, false, originXCoordinate - cityGridSquareSize, originZCoordinate + cityGridSquareSize);
            createStreets(1, false, originXCoordinate, originZCoordinate - cityGridSquareSize);
        }
        else
        {
            createStreets(2, true, originXCoordinate, originZCoordinate - cityGridSquareSize);
            createStreets(1, false, originXCoordinate + cityGridSquareSize, originZCoordinate - cityGridSquareSize);
            createStreets(1, false, originXCoordinate - cityGridSquareSize, originZCoordinate);
        }
    }

    void createRightChicane(bool chicaneIsNorthAndSouth, float originXCoordinate, float originZCoordinate)
    {
        if (chicaneIsNorthAndSouth)
        {
            createStreets(2, false, originXCoordinate, originZCoordinate);
            createStreets(1, false, originXCoordinate + cityGridSquareSize, originZCoordinate + cityGridSquareSize);
            createStreets(1, false, originXCoordinate, originZCoordinate - cityGridSquareSize);
        }
        else
        {
            createStreets(2, true, originXCoordinate, originZCoordinate);
            createStreets(1, false, originXCoordinate + cityGridSquareSize, originZCoordinate + cityGridSquareSize);
            createStreets(1, false, originXCoordinate - cityGridSquareSize, originZCoordinate);
        }
    }


}
