using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player {

	public class Player : MonoBehaviour {

		public float speed = 3;
		public float maxSpeed = 5;

		public float rotationSpeed = 180;
		public float maxRotation = 15;

		private bool isJumping = false;
		private bool isOnSurface = false;
		private bool isPlayerMoving = false;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(this.getLeftPressed()){
				this.isPlayerMoving = true;
				this.moveHorizontal(-1f);
				this.lean(-1f);
			} else if(this.getRightPressed()) {
				this.isPlayerMoving = true;
				this.moveHorizontal(1f);
				this.lean(1f);
			} else {
				this.isPlayerMoving = false;
			}

			if(this.getJumpPressed() && !this.getIsJumping()){
				this.actionJump();
			}

			if(this.getShouldBob()) {
				float percentInRotation = this.getRelativeRotation() / this.maxRotation;

				float leanCurve = (Mathf.PingPong(Time.time, 2f) - 1f);
				float leanOffset = percentInRotation;
				float leanValue = leanCurve - leanOffset;

				this.lean(leanValue);
			}
		}

		// Actions
		private void moveHorizontal(float magnitude) {
			transform.Translate(Vector3.right * magnitude * this.speed * Time.deltaTime, Space.World);
		}

		private void actionJump() {
			//todo
		}

		//TODO update this to use getRelativeRotation()
		private void lean(float magnitude) {
			float currRot = transform.localEulerAngles.z;
			//moving left: rotate positive
			//moving right: rotate negative
			float rotateAttempt = currRot - magnitude * this.rotationSpeed * Time.deltaTime;

			float direction = Mathf.Ceil(magnitude);
			float max360 = 360f - this.maxRotation;

			// print("current: " + currRot + " max: " + maxRotation);

			if(direction > 0 && (currRot < -this.maxRotation || (currRot > 180 && currRot < max360) )) {//right
				//do not rotate
			} else if(direction < 0 && (currRot < 180 && currRot > this.maxRotation)) {//left
				//do not rotate
			} else {
				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rotateAttempt);
			}
			
		}

		//getters
		public bool getIsJumping() {
			return this.isJumping;
		}

		public bool getIsOnSurface() {
			return this.isOnSurface;
		}

		public bool getShouldBob() {
			return !this.isJumping && !this.isPlayerMoving;
		}

		//returns rotation assuming 0 is straight and left is negative and right is positive
		public float getRelativeRotation() {
			float currRot = transform.localEulerAngles.z;
			if(currRot > 180) {//leaning right
				return Mathf.Abs(360 - currRot);
			} else if (currRot < 180) {//leaning left
				return -Mathf.Abs(currRot);
			}
			return 0;
		}

		//placeholder for input manager telling me what to do
		public bool getLeftPressed() {
			if(Input.GetKey(KeyCode.A)) {
				return true;
			}
			return false;
		}

		public bool getRightPressed() {
			if(Input.GetKey(KeyCode.D)) {
				return true;
			}
			return false;
		}

		public bool getJumpPressed() {
			if(Input.GetKey(KeyCode.W)) {
				return true;
			}
			return false;
		}
	}

}