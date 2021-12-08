using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildibg_placementAndValidation : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Building_List buildingList;
    private GameObject buildingToPlace;
    private bool selectionModeON = false;
    private bool placeValidated = false;
    private Vector3 cursorWolrdPosRounded;
    [SerializeField] GameObject preview;
    [SerializeField] Material green;
    [SerializeField] Material red;
    void Update()
    {
        if (selectionModeON)
        {
            GameObject raycastHit = Calculus();
            ValidateSelection(raycastHit);
            BuildingPreview();
            if (Input.anyKeyDown)
            {
                if (Input.GetMouseButton(0) && placeValidated)
                {
                    GameObject newBuilding = GameObject.Instantiate(buildingToPlace, cursorWolrdPosRounded, Quaternion.identity);
                    buildingList.AddToList(newBuilding);
                }
                selectionModeON = false;
                preview.SetActive(false);
                buildingToPlace = null;
            }
        }
    }

    private GameObject Calculus()
    {
        bool xNegative = false;
        bool zNegative = false;
        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            cursorWolrdPosRounded = hit.point;
            if (cursorWolrdPosRounded.x < 0) { xNegative = true; cursorWolrdPosRounded.x = -cursorWolrdPosRounded.x; }
            if (cursorWolrdPosRounded.z < 0) { zNegative = true; cursorWolrdPosRounded.z = -cursorWolrdPosRounded.z; }
            if (cursorWolrdPosRounded.x - (int)cursorWolrdPosRounded.x > 0.5f) cursorWolrdPosRounded.x = (int)cursorWolrdPosRounded.x + 1;
            else cursorWolrdPosRounded.x = (int)cursorWolrdPosRounded.x;
            if (cursorWolrdPosRounded.z - (int)cursorWolrdPosRounded.z > 0.5f) cursorWolrdPosRounded.z = (int)cursorWolrdPosRounded.z + 1;
            else cursorWolrdPosRounded.z = (int)cursorWolrdPosRounded.z;
            if (xNegative) cursorWolrdPosRounded.x = -cursorWolrdPosRounded.x;
            if (zNegative) cursorWolrdPosRounded.z = -cursorWolrdPosRounded.z;
            cursorWolrdPosRounded.y = buildingToPlace.GetComponent<Collider>().bounds.extents.y;
            return hit.transform.gameObject;
        }
        else return null;
    }
    private void ValidateSelection(GameObject hit)
    {
        placeValidated = true;
        Vector3 myBuildingAABB = buildingToPlace.GetComponent<Collider>().bounds.extents;
        foreach (GameObject item in buildingList.AccessList())
        {
            Vector3 distance = item.transform.position - cursorWolrdPosRounded;
            Vector3 itemAABB = item.GetComponent<Collider>().bounds.extents;
            if (distance.x < (itemAABB.x + myBuildingAABB.x) || distance.z < (itemAABB.z + myBuildingAABB.z)) placeValidated = false;
        }
    }
    private void BuildingPreview()
    {
        if (placeValidated) preview.GetComponent<Renderer>().material = green;
        else preview.GetComponent<Renderer>().material = red;
        preview.transform.position = cursorWolrdPosRounded;
    }
    public void EnterBuildingPlacement(GameObject building)
    { 
        selectionModeON = true; 
        buildingToPlace = building;
        preview.SetActive(true);
        preview.transform.localScale = building.GetComponent<Collider>().bounds.size;
    }
}
