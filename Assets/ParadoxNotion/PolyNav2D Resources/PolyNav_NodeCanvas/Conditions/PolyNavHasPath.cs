using UnityEngine;
using ParadoxNotion.Design;
using NodeCanvas.Framework;
using PolyNav;

namespace NodeCanvas.Tasks.Conditions{

	[Name("Has Path")]
	[Category("PolyNav")]
	public class PolyNavHasPath : ConditionTask<PolyNavAgent> {

		protected override string info{
			get {return string.Format("{0} has path", agentInfo);}
		}

		protected override bool OnCheck(){
			return agent.hasPath;
		}
	}
}