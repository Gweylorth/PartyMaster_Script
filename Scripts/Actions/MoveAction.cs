using System.Linq;

public abstract class MoveAction : PlayerAction
{
    protected int moveDirection = 0;

    public MoveAction()
    {
        Execute = () => { };
    }

    public override void Select()
    {
        Move();
        var existingMove = GameManager.Instance.ActionManager.PickedActions.OfType<MoveAction>().LastOrDefault();
        if (existingMove == null || existingMove == this)
        {
            base.Select();
            return;
        }

        GameManager.Instance.ActionManager.ClearActions();
    }

    private void Move()
    {
        GameManager.Instance.CurrentPlayer.MoveDirection = moveDirection;
    }
}
