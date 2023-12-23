namespace FinalProject_DBMSFin_
{
    partial class Table2Events
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Cornsilk;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(1, -2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 592);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Red;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button2.Location = new System.Drawing.Point(141, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(79, 40);
            this.button2.TabIndex = 8;
            this.button2.Text = "Del";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Delete Selected Row";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FloralWhite;
            this.panel3.Controls.Add(this.label1);
            this.panel3.Location = new System.Drawing.Point(11, 14);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(209, 145);
            this.panel3.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(19, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(172, 120);
            this.label1.TabIndex = 1;
            this.label1.Text = "Welcome to Schedule\r\nPage. This page is \r\nwhere you can edit \r\nand or delete\r\nexi" +
    "sting data directly\r\nfrom the database.";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(11, 534);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(46, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Cornsilk;
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Location = new System.Drawing.Point(234, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(651, 136);
            this.panel2.TabIndex = 1;
            // 
            // button5
            // 
            this.button5.BackgroundImage = global::FinalProject_DBMSFin_.Properties.Resources.refresh__1_;
            this.button5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button5.Location = new System.Drawing.Point(526, 96);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(44, 30);
            this.button5.TabIndex = 3;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(91, 96);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(429, 22);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(10, 96);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 30);
            this.button4.TabIndex = 0;
            this.button4.Text = "Search";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(244, 143);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(632, 427);
            this.dataGridView1.TabIndex = 2;
            // 
            // Table2Events
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 581);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Table2Events";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Table2Events";
            this.Load += new System.EventHandler(this.Table2Events_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}