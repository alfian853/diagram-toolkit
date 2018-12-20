using DiagramToolkit.Shapes;
using System;
using System.Collections.Generic;
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
                    if (child.Item2 != null && child.Item2.getVisit())
                    {
                        child.Item1.setVisit(false);
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
                if (child.Item2 != null && !child.Item2.getVisit())
                {
                    child.Item1.setVisit(true);
                    dfsUtil(child.Item2);
                }
            }
        }

        public static void dfs(INode cNode)
        {
            dfsUtil(cNode);
            canvas.RepaintFromOtherThread();
            unVisitAll(cNode);
        }

        public static void bfs(INode node)
        {
            Queue<INode> queue = new Queue<INode>();
            queue.Enqueue(node);
            INode cNode;
            node.setVisit(true);
            while (queue.Count != 0)
            {
                cNode = queue.Dequeue();
                canvas.RepaintFromOtherThread();
                Thread.Sleep(1000);
                foreach (Tuple<IEdge, INode> child in cNode.GetChilds())
                {
                    if (child.Item2 != null && !child.Item2.getVisit())
                    {
                        child.Item1.setVisit(true);
                        child.Item2.setVisit(true);
                        queue.Enqueue(child.Item2);
                    }
                }
            }
            unVisitAll(node);

        }



    }
}
