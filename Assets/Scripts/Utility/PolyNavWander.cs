using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEditor;

using NavMeshHit = UnityEngine.AI.NavMeshHit;
[Category("Movement/PolyNavWander")]
[Description("Makes the agent wander randomly within the navigation map")]
public class PolyNavWander : ActionTask<PolyNav.PolyNavAgent>
{
    public BBParameter<float> speed = 100;
    public BBParameter<float> keepDistance = 1f;
    public BBParameter<float> minWanderDistance = 50;
    public BBParameter<float> maxWanderDistance = 200;
    public BBParameter<Vector3> NextPos;
    public BBParameter<Vector3> agentPos;
    public bool repeat = true;

    
    protected override void OnExecute()
    {
        agent.maxSpeed = speed.value;
        DoWander();
    }

    protected override void OnUpdate()
    {
        DoWander();
        /*
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + keepDistance.value)
        {
            if (repeat)
            {
                DoWander();
            }
            else
            {
                EndAction();
            }
        }
        */
    }

    void DoWander()
    {
        var min = minWanderDistance.value;
        var max = maxWanderDistance.value;
        min = Mathf.Clamp(min, 0.01f, max);
        max = Mathf.Clamp(max, min, max);
        var wanderPos = agent.transform.position;
        agentPos = agent.transform.position;
        while ((wanderPos - agent.transform.position).sqrMagnitude < (min * min))
        {
            wanderPos = (Random.insideUnitSphere * max) + agent.transform.position;
            NextPos = (Random.insideUnitSphere * max) + agent.transform.position;
            if (!agent.SetDestination(new Vector2(wanderPos.x, wanderPos.y),(bool canGo) => { EndAction(canGo); }))
                EndAction(false);
        }

        //NavMeshHit hit;
        
        //if (NavMesh.SamplePosition(wanderPos, out hit, float.PositiveInfinity, NavMesh.AllAreas))
        //{
        //    agent.SetDestination(hit.position);
        //}
    }

    protected override void OnPause() { OnStop(); }
    protected override void OnStop()
    {
        if (agent != null && agent.gameObject.activeSelf)
        {
            agent.Stop();
        }
    }
    
}
