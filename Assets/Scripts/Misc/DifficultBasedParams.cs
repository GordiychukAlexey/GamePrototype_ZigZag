using System;
using Main;

namespace Misc {
	[Serializable]
	public class DifficultBasedParams<P> : IKeyValue<MainSettings.Difficult, P> {
		public MainSettings.Difficult Difficult;
		public P Params;
		public MainSettings.Difficult Key => Difficult;
		public P Value => Params;
	}
}