using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player {

	public class Player : MonoBehaviour {

		public float speed = 3;
		public float maxSpeed = 5;

		public float rotationSpeed = 50;
		private float maxRotation = 20;

		private bool isJumping = false;
		private bool isOnSurface = false;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(this.getLeftPressed()){
				this.moveHorizontal(-1f);
				this.lean(-1f);
			} else if(this.getRightPressed()) {
				this.moveHorizontal(1f);
				this.lean(1f);
			}

			if(this.getJumpPressed() && !this.getIsJumping()){
				this.actionJump();
			}
		}

		// Actions
		private void moveHorizontal(float magnitude) {
			transform.Translate(Vector3.right * magnitude * this.speed * Time.deltaTime);
		}

		private void actionJump() {
			//todo
		}

		//todo vector3
		private void lean(float magnitude) {
			Vector3 currentRotation = transform.eulerAngles;
			float rotateAttempt = currentRotation.z + magnitude * this.rotationSpeed * Time.deltaTime;
			transform.eulerAngles = new Vector3(currentRotation.x, currentRotation.y, rotateAttempt);
		}

		//getters
		public bool getIsJumping() {
			return this.isJumping;
		}

		public bool getIsOnSurface() {
			return this.isOnSurface;
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