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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiAddVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRemoveVertex = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiAddVertex,
            this.tsmiRemoveVertex});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(180, 48);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // tsmiAddVertex
            // 
            this.tsmiAddVertex.Name = "tsmiAddVertex";
            this.tsmiAddVertex.Size = new System.Drawing.Size(180, 22);
            this.tsmiAddVertex.Text = "Добавить вершину";
            // 
            // tsmiRemoveVertex
            // 
            this.tsmiRemoveVertex.Name = "tsmiRemoveVertex";
            this.tsmiRemoveVertex.Size = new System.Drawing.Size(180, 22);
            this.tsmiRemoveVertex.Text = "Удалить вершину";
            // 
            // ViewUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "ViewUC";
            this.Size = new System.Drawing.Size(506, 378);
            this.Resize += new System.EventHandler(this.ViewUC_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiAddVertex;
        private System.Windows.Forms.ToolStripMenuItem tsmiRemoveVertex;
    }
}
