using System;
using System.Collections.Generic;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Entities.Road.RoadBuilder {
	public class RoadBuilder {
		[Inject] private readonly Road.Settings roadSettings;
		[Inject] private readonly RoadPart.Factory roadPartFactory;
		[Inject] private readonly Tile.Factory tileFactory;
		[Inject] private readonly Crystal.Crystal.Factory crystalFactory;
		[Inject] private readonly RoadBuilderSettingsGroups roadBuilderSettingsGroups;
		[Inject] private readonly Road road;

		private Vector2 lastRoadPartPosition;
		private Road.AbsoluteMoveDirection lastAbsoluteMoveDirection;

		public void CreateStartingPad(){
			List<Vector2[]> startingPadRoadPartsTilesPlacements = new List<Vector2[]>{
				new[]{
					new Vector2(-1.0f, -1.0f),
					new Vector2(0.0f, -1.0f),
					new Vector2(1.0f, -1.0f)
				},
				new[]{
					new Vector2(-1.0f, 0.0f),
					new Vector2(0.0f, 0.0f),
					new Vector2(1.0f, 0.0f)
				},
				new[]{
					new Vector2(-1.0f, 1.0f),
					new Vector2(0.0f, 1.0f),
					new Vector2(1.0f, 1.0f)
				}
			};

			foreach (Vector2[] roadPartTilesPlacement in startingPadRoadPartsTilesPlacements){
				Transform roadPartRoot = new GameObject("RoadPart(StartingPad)").transform;
				roadPartRoot.parent                  = road.transform;
				roadPartRoot.transform.localPosition = Vector3.zero;
				roadPartRoot.transform.localRotation = Quaternion.identity;

				RoadPart newRoadPart = roadPartFactory.Create();
				newRoadPart.Root = roadPartRoot;

				foreach (Vector2 tilePosition in roadPartTilesPlacement){
					Tile newTile = tileFactory.Create();
					newTile.transform.parent        = roadPartRoot;
					newTile.transform.rotation      = road.transform.rotation;
					newTile.transform.localPosition = tilePosition.ToV3FromX0Y();

					newRoadPart.Tiles.Add(newTile);
				}

				AddRoadPart(newRoadPart);
			}

			lastRoadPartPosition      = new Vector2(0.0f, 1.5f + roadBuilderSettingsGroups.GetObject().StartPointShift);
			lastAbsoluteMoveDirection = Road.AbsoluteMoveDirection.Forward;
		}


		public RoadPart CreateNextRoadPart(bool isSwitchDirection, bool needCrystal){
			Road.AbsoluteMoveDirection newAbsoluteMoveDirection = lastAbsoluteMoveDirection;
			Road.RelativeMoveDirection newRelativeMoveDirection = Road.RelativeMoveDirection.Forward;
			if (isSwitchDirection){
				switch (lastAbsoluteMoveDirection){
					case Road.AbsoluteMoveDirection.Forward:
						newAbsoluteMoveDirection = Road.AbsoluteMoveDirection.Right;
						newRelativeMoveDirection = Road.RelativeMoveDirection.Right;
						break;
					case Road.AbsoluteMoveDirection.Right:
						newAbsoluteMoveDirection = Road.AbsoluteMoveDirection.Forward;
						newRelativeMoveDirection = Road.RelativeMoveDirection.Left;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			Vector2    newRoadPartPositionShift;
			Quaternion newRoadPartLocalRotationQuaternion;
			switch (newAbsoluteMoveDirection){
				case Road.AbsoluteMoveDirection.Forward:
					newRoadPartPositionShift           = Vector2.up;
					newRoadPartLocalRotationQuaternion = Quaternion.identity;
					break;
				case Road.AbsoluteMoveDirection.Right:
					newRoadPartPositionShift           = Vector2.right;
					newRoadPartLocalRotationQuaternion = Quaternion.Euler(0.0f, 90.0f, 0.0f);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			Vector2 newRoadPartLocalPosition = lastRoadPartPosition + newRoadPartPositionShift;

			RoadPart newRoadPart = roadPartFactory.Create();

			Vector2[] tilesPlacement = roadBuilderSettingsGroups.GetTilesPlacement(newRelativeMoveDirection);

			Transform roadPartRoot = new GameObject($"RoadPart({newRelativeMoveDirection})").transform;
			roadPartRoot.parent                  = road.transform;
			roadPartRoot.transform.localPosition = newRoadPartLocalPosition.ToV3FromX0Y();
			roadPartRoot.transform.localRotation = newRoadPartLocalRotationQuaternion;

			newRoadPart.Root = roadPartRoot;

			foreach (Vector2 tilePosition in tilesPlacement){
				Tile newTile = tileFactory.Create();
				newTile.transform.parent        = roadPartRoot;
				newTile.transform.localPosition = tilePosition.ToV3FromX0Y();
				newTile.transform.rotation      = road.transform.rotation;

				newRoadPart.Tiles.Add(newTile);
			}

			lastRoadPartPosition      = newRoadPartLocalPosition;
			lastAbsoluteMoveDirection = newAbsoluteMoveDirection;

			if (needCrystal){
				Crystal.Crystal newCrystal = crystalFactory.Create();
				newCrystal.transform.parent        = roadPartRoot;
				newCrystal.transform.localPosition = Vector3.forward * roadBuilderSettingsGroups.GetObject().CrystalPositionShift;
				newCrystal.transform.rotation      = road.transform.rotation;
				newCrystal.transform.localScale    = Vector3.one;

				newRoadPart.Crystal = newCrystal;
			}

			AddRoadPart(newRoadPart);

			return newRoadPart;
		}

		private void AddRoadPart(RoadPart roadPart){
			while (road.RoadParts.Count >= roadSettings.MaxLength){
				RoadPart roadPartForDestroy = road.RoadParts.Dequeue();
				roadPartForDestroy.Destroy();
			}

			road.RoadParts.Enqueue(roadPart);
		}

		[Serializable]
		public class Settings {
			public int RoadSize;

			[Tooltip("Для правильного расположения новых тайлов")]
			public float StartPointShift;

			public float CrystalPositionShift;
			public Vector2[] ForwardMoveTilesPlacement;
			public Vector2[] RightMoveTilesPlacement;
		}

		public class Factory : PlaceholderFactory<Road, RoadBuilder> { }
	}
}