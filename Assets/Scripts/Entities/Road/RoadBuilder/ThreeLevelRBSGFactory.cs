using System.Collections.Generic;
using Main;
using Misc;
using UnityEngine;
using Zenject;

namespace Entities.Road.RoadBuilder {
	/// <summary>
	/// Параметры для построения дороги из тайлов для трёх уровней сложности
	/// </summary>
	public class ThreeLevelRBSGFactory : IFactory<RoadBuilderSettingsGroups> {
		[Inject] private readonly RoadBuilderSettingsGroups.Factory fact;

		public RoadBuilderSettingsGroups Create(){
			RoadBuilderSettingsGroups roadBuilderSettingsGroups = fact.Create();

			roadBuilderSettingsGroups.Groups = new List<IKeyValue<MainSettings.Difficult, RoadBuilder.Settings>>{
				new RoadBuilderSettingsGroup{
					Difficult = MainSettings.Difficult.Hard,
					Params = new RoadBuilder.Settings{
						RoadSize             = 1,
						StartPointShift      = -0.5f,
						CrystalPositionShift = 0.0f,
						ForwardMoveTilesPlacement = new Vector2[]{
							new Vector2(0.0f, 0.0f)
						},
						RightMoveTilesPlacement = new Vector2[]{
							new Vector2(0.0f, 0.0f)
						}
					}
				},
				new RoadBuilderSettingsGroup{
					Difficult = MainSettings.Difficult.Medium,
					Params = new RoadBuilder.Settings{
						RoadSize             = 2,
						StartPointShift      = 0.0f,
						CrystalPositionShift = -0.5f,
						ForwardMoveTilesPlacement = new Vector2[]{
							new Vector2(-0.5f, -0.5f),
							new Vector2(0.5f, -0.5f)
						},
						RightMoveTilesPlacement = new Vector2[]{
							new Vector2(-0.5f, -0.5f),
							new Vector2(-0.5f, -1.5f)
						}
					}
				},
				new RoadBuilderSettingsGroup{
					Difficult = MainSettings.Difficult.Easy,
					Params = new RoadBuilder.Settings{
						RoadSize             = 3,
						StartPointShift      = -0.5f,
						CrystalPositionShift = 0.0f,
						ForwardMoveTilesPlacement = new Vector2[]{
							new Vector2(-1.0f, 0.0f),
							new Vector2(0.0f, 0.0f),
							new Vector2(1.0f, 0.0f)
						},
						RightMoveTilesPlacement = new Vector2[]{
							new Vector2(-1.0f, 0.0f),
							new Vector2(-1.0f, -1.0f),
							new Vector2(-1.0f, -2.0f)
						}
					}
				}
			};

			return roadBuilderSettingsGroups;
		}
	}
}