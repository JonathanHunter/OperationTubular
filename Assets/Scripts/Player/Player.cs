using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

namespace Assets.Scripts.Player {

	public class Player : MonoBehaviour {

		public float acceleration = 0.5f;
		public float decceleration = 8f;
		public float defaultSpeed = 3f; //if you don't use acceleration
		public float maxSpeed = 5f;
		private float speed = 0;

		public float rotationSpeed = 180;
		public float maxRotation = 15;

		public bool useAcceleration = true;//should the player slide when moving
		public bool shouldBob = true;//should the player bob in the water

		private bool isJumping = false; //is player jumping
		private bool isOnSurface = false; //is player on surface of water
		private bool isPlayerMoving = false; //is player asking the character to move

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(this.getLeftPressed()){
				this.isPlayerMoving = true;
				this.handlePlayerMove(-1f);
				this.lean(-1f);
			} else if(this.getRightPressed()) {
				this.isPlayerMoving = true;
				this.handlePlayerMove(1f);
				this.lean(1f);
			} else {
				if(this.useAcceleration){
					if(Mathf.Abs(this.speed) < 0.8f) {
						this.speed = 0f;
					}

					if(Mathf.Abs(this.speed) > 0){
						var deceVal = (this.speed < 0 ? this.decceleration : -this.decceleration) * Time.deltaTime;
						this.speed += deceVal;
					}
					
				}
				this.isPlayerMoving = false;
			}

			//we're constantly moving the character
			this.moveHorizontal();

			if(this.getJumpPressed() && !this.getIsJumping()){
				this.actionJump();
			}

			if(this.getShouldBob()) {
				float percentInRotation = PlayerUtil.getRelativeRotation(transform.localEulerAngles.z) / this.maxRotation;

				float leanCurve = (Mathf.PingPong(Time.time, 2f) - 1f);
				float leanOffset = percentInRotation;
				float leanValue = leanCurve - leanOffset;

				this.lean(leanValue);
			}
		}

		// Handlers
		private void handlePlayerMove(float magnitude) {
			if(this.useAcceleration) {
				this.speed = this.speed + this.acceleration * magnitude;
			} else {
				this.speed = defaultSpeed * magnitude;
			}
			if(this.speed > this.maxSpeed) {
				this.speed = this.maxSpeed * magnitude;
			}
			this.moveHorizontal();
		}
		// Actions
		private void moveHorizontal() {
			if(this.speed > this.maxSpeed) {
				this.speed = this.maxSpeed;
			} else if(this.speed < -this.maxSpeed) {
				this.speed = -this.maxSpeed;
			}
			// print(this.speed);
			if(this.speed != 0){
				transform.Translate(Vector3.right * this.speed * Time.deltaTime, Space.World);
			}
		}

		private void actionJump() {
			//todo
		}

		//TODO update this to use getRelativeRotation()
		private void lean(float magnitude) {
			float currRot = transform.localEulerAngles.z;
			//moving left: rotate positive, moving right: rotate negative
			float rotateAttempt = currRot - magnitude * this.rotationSpeed * Time.deltaTime;

			float direction = Mathf.Ceil(magnitude);
			float max360 = 360f - this.maxRotation;

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
			return this.shouldBob && !this.getIsJumping() && !this.isPlayerMoving;
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