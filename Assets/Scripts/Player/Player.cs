using UnityEngine;
using System.Collections;
using Assets.Scripts.Player;

namespace Assets.Scripts.Player {

	public class Player : MonoBehaviour {

		private Vector2 movement = new Vector2(0, 0);

		public float acceleration = 0.8f;
		public float decceleration = 12f;
		public float maxSpeed = 6f;

		public float jumpSpeed = 5f;
		private float jumpStartTime = 0f;

		public float rotationSpeed = 180;
		public float maxRotation = 15;

		public bool useAcceleration = true;//should the player slide when moving
		public bool shouldBob = true;//should the player bob in the water

		private bool isJumping = false; //is player jumping
		// private bool isOnSurface = true; //is player on surface of water
		private bool isPlayerMoving = false; //is player asking the character to move

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			//Horizontal Movement
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
					if(Mathf.Abs(this.movement.x) < 0.5f) {
						this.movement.x = 0f;
					}

					if(Mathf.Abs(this.movement.x) > 0){
						var deceVal = (this.movement.x < 0 ? this.decceleration : -this.decceleration) * Time.deltaTime;
						this.movement.x += deceVal;
					}
				} else {
					this.movement.x = 0;
				}
				this.isPlayerMoving = false;
			}
			//Vertical Movement
			if(this.getJumpPressed() && !this.getAirborne() && this.getIsOnSurface()){
				this.actionJump();
			}
			if(this.isJumping) {
				this.handleJumping();
			}

			//Passive Movement
			if(this.getShouldBob()) {
				// TODO properly loop animation
				float percentInRotation = PlayerUtil.getPercentInRotation(transform.localEulerAngles.z, this.maxRotation);

				float leanCurve = Mathf.PingPong(Time.time, 2f) - 1f;
				float leanOffset = percentInRotation;
				float leanValue = leanCurve - leanOffset;

				this.lean(leanValue);
			} else {
				//rotate back towards center
				float percentInRotation = PlayerUtil.getPercentInRotation(transform.localEulerAngles.z, this.maxRotation);
				this.lean(-percentInRotation);
			}

			//constantly move the character
			this.handleMovement();
		}

		// Handlers
		private void handlePlayerMove(float magnitude) {
			if(this.useAcceleration) {
				this.movement.x = this.movement.x + this.acceleration * magnitude;
			} else {
				this.movement.x = this.maxSpeed * magnitude;
			}
			if(this.movement.x > this.maxSpeed) {
				this.movement.x = this.maxSpeed * magnitude;
			}
		}
		// Actions
		private void handleMovement() {
			if(this.movement.x > this.maxSpeed) {
				this.movement.x = this.maxSpeed;
			} else if(this.movement.x < -this.maxSpeed) {
				this.movement.x = -this.maxSpeed;
			}

			if(transform.position.y > PlayerUtil.surfacePos) {
				this.movement.y -= 0.5f;
			} else if(transform.position.y <= PlayerUtil.surfacePos && (Time.time - this.jumpStartTime) > 0.5f) {
				this.movement.y = 0;
				transform.position = new Vector2(transform.position.x, PlayerUtil.surfacePos);
			}

			transform.Translate(this.movement * Time.deltaTime, Space.World);
		}

		private void handleJumping() {
			float timeSinceJumping = (Time.time - this.jumpStartTime);

			if(this.getIsOnSurface() && timeSinceJumping > 0.5f){
				this.isJumping = false;
			}
		}

		private void actionJump() {
			if(!this.getAirborne()) {
				this.isJumping = true;
				this.jumpStartTime = Time.time;
				this.movement.y = this.jumpSpeed;
			}
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
		public bool getAirborne() {
			return this.isJumping;
		}

		public bool getIsOnSurface() {
			if(transform.position.y <= PlayerUtil.surfacePos){
				return true;
			} else {
				return false;
			}
			// return this.isOnSurface;
		}

		public bool getShouldBob() {
			return this.shouldBob && !this.getAirborne() && !this.isPlayerMoving;
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