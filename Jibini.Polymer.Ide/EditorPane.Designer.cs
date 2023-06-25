namespace Jibini.Polymer.Ide
{
    partial class EditorPane
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorPane));
            richText = new RichTextBox();
            SuspendLayout();
            // 
            // richText
            // 
            richText.AcceptsTab = true;
            richText.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            richText.AutoWordSelection = true;
            richText.BackColor = Color.Black;
            richText.BorderStyle = BorderStyle.None;
            richText.Font = new Font("Source Code Pro Medium", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            richText.ForeColor = Color.White;
            richText.HideSelection = false;
            richText.Location = new Point(0, 0);
            richText.Margin = new Padding(0);
            richText.Name = "richText";
            richText.Size = new Size(150, 150);
            richText.TabIndex = 3;
            richText.Text = resources.GetString("richText.Text");
            richText.WordWrap = false;
            richText.ZoomFactor = 1.2F;
            richText.TextChanged += richText_TextChanged;
            // 
            // EditorPane
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(richText);
            Name = "EditorPane";
            Load += EditorPane_Load;
            ResumeLayout(false);
        }

        #endregion

        public RichTextBox richText;
    }
}
