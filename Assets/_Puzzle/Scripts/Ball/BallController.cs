using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public IBallMovementController movementController;
    public IBallTouchInputProvider touchInputProvider;

    [SerializeField]
    private PipeController _pipe;
    [SerializeField]
    private int _pipeIndex = 0;

    public PipeController Pipe { get { return _pipe; } }
    public int PipeIndex { get { return _pipeIndex;} }

    public void SetPipeAndIndex(PipeController pipe, int index)
    {
        _pipe = pipe;
        _pipeIndex = index;
    }

    protected virtual void Awake()
    {
        movementController = GetComponent<IBallMovementController>();
        touchInputProvider = GetComponent<IBallTouchInputProvider>();
    }

    private void Start()
    {
        //movementController.Move(new Vector2(0,0));
    }
}
