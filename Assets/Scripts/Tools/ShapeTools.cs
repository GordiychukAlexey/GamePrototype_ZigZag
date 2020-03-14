using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Tools {
	public static class ShapeTools {
		public static bool RectContains(Vector2 rectPosition, Vector2 rectSize, Vector2 point) =>
			//глючил float
//			point.x >= rectPosition.x
//			&& point.x <= rectPosition.x + rectSize.x
//			&& point.y >= rectPosition.y
//			&& point.y <= rectPosition.y + rectSize.y;
			(point.x > rectPosition.x || FloatComparer.s_ComparerWithDefaultTolerance.Equals(point.x, rectPosition.x))
			&& (point.x < rectPosition.x + rectSize.x || FloatComparer.s_ComparerWithDefaultTolerance.Equals(point.x, rectPosition.x + rectSize.x))
			&& (point.y > rectPosition.y || FloatComparer.s_ComparerWithDefaultTolerance.Equals(point.x, rectPosition.y))
			&& (point.y < rectPosition.y + rectSize.y || FloatComparer.s_ComparerWithDefaultTolerance.Equals(point.x, rectPosition.y + rectSize.y));

		public static bool IsIntersectionOf_Circle_Circle(Vector2 c1Position, float c1Radius, Vector2 c2Position, float c2Radius) =>
			Vector2.Distance(c1Position, c2Position) <= c1Radius + c2Radius;
	}
}