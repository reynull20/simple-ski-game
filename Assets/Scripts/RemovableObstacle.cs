using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableObstacle : Obstacle
{
    [SerializeField]
    private GameObject particle;
    public override void HandleCollision(Collision other)
    {
        base.HandleCollision(other);
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion() {
        Instantiate(particle);
        Destroy(this.gameObject);
        yield return null;
    }
}
