using DiagramToolkit.Shapes;
using DiagramToolkit.States;
using System;
using System.Drawing;

namespace DiagramToolkit.Api.Shapes
{
    public class Edge : LineSegment, IEdge
    {

        private bool isDirected = false;
        private INode src;
        private INode dst;


        public Edge()
        {

        }

        public Edge(bool isDirected)
        {
            this.pen.Width = 3;
            this.isDirected = isDirected;
        }

        private void updateConnectLine()
        {
            Point res = src.getOutLinePointFrom(this.Endpoint.X, this.Endpoint.Y);
            Startpoint = res;
            Point res2 = dst.getOutLinePointFrom(Startpoint.X, Startpoint.Y);
            Endpoint = res2;
        }


        public void onNodeMove()
        {
            this.ChangeState(EditState.GetInstance());
            this.updateConnectLine();
        }

        public void onNodeStopMove()
        {
            this.ChangeState(StaticState.GetInstance());
        }

        public void setNode(INode src, INode dst)
        {
            this.src = src;
            this.dst = dst;
            updateConnectLine();
        }

        private void drawCap(int x1,int y1,int x2,int y2)
        {
            Brush b = new SolidBrush(pen.Color);
            double angle = Math.Atan2(y2 - y1, x2 - x1);
            PointF p1 = new PointF(x1 + (float)(Math.Cos(angle - Math.PI / 2.0) * 10), y1 + (float)(Math.Sin(angle - Math.PI / 2.0) * 10));
            PointF p2 = new PointF(x1 - (float)(Math.Cos(angle) * 10), y1 - (float)(Math.Sin(angle) * 10));
            PointF p3 = new PointF(x1 - (float)(Math.Cos(angle - Math.PI / 2.0) * 10), y1 - (float)(Math.Sin(angle - Math.PI / 2.0) * 10));
            this.GetGraphics().FillPolygon(b, new PointF[] { p1, p2, p3 });
        }

        public override void RenderOnEditingView()
        {
            this.pen.Color = Color.Blue;
            base.RenderOnStaticView();

            if (this.isDirected)
            {
                this.drawCap(Endpoint.X, Endpoint.Y, Startpoint.X, Startpoint.Y);
            }
        }

        public override void RenderOnStaticView()
        {
            base.RenderOnStaticView();
            this.pen.Color = Color.Black;
            if (this.isDirected)
            {
                this.drawCap(Endpoint.X, Endpoint.Y, Startpoint.X, Startpoint.Y);
            }
        }

        public override void RenderOnPreview()
        {
            base.RenderOnPreview();
            this.pen.Color = Color.Red;
            if (this.isDirected)
            {
                this.drawCap(Endpoint.X, Endpoint.Y, Startpoint.X, Startpoint.Y);
            }
        }

        public void setVisit(bool isVisited)
        {
            if (isVisited)
            {
                this.ChangeState(PreviewState.GetInstance());
            }
            else
            {
                this.ChangeState(StaticState.GetInstance());
            }
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            
        }
    }
}
