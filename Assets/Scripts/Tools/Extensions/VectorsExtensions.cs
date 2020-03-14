using UnityEngine;

namespace Tools.Extensions {
	public static class VectorsExtensions {
		public static Vector2 ToV2FromXZ(this Vector3 vector3){
			return new Vector2(vector3.x, vector3.z);
		}

		public static Vector3 ToV3FromX0Y(this Vector2 vector3){
			return new Vector3(vector3.x, 0.0f, vector3.y);
		}
	}
}