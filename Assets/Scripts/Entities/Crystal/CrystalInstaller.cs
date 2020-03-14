using Tools;
using Zenject;

namespace Entities.Crystal {
	public class CrystalInstaller : MonoInstaller {
		public override void InstallBindings(){
			Container.Bind<IShapeCollisionDetection>().To<CircleShapeCollisionDetection>().FromNew().AsSingle();
		}
	}
}