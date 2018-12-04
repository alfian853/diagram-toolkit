using DiagramToolkit.Api.Shapes;
using DiagramToolkit.States;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DiagramToolkit.Tools
{
    class NodeTool : ToolStripButton, ITool
    {
        private ICanvas canvas;

        private Node nodeStart;
        private Edge edge;

        private Dictionary<Keys, bool> isPressed;

        int nextNodeNum;
        private BackgroundWorker worker;

        public NodeTool()
        {
            this.Name = "Node tool";
            this.ToolTipText = "Node tool";
            this.Image = IconSet.node;
            this.CheckOnClick = true;
            this.nextNodeNum = 0;
            GraphVisualizer.canvas = this.canvas;
            isPressed = new Dictionary<Keys, bool>();

            isPressed[Keys.ShiftKey] = false;
            isPressed[Keys.G] = false;
            isPressed[Keys.D] = false;
            isPressed[Keys.B] = false;
        }

        public Cursor Cursor
        {
            get
            {
                return Cursors.Arrow;
            }
        }

        public ICanvas TargetCanvas
        {
            get
            {
                return this.canvas;
            }

            set
            {
                this.canvas = value;
                GraphVisualizer.canvas = value;

            }
        }

        public void ToolHotKeysDown(object sender, Keys e)
        {
        }

        public void ToolKeyDown(object sender, KeyEventArgs e)
        {
            isPressed[e.KeyCode] = true;
            if(e.KeyCode == Keys.R)
            {
                this.nextNodeNum = 0;
            }
        }

        public void ToolKeyUp(object sender, KeyEventArgs e)
        {
            isPressed[e.KeyCode] = false;
        }

        public void ToolMouseDoubleClick(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }


        private int getDistance(int x1,int y1,int x2,int y2)
        {
            return (int) Math.Pow((x2-x1)*(x2-x1) + (y2-y1)*(y2-y1), 2);
        }

        private void runVisualization(object sender, DoWorkEventArgs e)
        {
            if (isPressed[Keys.D])
            {
                GraphVisualizer.dfs((INode)e.Argument);
            }
            else if (isPressed[Keys.B])
            {
                GraphVisualizer.bfs((INode)e.Argument);
            }
        }

        public void ToolMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (isPressed[Keys.G])
                {
                    DrawingObject init = this.canvas.GetObjectAt(e.X, e.Y);
                    
                    if (init != null && init is Node)
                    {
                        Debug.WriteLine("dfs start");
                        this.worker = new BackgroundWorker();
                        worker.DoWork += runVisualization;
                        worker.RunWorkerAsync(argument: (INode)init);                      
                    }
                }
                else
                {
                    this.nodeStart = new Node(this.nextNodeNum++);
                    this.nodeStart.cX = e.X;
                    this.nodeStart.cY = e.Y;
                    this.nodeStart.radius = 30;
                    this.canvas.AddDrawingObject(this.nodeStart);
                }
            }
            else
            {
                DrawingObject init = this.canvas.GetObjectAt(e.X,e.Y);
                if(init!=null && init is INode )
                {
                    this.edge = new Edge(this.isPressed[Keys.ShiftKey]);
                    this.edge.Startpoint = new Point(e.X, e.Y);
                    this.edge.Endpoint = new Point(e.X, e.Y);
                    this.canvas.AddDrawingObject(this.edge);
                    this.nodeStart = (Node)init;
                }
            }
        }

        public void ToolMouseMove(object sender, MouseEventArgs e)
        {
            if(this.edge != null)
            {
                this.edge.Endpoint = new Point(e.X, e.Y);
            }
        }

        public void ToolMouseUp(object sender, MouseEventArgs e)
        {
            DrawingObject end = this.canvas.GetObjectAt(e.X, e.Y);
            if (this.nodeStart != null && end != null && this.edge != null && end is INode && end != nodeStart)
            {
                INode endNode = (INode)end;
                this.edge.setNode((INode)nodeStart, (INode)end);

                this.nodeStart.addChild(new Tuple<IEdge, INode>(this.edge,(INode)end));

                // if undirected
                if (!this.isPressed[Keys.ShiftKey])
                {
                    endNode.addChild(new Tuple<IEdge, INode>(this.edge, nodeStart));
                }
                else
                {
                    endNode.addChild(new Tuple<IEdge, INode>(this.edge, null));
                }

                this.edge.ChangeState(StaticState.GetInstance());
            }
            else if(this.edge != null)
            {
                this.canvas.RemoveDrawingObject(this.edge);
            }
            this.nodeStart = null;
            this.edge = null;
        }
    }
}
