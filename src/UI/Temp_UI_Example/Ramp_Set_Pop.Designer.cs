
namespace Temp_UI_Example
{
    partial class Ramp_Set_Pop
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
            this.label6 = new System.Windows.Forms.Label();
            this.ALL_TB16 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label6.Font = new System.Drawing.Font("굴림", 12F);
            this.label6.Location = new System.Drawing.Point(8, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(136, 56);
            this.label6.TabIndex = 85;
            this.label6.Text = "ALL Ramp Set";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ALL_TB16
            // 
            this.ALL_TB16.BackColor = System.Drawing.SystemColors.Info;
            this.ALL_TB16.Font = new System.Drawing.Font("굴림", 12F);
            this.ALL_TB16.Location = new System.Drawing.Point(8, 72);
            this.ALL_TB16.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ALL_TB16.Multiline = true;
            this.ALL_TB16.Name = "ALL_TB16";
            this.ALL_TB16.Size = new System.Drawing.Size(136, 64);
            this.ALL_TB16.TabIndex = 129;
            this.ALL_TB16.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 40);
            this.button1.TabIndex = 130;
            this.button1.Text = "APPLY";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Ramp_Set_Pop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(152, 189);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ALL_TB16);
            this.Controls.Add(this.label6);
            this.Name = "Ramp_Set_Pop";
            this.Text = "Ramp_Set_Pop";
            this.Load += new System.EventHandler(this.Ramp_Set_Pop_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ALL_TB16;
        private System.Windows.Forms.Button button1;
    }
}