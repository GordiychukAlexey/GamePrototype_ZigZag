using Entities.Ball;
using Entities.Road;
using Entities.Road.RoadBuilder;
using Main;
using UI;
using UnityEngine;
using Zenject;

namespace Installers {
	public class GameInstaller : MonoInstaller {
		[SerializeField] private Transform worldObjectRootTransform;
		[SerializeField] private Road roadPrefab;
		[SerializeField] private Ball ballPrefab;

		public override void InstallBindings(){
			Container.Bind<Transform>().WithId(MainIdentifiers.WorldObjectRootTransform).FromInstance(worldObjectRootTransform);
			Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();

			Container.Bind<IInGameUI>().To<InGameUI>().FromResolve().NonLazy();

			Container.BindInterfacesAndSelfTo<InputManager>().AsSingle().NonLazy();

			Container.BindFactory<RoadBuilderSettingsGroups, RoadBuilderSettingsGroups.Factory>()
					 .AsSingle();

			Container.Bind<RoadBuilderSettingsGroups>()
					 .FromFactory<ThreeLevelRBSGFactory>()
					 .AsSingle()
					 .WhenInjectedInto(
						 typeof(RoadInstaller),
						 typeof(RoadBuilder),
						 typeof(Road)
					 );

			{
				SignalBusInstaller.Install(Container);
				Container.DeclareSignal<SwitchPauseSignal>();
				Container.DeclareSignal<GameOverSignal>();
				Container.DeclareSignal<RestartGameSignal>();

				Container.BindSignal<SwitchPauseSignal>().ToMethod<GameController>(x => x.SwitchPauseCallback).FromResolve();
				Container.BindSignal<GameOverSignal>().ToMethod<InGameUI>(x => x.GameOverCallback).FromResolve();
				Container.BindSignal<RestartGameSignal>().ToMethod<GameController>(x => x.ResetState).FromResolve();
			}

			Container.Bind<Road>().FromComponentInNewPrefab(roadPrefab).WithGameObjectName("Road").UnderTransform(worldObjectRootTransform)
					 .AsSingle().NonLazy();
			Container.Bind<IBall>().FromComponentInNewPrefab(ballPrefab).WithGameObjectName("Ball").UnderTransform(worldObjectRootTransform)
					 .AsSingle().NonLazy();
		}
	}

	enum MainIdentifiers {
		WorldObjectRootTransform,
	}
}