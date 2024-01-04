using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Animations.Rigging;

public class EnvironmentInteractionStateMachine : StateManager<EEnvironmentInteractionState>
{
    private TwoBoneIKConstraint _leftIKConstraint;
    private TwoBoneIKConstraint _rightIKConstraint;
    private MultiRotationConstraint _leftRotationConstraint;
    private MultiRotationConstraint _rightRotationConstraint;

    private new Rigidbody rigidbody;
    public void OnAwake() 
    {
        rigidbody = GetComponent<Rigidbody>();
        ValidateConstraints();
    }
    private void ValidateConstraints()
    {
        Assert.IsNotNull(_leftIKConstraint, "Left IK constraint is not assigned");
        Assert.IsNotNull(_rightIKConstraint, "Right IK constraint is not assigned");
        Assert.IsNotNull(_leftRotationConstraint, "Left Multi-Rotation constraint is not assigned");
        Assert.IsNotNull(_rightRotationConstraint, "Right Multi-Rotation constraint is not assigned");
    }
}
