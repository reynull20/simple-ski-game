using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallObstacle : Obstacle
{
    [SerializeField]
    private float rotationSpeed = 90f;
    private float dt = 0;
    public override void HandleCollision(Collision player)
    {
        base.HandleCollision(player);
        StartCoroutine(Fall());
    }

    private IEnumerator Fall() {
        Quaternion targetAngle = Quaternion.Euler(transform.rotation.x, -90f, 90f);
        while (Quaternion.Angle(transform.rotation, targetAngle) > 0.01f){
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
