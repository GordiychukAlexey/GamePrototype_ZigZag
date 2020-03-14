using System;

namespace Main {
	[Serializable]
	public class MainSettings {
		public Difficult OverallDifficult;

		public enum Difficult {
			Easy,
			Medium,
			Hard,
		}
	}
}