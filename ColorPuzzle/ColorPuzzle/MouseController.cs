
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MouseLib
{
	public class MouseController
	{
		private Point MouseStart;

		private Func<bool> OnLeftDragStart;
		private Action<PointF> OnLeftDragProcess;
		private Action<PointF> OnLeftDragEnd;
		private Action<PointF> OnLeftClick;

		private bool LeftMouseDown;
		private bool LeftDrag;

		public PointF MouseCurrent { get; private set; }

		public MouseController()
		{
		}

		public void InitLeftDrag(Func<bool> onLeftDragStart, Action<PointF> onLeftDragProcess, Action<PointF> onLeftDragEnd)
		{
			OnLeftDragStart = onLeftDragStart;
			OnLeftDragProcess = onLeftDragProcess;
			OnLeftDragEnd = onLeftDragEnd;
		}

		public void InitLeftClick(Action<PointF> onLeftClick)
		{
			OnLeftClick = onLeftClick;
		}

		public void MouseDown(MouseEventArgs e)
		{
			MouseCurrent = e.Location;
			if (e.Button == MouseButtons.Left && OnLeftDragStart != null)
			{
				if (OnLeftDragStart())
				{
					LeftMouseDown = true;
					LeftDrag = false;
					MouseStart = e.Location;
				}
			}

			if (e.Button == MouseButtons.Left && OnLeftDragStart == null)
			{
				LeftMouseDown = true;
				LeftDrag = false;
				MouseStart = e.Location;
			}
		}

		public void MouseUp(MouseEventArgs e)
		{
			MouseCurrent = e.Location;
			if (e.Button == MouseButtons.Left && LeftMouseDown)
			{
				if (LeftDrag)
				{
					var shift = new PointF(e.Location.X - MouseStart.X, e.Location.Y - MouseStart.Y);
					OnLeftDragEnd(shift);
					LeftMouseDown = false;
				}
				else
				{
					if(OnLeftClick != null)
						OnLeftClick(e.Location);

					LeftMouseDown = false;
				}
			}
		}

		public void MouseMove(MouseEventArgs e)
		{
			MouseCurrent = e.Location;
			if (LeftMouseDown)
			{
				var shift = new PointF(e.Location.X - MouseStart.X, e.Location.Y - MouseStart.Y);
				if (Len(shift) > 6 && LeftDrag == false && OnLeftDragStart != null)
				{
					LeftDrag = true;
					OnLeftDragStart();
				}

				if (LeftDrag)
					OnLeftDragProcess(shift);
			}
		}

		private static float Len(PointF point)
		{
			return (float)Math.Sqrt(point.X * point.X + point.Y * point.Y);
		}
	}
}
