using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapCam : MonoBehaviour
{

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PoiterEventData;
    [SerializeField] EventSystem m_EventSystem;

    

    [SerializeField] private GameObject mapZone;

    // Positions values
    private Vector3 originPos;
    private Vector3 direction;

    private float wheelDir = 1;

    // Cam Values
    private Camera miniCam;
    private float originSize;

    [Header("Camera Values")]
    [SerializeField] private float camZoomSpeed = 2;
    [SerializeField] private float ZoomLimit = 3;
    [SerializeField] private float camDirSpeed = 3;
    [SerializeField] private Vector3 startPos;
    public bool activateMinimapCamBehaviour = true;


    private void Start()
    {
        originPos = transform.position;
        miniCam = GetComponent<Camera>();
        originSize = miniCam.orthographicSize;
        InitPosAndSize();
    }


    private void InitPosAndSize()
    {
        miniCam.orthographicSize = (originSize + ZoomLimit) / 2;
        transform.localPosition = startPos;
    }

    private void Update()
    {
        if (activateMinimapCamBehaviour && IsWheeling())
        {
            CheckHitMinimap();
        }
    }

    private void CheckHitMinimap()
    {
        m_PoiterEventData = new PointerEventData(m_EventSystem);
        
        m_PoiterEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PoiterEventData, results);


        foreach (RaycastResult item in results)
        {
            if (item.gameObject.name == "MapZone")
            {
                ZoomCam();
                MoveCam();
            }
        }
    }

    private bool IsWheeling()
    {
        if (Input.mouseScrollDelta != Vector2.zero)
            return true;

        return false;
    }

    private void ZoomCam()
    {
        UpdateWheelDir();
        miniCam.orthographicSize -= wheelDir * camZoomSpeed;
        CheckZoomLimit();
    }

    private void MoveCam()
    {

        if (wheelDir > 0)
        {
            var actualMousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
            direction = actualMousePos - mapZone.transform.position;
            direction = new Vector3(direction.y, 0, -direction.x);
            transform.position += direction * Time.deltaTime * camDirSpeed;
        }

        if (wheelDir < 0 )
        {
            direction = originPos - transform.position;
            direction = new Vector3(direction.x, 0, direction.z);
            transform.position += direction * Time.deltaTime * camDirSpeed;
        }
    }

    private void CheckZoomLimit()
    {
        if (miniCam.orthographicSize > originSize)
        {
            miniCam.orthographicSize = originSize;
        }

        else if (miniCam.orthographicSize < ZoomLimit)
        {
            miniCam.orthographicSize = ZoomLimit;
        }
    }

    private void UpdateWheelDir()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            wheelDir = 1;
        }

        else
        {
            wheelDir = -1;
        }
    }

}
