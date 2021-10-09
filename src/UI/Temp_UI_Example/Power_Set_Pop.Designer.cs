
namespace Temp_UI_Example
{
    partial class Power_Set_Pop
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
            this.All_TB20 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label5.Font = new System.Drawing.Font("굴림", 12F);
            this.label5.Location = new System.Drawing.Point(8, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(144, 72);
            this.label5.TabIndex = 80;
            this.label5.Text = "ALL Power Set";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // All_TB20
            // 
            this.All_TB20.BackColor = System.Drawing.SystemColors.Info;
            this.All_TB20.Font = new System.Drawing.Font("굴림", 12F);
            this.All_TB20.Location = new System.Drawing.Point(8, 88);
            this.All_TB20.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.All_TB20.Multiline = true;
            this.All_TB20.Name = "All_TB20";
            this.All_TB20.Size = new System.Drawing.Size(144, 80);
            this.All_TB20.TabIndex = 133;
            this.All_TB20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(160, 128);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 40);
            this.button1.TabIndex = 134;
            this.button1.Text = "APPLY";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Power_Set_Pop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 183);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.All_TB20);
            this.Controls.Add(this.label5);
            this.Name = "Power_Set_Pop";
            this.Text = "Power_Set_Pop";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox All_TB20;
        private System.Windows.Forms.Button button1;
    }
}