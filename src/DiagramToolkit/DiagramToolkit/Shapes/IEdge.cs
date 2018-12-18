namespace DiagramToolkit.Shapes
{
    public interface IEdge
    {
        void onNodeMove();
        void onNodeStopMove();
        void setNode(INode src, INode dst);
        void setVisit(bool isVisited);
    }
}
