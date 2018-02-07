using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour {
    public bool hasObject = false;
    private DropZoneHandler dropZoneHandlerScript;
    public float placedWeight;
    private bool triggered = false;

    private void Start()
    {
        dropZoneHandlerScript = transform.parent.GetComponent<DropZoneHandler>();
    }
    public void Update()
    {
        //triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        triggered = true;
        hasObject = true;
        Transform dropZone = other.gameObject.transform.parent;
        if (dropZone.tag.Contains("Weight"))
        {
            placedWeight = dropZone.GetComponent<Rigidbody>().mass;
            dropZoneHandlerScript.AddValidDrop();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!triggered) return;
        triggered = false;
        hasObject = false;
        Transform dropZone = other.gameObject.transform.parent;
        if (dropZone.tag.Contains("Weight"))
        {
            placedWeight = 0;
            dropZoneHandlerScript.RemoveValidDrop();
        }
    }
}
