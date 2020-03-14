using System;
using Lean.Touch;
using UnityEngine;
using Zenject;


public interface IInputManager {
	event Action UserInput;
}

public class InputManager : IInputManager, IInitializable, IDisposable, ITickable {
	public event Action UserInput;

	public void Initialize(){
		LeanTouch.OnFingerDown += OnFingerDownHandler;
	}

	public void Dispose(){
		LeanTouch.OnFingerDown -= OnFingerDownHandler;
	}

	private void OnFingerDownHandler(LeanFinger finger){
		if (!finger.IsOverGui){
			UserInput?.Invoke();
		}
	}

	public void Tick(){
		if (Input.GetKeyDown(KeyCode.Space)){
			UserInput?.Invoke();
		}
	}
}