using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player {
	class PlayerUtil : MonoBehaviour {
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
	}
}
