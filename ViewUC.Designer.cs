namespace Triangulation
{
    partial class ViewUC
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemoveVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu
            // 
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddVertex,
            this.tsmiRemoveVertex});
            this.contextMenu.Name = "contextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(180, 48);
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // tsmiAddVertex
            // 
            this.tsmiAddVertex.Name = "tsmiAddVertex";
            this.tsmiAddVertex.Size = new System.Drawing.Size(180, 22);
            this.tsmiAddVertex.Text = "Добавить вершину";
            this.tsmiAddVertex.Click += new System.EventHandler(this.tsmiAddVertex_Click);
            // 
            // tsmiRemoveVertex
            // 
            this.tsmiRemoveVertex.Name = "tsmiRemoveVertex";
            this.tsmiRemoveVertex.Size = new System.Drawing.Size(180, 22);
            this.tsmiRemoveVertex.Text = "Удалить вершину";
            this.tsmiRemoveVertex.Click += new System.EventHandler(this.tsmiRemoveVertex_Click);
            // 
            // ViewUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenu;
            this.MinimumSize = new System.Drawing.Size(200, 100);
            this.Name = "ViewUC";
            this.Size = new System.Drawing.Size(405, 216);
            this.Load += new System.EventHandler(this.ViewUC_Load);
            this.Resize += new System.EventHandler(this.ViewUC_Resize);
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddVertex;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemoveVertex;
    }
}
