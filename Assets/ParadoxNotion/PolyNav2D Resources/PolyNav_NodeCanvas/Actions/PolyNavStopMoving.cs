using UnityEngine;
using System.Collections;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Actions{

	[Name("Stop Moving")]
	[Category("PolyNav")]
	public class PolyNavStopMoving : ActionTask<PolyNavAgent> {

		protected override void OnExecute(){
			agent.Stop();
			EndAction();
		}
	}
}