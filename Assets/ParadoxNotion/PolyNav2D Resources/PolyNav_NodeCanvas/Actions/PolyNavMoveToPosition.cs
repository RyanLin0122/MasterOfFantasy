using UnityEngine;
using System.Collections;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Actions{

	[Name("Go To Position")]
	[Category("PolyNav")]
	public class PolyNavMoveToPosition : ActionTask<PolyNavAgent> {

		public BBParameter<Vector2> targetPosition;
		public BBParameter<float> speed = 4f;


		protected override string info{
			get {return string.Format("GoTo {0}", targetPosition);}
		}

		protected override void OnExecute(){
			agent.maxSpeed = speed.value;
			if (! agent.SetDestination(targetPosition.value, (bool canGo) => { EndAction(canGo); } ))
				EndAction(false);
		}

		protected override void OnStop(){
			agent.Stop();
		}
	}
}