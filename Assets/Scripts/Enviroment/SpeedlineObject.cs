using UnityEngine;
using System.Collections;

public class SpeedlineObject : MonoBehaviour {

    public SpeedlineGenerator myParent;
    public float reduceSpeed = .2f;

    public SpeedlineObject(SpeedlineGenerator parent)
    {
        myParent = parent;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        rotate();
        reduce();
	}

    public void rotate()
    {
        Quaternion rotation = Quaternion.LookRotation(myParent.transform.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles);
    }

    public void reduce()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - reduceSpeed, transform.localScale.z);
        if (transform.localScale.y <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
