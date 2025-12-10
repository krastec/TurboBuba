namespace CommandCenter.UI.OrderBook
{
    partial class OrderBookPanel
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
            orderBookCanvas = new OrderBookCanvas();
            SuspendLayout();
            // 
            // orderBookCanvas
            // 
            orderBookCanvas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            orderBookCanvas.Location = new Point(0, 0);
            orderBookCanvas.Name = "orderBookCanvas";
            orderBookCanvas.Size = new Size(308, 596);
            orderBookCanvas.TabIndex = 0;
            // 
            // OrderBookPanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(orderBookCanvas);
            Name = "OrderBookPanel";
            Size = new Size(308, 596);
            ResumeLayout(false);
        }

        #endregion

        private OrderBookCanvas orderBookCanvas;
    }
}
