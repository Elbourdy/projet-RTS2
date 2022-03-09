using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionPlayer : MonoBehaviour
{
    public static SelectionPlayer instance;


    private List<string> sounds = new List<string>();

    private void Awake()
    {
        instance = this;
    }


    //Add all units in the scene to this array
    //public GameObject[] allUnits;
    public List<GameObject> allFriendlyUnits = new List<GameObject>();
    //The selection square we draw when we drag the mouse to select units
    public RectTransform selectionSquareTrans;
    //To test the square's corners
    //public Transform sphere1;
    //public Transform sphere2;
    //public Transform sphere3;
    //public Transform sphere4;
    //The materials
    public Material normalMaterial;
    public Material highlightMaterial;
    public Material selectedMaterial;

    //All currently selected units
    public List<GameObject> selectedUnits = new List<GameObject>();

    //We have hovered above this unit, so we can deselect it next update
    //and dont have to loop through all units
    GameObject highlightThisUnit;

    //To determine if we are clicking with left mouse or holding down left mouse
    float delay = 0.3f;
    float clickTime = 0f;
    //The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    //If it was possible to create a square
    bool hasCreatedSquare;
    //The selection squares 4 corner positions
    Vector3 TL, TR, BL, BR;

    public bool canSelect = true;

    void Start()
    {
        //Deactivate the square selection image
        selectionSquareTrans.gameObject.SetActive(false);
    }

    void Update()
    {
        if (canSelect)
        {
            //Select one or several units by clicking or draging the mouse
            SelectUnits();

            //Highlight by hovering with mouse above a unit which is not selected
            HighlightUnit();

            sounds.Clear();
        }
    }

    //Select units with click or by draging the mouse
    void SelectUnits()
    {
        //Are we clicking with left mouse or holding down left mouse
        bool isClicking = false;
        bool isHoldingDown = false;

        //Click the mouse button
        if (Input.GetMouseButtonDown(0))
        {
            clickTime = Time.time;

            //We dont yet know if we are drawing a square, but we need the first coordinate in case we do draw a square
            RaycastHit hit;
            //Fire ray from camera
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //The corner position of the square
                squareStartPos = hit.point;
            }
        }
        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {

            if (Time.time - clickTime <= delay)
            {
                isClicking = true;
            }

            //Select all units within the square if we have created a square
            if (hasCreatedSquare)
            {
                hasCreatedSquare = false;

                //Deactivate the square selection image
                selectionSquareTrans.gameObject.SetActive(false);
                //Clear the list with selected unit
                selectedUnits.Clear();
                //Select the units
                for (int i = 0; i < allFriendlyUnits.Count; i++)
                {


                    GameObject currentUnit = allFriendlyUnits[i];

                    //Is this unit within the square
                    if (IsWithinPolygon(currentUnit.transform.position))
                    {
                        LaunchSoundSelectionForUnit(currentUnit);
                        selectedUnits.Add(currentUnit);
                        if (currentUnit.transform.Find("TorusFeedback") != null)
                            currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                    }
                    //Otherwise deselect the unit if it's not in the square
                    else
                    {
                        if (currentUnit.transform.Find("TorusFeedback") != null)
                            currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
        //Holding down the mouse button
        if (Input.GetMouseButton(0))
        {
            if (Time.time - clickTime > delay)
            {
                isHoldingDown = true;
            }
        }

        //Select one unit with left mouse and deselect all units with left mouse by clicking on what's not a unit
        if (isClicking)
        {
            if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
            {
                //Deselect all units
                for (int i = 0; i < selectedUnits.Count; i++)
                {

                    if (selectedUnits[i].GetComponent<MeshRenderer>())
                    {
                        selectedUnits[i].GetComponent<MeshRenderer>().material = normalMaterial;
                        if (selectedUnits[i].transform.Find("TorusFeedback") != null)
                            selectedUnits[i].transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        selectedUnits[i].GetComponentInChildren<MeshRenderer>().material = normalMaterial;
                        if (selectedUnits[i].transform.Find("TorusFeedback") != null)
                            selectedUnits[i].transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                }
                if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
                {
                    //Clear the list with selected units
                    selectedUnits.Clear();
                }
                //Try to select a new unit
                RaycastHit hit;
                //Fire ray from camera
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("GameplayUnits")))
                {
                    if (hit.collider.gameObject.GetComponent<Agent_Type>() != null)
                    //if (hit.collider.CompareTag("Friendly"))
                    {
                        GameObject activeUnit = hit.collider.gameObject;
                        //Set this unit to selected
                        if (activeUnit.GetComponent<MeshRenderer>() != null)
                        {
                            if (activeUnit.transform.Find("TorusFeedback") != null)
                                activeUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                        }

                        else
                        {
                            activeUnit.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;
                            if (activeUnit.transform.Find("TorusFeedback") != null)
                                activeUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                        }
                        //Add it to the list of selected units, which is now just 1 unit

                        LaunchSoundSelectionForUnit(activeUnit);
                        selectedUnits.Add(activeUnit);
                    }
                }
            }
        }

        //Drag the mouse to select all units within the square
        if (isHoldingDown)
        {
            //Activate the square selection image
            if (!selectionSquareTrans.gameObject.activeInHierarchy)
            {
                selectionSquareTrans.gameObject.SetActive(true);
            }

            //Get the latest coordinate of the square
            squareEndPos = Input.mousePosition;

            //Display the selection with a GUI image
            DisplaySquare();

            //Highlight the units within the selection square, but don't select the units
            if (hasCreatedSquare)
            {
                for (int i = 0; i < allFriendlyUnits.Count; i++)
                {
                    GameObject currentUnit = allFriendlyUnits[i];

                    //Is this unit within the square
                    if (IsWithinPolygon(currentUnit.transform.position))
                    {
                        if (currentUnit.GetComponent<MeshRenderer>()) currentUnit.GetComponent<MeshRenderer>().material = highlightMaterial;
                        if (currentUnit.transform.Find("TorusFeedback") != null)
                            currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                    }
                    //Otherwise deactivate
                    else
                    {
                        if (currentUnit.GetComponent<MeshRenderer>()) currentUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                        if (currentUnit.transform.Find("TorusFeedback") != null)
                            currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }
    }

    //Highlight a unit when mouse is above it
    void HighlightUnit()
    {
        //Change material on the latest unit we highlighted
        if (highlightThisUnit != null)
        {
            //But make sure the unit we want to change material on is not selected
            bool isSelected = false;
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] == highlightThisUnit)
                {
                    isSelected = true;
                    break;
                }
            }
            if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
            {
                if (!isSelected)
                {
                    if (highlightThisUnit.GetComponent<MeshRenderer>() != null)
                    {
                        highlightThisUnit.GetComponent<MeshRenderer>().material = normalMaterial;
                        if (highlightThisUnit.transform.Find("TorusFeedback") != null)
                            highlightThisUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                    else
                    {
                        highlightThisUnit.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
                        if (highlightThisUnit.transform.Find("TorusFeedback") != null)
                            highlightThisUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                    }
                }

                highlightThisUnit = null;
            }
        }

        //Fire a ray from the mouse position to get the unit we want to highlight
        RaycastHit hit;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("GameplayUnits")))
        {
            //Did we hit a friendly unit?
            if (hit.collider.gameObject.GetComponent<Agent_Type>() != null)
            //if (hit.collider.CompareTag("Friendly"))
            {
                //Get the object we hit
                GameObject currentObj = hit.collider.gameObject;

                //Highlight this unit if it's not selected
                bool isSelected = false;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] == currentObj)
                    {
                        isSelected = true;
                        break;
                    }
                }

                if (!isSelected)
                {
                    highlightThisUnit = currentObj;
                    if (highlightThisUnit.GetComponent<MeshRenderer>() != null)
                    {
                        highlightThisUnit.GetComponent<MeshRenderer>().material = highlightMaterial;
                        if (highlightThisUnit.transform.Find("TorusFeedback") != null)
                            highlightThisUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                    }
                    else
                    {
                        if (highlightThisUnit.GetComponentInChildren<MeshRenderer>()) highlightThisUnit.GetComponentInChildren<MeshRenderer>().material = highlightMaterial;
                        if (highlightThisUnit.transform.Find("TorusFeedback") != null)
                            highlightThisUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                    }
                }
            }
        }
    }

    //Is a unit within a polygon determined by 4 corners
    bool IsWithinPolygon(Vector3 unitPos)
    {
        bool isWithinPolygon = false;

        //The polygon forms 2 triangles, so we need to check if a point is within any of the triangles
        //Triangle 1: TL - BL - TR
        if (IsWithinTriangle(unitPos, TL, BL, TR))
        {
            return true;
        }

        //Triangle 2: TR - BL - BR
        if (IsWithinTriangle(unitPos, TR, BL, BR))
        {
            return true;
        }

        return isWithinPolygon;
    }

    //Is a point within a triangle
    //From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
    bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        bool isWithinTriangle = false;

        //Need to set z -> y because of other coordinate system
        float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));

        float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        float c = 1 - a - b;

        //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
        {
            isWithinTriangle = true;
        }

        return isWithinTriangle;
    }

    //Display the selection with a GUI square
    void DisplaySquare()
    {
        //The start position of the square is in 3d space, or the first coordinate will move
        //as we move the camera which is not what we want
        Vector3 squareStartScreen = Camera.main.WorldToScreenPoint(squareStartPos);

        squareStartScreen.z = 0f;

        //Get the middle position of the square
        Vector3 middle = (squareStartScreen + squareEndPos) / 2f;

        //Set the middle position of the GUI square
        selectionSquareTrans.position = middle;

        //Change the size of the square
        float sizeX = Mathf.Abs(squareStartScreen.x - squareEndPos.x);
        float sizeY = Mathf.Abs(squareStartScreen.y - squareEndPos.y);

        //Set the size of the square
        selectionSquareTrans.sizeDelta = new Vector2(sizeX, sizeY);

        //The problem is that the corners in the 2d square is not the same as in 3d space
        //To get corners, we have to fire a ray from the screen
        //We have 2 of the corner positions, but we don't know which,  
        //so we can figure it out or fire 4 raycasts
        TL = new Vector3(middle.x - sizeX / 2f, middle.y + sizeY / 2f, 0f);
        TR = new Vector3(middle.x + sizeX / 2f, middle.y + sizeY / 2f, 0f);
        BL = new Vector3(middle.x - sizeX / 2f, middle.y - sizeY / 2f, 0f);
        BR = new Vector3(middle.x + sizeX / 2f, middle.y - sizeY / 2f, 0f);

        //From screen to world
        RaycastHit hit;
        int i = 0;
        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TL), out hit))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TR), out hit))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BL), out hit))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BR), out hit))
        {
            BR = hit.point;
            i++;
        }

        //Could we create a square?
        hasCreatedSquare = false;

        //We could find 4 points
        if (i == 4)
        {
            //Display the corners for debug
            //sphere1.position = TL;
            //sphere2.position = TR;
            //sphere3.position = BL;
            //sphere4.position = BR;

            hasCreatedSquare = true;
        }
    }


    private void LaunchSoundSelectionForUnit(GameObject currentUnit)
    {
        string currentSound;
        if (currentUnit.GetComponent<SoundFeedbacks>() || currentUnit.GetComponentInChildren<SoundFeedbacks>())
        {
            if (currentUnit.GetComponent<SoundFeedbacks>())
                currentSound = currentUnit.GetComponent<SoundFeedbacks>().GetSoundNameSelection();
            else currentSound = currentUnit.GetComponentInChildren<SoundFeedbacks>().GetSoundNameSelection();

            if (sounds.Count > 0)
            {
                foreach (var item in sounds)
                {
                    if (currentSound == item)
                    {
                        Debug.Log(item + " " + currentSound);
                        break;
                    }

                    else
                    {
                        Debug.Log("Launch Sound");
                        sounds.Add(currentSound);
                        if (currentUnit.GetComponent<SoundFeedbacks>()) currentUnit.GetComponent<SoundFeedbacks>().LaunchSelectedSound();
                        else if (currentUnit.GetComponentInChildren<SoundFeedbacks>()) currentUnit.GetComponentInChildren<SoundFeedbacks>().LaunchSelectedSound();
                    }
                }
            }

            else
            {
                sounds.Add(currentSound);
                if (currentUnit.GetComponent<SoundFeedbacks>()) currentUnit.GetComponent<SoundFeedbacks>().LaunchSelectedSound();
                else if (currentUnit.GetComponentInChildren<SoundFeedbacks>()) currentUnit.GetComponentInChildren<SoundFeedbacks>().LaunchSelectedSound();
            }
        }
    }
}
