namespace DiagramToolkit.States
{
    public class EditState : DrawingState
    {
        private static DrawingState instance;

        public static DrawingState GetInstance()
        {
            if (instance == null)
            {
                instance = new EditState();
            }
            return instance;
        }

        public override void Draw(DrawingObject obj)
        {
            obj.RenderOnEditingView();
        }

        public override void Deselect(DrawingObject obj)
        {
            obj.ChangeState(StaticState.GetInstance());
        }
    }
}
