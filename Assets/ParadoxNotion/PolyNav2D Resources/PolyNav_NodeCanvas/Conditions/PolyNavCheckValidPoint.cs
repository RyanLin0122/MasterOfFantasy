using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Conditions{

	[Name("Check Valid Point")]
	[Category("PolyNav")]
	public class PolyNavCheckValidPoint : ConditionTask {

		public BBParameter<Vector2> position;

		protected override string info{
			get {return string.Format("{0} is valid", position);}
		}

		protected override bool OnCheck(){
			if (!PolyNav2D.current)
				return false;
			return PolyNav2D.current.PointIsValid(position.value);
		}
	}
}