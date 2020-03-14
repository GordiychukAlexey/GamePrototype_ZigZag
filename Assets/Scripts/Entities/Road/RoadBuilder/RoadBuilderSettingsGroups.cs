using System;
using System.Collections.Generic;
using System.Linq;
using Main;
using Misc;
using UnityEngine;
using Zenject;

namespace Entities.Road.RoadBuilder {
	[Serializable]
	public class RoadBuilderSettingsGroups : DifficultBasedSettingsGroups<RoadBuilder.Settings> {
		private Dictionary<MainSettings.Difficult, Dictionary<Road.RelativeMoveDirection, Vector2[]>> tilesPlacements =
			new Dictionary<MainSettings.Difficult, Dictionary<Road.RelativeMoveDirection, Vector2[]>>();

		public Vector2[] GetTilesPlacement(Road.RelativeMoveDirection relativeMoveDirection){
			return GetTilesPlacement(relativeMoveDirection, mainSettings.OverallDifficult);
		}

		public Vector2[] GetTilesPlacement(Road.RelativeMoveDirection relativeMoveDirection, MainSettings.Difficult difficult){
			if (!tilesPlacements.ContainsKey(difficult)){
				RoadBuilder.Settings roadSettings = GetObject(difficult);
				tilesPlacements.Add(difficult,
					new Dictionary<Road.RelativeMoveDirection, Vector2[]>{
						{Road.RelativeMoveDirection.Forward, roadSettings.ForwardMoveTilesPlacement},
						{Road.RelativeMoveDirection.Right, roadSettings.RightMoveTilesPlacement},{
							Road.RelativeMoveDirection.Left,
							roadSettings.RightMoveTilesPlacement.Select(vector => vector * new Vector2(-1, 1)).ToArray()
						}
					});
			}

			return tilesPlacements[difficult][relativeMoveDirection];
		}

		public new class Factory : PlaceholderFactory<RoadBuilderSettingsGroups> { }
	}

	[Serializable]
	public class DifficultBasedSettingsGroups<TParamsValue> : KeyValueGroups<MainSettings.Difficult, TParamsValue> {
		[Inject] protected readonly MainSettings mainSettings;

		public TParamsValue GetObject(){
			return GetObject(mainSettings.OverallDifficult);
		}

		public class Factory : PlaceholderFactory<DifficultBasedSettingsGroups<TParamsValue>> { }
	}

	[Serializable]
	public class RoadBuilderSettingsGroup : DifficultBasedParams<RoadBuilder.Settings> { }
}