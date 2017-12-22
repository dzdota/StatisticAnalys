namespace testgistogr
{
    partial class CreatData
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Q1textBox = new System.Windows.Forms.TextBox();
            this.Q1label = new System.Windows.Forms.Label();
            this.Q2label = new System.Windows.Forms.Label();
            this.Q2textBox = new System.Windows.Forms.TextBox();
            this.Nlabel = new System.Windows.Forms.Label();
            this.NtextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Namelabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Q3textBox = new System.Windows.Forms.TextBox();
            this.Q3label = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(274, 19);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(158, 21);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Тип";
            // 
            // Q1textBox
            // 
            this.Q1textBox.Location = new System.Drawing.Point(274, 72);
            this.Q1textBox.Name = "Q1textBox";
            this.Q1textBox.Size = new System.Drawing.Size(155, 20);
            this.Q1textBox.TabIndex = 2;
            this.Q1textBox.Text = "1";
            // 
            // Q1label
            // 
            this.Q1label.AutoSize = true;
            this.Q1label.Location = new System.Drawing.Point(9, 72);
            this.Q1label.Name = "Q1label";
            this.Q1label.Size = new System.Drawing.Size(21, 13);
            this.Q1label.TabIndex = 1;
            this.Q1label.Text = "Q1";
            // 
            // Q2label
            // 
            this.Q2label.AutoSize = true;
            this.Q2label.Location = new System.Drawing.Point(9, 98);
            this.Q2label.Name = "Q2label";
            this.Q2label.Size = new System.Drawing.Size(21, 13);
            this.Q2label.TabIndex = 1;
            this.Q2label.Text = "Q2";
            // 
            // Q2textBox
            // 
            this.Q2textBox.Location = new System.Drawing.Point(274, 98);
            this.Q2textBox.Name = "Q2textBox";
            this.Q2textBox.Size = new System.Drawing.Size(155, 20);
            this.Q2textBox.TabIndex = 2;
            this.Q2textBox.Text = "1";
            // 
            // Nlabel
            // 
            this.Nlabel.AutoSize = true;
            this.Nlabel.Location = new System.Drawing.Point(9, 46);
            this.Nlabel.Name = "Nlabel";
            this.Nlabel.Size = new System.Drawing.Size(26, 13);
            this.Nlabel.TabIndex = 1;
            this.Nlabel.Text = "Тип";
            // 
            // NtextBox
            // 
            this.NtextBox.Location = new System.Drawing.Point(274, 46);
            this.NtextBox.Name = "NtextBox";
            this.NtextBox.Size = new System.Drawing.Size(155, 20);
            this.NtextBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(366, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Створити";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Namelabel
            // 
            this.Namelabel.AutoSize = true;
            this.Namelabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.Namelabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Namelabel.Location = new System.Drawing.Point(0, 0);
            this.Namelabel.Name = "Namelabel";
            this.Namelabel.Size = new System.Drawing.Size(16, 16);
            this.Namelabel.TabIndex = 1;
            this.Namelabel.Text = "1";
            this.Namelabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Q1textBox);
            this.groupBox1.Controls.Add(this.comboBox2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.NtextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Nlabel);
            this.groupBox1.Controls.Add(this.Q1label);
            this.groupBox1.Controls.Add(this.Q3textBox);
            this.groupBox1.Controls.Add(this.Q2textBox);
            this.groupBox1.Controls.Add(this.Q3label);
            this.groupBox1.Controls.Add(this.Q2label);
            this.groupBox1.Location = new System.Drawing.Point(3, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 193);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "программу",
            "файл"});
            this.comboBox2.Location = new System.Drawing.Point(333, 154);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(96, 21);
            this.comboBox2.TabIndex = 0;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(261, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Створити в ";
            // 
            // Q3textBox
            // 
            this.Q3textBox.Location = new System.Drawing.Point(274, 124);
            this.Q3textBox.Name = "Q3textBox";
            this.Q3textBox.Size = new System.Drawing.Size(155, 20);
            this.Q3textBox.TabIndex = 2;
            this.Q3textBox.Text = "1";
            // 
            // Q3label
            // 
            this.Q3label.AutoSize = true;
            this.Q3label.Location = new System.Drawing.Point(9, 124);
            this.Q3label.Name = "Q3label";
            this.Q3label.Size = new System.Drawing.Size(21, 13);
            this.Q3label.TabIndex = 1;
            this.Q3label.Text = "Q3";
            // 
            // CreatData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 261);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Namelabel);
            this.Name = "CreatData";
            this.Text = "Створення даних";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Q1textBox;
        private System.Windows.Forms.Label Q1label;
        private System.Windows.Forms.Label Q2label;
        private System.Windows.Forms.TextBox Q2textBox;
        private System.Windows.Forms.Label Nlabel;
        private System.Windows.Forms.TextBox NtextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label Namelabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Q3textBox;
        private System.Windows.Forms.Label Q3label;
    }
}