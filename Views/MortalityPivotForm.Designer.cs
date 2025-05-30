namespace Apos_AquaProductManageApp.Views
{
    partial class MortalityPivotForm
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
            LoadPivot = new Button();
            SuspendLayout();
            // 
            // LoadPivot
            // 
            LoadPivot.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            LoadPivot.Location = new Point(20, 396);
            LoadPivot.Name = "LoadPivot";
            LoadPivot.Size = new Size(94, 29);
            LoadPivot.TabIndex = 0;
            LoadPivot.Text = "Load Pivot";
            LoadPivot.UseVisualStyleBackColor = true;
            LoadPivot.Click += LoadPivot_Click;
            // 
            // MortalityPivotForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LoadPivot);
            Name = "MortalityPivotForm";
            Text = "MortalityPivotForm";
            ResumeLayout(false);
        }

        #endregion

        private Button LoadPivot;
    }
}