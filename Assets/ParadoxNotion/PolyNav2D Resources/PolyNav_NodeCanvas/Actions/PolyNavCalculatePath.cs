using UnityEngine;
using System.Collections.Generic;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using System.Linq;
using PolyNav;

namespace NodeCanvas.Tasks.Actions{

	[Name("Calculate Path")]
	[Category("PolyNav")]
	[Description("Calculate a path and return success if path exists or failure if not")]
	public class PolyNavCalculatePath : ActionTask {

		public BBParameter<Vector2> from;
		public BBParameter<Vector2> to;
		[BlackboardOnly]
		public BBParameter<List<Vector2>> resultPath;

		protected override string info{
			get {return string.Format("CalcPath {0} - {1}", from, to);}
		}

		protected override void OnExecute(){

			if (!PolyNav2D.current){
				EndAction(false);
				return;
			}

			PolyNav2D.current.FindPath(from.value, to.value, PathReady);
		}

		void PathReady(Vector2[] path){
			resultPath.value = path != null? path.ToList() : null;
			EndAction(path != null);
		}
	}
}