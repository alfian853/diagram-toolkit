using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramToolkit.Api.Shapes
{
    public interface INode
    {
        void addChild(Tuple<IEdge, INode> child);
        List<Tuple<IEdge, INode>> GetChilds();
        void setVisit(bool isVisited);
        bool isVisited();
        Point getOutLinePointFrom(int sX, int sY);
    }
}
