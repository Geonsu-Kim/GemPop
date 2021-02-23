using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BlockActionObj:MonoBehaviour
{
    private Queue<Vector3> mMovementQueue = new Queue<Vector3>();
    private Animator animator;
    public bool isMoving { get; set; }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void PopAction()
    {
        animator.SetTrigger("Pop");
    }
    public void NaviAction()
    {

        animator.SetTrigger("Matchable");
    }
    public void MoveDrop(Vector2 dropDistance)
    {
        mMovementQueue.Enqueue(new Vector3(dropDistance.x, dropDistance.y, 1));
        if (!isMoving)
        {
            StartCoroutine(DoActionMoveDrop());
        }
    }
    private IEnumerator DoActionMoveDrop(float acc = 1.0f)
    {
        isMoving = true;
        while (mMovementQueue.Count > 0)
        {
            Vector2 vtDestination = mMovementQueue.Dequeue();
            yield return CoStartDropSmooth(vtDestination, 0.5f * acc);
        }
        isMoving = false;
        yield break;
    }
    private IEnumerator CoStartDropSmooth(Vector2 vtDestination,float duration)
    {
        Vector3 to = new Vector3(transform.position.x + vtDestination.x, transform.position.y - vtDestination.y, transform.position.z);
        yield return Action2D.MoveTo(transform, to, duration);
    }
}