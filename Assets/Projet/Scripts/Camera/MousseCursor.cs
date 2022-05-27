using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Texture2D cursor;
    [SerializeField] Texture2D cursorAttack;

    [SerializeField] private Vector3 boxSize = new Vector3(1.5f, 1.5f, 1.5f);

    public float cdChange = 0.5f;
    private float timer = 0f;

    public bool isTargeting = false;

    void Start()
    {
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > cdChange)
        {
            CheckCursor();
            timer = 0f;
        }
    }


    private void CheckCursor()
    {
        RaycastHit hit;
        if (Physics.BoxCast(Camera.main.ScreenPointToRay(Input.mousePosition).origin, boxSize, Camera.main.ScreenPointToRay(Input.mousePosition).direction, out hit, Quaternion.identity, Mathf.Infinity, LayerMask.GetMask("GameplayUnits")))
        {
            if (hit.collider.GetComponent<Agent_Type>() != null && !isTargeting)
            {
                if (hit.collider.GetComponent<Agent_Type>().Type == Agent_Type.TypeAgent.Enemy)
                {
                    Debug.Log("Test cursor Rouge");
                    Cursor.SetCursor(cursorAttack, Vector3.zero, CursorMode.ForceSoftware);
                    isTargeting = true;
                }
            }
        }

        else
        {
            if (isTargeting)
            {
                Debug.Log("Test cursor bleu");
                isTargeting = false;
                Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
            }
        }
    }
    
}
