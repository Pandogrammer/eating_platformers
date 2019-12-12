using System.Collections;
using System.Collections.Generic;
using Flying.Scripts.Rotate;
using UnityEngine;

public class MultiAgent : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private UnityRotateAgent rotate;
    [SerializeField] private UnityForwardAgent forward;
    void Start()
    {            
        rotate.model = RotateAgent.Create(Quaternion.identity, target.localPosition, 5f);
        forward.model = ForwardAgent.Create(transform.localPosition, target.localPosition, 1f);
    }

    void Update()
    {
        UpdateRotateAgent();
        UpdateForwardAgent();
        
        if(Keys.ROTATE)
            rotate.RequestDecision();
        if(Keys.FORWARD)
            forward.RequestDecision();
    }

    private void UpdateForwardAgent()
    {
        var position = forward.body.transform.localPosition;
        var targetPosition = target.localPosition;
        var velocity = forward.body.velocity;
        
        forward.model = ForwardAgent.Update(forward.model, position, targetPosition, velocity);
    }

    private void UpdateRotateAgent()
    {
        var agentBody = rotate.body;
        var agentPosition = agentBody.transform.localPosition;
        var agentRotation = agentBody.rotation;
        var agentVelocity = agentBody.angularVelocity;
        var targetPosition = target.localPosition;
        var turnDirection = CalculateTurnDirection(agentBody, targetPosition, agentPosition);

        rotate.model = RotateAgent.Update(rotate.model, agentRotation, targetPosition, agentVelocity, turnDirection);
    }

    private static int CalculateTurnDirection(Rigidbody agentBody, Vector3 targetPosition, Vector3 agentPosition)
    {
        var rightAngle = Vector3.Angle(agentBody.transform.right, targetPosition - agentPosition);
        var leftAngle = Vector3.Angle(-agentBody.transform.right, targetPosition - agentPosition);
        return rightAngle < leftAngle ? 1 : -1;
    }
    
    internal static class Keys
    {
        public static bool ROTATE => Input.GetKey(KeyCode.R);
        public static bool FORWARD => Input.GetKey(KeyCode.F);
    }
}
