using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateObjects : MonoBehaviour {

    protected GameObject weight;
    public GameObject prefab;
    protected float[] weights;
    protected float lowestPairNum = 500;
    private StatManager statManager;

    // Use this for initialization
    void Start () {
        statManager = GameObject.Find("AppManager").transform.GetComponent<StatManager>();
    }

	// Update is called once per frame
	void Update () {

	}
    void Awake()
    {
        statManager = GameObject.Find("AppManager").transform.GetComponent<StatManager>();
    }
    public virtual void Create() {
    }

    public virtual void UpdatePairNum(float p)
    {
        if (p < lowestPairNum)
        {
            lowestPairNum = p;
        }
    }

    public void SetPairNum()
    {
        statManager.SetPair((float)lowestPairNum);
    }
}
