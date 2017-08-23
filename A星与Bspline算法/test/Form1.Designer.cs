namespace test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.导入地图 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.设置起始点 = new System.Windows.Forms.Button();
            this.设置终止点 = new System.Windows.Forms.Button();
            this.startPoint = new System.Windows.Forms.Label();
            this.endPoint = new System.Windows.Forms.Label();
            this.规划路径 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // 导入地图
            // 
            this.导入地图.Location = new System.Drawing.Point(308, 35);
            this.导入地图.Name = "导入地图";
            this.导入地图.Size = new System.Drawing.Size(306, 51);
            this.导入地图.TabIndex = 1;
            this.导入地图.Text = "导入地图";
            this.导入地图.UseVisualStyleBackColor = true;
            this.导入地图.Click += new System.EventHandler(this.导入地图_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(843, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // 设置起始点
            // 
            this.设置起始点.Location = new System.Drawing.Point(1306, 47);
            this.设置起始点.Name = "设置起始点";
            this.设置起始点.Size = new System.Drawing.Size(216, 39);
            this.设置起始点.TabIndex = 3;
            this.设置起始点.Text = "设置起始点";
            this.设置起始点.UseVisualStyleBackColor = true;
            this.设置起始点.Click += new System.EventHandler(this.设置起始点_Click);
            // 
            // 设置终止点
            // 
            this.设置终止点.Location = new System.Drawing.Point(1306, 227);
            this.设置终止点.Name = "设置终止点";
            this.设置终止点.Size = new System.Drawing.Size(216, 43);
            this.设置终止点.TabIndex = 4;
            this.设置终止点.Text = "设置终止点";
            this.设置终止点.UseVisualStyleBackColor = true;
            this.设置终止点.Click += new System.EventHandler(this.设置终止点_Click);
            // 
            // startPoint
            // 
            this.startPoint.AutoSize = true;
            this.startPoint.Location = new System.Drawing.Point(1302, 142);
            this.startPoint.Name = "startPoint";
            this.startPoint.Size = new System.Drawing.Size(130, 24);
            this.startPoint.TabIndex = 5;
            this.startPoint.Text = "startPoint";
            // 
            // endPoint
            // 
            this.endPoint.AutoSize = true;
            this.endPoint.Location = new System.Drawing.Point(1302, 345);
            this.endPoint.Name = "endPoint";
            this.endPoint.Size = new System.Drawing.Size(106, 24);
            this.endPoint.TabIndex = 6;
            this.endPoint.Text = "endPoint";
            // 
            // 规划路径
            // 
            this.规划路径.Location = new System.Drawing.Point(1306, 446);
            this.规划路径.Name = "规划路径";
            this.规划路径.Size = new System.Drawing.Size(216, 45);
            this.规划路径.TabIndex = 7;
            this.规划路径.Text = "规划路径";
            this.规划路径.UseVisualStyleBackColor = true;
            this.规划路径.Click += new System.EventHandler(this.规划路径_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1306, 591);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(184, 80);
            this.button1.TabIndex = 8;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Gray;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(61, 117);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1048, 1065);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1557, 1217);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.规划路径);
            this.Controls.Add(this.endPoint);
            this.Controls.Add(this.startPoint);
            this.Controls.Add(this.设置终止点);
            this.Controls.Add(this.设置起始点);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.导入地图);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button 导入地图;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button 设置起始点;
        private System.Windows.Forms.Button 设置终止点;
        private System.Windows.Forms.Label startPoint;
        private System.Windows.Forms.Label endPoint;
        private System.Windows.Forms.Button 规划路径;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

