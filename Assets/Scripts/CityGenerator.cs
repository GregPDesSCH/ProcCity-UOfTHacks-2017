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
    public float cityGridSquareSize = 1f;
    public int numberOfRows = 19;
    public int numberOfColumns = 19;

    // Prefab Assets
    public Transform initialBuilding;
    public Transform initialRoad;

    // Properties of the city
    private GameObject[] cityGrid;
    private Transform[] cityGridPositions;

	// Use this for initialization
	void Start ()
    {
        // Singleton initialization
        if (cityGenerator == null)
            cityGenerator = this;
        else
            Destroy(gameObject);

        // Create array to keep track of adding the elements.
        cityGrid = new GameObject[numberOfRows * numberOfColumns];
        cityGridPositions = new Transform[numberOfRows * numberOfColumns];

        // Create the roads.
        createRoads();
        createBuildings();


        // Get rid of all the references this object provides.
        cityGenerator = null;
        //foreach (GameObject cityObject in cityGrid)
            //Destroy(cityObject);

        // Get rid of the city generator after the scene has been made.
        Destroy(gameObject);
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



        /* Increment 1: Rows of Buildings and Roads (all blank white)
        
        For now, the roads for the city are relatively simple; a bunch of north-south roads with two west-east roads on top and bottom.*/
        for (int colIndex = 0; colIndex < numberOfColumns; colIndex++)
        {
            if (colIndex % 2 == 0)
                createStreets(numberOfRows - 2, true, (numberOfColumns / 2) * -cityGridSquareSize + colIndex * cityGridSquareSize,
                    (numberOfRows / 2) * -cityGridSquareSize + cityGridSquareSize);
        }

        createStreets(numberOfColumns, false, (numberOfColumns / 2) * -cityGridSquareSize, (numberOfRows / 2) * -cityGridSquareSize);
        createStreets(numberOfColumns, false, (numberOfColumns / 2) * -cityGridSquareSize, (numberOfRows / 2) * cityGridSquareSize);
        
    }

    // This function creates a straight line of road.
    void createStreets(int numberOfRoadPieces, bool streetIsNorthAndSouth, float initialX, float initialZ)
    {
        Vector3 positionVector;

        for (int roadPieceIndex = 0; roadPieceIndex < numberOfRoadPieces; roadPieceIndex++)
        {
            if (streetIsNorthAndSouth)
                positionVector = new Vector3(initialX, 0, initialZ + cityGridSquareSize * roadPieceIndex);
            else
                positionVector = new Vector3(initialX + cityGridSquareSize * roadPieceIndex, 0, initialZ);
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




    void createBuildings()
    {
        Vector3 positionVector;
        for (int colIndex = 0; colIndex < numberOfColumns; colIndex++)
        {
            for (int rowIndex = 1; rowIndex < numberOfRows - 1 && colIndex % 2 == 1; rowIndex++)
            {
                positionVector = new Vector3((numberOfColumns / 2) * -cityGridSquareSize + colIndex * cityGridSquareSize,
                    initialBuilding.localScale.y / 2, (numberOfRows / 2) * -cityGridSquareSize + cityGridSquareSize * rowIndex);
                Instantiate(initialBuilding, positionVector, Quaternion.identity);
            }
                
        }

        // createStreets(numberOfRows - 2, true, (numberOfColumns / 2) * -cityGridSquareSize + colIndex * cityGridSquareSize,
                    //(numberOfRows / 2) * -cityGridSquareSize + cityGridSquareSize);

    }

}
