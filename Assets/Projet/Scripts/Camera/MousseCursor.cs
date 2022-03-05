using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousseCursor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Texture2D cursor;
    void Start()
    {
        Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
    }

   
}
