using DiagramToolkit.States;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DiagramToolkit.Shapes
{
    public class Node : DrawingObject, INode
    {

        private Pen pen;

        public int cX;
        public int cY;
        public int radius;
        public int nodeNum;
        private bool isVisited = false;
        List<Tuple<IEdge, INode>> childs;


        public Node(int nodeNum)
        {
            this.childs = new List<Tuple<IEdge, INode>>();
            this.pen = new Pen(Color.Black);
            this.pen.Width = 4f;
            this.nodeNum = nodeNum;
            this.ChangeState(StaticState.GetInstance());
        }

        public override bool Add(DrawingObject obj)
        {
            return true;
        }

        public override bool Intersect(int xTest, int yTest)
        {
            float ax2 = (float)Math.Pow(cX - xTest, 2);
            float by2 = (float)Math.Pow(cY - yTest, 2);
           
            return ax2 + by2 - radius * radius <= 0.4;
        }

        public override bool Remove(DrawingObject obj)
        {
            return false;
        }

        private void drawNodeNum()
        {
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 13);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

            this.GetGraphics().DrawString(this.nodeNum.ToString(), drawFont, drawBrush, cX-10, cY-10);
        }

        public override void RenderOnEditingView()
        {
            this.pen.DashStyle = DashStyle.Dash;
            this.pen.Color = Color.Blue;
            this.GetGraphics().DrawEllipse(pen, cX-radius/2, cY-radius/2, radius, radius);
            this.drawNodeNum();
        }

        public override void RenderOnPreview()
        {
            this.pen.DashStyle = DashStyle.Solid;
            this.pen.Color = Color.Red;
            this.GetGraphics().DrawEllipse(pen, cX - radius / 2, cY - radius / 2, radius, radius);
            this.drawNodeNum();
        }

        public override void RenderOnStaticView()
        {
            if (this.isVisited)
            {
                this.pen.Color = Color.Red;
                this.pen.DashStyle = DashStyle.Solid;
                this.GetGraphics().DrawEllipse(pen, cX - radius / 2, cY - radius / 2, radius, radius);
                this.drawNodeNum();
            }
            else
            {
                this.pen.Color = Color.Black;
                this.pen.DashStyle = DashStyle.Solid;
                this.GetGraphics().DrawEllipse(pen, cX - radius / 2, cY - radius / 2, radius, radius);
                this.drawNodeNum();
            }
        }

        public override void Translate(int x, int y, int xAmount, int yAmount)
        {
            this.cX += xAmount;
            this.cY += yAmount;
            foreach(Tuple<IEdge,INode > child in childs)
            {
                child.Item1.onNodeMove();
            }
        }

        public void addNeigbor(Tuple<IEdge, INode> child)
        {
            this.childs.Add(child);
        }

        public Point getOutLinePointFrom(int sX, int sY)
        {
            
            RectangleF rect = new RectangleF(cX-radius,cY-radius,radius*2,radius*2);
            PointF pt1 = new PointF(sX,sY);

            float cx = rect.Left + rect.Width / 2f;
            float cy = rect.Top + rect.Height / 2f;
            PointF pt2 = new PointF(0,0);

            rect.X -= cx;
            rect.Y -= cy;
            pt1.X -= cx;
            pt1.Y -= cy;

            // Get the semimajor and semiminor axes.
            float a = this.radius;
            float b = this.radius;

            // Calculate the quadratic parameters.
            float A = (pt2.X - pt1.X) * (pt2.X - pt1.X) / a / a +
                      (pt2.Y - pt1.Y) * (pt2.Y - pt1.Y) / b / b;
            float B = 2 * pt1.X * (pt2.X - pt1.X) / a / a +
                      2 * pt1.Y * (pt2.Y - pt1.Y) / b / b;
            float C = pt1.X * pt1.X / a / a + pt1.Y * pt1.Y / b / b - 1;

            // Make a list of t values.
            List<float> t_values = new List<float>();
            Point point = new Point();

            // Calculate the discriminant.
            float discriminant = B * B - 4 * A * C;
            float t = 0;
            if (discriminant == 0)
            {
                // One real solution.
                t = (-B / 2 / A);
            }
            else if (discriminant > 0)
            {
                t = (float)(-B - Math.Sqrt(discriminant)) / 2 / A;
            }
            int x = (int)(pt1.X + ((pt2.X - pt1.X) * t) + cx);
            int y = (int)(pt1.Y + ((pt2.Y - pt1.Y) * t) + cy);

            return new Point(x,y);
        }

        public void setVisit(bool isVisited)
        {
            this.isVisited = isVisited;
        }

        public bool getVisit()
        {
            return this.isVisited;
        }

        public List<Tuple<IEdge, INode>> GetChilds()
        {
            return this.childs;
        }

    }
}
