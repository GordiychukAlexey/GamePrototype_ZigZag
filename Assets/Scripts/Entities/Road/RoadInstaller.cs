using System;
using Entities.Road.RoadBuilder;
using Main;
using UnityEngine;
using Zenject;

namespace Entities.Road {
	public class RoadInstaller : MonoInstaller {
		[Inject] private readonly MainSettings mainSettings;
		[Inject] private readonly Road.Settings roadSettings;
		[Inject] private readonly RoadBuilderSettingsGroups roadBuilderSettingsGroups;

		public override void InstallBindings(){
			Container.BindFactory<Road, RoadBuilder.RoadBuilder, RoadBuilder.RoadBuilder.Factory>().AsTransient().NonLazy();

			Container.BindFactory<Tile, Tile.Factory>()
					 .FromPoolableMemoryPool<Tile, Tile.Pool>(poolBinder =>
						 poolBinder
							 .WithInitialSize(roadBuilderSettingsGroups.GetObject(mainSettings.OverallDifficult).RoadSize * roadSettings.MaxLength)
							 .FromComponentInNewPrefab(roadSettings.tilePrefab)
							 .WithGameObjectName("Tile")
							 .UnderTransformGroup("Tiles"))
					 .NonLazy();


			Container.BindFactory<Crystal.Crystal, Crystal.Crystal.Factory>()
					 .FromPoolableMemoryPool<Crystal.Crystal, Crystal.Crystal.Pool>(poolBinder =>
						 poolBinder
							 .WithInitialSize(Math.Max(1, Mathf.CeilToInt(roadSettings.MaxLength / 5.0f))) //#5245
							 .FromComponentInNewPrefab(roadSettings.crystalPrefab)
							 .WithGameObjectName("Crystal")
							 .UnderTransformGroup("Crystals"))
					 .NonLazy();

			Container.BindFactory<RoadPart, RoadPart.Factory>().AsSingle().NonLazy();

			Container.Bind<CrystalSpawnStrategyFactory>().AsSingle().NonLazy();
		}
	}
}