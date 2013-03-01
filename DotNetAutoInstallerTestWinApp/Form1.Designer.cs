namespace DotNetAutoInstallerTestWinApp
{
    partial class Form1
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
            this.txtJSON = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtJSON
            // 
            this.txtJSON.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJSON.Location = new System.Drawing.Point(12, 12);
            this.txtJSON.Multiline = true;
            this.txtJSON.Name = "txtJSON";
            this.txtJSON.Size = new System.Drawing.Size(457, 160);
            this.txtJSON.TabIndex = 0;
            this.txtJSON.Text = "{\r\n\t\"LastName\" : \"Doe\", \r\n\t\"Frederic\" : \"Joe\",\r\n\t\"Age\" : 48,\r\n\t\"BirthDate\" : \"196" +
    "4-12-11T00:00:00Z\",\r\n\t\"Other\" : null\r\n}\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(205, 199);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Parse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 249);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtJSON);
            this.Name = "Form1";
            this.Text = "DotNet Auto Installer Win App Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtJSON;
        private System.Windows.Forms.Button button1;
    }
}

