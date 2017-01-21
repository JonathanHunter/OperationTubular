using UnityEngine;
using System.Collections;
using Assets.Scripts.Util;

namespace Assets.Scripts.Player {
	class PlayerUtil : MonoBehaviour {
		public static float surfacePos = -2.5f; //TODO: something smarter than this

		public static Vector3 defaultPlayerSpawn = new Vector3(-8, surfacePos, -1);
		public static Vector3 defaultCrosshairSpawn = new Vector3(1, 4, -1);

		public static Vector2 playerSize = new Vector2(2f, 3.2f); //collider - total width, total height


		/*
			creates a vector3 of which direction it came from
			@param me (Vector3) - player persepctive position
			@param you (Vector3) - thing collided into position
			@return (Vector3) - (x: -1 left to 1 right,
								 y: -1 top to 1 bottom,
								 z: -1 towards camera to 1 away camera )
		*/
		public static Vector3 getCollisionDirection(Vector3 me, Vector3 you) {
			Vector3 dir = Vector3.zero;
			if(PlayerUtil.nearZero(me.x - you.x, 0.3f)) {
				dir.x = 0;
			} else {
				dir.x = me.x < you.x ? -1 : 1;
			}

			if(PlayerUtil.nearZero(me.y - you.y, 0.3f)) {
				dir.y = 0;
			} else {
				dir.y = me.y < you.y ? -1 : 1;
			}
			//TODO: z direction
			return dir;
		}
		/*
			@param player (int) - player num
			@return Vector2 - the values of the left joystick
		*/
		public static Vector2 getLeftJoystick(int player) {
			switch(player) {
				case 1:
					return new Vector2(InputManager.instance.lStickX1, InputManager.instance.lStickY1);
				case 2:
					return new Vector2(InputManager.instance.lStickX2, InputManager.instance.lStickY2);
			}
			return new Vector2(0, 0);
		}
		/*
			@param player (int) - player num
			@return Vector2 - the values of the right joystick
		*/
		public static Vector2 getRightJoystick(int player) {
			switch(player) {
				case 1:
					return new Vector2(InputManager.instance.rStickX1, InputManager.instance.rStickY1);
				case 2:
					return new Vector2(InputManager.instance.rStickX2, InputManager.instance.rStickY2);
			}
			return new Vector2(0, 0);
		}
		/*
			@param player (int) - player num
			@return Vector2 - the values of the triggers - 
							1 is pressed
			 				0 is not pressed
		*/
		public static Vector2 getControllerTriggers(int player) {
			switch(player) {
				case 1:
					return new Vector2(InputManager.instance.ltPressed1 ? 1f : 0f, 
									   InputManager.instance.rtPressed1 ? 1f : 0f);
				case 2:
					return new Vector2(InputManager.instance.ltPressed2 ? 1f : 0f, 
									   InputManager.instance.rtPressed2 ? 1f : 0f);					
			}
			return new Vector2(0, 0);
		}

		/*
			returns unity values of rotation
			@params currentRotation - pass in the z rotation
			@return float - returns rotation assuming 0 is straight and 
							left is negative and 
							right is positive

		*/
		public static float getRelativeRotation(float currentRotation) {
			if(currentRotation > 180) {//leaning right
				return Mathf.Abs(360 - currentRotation);
			} else if (currentRotation < 180) {//leaning left
				return -Mathf.Abs(currentRotation);
			}
			return 0;
		}

		/*
			returns how far into the bobbing rotation it is in
			@params currentRotation - pass in the z rotation
			@params maxRotation - pass in the max rotation
			@return float - returns between -1 and 1

		*/
		public static float getPercentInRotation(float currentRotation, float maxRotation) {
			float rotationValue = PlayerUtil.getRelativeRotation(currentRotation);
			float direction = rotationValue > 0 ? 1 : -1;
			float absPercent = Mathf.Abs(rotationValue) / maxRotation;
			return absPercent * direction;
		}


        public static bool nearZero(float i, float n)
        {
            return -n <= i && i <= n;
        }
	}
}
