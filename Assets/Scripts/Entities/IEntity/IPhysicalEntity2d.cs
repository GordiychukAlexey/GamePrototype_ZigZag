using Tools;
using UnityEngine;

namespace Entities.IEntity {
	public interface IPhysicalEntity2d : IShapeCollisionDetection {
		Vector2 Position{ get; set; }
		Vector2 Rotation{ get; set; }
		Vector2 Scale{ get; set; }
	}
}