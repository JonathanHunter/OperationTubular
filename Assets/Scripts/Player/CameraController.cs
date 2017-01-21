using UnityEngine;
using System.Collections;
using Assets.Scripts.Manager;

public class CameraController : MonoBehaviour {

	private Vector3 defaultPos = new Vector3(0, 0, -5);

	public GameObject[] trackables; //things for camera to track

	public float trackingSpeed = 2f;

	public bool useBounds = true; //should camera be constrained
	private float windowWidth = 6;
	public Vector2 boundsPaddingX = new Vector2(-4, 4);//minimum left, maximum right
	public Vector2 boundsPaddingY = new Vector2(0.5f, 1);//minimum down, maximum up

	private float verticalOffset = 5f;

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
				if(this.trackables[i] != null){
					sumTrackables += this.trackables[i].transform.position;
				}
			}

			Vector3 newPos = sumTrackables / (trackableCount + 1);

			if(this.useBounds) {
				//horizontal
				Vector2 myBoundsMax = new Vector2(
					GameManager.xBounds.x - (this.windowWidth / 2) + this.boundsPaddingX.x,
					GameManager.xBounds.y + (this.windowWidth / 2) + this.boundsPaddingX.y);

				if(newPos.x > myBoundsMax.y) {
					newPos.x = myBoundsMax.y;
				} else if(newPos.x < myBoundsMax.x) {
					newPos.x = myBoundsMax.x;
				}
				//vertical - inverse
				Vector2 myBoundsMaxY = new Vector2(
					GameManager.yBounds.x - this.verticalOffset - this.boundsPaddingY.x,
					GameManager.yBounds.y + this.verticalOffset + this.boundsPaddingY.y);

				if(newPos.y < myBoundsMaxY.x) {
					newPos.y = myBoundsMaxY.x;
				} else if(newPos.y > myBoundsMaxY.y) {
					newPos.y = myBoundsMaxY.y;
				}
			}

			newPos = new Vector3(newPos.x, newPos.y, defaultPos.z);

			transform.position = Vector3.Lerp(transform.position, newPos, trackingSpeed * Time.deltaTime);
		}	
	}
}
