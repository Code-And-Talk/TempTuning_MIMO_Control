
namespace Temp_UI_Example
{
    partial class Temp_Set_Pop
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
            this.label18 = new System.Windows.Forms.Label();
            this.All_TB12 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.label18.Font = new System.Drawing.Font("굴림", 12F);
            this.label18.Location = new System.Drawing.Point(8, 8);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(136, 56);
            this.label18.TabIndex = 75;
            this.label18.Text = "ALL Temp Set";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // All_TB12
            // 
            this.All_TB12.BackColor = System.Drawing.SystemColors.Info;
            this.All_TB12.Font = new System.Drawing.Font("굴림", 12F);
            this.All_TB12.Location = new System.Drawing.Point(8, 72);
            this.All_TB12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.All_TB12.Multiline = true;
            this.All_TB12.Name = "All_TB12";
            this.All_TB12.Size = new System.Drawing.Size(136, 64);
            this.All_TB12.TabIndex = 76;
            this.All_TB12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(8, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 40);
            this.button1.TabIndex = 77;
            this.button1.Text = "APPLY";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Temp_Set_Pop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(152, 189);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.All_TB12);
            this.Controls.Add(this.label18);
            this.Name = "Temp_Set_Pop";
            this.Text = "Temp_Set_Pop";
            this.Load += new System.EventHandler(this.Temp_Set_Pop_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox All_TB12;
        private System.Windows.Forms.Button button1;
    }
}