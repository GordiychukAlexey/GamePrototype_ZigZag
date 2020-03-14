using System;
using Entities.IEntity;
using Installers;
using Tools;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Entities {
	public class Tile : MonoBehaviour, IPhysicalEntity2d, IPoolable<IMemoryPool> {
		[SerializeField] private Vector2 sizes;

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
		private IMemoryPool pool;

		public bool DetectCollision(IShapeCollisionDetection second) => collisionDetection.DetectCollision(second);

		private void Awake(){
			collisionDetection = new RectShapeCollisionDetection(
				() => worldObjectRootTransform.InverseTransformPoint(transform.position).ToV2FromXZ(),
				() => sizes);
		}

		public void Despawn(){
			pool.Despawn(this);
		}

		public void OnSpawned(IMemoryPool pool){
			this.pool = pool;
		}

		public void OnDespawned(){ }

		public class Pool : MonoPoolableMemoryPool<IMemoryPool, Tile> { }

		public class Factory : PlaceholderFactory<Tile> { }
	}
}