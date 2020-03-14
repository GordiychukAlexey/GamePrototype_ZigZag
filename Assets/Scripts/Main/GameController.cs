using System;
using System.Linq;
using Entities.Ball;
using Entities.Road;
using ModestTree;
using Tools;
using UI;
using UnityEngine;
using Zenject;

namespace Main {
	public class GameController : IInitializable, IFixedTickable, IDisposable {
		[Inject] private readonly SignalBus signalBus;
		[Inject] private readonly Road road;
		[Inject] private readonly IBall ball;
		[Inject] private readonly Ball.Settings ballSettings;
		[Inject] private readonly IInGameUI inGameUi;
		[Inject] private readonly IInputManager inputManager;

		private bool isStarted;
		private bool isPause;
		private float distance;
		private float nextRoadSpawnDistancePoint;
		private bool needSwitchBallMovingDirection = false;

		private int _crystalsCollected;

		public int CrystalsCollected{
			get => _crystalsCollected;
			private set{
				_crystalsCollected = value;
				inGameUi.SetScore(_crystalsCollected);
			}
		}

		public void Initialize(){
			inputManager.UserInput += UserInput;

			ResetState();
		}

		public void Dispose(){
			inputManager.UserInput -= UserInput;
		}

		private void UserInput(){
			if (!isStarted){
				Start();
			} else{
				if (!isPause){
					needSwitchBallMovingDirection = true;
				}
			}
		}

		public void ResetState(){
			road.ResetState();
			ball.ResetState();
			inGameUi.ResetState();

			road.transform.localPosition = Vector3.zero;

			isStarted                  = false;
			isPause                    = false;
			distance                   = 0.0f;
			nextRoadSpawnDistancePoint = distance + 1.0f;
			CrystalsCollected          = 0;


			road.Prepare();
			inGameUi.ShowMainPanel();
		}

		public void Start(){
			Assert.That(!isStarted);

			isStarted = true;
		}

		public void Stop(){
			Assert.That(isStarted);
			isStarted = false;
		}

		public void SwitchPauseCallback(){
			if (isStarted){
				Debug.Log("Switch pause");
				isPause = !isPause;
			}
		}

		public void FixedTick(){
			if (!isStarted || isPause) return;

			float newMovingDistance = ballSettings.MovingSpeed * Time.fixedDeltaTime;
			ball.Move(needSwitchBallMovingDirection, newMovingDistance);
			if (needSwitchBallMovingDirection){
				needSwitchBallMovingDirection = false;
			}

			distance += newMovingDistance;

			{ //suppress scrolling in editor
				road.Position -= ball.Position;
				ball.Position =  Vector2.zero;
			}

			while (distance >= nextRoadSpawnDistancePoint){
				nextRoadSpawnDistancePoint += 1.0f;
				road.CreateNextRoadPart();
			}

			if (!road.DetectCollision((Point2dShapeCollisionDetection) ball.Position)){
				Debug.Log($"GameOver. Score: {CrystalsCollected}");
				Stop();

				signalBus.Fire(new GameOverSignal(){
					Score = CrystalsCollected,
				});
			}

			foreach (RoadPart roadPart in road.RoadParts.Where(roadPart =>
				roadPart.Crystal
				&& roadPart.Crystal.DetectCollision(ball))){
				roadPart.Crystal.Despawn();
				roadPart.Crystal = null;

				++CrystalsCollected;
			}
		}
	}
}