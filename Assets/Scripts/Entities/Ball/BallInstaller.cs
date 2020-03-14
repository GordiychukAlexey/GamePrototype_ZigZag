using Tools;
using Zenject;

namespace Entities.Ball {
	public class BallInstaller : MonoInstaller {
		public override void InstallBindings(){
			Container.Bind<IShapeCollisionDetection>().To<CircleShapeCollisionDetection>().FromNew().AsSingle();
		}
	}
}