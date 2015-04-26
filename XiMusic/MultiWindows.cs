using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace XiMusic
{

    public class Multiwindows
    {
        private MultiwindowsPosition Pos;       //位置属性
        private Form MainForm, ChildForm;
        private bool isFirstPos;            //是否第一次定位ChildForm子窗体
        public int step;            //磁性子窗体ChildForm移动步长
        public Point locationPoint; //定位坐标
        protected delegate void LocationEventHandler();       //移动窗体的委托

        public Multiwindows(Form mainfrm, Form childfrom, MultiwindowsPosition pos)
        {
            this.isFirstPos = false;
            step = 20;
            this.MainForm = mainfrm;
            this.ChildForm = childfrom;
            this.Pos = pos;
            this.MainForm.LocationChanged += new EventHandler(MainForm_LocationChange);
            this.ChildForm.LocationChanged += new EventHandler(ChildForm_LocationChange);
            this.MainForm.SizeChanged += new EventHandler(MainForm_SizeChanged);
            this.ChildForm.SizeChanged += new EventHandler(ChildForm_SizeChanged);
            this.MainForm.Load += new EventHandler(MainForm_Load);
            this.ChildForm.Load += new EventHandler(ChildForm_Load);
        }

        /// <summary>
        /// 加载主窗体时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainForm_Load(object sender, EventArgs e)
        {
            GetLocation();
        }

        /// <summary>
        /// 加载子窗体时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChildForm_Load(object sender, EventArgs e)
        {
            GetLocation();
        }

        /// <summary>
        /// 当MainForm的坐标改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainForm_LocationChange(object sender, EventArgs e)
        {
            GetLocation();
        }

        /// <summary>
        /// 当ChildForm的坐标改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChildForm_LocationChange(object sender, EventArgs e)
        {
            GetLocation();
        }

        /// <summary>
        /// 当MainForm的大小改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MainForm_SizeChanged(object sender, EventArgs e)
        {
            GetLocation();
        }

        /// <summary>
        /// 当ChildForm的大小改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChildForm_SizeChanged(object sender, EventArgs e)
        {
            GetLocation();
        }

        protected void GetLocation()
        {
            if (this.ChildForm == null)
                return;
            if (this.Pos == MultiwindowsPosition.Left)
                locationPoint = new Point(this.MainForm.Left - this.ChildForm.Width, this.MainForm.Top);
            else if (this.Pos == MultiwindowsPosition.Top)
                locationPoint = new Point(this.MainForm.Left, this.MainForm.Top - this.ChildForm.Top);
            else if (this.Pos == MultiwindowsPosition.Right)
                locationPoint = new Point(this.MainForm.Right, this.MainForm.Top);
            else if (this.Pos == MultiwindowsPosition.Bottom)
                locationPoint = new Point(this.MainForm.Left, this.MainForm.Bottom);
            this.ChildForm.Location = this.locationPoint;
        }

        /// <summary>
        /// 当子窗体坐标改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChildForm_LocationChanged(object sender, EventArgs e)
        {
            if (!this.isFirstPos)
            {
                this.isFirstPos = true;
                return;
            }
            //委托
            LocationEventHandler locationEventHandler = new LocationEventHandler(OnMove);
            //调用
            MainForm.BeginInvoke(locationEventHandler);
        }

        /// <summary>
        /// 移动子窗体
        /// </summary>
        protected void OnMove()
        {
            if (this.ChildForm.Left > this.locationPoint.X)
                if (this.ChildForm.Left - this.locationPoint.X > step)
                    this.ChildForm.Left -= step;
                else
                    this.ChildForm.Left = this.locationPoint.X;
            else if (this.ChildForm.Left < this.locationPoint.X)
                if (this.ChildForm.Left - this.locationPoint.X < -step)
                    this.ChildForm.Left += step;
                else
                    this.ChildForm.Left = this.locationPoint.X;
            if (this.ChildForm.Top > this.locationPoint.Y)
                if (this.ChildForm.Top - this.locationPoint.Y > step)
                    this.ChildForm.Top -= step;
                else
                    this.ChildForm.Top = this.locationPoint.Y;
            else if (this.ChildForm.Top < this.locationPoint.Y)
                if (this.ChildForm.Top - this.locationPoint.Y < -step)
                    this.ChildForm.Top += step;
                else
                    this.ChildForm.Top = this.locationPoint.Y;

        }
    }

    public enum MultiwindowsPosition
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Bottom = 3
    }
}

