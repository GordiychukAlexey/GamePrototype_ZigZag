using Entities.IEntity;

namespace Entities.Ball {
	public interface IBall : IPhysicalEntity2d, IStateResetable {
		void Move(bool isSwitchDirection, float distance);
	}
}