using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour {

	public enum ButtonDirection
        {
            autodetect,
            x,
            y,
            z,
            negX,
            negY,
            negZ
        }
	public ButtonDirection direction = ButtonDirection.y;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
