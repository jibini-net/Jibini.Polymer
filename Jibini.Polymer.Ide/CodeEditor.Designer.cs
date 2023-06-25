﻿namespace Jibini.Polymer.Ide
{
    partial class CodeEditor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            editorPane = new EditorPane();
            jsonOutput = new TextBox();
            SuspendLayout();
            // 
            // editorPane
            // 
            editorPane.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            editorPane.AutoScroll = true;
            editorPane.Location = new Point(0, 0);
            editorPane.Margin = new Padding(0);
            editorPane.Name = "editorPane";
            editorPane.Size = new Size(532, 450);
            editorPane.TabIndex = 1;
            editorPane.SourceChanged += editorPane_SourceChanged;
            // 
            // jsonOutput
            // 
            jsonOutput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            jsonOutput.Font = new Font("Courier New", 9F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            jsonOutput.Location = new Point(532, 0);
            jsonOutput.Margin = new Padding(0);
            jsonOutput.Multiline = true;
            jsonOutput.Name = "jsonOutput";
            jsonOutput.ScrollBars = ScrollBars.Both;
            jsonOutput.Size = new Size(270, 450);
            jsonOutput.TabIndex = 2;
            // 
            // CodeEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(jsonOutput);
            Controls.Add(editorPane);
            Name = "CodeEditor";
            Text = "Code Editor";
            Load += form_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private EditorPane editorPane;
        private TextBox jsonOutput;
    }
}