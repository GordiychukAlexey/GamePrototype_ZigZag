using System;
using System.Collections.Generic;

namespace Misc {
	public interface IKeyValue<K, V> {
		K Key{ get; }
		V Value{ get; }
	}

	[Serializable]
	public class KeyValueGroups<TParamsKey, TParamsValue> {
		public List<IKeyValue<TParamsKey, TParamsValue>> Groups = new List<IKeyValue<TParamsKey, TParamsValue>>(); //Не отображается в инспекторе

		public TParamsValue GetObject(TParamsKey key) =>
			Groups.Find(group => Equals(group.Key, key)).Value;
	}

	[Serializable]
	public class KeyValueGroups<TParamsGroup, TParamsKey, TParamsValue>
		where TParamsGroup : IKeyValue<TParamsKey, TParamsValue> {
		public List<TParamsGroup> Groups = new List<TParamsGroup>(); //Для отображения в инспекторе нужен избыточный TParamsGroup

		public TParamsValue GetObject(TParamsKey key) =>
			Groups.Find(group => Equals(group.Key, key)).Value;
	}
}