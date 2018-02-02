using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour {
    public bool hasObject = false;
    private DropZoneHandler dropZoneHandlerScript;

    private void Start()
    {
        dropZoneHandlerScript = transform.parent.GetComponent<DropZoneHandler>();
    }

    private void OnCollisionEnter(Collision other)
    {
        hasObject = true;
        if (other.gameObject.tag.Contains("Weight"))
        {
            dropZoneHandlerScript.addCorrectDrop();
        }
        
    }

    private void OnCollisionExit(Collision other)
    {
        hasObject = false;
        if (other.gameObject.tag.Contains("Weight"))
        {
            dropZoneHandlerScript.removeCorrectDrop();
        }
    }
}
