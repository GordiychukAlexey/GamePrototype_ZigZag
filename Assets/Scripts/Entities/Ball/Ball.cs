using System;
using Installers;
using Tools;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Entities.Ball {
	public class Ball : MonoBehaviour, IBall {
		[SerializeField] private float radius;

		[Inject(Id = MainIdentifiers.WorldObjectRootTransform)]
		private readonly Transform worldObjectRootTransform;

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

		private IShapeCollisionDetection collisionDetection;
		private Road.Road.AbsoluteMoveDirection lastAbsoluteMoveDirection;

		public bool DetectCollision(IShapeCollisionDetection second) => collisionDetection.DetectCollision(second);

		private void Awake(){
			collisionDetection = new CircleShapeCollisionDetection(
				() => worldObjectRootTransform.InverseTransformPoint(transform.position).ToV2FromXZ(),
				() => radius);
		}

		public void ResetState(){
			lastAbsoluteMoveDirection = Road.Road.AbsoluteMoveDirection.Forward;
		}

		public void Move(bool isSwitchDirection, float distance){
			Road.Road.AbsoluteMoveDirection newAbsoluteMoveDirection = lastAbsoluteMoveDirection;
			if (isSwitchDirection){
				switch (lastAbsoluteMoveDirection){
					case Road.Road.AbsoluteMoveDirection.Forward:
						newAbsoluteMoveDirection = Road.Road.AbsoluteMoveDirection.Right;
						break;
					case Road.Road.AbsoluteMoveDirection.Right:
						newAbsoluteMoveDirection = Road.Road.AbsoluteMoveDirection.Forward;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			Vector2 newRoadPartPositionShift;
			switch (newAbsoluteMoveDirection){
				case Road.Road.AbsoluteMoveDirection.Forward:
					newRoadPartPositionShift = Vector2.up * distance;
					break;
				case Road.Road.AbsoluteMoveDirection.Right:
					newRoadPartPositionShift = Vector2.right * distance;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			lastAbsoluteMoveDirection =  newAbsoluteMoveDirection;
			Position                  += newRoadPartPositionShift;
		}

		[Serializable]
		public class Settings {
			[Range(1.0f, 6.0f)] public float MovingSpeed;
		}
	}
}