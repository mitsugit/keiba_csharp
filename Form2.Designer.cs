
namespace keiba_c
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.Button1 = new System.Windows.Forms.Button();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(62, 268);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(657, 35);
            this.ProgressBar1.TabIndex = 0;
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(62, 73);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 1;
            this.Button1.Text = "キャンセル";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Timer1
            // 
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(398, 172);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(0, 18);
            this.Label1.TabIndex = 2;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.ProgressBar1);
            this.Name = "Form2";
            this.Text = "ダウンロード";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar ProgressBar1;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.Timer Timer1;
        private System.Windows.Forms.Label Label1;
    }
}