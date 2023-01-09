namespace Triangulation
{
    partial class ViewForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewUC1 = new Triangulation.ViewUC();
            this.SuspendLayout();
            // 
            // viewUC1
            // 
            this.viewUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewUC1.Location = new System.Drawing.Point(12, 12);
            this.viewUC1.Name = "viewUC1";
            this.viewUC1.Size = new System.Drawing.Size(776, 426);
            this.viewUC1.TabIndex = 0;
            this.viewUC1.VertexCount = 0;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.viewUC1);
            this.Name = "ViewForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Триангуляция";
            this.Load += new System.EventHandler(this.ViewForm_Load);
            this.SizeChanged += new System.EventHandler(this.ViewForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewUC viewUC1;
    }
}

