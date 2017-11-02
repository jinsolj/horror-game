using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public enum StackOperation
{
    PUSH, POP
}

public class GameManager : MonoBehaviour {

    private static GameManager _instance;
    public static GameManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<GameManager>();
            return _instance;
        }
    }

    PlayerManager player;
    Flowchart flowchart;

    // The state of the game will be organized as a stack of states.
    Stack<GameState> gameState;

    // To prevent inputs from reused between state transitions,
    // Queue all state changes and apply them all at once at the end of the frame.
    Queue<ChangeStateOp> stateChanges;
    GameState currentState;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerManager>();
        flowchart = FindObjectOfType<Flowchart>();

        FlowchartEventHandler flowchartEvents = flowchart.GetComponent<FlowchartEventHandler>();

        // Subscribe functions to flowchart events to change game state appropriately.
        flowchartEvents.OnStartExecuting += AddDialogState;
        flowchartEvents.OnStopExecuting += RevertState;
        flowchartEvents.OnStartMenu += AddChoicesState;
        flowchartEvents.OnStopMenu += RevertState;

        gameState = new Stack<GameState>();
        stateChanges = new Queue<ChangeStateOp>();

        gameState.Push(new PlayerState());
	}
	
	// Update is called once per frame
	void Update () {
        currentState = gameState.Peek();
        currentState.Update(this);
	}

    // Apply all queued state changes.
    private void LateUpdate()
    {
        while(stateChanges.Count > 0)
        {
            ChangeStateOp op = stateChanges.Dequeue();
            if (op.operation == StackOperation.POP)
            {
                GameState removedState = gameState.Pop();
                removedState.OnExit(this);
                gameState.Peek().OnEnter(this);
            }
            else if(op.operation == StackOperation.PUSH)
            {
                GameState hiddenState = gameState.Peek();
                hiddenState.OnExit(this);
                gameState.Push(op.state);
                gameState.Peek().OnEnter(this);
            }
        }
    }

    void AddPlayerState()
    {
        AddState(new PlayerState());
    }

    void AddDialogState()
    {
        AddState(new DialogState());
    }

    void AddChoicesState()
    {
        AddState(new ChoicesState());
    }
    
    public void AddState(GameState state)
    {
        stateChanges.Enqueue(new ChangeStateOp(StackOperation.PUSH, state));
    }

    public void RevertState()
    {
        stateChanges.Enqueue(new ChangeStateOp(StackOperation.POP, null));
    }

    public class ChangeStateOp
    {
        public StackOperation operation;
        public GameState state;

        public ChangeStateOp(StackOperation _operation, GameState _state)
        {
            operation = _operation;
            state = _state;
        }
    }

    public abstract class GameState
    {
        public virtual void OnEnter(GameManager game)
        {
        }
        public virtual void Update(GameManager game)
        {
        }
        public virtual void OnExit(GameManager game)
        {
        }
    }

    public class PlayerState : GameState
    {
        public override void OnEnter(GameManager game)
        {
            game.player.allowInput = true;
        }

        public override void Update(GameManager game)
        {

        }
    }

    public class DialogState : GameState
    {
        public override void OnEnter(GameManager game)
        {
            game.player.allowInput = false;
        }

        public override void Update(GameManager game)
        {
            
        }
    }

    public class ChoicesState : GameState
    {
        public override void OnEnter(GameManager game)
        {
            game.player.allowInput = false;
        }

        public override void Update(GameManager game)
        {

        }
    }
}