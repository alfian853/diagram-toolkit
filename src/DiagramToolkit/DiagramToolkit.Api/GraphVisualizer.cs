using DiagramToolkit.Api.Shapes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DiagramToolkit
{
    public class GraphVisualizer
    {
        public static ICanvas canvas;

        private static void unVisitAll(INode node)
        {
            Queue<INode> queue = new Queue<INode>();
            queue.Enqueue(node);
            INode cNode;

            while (queue.Count != 0)
            {
                cNode = queue.Dequeue();
                cNode.setVisit(false);
                foreach (Tuple<IEdge, INode> child in cNode.GetChilds())
                {
                    if (child.Item2 != null && child.Item2.isVisited())
                    {
                        queue.Enqueue(child.Item2);
                    }
                }
            }
            canvas.RepaintFromOtherThread();
        }

        private static void dfsUtil(INode cNode)
        {
            cNode.setVisit(true);
            canvas.RepaintFromOtherThread();
            Thread.Sleep(1000);
            foreach (Tuple<IEdge, INode> child in cNode.GetChilds())
            {
                if (child.Item2 != null && !child.Item2.isVisited())
                {
                    child.Item1.setVisit(true);
                    dfs(child.Item2);
                    child.Item1.setVisit(false);
                }
            }
            cNode.setVisit(false);
        }

        public static void dfs(INode cNode)
        {
            dfsUtil(cNode);
            canvas.RepaintFromOtherThread();
        }

        public static void bfs(INode node)
        {

            Queue<INode> queue = new Queue<INode>();
            queue.Enqueue(node);
            INode cNode;

            while (queue.Count != 0)
            {
                cNode = queue.Dequeue();
                cNode.setVisit(true);
                canvas.RepaintFromOtherThread();
                Thread.Sleep(1000);
                foreach (Tuple<IEdge, INode> child in cNode.GetChilds())
                {
                    if (child.Item2 != null && !child.Item2.isVisited())
                    {
                        queue.Enqueue(child.Item2);
                        child.Item1.setVisit(true);
                    }
                }
            }
            unVisitAll(node);

        }



    }
}
