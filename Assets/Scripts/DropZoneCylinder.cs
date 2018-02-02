using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneCylinder : MonoBehaviour {
    public bool hasObject = false;
    private DropZoneHandler dropZoneHandlerScript;

    private void Start()
    {
        dropZoneHandlerScript = transform.parent.GetComponent<DropZoneHandler>();
    }

    private void OnCollisionEnter(Collision other)
    {
        hasObject = true;
        Debug.Log("collision enter " + this + " and " + other);
        dropZoneHandlerScript.addCorrectDrop();
    }

    private void OnCollisionExit(Collision other)
    {
        hasObject = false;
        dropZoneHandlerScript.removeCorrectDrop();
    }
}
