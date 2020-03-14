using Entities.Ball;
using Entities.Road;
using Main;
using UnityEngine;
using Zenject;

namespace Installers {
	[CreateAssetMenu(menuName = "Game/Game Settings")]
	public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller> {
		public MainSettings Main;
		public Road.Settings Road;
		public Ball.Settings Ball;

		public override void InstallBindings(){
			Container.BindInstance(Main).AsSingle();
			Container.BindInstance(Road).AsSingle();
			Container.BindInstance(Ball).AsSingle();
		}
	}
}