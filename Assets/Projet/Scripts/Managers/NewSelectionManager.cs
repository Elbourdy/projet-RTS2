using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewSelectionManager : MonoBehaviour
{
    public static NewSelectionManager instance;

    public delegate void SelectionEvent();
    public SelectionEvent onChangeSelection;


    // Lists de selection
    public List<SelectableObject> selectableList = new List<SelectableObject>(); // = allfriendlyUnits
    
    [SerializeField] private List<SelectableObject> selectedObjects = new List<SelectableObject>(); // = selectedUnits

    public bool canSelect = true;
    //To determine if we are clicking with left mouse or holding down left mouse
    float delay = 0.3f;
    float clickTime = 0f;

    float delayDoubleClick = 0.3f;
    float clickTimeDoubleClick = 0f;

    //The selection square we draw when we drag the mouse to select units
    public RectTransform selectionSquareTrans;
    //The start and end coordinates of the square we are making
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    //If it was possible to create a square
    bool hasCreatedSquare;
    //The selection squares 4 corner positions
    Vector3 TL, TR, BL, BR;

    bool hasDoubleClick;
    private bool isHoldingCTRL;

    private SelectableObject hoveredObject = null;

    private Vector3 boxSize = new Vector3(1.5f, 1.5f, 1.5f);


    



    //Sounds
    private List<SoundFeedbacks> sounds = new List<SoundFeedbacks>();



    [Header("Selection Keyboard")]
    public GameObject nexus;
    public GameObject renard;
    public GameObject serpent;





    public List<SelectableObject> SelectedObjects 
    { get => selectedObjects; 
        set 
        {
            selectedObjects = value;
        } 
    }

    private void Awake()
    {
        instance = this;
        selectionSquareTrans.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (canSelect)
        {
            SelectUnits();
            HighlightUnits();
            CheckKeyboardInputs();
        }
        
        else
        {
            hasCreatedSquare = false;
            clickTime = Time.time;
        }
        sounds.Clear();
    }


    #region Simple Selection Fonctions

    // Fonction principale de sélection
    private void SelectUnits()
    {
        //Are we clicking with left mouse or holding down left mouse
        bool isClicking = false;
        bool isHoldingDown = false;
        bool isDoubleClicking = false;
        isHoldingCTRL = false;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isHoldingCTRL = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isHoldingCTRL = false;
        }

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
            if (Time.time - clickTimeDoubleClick <= delayDoubleClick)
            {
                isDoubleClicking = true;
            }

        }
        //Release the mouse button
        if (Input.GetMouseButtonUp(0))
        {
            clickTimeDoubleClick = Time.time;
            if (Time.time - clickTime <= delay && !hasDoubleClick)
            {
                isClicking = true;
            }
            hasDoubleClick = false;


            //Select all units within the square if we have created a square
            if (hasCreatedSquare)
            {
                hasCreatedSquare = false;

                //Deactivate the square selection image
                selectionSquareTrans.gameObject.SetActive(false);
                //Clear the list with selected unit
                if (!isHoldingCTRL)
                ClearSelection();
                //Select the units
                for (int i = 0; i < selectableList.Count; i++)
                {


                    SelectableObject currentUnit = selectableList[i];
                    if (currentUnit.canBeMultiSelected)
                    {
                        //Is this unit within the square
                        if (IsWithinPolygon(currentUnit.transform.position))
                        {
                            if (!isHoldingCTRL)
                            {
                                LaunchSoundSelectionForUnit(currentUnit.gameObject);
                                SelectedObjects.Add(currentUnit);
                                currentUnit.IsSelected = true;
                                onChangeSelection?.Invoke();
                                //if (currentUnit.transform.Find("TorusFeedback") != null)
                                //    currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = true;
                            }

                            else
                            {
                                CTRLChecking(currentUnit.gameObject);
                            }
                        }
                        //Otherwise deselect the unit if it's not in the square
                        else
                        {
                            //if (currentUnit.transform.Find("TorusFeedback") != null)
                            //    currentUnit.transform.Find("TorusFeedback").GetComponent<MeshRenderer>().enabled = false;
                        }
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
                if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
                {
                    //Clear the list with selected units
                    ClearSelection();
                }




                //Try to select a new unit
                RaycastHit hit;


                if (Physics.BoxCast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, boxSize, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("GameplayUnits") + LayerMask.GetMask("Crystal")))
                //Fire ray from camera
                //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("GameplayUnits") + LayerMask.GetMask("Crystal")))
                {
                    if (hit.collider.gameObject.GetComponent<SelectableObject>() != null)
                    {
                        GameObject activeUnit = hit.collider.gameObject;
                        //Set this unit to selected
                        LaunchSoundSelectionForUnit(activeUnit);
                        if (!isHoldingCTRL)
                        {
                            SelectedObjects.Add(activeUnit.GetComponent<SelectableObject>());
                            activeUnit.GetComponent<SelectableObject>().IsSelected = true;
                            onChangeSelection?.Invoke();
                        }

                        else
                        {
                            CTRLChecking(activeUnit);
                        }
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
                for (int i = 0; i < selectableList.Count; i++)
                {
                    SelectableObject currentUnit = selectableList[i];

                    //Is this unit within the square
                    if (IsWithinPolygon(currentUnit.transform.position))
                    {
                        currentUnit.ShowVisualFeeback();
                    }
                    //Otherwise deactivate
                    else
                    {
                        currentUnit.HideVisualFeedback();
                    }
                }

            }
        }

        //Select all the same unit by double-clicking
        if (isDoubleClicking)
        {
            if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
            {
                if (!EventSystem.current.IsPointerOverGameObject())  ///// j'ai rajouté ça guillaume
                {
                    //Clear the list with selected units
                    ClearSelection();
                }

                //Try to select a new unit
                RaycastHit hit;


                if (Physics.BoxCast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, boxSize, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("GameplayUnits") + LayerMask.GetMask("Crystal")))
                //Fire ray from camera
                //if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("GameplayUnits") + LayerMask.GetMask("Crystal")))
                {

                    if (hit.collider.gameObject.TryGetComponent(out Agent_Type type))
                    {
                        if (type.Type == Agent_Type.TypeAgent.Ally)
                        {
                            foreach (var item in selectableList)
                            {
                                if (item.name == type.name)
                                {
                                    SelectedObjects.Add(item);
                                    item.IsSelected = true;
                                    onChangeSelection?.Invoke();
                                }
                            }
                            hasDoubleClick = true;
                        }
                    }
                }
            }
        }
    }

    private void HighlightUnits()
    {
        RaycastHit hit;
        if (Physics.BoxCast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, boxSize, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("GameplayUnits") + LayerMask.GetMask("Crystal")))
        {
            if (hit.collider.gameObject.TryGetComponent(out SelectableObject tmp))
            {
                hoveredObject = tmp;
                tmp.ShowVisualFeeback();
            }
        }

        else
        {
            if (hoveredObject != null)
            {
                if (!hoveredObject.IsSelected)
                hoveredObject.HideVisualFeedback();
                hoveredObject = null;
            }
        }

    }


    private void ClearSelection ()
    {
        if (!isHoldingCTRL)
        {
            foreach (var item in SelectedObjects)
            {
                item.IsSelected = false;
            }
            SelectedObjects.Clear();
            onChangeSelection?.Invoke();
        }
    }

    private void CTRLChecking(GameObject goToAnalyse)
    {
        var tmp = goToAnalyse.GetComponent<SelectableObject>();
        bool isInList = false;
        foreach (var item in SelectedObjects)
        {
            if (tmp.gameObject == item.gameObject)
            {
                isInList = true;
                break;
            }
        }
        if (isInList)
        {
            tmp.IsSelected = false;
            SelectedObjects.Remove(tmp);
        }
        else
        {
            SelectedObjects.Add(tmp);
            tmp.IsSelected = true;
        }
        onChangeSelection?.Invoke();
    }


    #endregion

    #region Selection Keyboard Functions

    private void CheckKeyboardInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            ClearSelection();
            if (!isHoldingCTRL)
            {
                SelectedObjects.Add(nexus.GetComponent<SelectableObject>());
                nexus.GetComponent<SelectableObject>().IsSelected = true;
                onChangeSelection?.Invoke();
            }

            else
            {
                CTRLChecking(nexus);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ClearSelection();
            var classSelec = renard.GetComponent<ClassAgentContainer>();
            foreach (var item in selectableList)
            {
                if (item.gameObject.TryGetComponent(out ClassAgentContainer classItem))
                {
                    if (classItem.myClass == classSelec.myClass)
                    {
                        if (!isHoldingCTRL)
                        {

                            selectedObjects.Add(item);
                            item.IsSelected = true;
                        }

                        else
                        {
                            CTRLChecking(item.gameObject);
                        }
                    }
                }
            }
            onChangeSelection?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ClearSelection();
            var classSelec = serpent.GetComponent<ClassAgentContainer>();
            foreach (var item in selectableList)
            {
                if (item.gameObject.TryGetComponent(out ClassAgentContainer classItem))
                {
                    if (classItem.myClass == classSelec.myClass)
                    {
                        if (!isHoldingCTRL)
                        {

                            selectedObjects.Add(item);
                            item.IsSelected = true;
                        }

                        else
                        {
                            CTRLChecking(item.gameObject);
                        }
                    }
                }
            }
            onChangeSelection?.Invoke();
        }


    }


    #endregion

    #region Sound Functions
    private void LaunchSoundSelectionForUnit(GameObject currentUnit)
    {
        SoundFeedbacks currentSound;

        if (currentUnit.GetComponent<SoundFeedbacks>() || currentUnit.GetComponentInChildren<SoundFeedbacks>())
        {
            if (currentUnit.GetComponent<SoundFeedbacks>())
                currentSound = currentUnit.GetComponent<SoundFeedbacks>();
            else currentSound = currentUnit.GetComponentInChildren<SoundFeedbacks>();
        
            if (sounds.Count == 0)
            {
                sounds.Add(currentSound);
                currentSound.LaunchSelectedSound();
            }

            else
            {
                bool isAlreadyLaunch = false;
                foreach (var item in sounds)
                {
                    if (item.GetSoundNameSelection() == currentSound.GetSoundNameSelection())
                    {
                        isAlreadyLaunch = true;
                        break;
                    }
                }

                if (!isAlreadyLaunch)
                {
                    sounds.Add(currentSound);
                    currentSound.LaunchSelectedSound();
                }
            }



        //    if (sounds.Count > 0)
        //    {
        //        foreach (var item in sounds)
        //        {
        //            if (currentSound == item)
        //            {
        //                Debug.Log(item + " " + currentSound);
        //            }
        //
        //            else
        //            {
        //                Debug.Log("Launch Sound");
        //                sounds.Add(currentSound);
        //                if (currentUnit.GetComponent<SoundFeedbacks>()) currentUnit.GetComponent<SoundFeedbacks>().LaunchSelectedSound();
        //                else if (currentUnit.GetComponentInChildren<SoundFeedbacks>()) currentUnit.GetComponentInChildren<SoundFeedbacks>().LaunchSelectedSound();
        //            }
        //        }
        //    }
        //
        //    else
        //    {
        //        sounds.Add(currentSound);
        //        if (currentUnit.GetComponent<SoundFeedbacks>()) currentUnit.GetComponent<SoundFeedbacks>().LaunchSelectedSound();
        //        else if (currentUnit.GetComponentInChildren<SoundFeedbacks>()) currentUnit.GetComponentInChildren<SoundFeedbacks>().LaunchSelectedSound();
        //    }
        }
    }




    #endregion


    #region Maths Fonctions Pour Selection
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TL), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            TL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(TR), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            TR = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BL), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
        {
            BL = hit.point;
            i++;
        }
        if (Physics.Raycast(Camera.main.ScreenPointToRay(BR), out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
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
    #endregion
}
