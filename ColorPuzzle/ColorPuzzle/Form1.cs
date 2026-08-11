using MouseLib;
using System;	
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraverseHelperLib;

namespace ColorPuzzle
{
	public partial class Form1 : Form
	{
		private const int CornerX = 50;
		private const int CornerY = 50;
		private const int CellSize = 50;
		private const int RowCount = 50;
		private const int ColumnCount = 9;
		private const int ColorMax = 5;

		private int[,] Field = new int[RowCount, ColumnCount];
		private int FieldCount;

		private int SelectedRow = -1;
		private int SelectedColumn = -1;
		private int Pair;
		private int Score;
		private int ColorCount;
		private int TotalLeft;
		private bool GameOver = false;

		private float ShiftY;
		private float ShiftYStart;
		private readonly MouseController MouseController;
		private Pen Pen;

		public Form1()
		{
			InitializeComponent();
			Reset();
			MouseController = new MouseController();
			MouseController.InitLeftClick(Form1_MouseClick);
			MouseController.InitLeftDrag(OnLeftDragStart, OnLeftDragProcess, OnLeftDragEnd);

			Pen = new Pen(Color.Gray);
			Pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			var gr = e.Graphics;
			
			gr.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);
			gr.TranslateTransform(0, ShiftY);

			foreach (var i in FieldCount.Traverse())
			{
				var r = i / ColumnCount;
				var c = i % ColumnCount;

				var col = Field[r, c];
				var rect = GetRectangle(r, c);

				if (col > 0)
					gr.FillRectangle(new SolidBrush(GetColor(col)), rect);

				if (r == SelectedRow && c == SelectedColumn)
				{
					gr.DrawLine(Pens.Black, rect.X, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
					gr.DrawLine(Pens.Black, rect.X + rect.Width, rect.Y, rect.X, rect.Y + rect.Height);
				}

				if (GameOver == false && (ExistPair(i, 1) || ExistPair(i, ColumnCount) || ExistPair(i, -1) || ExistPair(i, -ColumnCount)))
				{
					gr.DrawEllipse(Pens.Black, rect);
				}

				if (i == FieldCount - 1)
				{
					gr.DrawLine(Pens.Black, 0, rect.Y + rect.Height, rect.X + rect.Width, rect.Y + rect.Height);
					gr.DrawLine(Pens.Black, rect.X + rect.Width, rect.Y, rect.X + rect.Width, rect.Y + rect.Height);
					gr.DrawLine(Pens.Black, rect.X + rect.Width, rect.Y, this.Width, rect.Y);
				}
			}

			if(SelectedRow >= 0 && SelectedColumn >= 0)
			{
				var r = 0;
				var c = 0;
				if (FindCoord((int)MouseController.MouseCurrent.X, (int)MouseController.MouseCurrent.Y, ref r, ref c))
				{
					var overIndex = GetIndex(r, c);

					if (overIndex < FieldCount && Field[r, c] != 0 && Field[r, c] == Field[SelectedRow, SelectedColumn])
					{
						var selectedIndex = GetIndex(SelectedRow, SelectedColumn);
						var rect1 = GetRectangle(r, c);
						var rect2 = GetRectangle(SelectedRow, SelectedColumn);
						if (c == SelectedColumn && IsLineEmpty(selectedIndex, overIndex, ColumnCount))
						{
							gr.DrawLine(Pen,
								rect1.X + rect1.Width / 2,
								rect1.Y + rect1.Height / 2,
								rect2.X + rect2.Width / 2,
								rect2.Y + rect2.Height / 2);
						}
						else if (IsLineEmpty(selectedIndex, overIndex, 1))
						{
							if (SelectedRow > r)
							{
								var rect = rect1;
								rect1 = rect2;
								rect2 = rect;
							}

							if (r == SelectedRow)
							{
								gr.DrawLine(Pen,
									rect1.X + rect1.Width / 2,
									rect1.Y + rect1.Height / 2,
									rect2.X + rect2.Width / 2,
									rect2.Y + rect2.Height / 2);
							}
							else
							{
								gr.DrawLine(Pen,
									rect2.X + rect2.Width / 2,
									rect2.Y + rect2.Height / 2,
									(ColumnCount +1)* CellSize,
									rect2.Y + rect2.Height / 2);

								gr.DrawLine(Pen,
									rect1.X + rect1.Width / 2,
									rect1.Y + rect1.Height / 2,
									CornerX,
									rect1.Y + rect1.Height / 2);
							}
						}
					}
				}
			}
		}

		private Rectangle GetRectangle(int r, int c)
		{
			return new Rectangle(CornerX + c * CellSize, CornerY+ r * CellSize, CellSize - 5, CellSize - 5);
		}

		private Color GetColor(int index)
		{
			switch (index)
			{
				case 1: return Color.LightGreen;
				case 2: return Color.LightBlue;
				case 3: return Color.LightPink;
				case 4: return Color.Orange;
				case 5: return Color.OrangeRed;
				default: throw new Exception();
			}
		}

		private void ResetButton_Click(object sender, EventArgs e)
		{
			Reset();
			Invalidate();
		}

		private void Reset()
		{ 
			var r = new Random();
			foreach (var i in 3.Traverse())
				foreach (var o in ColumnCount.Traverse())
				{
					Field[i, o] = 1 + r.Next(ColorMax);
				}

			ShiftY = 0;
			GameOver = false;
			FieldCount = 3 * ColumnCount;
			Pair = 0;
			Score = 0;
			CalcColors();
		}

