using UnityEngine;
using System.Collections;

public class SpeedlineGenerator : MonoBehaviour {

    public SpeedlineObject linePic;
    private SpeedlineObject temp;

    public bool shouldGenerate = false;

	// Use this for initialization
	void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
        if(shouldGenerate)
             generateLine();
    }

    public void generateLine()
    {
        temp = Instantiate<SpeedlineObject>(linePic);
        linePic.myParent = this;
        temp.transform.position = new Vector3(Random.Range(-6, 6), Random.Range(-6, 6), Random.Range(-6, 6));
    }
}
