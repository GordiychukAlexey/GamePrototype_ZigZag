using System;
using System.Collections.Generic;
using System.Linq;
using Entities.IEntity;
using Entities.Road.RoadBuilder;
using Installers;
using Main;
using Tools;
using Tools.Extensions;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Entities.Road {
	public class Road : MonoBehaviour, IPhysicalEntity2d {
		public Queue<RoadPart> RoadParts{ get; } = new Queue<RoadPart>();

		[Inject(Id = MainIdentifiers.WorldObjectRootTransform)]
		private readonly Transform worldObjectRootTransform;

		[Inject] private readonly MainSettings mainSettings;
		[Inject] private readonly Settings roadSettings;
		[Inject] private readonly RoadBuilder.RoadBuilder.Factory roadBuilderFactory;
		[Inject] private readonly CrystalSpawnStrategyFactory crystalSpawnStrategyFactory;
		[Inject] private readonly RoadBuilderSettingsGroups roadBuilderSettingsGroups;

		public Vector2 Position{
			get => worldObjectRootTransform.InverseTransformPoint(transform.position).ToV2FromXZ();
			set => transform.position = worldObjectRootTransform.TransformPoint(value.ToV3FromX0Y());
		}

		public Vector2 Rotation{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public Vector2 Scale{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		private RoadBuilder.RoadBuilder roadBuilder;
		private ICrystalSpawnStrategy crystalSpawnStrategy;

		public IEnumerable<Tile> AllTiles() => RoadParts.SelectMany(tilesGroup => tilesGroup.Tiles.Select(tile => tile));
		public bool DetectCollision(IShapeCollisionDetection second) => AllTiles().Any(tile => tile.DetectCollision(second));

		private void Awake(){
			roadBuilder = roadBuilderFactory.Create(this);

			ResetState();
		}

		public void ResetState(){
			foreach (RoadPart roadPart in RoadParts){
				roadPart.Dispose();
			}

			RoadParts.Clear();

			crystalSpawnStrategy = crystalSpawnStrategyFactory.Create();
		}

		public void Prepare(){
			roadBuilder.CreateStartingPad();

			{
				for (int i = 1; i <= roadBuilderSettingsGroups.GetObject(mainSettings.OverallDifficult).RoadSize; i++){
					CreateNextRoadPart(false);
				}

				int startingRoadSize = roadSettings.MaxLength - 3; //3-длина StartingPad
				for (int i = roadBuilderSettingsGroups.GetObject(mainSettings.OverallDifficult).RoadSize + 1; i <= startingRoadSize; i++){
					CreateNextRoadPart(Random.value < roadSettings.RotateChance);
				}
			}
		}

		private void CreateNextRoadPart(bool isSwitchDirection){
			crystalSpawnStrategy.MoveNext();
			roadBuilder.CreateNextRoadPart(isSwitchDirection, crystalSpawnStrategy.Current);
		}

		public enum AbsoluteMoveDirection {
			Forward,
			Right
		}

		public enum RelativeMoveDirection {
			Forward,
			Right,
			Left
		}

		public enum CrystalSpawnMethod {
			Random,
			InOrder,
		}

		[Serializable]
		public class Settings {
			public Tile tilePrefab;
			public Crystal.Crystal crystalPrefab;
			[Range(6, 10)] public int MaxLength = 10;
			[Range(0.0f, 1.0f)] public float RotateChance = 0.4f;
			public CrystalSpawnMethod CrystalSpawnMethod;
		}

		public void CreateNextRoadPart(){
			CreateNextRoadPart(Random.value < roadSettings.RotateChance);
		}
	}
}