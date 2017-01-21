using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player {

	public class Player : MonoBehaviour {

		public float speed = 3;
		public float maxSpeed = 5;

		private bool isJumping = false;

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			if(this.getLeftPressed()){
				this.moveHorizontal(-1f);
			} else if(this.getRightPressed()) {
				this.moveHorizontal(1f);
			}

			if(this.getJumpPressed() && this.getIsJumping()){
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

		//getters
		public bool getIsJumping() {
			return this.isJumping;
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