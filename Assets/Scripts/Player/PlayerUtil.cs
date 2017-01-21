using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Player {
	class PlayerUtil : MonoBehaviour {
		/*
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
	}
}
