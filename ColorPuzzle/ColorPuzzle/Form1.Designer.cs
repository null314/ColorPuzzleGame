namespace ColorPuzzle
{
	partial class Form1
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.ResetButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.ScoreLabel = new System.Windows.Forms.Label();
			this.ColorCountLabel = new System.Windows.Forms.Label();
			this.PairLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// ResetButton
			// 
			this.ResetButton.Location = new System.Drawing.Point(464, 12);
			this.ResetButton.Name = "ResetButton";
			this.ResetButton.Size = new System.Drawing.Size(75, 23);
			this.ResetButton.TabIndex = 0;
			this.ResetButton.Text = "RESET";
			this.ResetButton.UseVisualStyleBackColor = true;
			this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
			// 
			// AddButton
			// 
			this.AddButton.Location = new System.Drawing.Point(12, 12);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(75, 23);
			this.AddButton.TabIndex = 1;
			this.AddButton.Text = "ADD";
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
			// 
			// ScoreLabel
			// 
			this.ScoreLabel.AutoSize = true;
			this.ScoreLabel.BackColor = System.Drawing.Color.White;
			this.ScoreLabel.Location = new System.Drawing.Point(185, 17);
			this.ScoreLabel.Name = "ScoreLabel";
			this.ScoreLabel.Size = new System.Drawing.Size(35, 13);
			this.ScoreLabel.TabIndex = 2;
			this.ScoreLabel.Text = "label1";
			// 
			// ColorCountLabel
			// 
			this.ColorCountLabel.AutoSize = true;
			this.ColorCountLabel.BackColor = System.Drawing.Color.White;
			this.ColorCountLabel.Location = new System.Drawing.Point(301, 17);
			this.ColorCountLabel.Name = "ColorCountLabel";
			this.ColorCountLabel.Size = new System.Drawing.Size(35, 13);
			this.ColorCountLabel.TabIndex = 3;
			this.ColorCountLabel.Text = "label1";
			// 
			// PairLabel
			// 
			this.PairLabel.AutoSize = true;
			this.PairLabel.BackColor = System.Drawing.Color.White;
			this.PairLabel.Location = new System.Drawing.Point(93, 17);
			this.PairLabel.Name = "PairLabel";
			this.PairLabel.Size = new System.Drawing.Size(35, 13);
			this.PairLabel.TabIndex = 4;
			this.PairLabel.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 809);
			this.Controls.Add(this.PairLabel);
			this.Controls.Add(this.ColorCountLabel);
			this.Controls.Add(this.ScoreLabel);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.ResetButton);
			this.DoubleBuffered = true;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Color Puzzle";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button ResetButton;
		private System.Windows.Forms.Button AddButton;
		private System.Windows.Forms.Label ScoreLabel;
		private System.Windows.Forms.Label ColorCountLabel;
		private System.Windows.Forms.Label PairLabel;
	}
}