		private void CalcColors()
		{
			var set = new HashSet<int>();
			foreach (var i in FieldCount.Traverse())
			{
				var r = i / ColumnCount;
				var c = i % ColumnCount;

				if (Field[r, c] != 0)
					set.Add(Field[r, c]);
			}
			ColorCount = set.Count();
			TotalLeft = set.Sum();

			PairLabel.Text = "Pairs: " + Pair.ToString();
			ScoreLabel.Text = "Score: " + Score.ToString();
			ColorCountLabel.Text = "Colors: " + ColorCount.ToString();
		}

		private bool IsMatch(int r1, int c1, int r2, int c2)
		{
			if (r1 == r2 && c1 == c2)
				return false;

			if (Field[r1, c1] == 0)
				return false;

			if (Field[r2, c2] == 0)
				return false;

			if (Field[r1, c1] != Field[r2, c2])
				return false;

			if (c1 == c2)
			{
				return IsLineEmpty(GetIndex(r1, c1), GetIndex(r2, c2), ColumnCount);
			}

			return IsLineEmpty(GetIndex(r1, c1), GetIndex(r2, c2), 1);
		}

		private int GetIndex(int r, int c)
		{
			return r * ColumnCount + c;
		}

		private bool ExistPair(int i1, int step)
		{
			var r1 = i1 / ColumnCount;
			var c1 = i1 % ColumnCount;
			for (var i2 = i1 + step; i2 < FieldCount && i2 >= 0; i2 += step)
			{
				var r2 = i2 / ColumnCount;
				var c2 = i2 % ColumnCount;
				if (Field[r2, c2] != 0)
				{
					return Field[r1, c1] == Field[r2, c2];
				}
			}
			return false;
		}

		private bool IsLineEmpty(int i1, int i2, int step)
		{
			if (i2 < i1)
			{
				var o = i1;
				i1 = i2;
				i2 = o;
			}

			for (var i = i1 + step; i < i2; i += step)
			{
				var r = i / ColumnCount;
				var c = i % ColumnCount;
				if (Field[r, c] != 0)
					return false;
			}
			return true;
		}


		private void Form1_MouseClick(PointF point)
		{
			var r = 0;
			var c = 0;

			if (FindCoord((int)point.X, (int)point.Y, ref r, ref c))
			{
				if (Field[r, c] == 0)
				{
					SelectedRow = -1;
					SelectedColumn = -1;
				}
				else if (SelectedRow >= 0 && SelectedColumn >= 0 && IsMatch(r, c, SelectedRow, SelectedColumn))
				{
					Score+= ColorCount;
					Pair++;
					Field[r, c] = 0;
					Field[SelectedRow, SelectedColumn] = 0;
					SelectedRow = -1;
					SelectedColumn = -1;
					RemoveEmptyRow();
					CalcColors();

					if (TotalLeft == 0)
					{
						GameOver = true;
						MessageBox.Show(string.Format("You Win!!!\n\nPairs: {0}\nScore: {1}", Pair, Score) , "CONGRATULATION", MessageBoxButtons.OK);
					}
				}
				else
				{
					SelectedRow = r;
					SelectedColumn = c;
				}
			}
			else
			{
				SelectedRow = -1;
				SelectedColumn = -1;
			}
			Invalidate();
		}

		private bool FindCoord(int x, int y, ref int row, ref int column)
		{
			y -= (int)ShiftY;
			foreach (var i in FieldCount.Traverse())
			{
				var r = i / ColumnCount;
				var c = i % ColumnCount;

				var rect = GetRectangle(r, c);

				if (x >= rect.X && x < rect.X + rect.Width &&
					y >= rect.Y && y < rect.Y + rect.Height)
				{
					row = r;
					column = c;
					return true;
				}
			}

			return false;
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			var end = FieldCount;

			foreach (var i in FieldCount.Traverse())
			{
				var r = i / ColumnCount;
				var c = i % ColumnCount;

				if (Field[r, c] != 0)
				{
					var r2 = end / ColumnCount;
					var c2 = end % ColumnCount;
					if (r2 >= RowCount)
					{
						GameOver = true;
						MessageBox.Show("GameOver", "", MessageBoxButtons.OK);
						break;
					}

					Field[r2, c2] = Field[r, c];
					end++;
				}
			}

			FieldCount = end;
			Invalidate();
		}

		private void RemoveEmptyRow()
		{
			for (var r = 0; r < (FieldCount / ColumnCount); r++)
			{
				while (ColumnCount.Traverse().All(c => Field[r, c] == 0) && (r+1) * ColumnCount <= FieldCount)
				{
					foreach (var i in Enumerable.Range(r * ColumnCount, FieldCount - r * ColumnCount))
					{
						var r2 = i / ColumnCount;
						var c2 = i % ColumnCount;
						Field[r2, c2] = Field[r2 + 1, c2];
					}
					FieldCount -= ColumnCount;
				}
			}
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			MouseController.MouseDown(e);
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			MouseController.MouseUp(e);
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			MouseController.MouseMove(e);
			Invalidate();
		}

		private bool OnLeftDragStart()
		{
			ShiftYStart = ShiftY;
			return true;
		}

		private void OnLeftDragProcess(PointF p)
		{
			ShiftY = ShiftYStart + p.Y;
			if (ShiftY > 0)
				ShiftY = 0;
			Invalidate();
		}

		private void OnLeftDragEnd(PointF p)
		{
			OnLeftDragProcess(p);
		}
	}
}
