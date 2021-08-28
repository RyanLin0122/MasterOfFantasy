using UnityEngine;
using System.Collections;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Actions{

	[Name("Go To GameObject")]
	[Category("PolyNav")]
	public class PolyNavMoveToGameObject : ActionTask<PolyNavAgent> {

		[RequiredField]
		public BBParameter<GameObject> targetObject;
		public BBParameter<float> speed = 4f;

		protected override string info{
			get {return string.Format("GoTo {0}", targetObject);}
		}

		protected override void OnExecute(){

			agent.maxSpeed = speed.value;
            agent.position = Vector3.MoveTowards(agent.position, targetObject.value.transform.position, speed.value * Time.deltaTime);
            //if(! agent.SetDestination(targetObject.value.transform.position, (bool canGo)=> { EndAction(canGo); } ))
            //	EndAction(false);
        }

		protected override void OnStop(){
			agent.Stop();
		}
	}
}