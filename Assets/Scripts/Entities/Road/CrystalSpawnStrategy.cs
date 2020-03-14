using System;
using System.Collections;
using System.Collections.Generic;
using Zenject;
using Random = UnityEngine.Random;

namespace Entities.Road {
	public interface ICrystalSpawnStrategy : IEnumerator<bool> { }

	public class CrystalSpawnStrategyFactory : IFactory<ICrystalSpawnStrategy> {
		[Inject] private readonly Road.Settings roadSettings;

		public ICrystalSpawnStrategy Create(){
			switch (roadSettings.CrystalSpawnMethod){
				case Road.CrystalSpawnMethod.Random:
					return new CrystalSpawnStrategy_Random();
				case Road.CrystalSpawnMethod.InOrder:
					return new CrystalSpawnStrategy_InOrder();
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public class CrystalSpawnStrategy_Random : ICrystalSpawnStrategy {
		private float spawnProbability = 1.0f / 5.0f; //#5245

		public bool Current{ get; private set; }
		object IEnumerator.Current => Current;

		public void Reset(){ }

		public bool MoveNext(){
			Current = Random.value <= spawnProbability;
			return true;
		}

		public void Dispose(){ }
	}

	public class CrystalSpawnStrategy_InOrder : ICrystalSpawnStrategy {
		private int currentId;
		private int nextSpawnId;

		public bool Current{ get; private set; }
		object IEnumerator.Current => Current;

		public CrystalSpawnStrategy_InOrder(){
			Reset();
		}

		public void Reset(){
			currentId   = 1;
			nextSpawnId = 1;
		}

		public bool MoveNext(){
			Current = currentId == nextSpawnId;

			currentId++;
			if (currentId > 5){ //#5245 причина магических констант
				currentId = 1;

				nextSpawnId++;
				if (nextSpawnId > 5){ //#5245
					nextSpawnId = 1;
				}
			}

			return true;
		}

		public void Dispose(){ }
	}
}