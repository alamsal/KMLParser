namespace ParseKml
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Input = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.Button();
            this.txtbxInput = new System.Windows.Forms.TextBox();
            this.txtbxOutput = new System.Windows.Forms.TextBox();
            this.Convert = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "\"kml files (*.kml)|*.kml|All files (*.*)|*.*\"";
            this.openFileDialog1.InitialDirectory = "\"c:\\\\\"";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // Input
            // 
            this.Input.Location = new System.Drawing.Point(246, 102);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(75, 23);
            this.Input.TabIndex = 0;
            this.Input.Text = "Input";
            this.Input.UseVisualStyleBackColor = true;
            this.Input.Click += new System.EventHandler(this.Input_Click);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(246, 160);
            this.Output.Name = "Output";
            this.Output.Size = new System.Drawing.Size(75, 23);
            this.Output.TabIndex = 1;
            this.Output.Text = "Output";
            this.Output.UseVisualStyleBackColor = true;
            this.Output.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtbxInput
            // 
            this.txtbxInput.Location = new System.Drawing.Point(30, 105);
            this.txtbxInput.Name = "txtbxInput";
            this.txtbxInput.Size = new System.Drawing.Size(199, 20);
            this.txtbxInput.TabIndex = 2;
            // 
            // txtbxOutput
            // 
            this.txtbxOutput.Location = new System.Drawing.Point(30, 162);
            this.txtbxOutput.Name = "txtbxOutput";
            this.txtbxOutput.Size = new System.Drawing.Size(199, 20);
            this.txtbxOutput.TabIndex = 3;
            // 
            // Convert
            // 
            this.Convert.Location = new System.Drawing.Point(154, 266);
            this.Convert.Name = "Convert";
            this.Convert.Size = new System.Drawing.Size(75, 23);
            this.Convert.TabIndex = 4;
            this.Convert.Text = "Convert";
            this.Convert.UseVisualStyleBackColor = true;
            this.Convert.Click += new System.EventHandler(this.Convert_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "\"kml files (*.kml)|*.kml|All files (*.*)|*.*\"";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 215);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select I/O files and click \"Convert\"";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 397);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Convert);
            this.Controls.Add(this.txtbxOutput);
            this.Controls.Add(this.txtbxInput);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Input);
            this.Name = "Form1";
            this.Text = "CherryLogistics KML Formatter";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button Input;
        private System.Windows.Forms.Button Output;
        private System.Windows.Forms.TextBox txtbxInput;
        private System.Windows.Forms.TextBox txtbxOutput;
        private System.Windows.Forms.Button Convert;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label1;
    }
}

