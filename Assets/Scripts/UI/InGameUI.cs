using Main;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI {
	public interface IInGameUI {
		void SetScore(int score);
		void ResetState();
		void ShowMainPanel();
	}

	public class InGameUI : MonoBehaviour, IInGameUI {
		[SerializeField] private RectTransform mainInGamePanel;
		[SerializeField] private Text scoreValueText;
		[SerializeField] private Button pauseButton;
		[SerializeField] private RectTransform gameOverPanel;
		[SerializeField] private Text finalScoreValueText;
		[SerializeField] private Button restartButton;

		[Inject] private readonly SignalBus signalBus;

		private void Awake(){
			pauseButton.onClick.AddListener(signalBus.Fire<SwitchPauseSignal>);
			restartButton.onClick.AddListener(signalBus.Fire<RestartGameSignal>);
		}

		public void ResetState(){
			mainInGamePanel.gameObject.SetActive(false);
			gameOverPanel.gameObject.SetActive(false);

			scoreValueText.text      = "";
			finalScoreValueText.text = "";
		}

		public void SetScore(int score){
			scoreValueText.text = score.ToString();
		}

		public void GameOverCallback(GameOverSignal signal){
			ShowGameOverPanel();
			finalScoreValueText.text = signal.Score.ToString();
		}

		public void ShowMainPanel(){
			mainInGamePanel.gameObject.SetActive(true);
			gameOverPanel.gameObject.SetActive(false);
		}

		public void ShowGameOverPanel(){
			mainInGamePanel.gameObject.SetActive(false);
			gameOverPanel.gameObject.SetActive(true);
		}
	}
}