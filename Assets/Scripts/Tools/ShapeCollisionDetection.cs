using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools {
	public interface IShapeCollisionDetection {
		bool DetectCollision(IShapeCollisionDetection second);
	}

	public abstract class ShapeCollisionDetection : IShapeCollisionDetection {
		private static readonly List<Type> InterfacesTypesSequence = new List<Type>{
			typeof(Point2dShapeCollisionDetection),
			typeof(CircleShapeCollisionDetection),
			typeof(RectShapeCollisionDetection),
		};

		private static int Compare(ShapeCollisionDetection a, ShapeCollisionDetection b){
			int aIndex = InterfacesTypesSequence.FindIndex(type => type.IsInstanceOfType(a));
			int bIndex = InterfacesTypesSequence.FindIndex(type => type.IsInstanceOfType(b));

			if (aIndex < 0 || bIndex < 0) throw new ArgumentException();

			return aIndex == bIndex ? 0 : aIndex > bIndex ? 1 : -1;
		}

		public bool DetectCollision(IShapeCollisionDetection other){
			if (other == null){
				throw new ArgumentNullException();
			}

			if (!(other is ShapeCollisionDetection)){
				return other.DetectCollision(this);
			}

			ShapeCollisionDetection first;
			ShapeCollisionDetection second;

			if (Compare(this, (ShapeCollisionDetection) other) <= 0){
				first  = this;
				second = (ShapeCollisionDetection) other;
			} else{
				first  = (ShapeCollisionDetection) other;
				second = this;
			}

			{
				switch (second){
					case Point2dShapeCollisionDetection a_point2d:
						switch (first){
							case Point2dShapeCollisionDetection b_point2d:
								throw new NotImplementedException();
						}

						break;
					case CircleShapeCollisionDetection a_circle:
						switch (first){
							case Point2dShapeCollisionDetection b_point2d:
								throw new NotImplementedException();
							case CircleShapeCollisionDetection b_circle:
								return ShapeTools.IsIntersectionOf_Circle_Circle(
									a_circle.Position, a_circle.Radius, b_circle.Position, b_circle.Radius);
						}

						break;
					case RectShapeCollisionDetection a_rect:
						switch (first){
							case Point2dShapeCollisionDetection b_point2d:
								return ShapeTools.RectContains(a_rect.Position - a_rect.Sizes / 2.0f, a_rect.Sizes, b_point2d.Position);
							case CircleShapeCollisionDetection b_circle:
								throw new NotImplementedException();
							case RectShapeCollisionDetection b_rect:
								throw new NotImplementedException();
						}

						break;
				}
			}

			throw new NotImplementedException();
		}
	}

	public class Point2dShapeCollisionDetection : ShapeCollisionDetection {
		public Vector2 Position => positionGetter.Invoke();
		private readonly Func<Vector2> positionGetter;

		public Point2dShapeCollisionDetection(Func<Vector2> positionGetter){
			this.positionGetter = positionGetter;
		}

		public static implicit operator Point2dShapeCollisionDetection(Vector2 position){
			return new Point2dShapeCollisionDetection(() => position);
		}

		public static explicit operator Vector2(Point2dShapeCollisionDetection point2dShapeCollisionDetection){
			return point2dShapeCollisionDetection.Position;
		}
	}

	public class CircleShapeCollisionDetection : ShapeCollisionDetection {
		public Vector2 Position => positionGetter.Invoke();
		private readonly Func<Vector2> positionGetter;

		public float Radius => radiusGetter.Invoke();
		private readonly Func<float> radiusGetter;

		public CircleShapeCollisionDetection(Func<Vector2> positionGetter, Func<float> radiusGetter){
			this.positionGetter = positionGetter;
			this.radiusGetter   = radiusGetter;
		}
	}

	public class RectShapeCollisionDetection : ShapeCollisionDetection {
		public Vector2 Position => positionGetter.Invoke();
		private readonly Func<Vector2> positionGetter;

		public Vector2 Sizes => sizesGetter.Invoke();
		private readonly Func<Vector2> sizesGetter;

		public RectShapeCollisionDetection(Func<Vector2> positionGetter, Func<Vector2> sizeGetter){
			this.positionGetter = positionGetter;
			this.sizesGetter    = sizeGetter;
		}
	}
}