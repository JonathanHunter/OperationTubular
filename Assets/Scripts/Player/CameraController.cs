using UnityEngine;
using System.Collections;
using Assets.Scripts.Manager;

public class CameraController : MonoBehaviour {

	private Vector3 defaultPos = new Vector3(0, 0, -5);

	public GameObject[] trackables; //things for camera to track

	public float trackingSpeed = 2f;

	public bool useBounds = true; //should camera be constrained
	private float windowWidth = 6;
	public float boundsPadding = 4;

	// Use this for initialization
	void Start () {
		transform.position = defaultPos;

	}

	// Update is called once per frame
	void Update () {
		int trackableCount = this.trackables.Length;

		if(trackableCount > 0){
			Vector3 sumTrackables = defaultPos;
			for(int i=0; i<trackableCount; i++){
				sumTrackables += this.trackables[0].transform.position;
			}

			Vector3 newPos = sumTrackables / (trackableCount + 1);

			if(this.useBounds) {
				Vector2 myBoundsMax = new Vector2(
					GameManager.xBounds.y - (this.windowWidth / 2) - this.boundsPadding,
					GameManager.xBounds.x + (this.windowWidth / 2) + this.boundsPadding);

				if(newPos.x > myBoundsMax.x) {
					newPos.x = myBoundsMax.x;
				} else if(newPos.x < myBoundsMax.y) {
					newPos.x = myBoundsMax.y;
				}
				//TODO: y bounds check
			}

			newPos = new Vector3(newPos.x, defaultPos.y, defaultPos.z);

			transform.position = Vector3.Lerp(transform.position, newPos, trackingSpeed * Time.deltaTime);
		}	
	}
}
