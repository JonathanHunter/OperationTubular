using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Util;

namespace Assets.Scripts.Util
{
	public class ZLayer : MonoBehaviour {
		public static float CameraZ = -30;
		public static float WaterZ = -10; //this is the benchmark

		public static float PlayerZ = -20;
		public static float UnderwaterPlayerZ = -9;

		public static float CrosshairZ = -21;
		public static float PlayerProjectileZ = -15;

		public static float EnemyZ = -9;
		public static float EnemyProjectileZ = -13;
		public static float DangerIndicatorZ = -21;

		public void start() {
			//unit test of sorts to make sure z positions are correct
			if(!(ZLayer.CameraZ > ZLayer.WaterZ) && !(ZLayer.CameraZ > ZLayer.PlayerZ) && !(ZLayer.CameraZ > ZLayer.CrosshairZ)  && !(ZLayer.CameraZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Camera should be above everything");
			}
			if(!(ZLayer.PlayerZ > ZLayer.WaterZ)) {
				print("Z WARNING: Player should be above water");
			}
			if(!(ZLayer.UnderwaterPlayerZ < ZLayer.WaterZ)) {
				print("Z WARNING: underwater player");
			}
			if(!(ZLayer.CrosshairZ > ZLayer.PlayerZ)) {
				print("Z WARNING: Crosshair should be in front of Player");
			}
			if(!(ZLayer.CrosshairZ > ZLayer.PlayerProjectileZ)) {
				print("Z WARNING: Crosshair should be in front of Player projectiles");
			}
			if(!(ZLayer.PlayerProjectileZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Player Projectiles should be in front of Enemy");
			}
			if(!(ZLayer.PlayerZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Player should be in front of Enemy");
			}
			if(!(ZLayer.PlayerProjectileZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Player Projectiles should be in front of Enemy");
			}
			if(!(ZLayer.EnemyProjectileZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Enemy Projectiles should be in front of Enemy");
			}
			if(!(ZLayer.DangerIndicatorZ > ZLayer.EnemyZ)) {
				print("Z WARNING: Danger Indicator should be in front of Enemy");
			}
			if(!(ZLayer.DangerIndicatorZ > ZLayer.PlayerZ)) {
				print("Z WARNING: Danger Indicator should be in front of Player");
			}
		}
    }
}