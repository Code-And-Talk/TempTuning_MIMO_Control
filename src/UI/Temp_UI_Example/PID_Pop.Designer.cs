
namespace Temp_UI_Example
{
    partial class PID_Pop
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
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tboxALL_P = new System.Windows.Forms.TextBox();
            this.tboxALL_I = new System.Windows.Forms.TextBox();
            this.tboxALL_D = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label5.Font = new System.Drawing.Font("굴림", 12F);
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 48);
            this.label5.TabIndex = 74;
            this.label5.Text = "P";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label6.Font = new System.Drawing.Font("굴림", 12F);
            this.label6.Location = new System.Drawing.Point(120, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 48);
            this.label6.TabIndex = 79;
            this.label6.Text = "I";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label7.Font = new System.Drawing.Font("굴림", 12F);
            this.label7.Location = new System.Drawing.Point(232, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(104, 48);
            this.label7.TabIndex = 84;
            this.label7.Text = "D";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tboxALL_P
            // 
            this.tboxALL_P.Font = new System.Drawing.Font("굴림", 12F);
            this.tboxALL_P.Location = new System.Drawing.Point(8, 64);
            this.tboxALL_P.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tboxALL_P.Multiline = true;
            this.tboxALL_P.Name = "tboxALL_P";
            this.tboxALL_P.Size = new System.Drawing.Size(105, 48);
            this.tboxALL_P.TabIndex = 85;
            this.tboxALL_P.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxALL_I
            // 
            this.tboxALL_I.Font = new System.Drawing.Font("굴림", 12F);
            this.tboxALL_I.Location = new System.Drawing.Point(120, 64);
            this.tboxALL_I.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tboxALL_I.Multiline = true;
            this.tboxALL_I.Name = "tboxALL_I";
            this.tboxALL_I.Size = new System.Drawing.Size(105, 48);
            this.tboxALL_I.TabIndex = 86;
            this.tboxALL_I.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tboxALL_D
            // 
            this.tboxALL_D.Font = new System.Drawing.Font("굴림", 12F);
            this.tboxALL_D.Location = new System.Drawing.Point(232, 64);
            this.tboxALL_D.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tboxALL_D.Multiline = true;
            this.tboxALL_D.Name = "tboxALL_D";
            this.tboxALL_D.Size = new System.Drawing.Size(105, 48);
            this.tboxALL_D.TabIndex = 87;
            this.tboxALL_D.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 64);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 48);
            this.button1.TabIndex = 88;
            this.button1.Text = "APPLY";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PID_Pop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 125);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tboxALL_D);
            this.Controls.Add(this.tboxALL_I);
            this.Controls.Add(this.tboxALL_P);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Name = "PID_Pop";
            this.Text = "PID_Pop";
            this.Load += new System.EventHandler(this.PID_Pop_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tboxALL_P;
        private System.Windows.Forms.TextBox tboxALL_I;
        private System.Windows.Forms.TextBox tboxALL_D;
        private System.Windows.Forms.Button button1;
    }
}