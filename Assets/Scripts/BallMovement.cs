using UnityEngine;
using GG.Infrastructure.Utils.Swipe;
using DG.Tweening;
using System.Collections.Generic;

public class BallMovement : MonoBehaviour
{
    [SerializeField] private SwipeListener swipeListener;
    [SerializeField] private LevelManager levelManager;

    [SerializeField] private float stepDuration = 0.1f;
    [SerializeField] private LayerMask wallsAndRoadsLayer;
    private const float MAX_RAY_DISTANCE = 10f;

    private Vector3 moveDirection;
    private bool canMove = true;

    private void Start()
    {
        //change default ball position
        transform.position = levelManager.defaultBallRoadPosition.position;
        
        swipeListener.OnSwipe.AddListener(swipe =>
        {
            moveDirection = swipe switch
            {
                "Right" => Vector3.right,
                "Left" => Vector3.left,
                "Up" => Vector3.forward,
                "Down" => Vector3.back,
                _ => moveDirection
            };
            MoveBall();
        });
    }

    private void MoveBall()
    {
        if (canMove)
        {
            //add raycast in the swipe direction
            var hits = Physics.RaycastAll(transform.position, moveDirection, MAX_RAY_DISTANCE,
                wallsAndRoadsLayer.value);
            var targetPosition = transform.position;
            var steps = 0;

            for (var i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                    //Road tile
                }
                else
                {
                    //Wall tile
                    if (i == 0)
                    {
                        canMove = true;
                        return;
                    }

                    steps = i;
                    targetPosition = hits[i - 1].transform.position;
                    break;
                }
            }
            //move the ball to targetPosition
            var moveDuration = stepDuration * steps;
            transform.DOMove(targetPosition, moveDuration).SetEase(Ease.OutExpo).OnComplete(() => canMove = true);
        }
    }
}
