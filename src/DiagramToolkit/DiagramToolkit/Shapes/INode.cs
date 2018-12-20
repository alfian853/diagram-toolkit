using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiagramToolkit.Shapes
{
    public interface INode
    {
        void addNeigbor(Tuple<IEdge, INode> child);
        List<Tuple<IEdge, INode>> GetChilds();
        void setVisit(bool isVisited);
        bool getVisit();
        Point getOutLinePointFrom(int sX, int sY);
    }
}
