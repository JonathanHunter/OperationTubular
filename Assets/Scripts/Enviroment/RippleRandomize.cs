using UnityEngine;
using System.Collections;

public class RippleRandomize : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void randomizeLoc()
    {
        transform.position = new Vector3(Random.Range(-9, 9), transform.position.y, transform.position.z);
    }
}
