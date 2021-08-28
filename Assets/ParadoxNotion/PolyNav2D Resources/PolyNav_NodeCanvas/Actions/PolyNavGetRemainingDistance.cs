using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Actions{

	[Name("Get Remaining Distance")]
	[Category("PolyNav")]
	public class PolyNavGetRemainingDistance : ActionTask<PolyNavAgent> {

		[BlackboardOnly]
		public BBParameter<float> saveAs;

		protected override string info{
			get {return string.Format("Get path distance as {0}", saveAs);}
		}

		protected override void OnExecute(){
			saveAs.value = agent.remainingDistance;
			EndAction(true);
		}
	}
}