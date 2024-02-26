using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public IBallMovementController movementController;
    public IBallTouchInputProvider touchInputProvider;
    public IBallEventHandler eventHandler;

    [SerializeField]
    private PipeController _pipe;
    [SerializeField]
    private int _pipeIndex = 0;

    public PipeController Pipe { get { return _pipe; } }
    public int PipeIndex { get { return _pipeIndex;} }

    public void SetPipe(PipeController pipe)
    {
        _pipe = pipe;
    }
    public void SetPipeIndex(int index)
    {
        _pipeIndex = index;
    }
    public void DestroyBall()
    {
        //death logic
        Destroy(gameObject);
    }

    protected virtual void Awake()
    {
        movementController = GetComponent<IBallMovementController>();
        touchInputProvider = GetComponent<IBallTouchInputProvider>();
        eventHandler = GetComponent<IBallEventHandler>();
    }
}
