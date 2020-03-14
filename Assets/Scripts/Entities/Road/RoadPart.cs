using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Entities.Road {
	public class RoadPart : IDisposable {
		public Transform Root;
		public List<Tile> Tiles = new List<Tile>();
		public Crystal.Crystal Crystal;

		[Inject] private readonly Ball.Ball.Settings ballSettings;

		private Sequence roadPartFallingSequence;

		public void Destroy(){
			roadPartFallingSequence = DOTween.Sequence();

			if (Crystal != null){
				Sequence crystallFallingSequence = DOTween.Sequence();
				crystallFallingSequence
					.PrependInterval((1.0f / ballSettings.MovingSpeed) * 3.0f * Random.value)
					.Append(Crystal.transform.DOScale(Vector3.zero,
									   0.5f)
								   .SetEase(Ease.InQuad))
					.AppendCallback(() => Crystal.Despawn());
				roadPartFallingSequence.Join(crystallFallingSequence);
			}

			foreach (Tile tile in Tiles){
				Sequence tileFallingSequence = DOTween.Sequence();
				tileFallingSequence
					.PrependInterval((1.0f / ballSettings.MovingSpeed) * 3.0f * Random.value)
					.Append(tile.transform.DOLocalMove(
									tile.transform.localPosition + Vector3.down,
									0.5f)
								.SetEase(Ease.InQuad))
					.Join(tile.transform.DOLocalRotateQuaternion(
								  tile.transform.localRotation *
								  Quaternion.Euler(Random.onUnitSphere * 10.0f),
								  0.5f)
							  .SetEase(Ease.Linear))
					.AppendCallback(() => tile.Despawn());
				roadPartFallingSequence.Join(tileFallingSequence);
			}

			roadPartFallingSequence.AppendCallback(delegate{ Object.Destroy(Root.gameObject); });
		}

		public void Dispose(){
			roadPartFallingSequence?.Kill();

			if (Crystal != null){
				Crystal.Despawn();
			}

			foreach (Tile tile in Tiles){
				tile.Despawn();
			}

			Object.Destroy(Root.gameObject);
		}

		public class Factory : PlaceholderFactory<RoadPart> { }
	}
}