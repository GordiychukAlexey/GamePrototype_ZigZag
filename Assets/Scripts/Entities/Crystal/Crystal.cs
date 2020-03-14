using System;
using Entities.IEntity;
using Installers;
using Tools;
using Tools.Extensions;
using UnityEngine;
using Zenject;

namespace Entities.Crystal {
	public class Crystal : MonoBehaviour, IPoolable<IMemoryPool>, IPhysicalEntity2d {
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
		IMemoryPool pool;

		public bool DetectCollision(IShapeCollisionDetection second) => collisionDetection.DetectCollision(second);

		private void Awake(){
			collisionDetection = new CircleShapeCollisionDetection(
				() => worldObjectRootTransform.InverseTransformPoint(transform.position).ToV2FromXZ(),
				() => radius);
		}

		public void Despawn(){
			pool.Despawn(this);
		}

		public void OnSpawned(IMemoryPool pool){
			this.pool = pool;
		}

		public void OnDespawned(){ }

		public class Pool : MonoPoolableMemoryPool<IMemoryPool, Crystal> { }

		public class Factory : PlaceholderFactory<Crystal> { }
	}
}