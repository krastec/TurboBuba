using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace CommandCenter.UI.OrderBook
{
    public class OrderBookCanvas : Panel
    {
        public class PriceLevel
        {
            public decimal Price { get; set; }
            public decimal Quantity { get; set; }
        }

        // Data
        private List<PriceLevel> bids = new();
        private List<PriceLevel> asks = new();
        private decimal midPrice;

        // Rendering settings
        private const int RowHeight = 20;
        private const float AutoCenterThreshold = 0.3f; // 30% from center

        // Scrolling
        private int scrollOffset = 0;
        private bool isMouseOver = false;

        // Cached resources
        private readonly Font priceFont = new("Consolas", 9f, FontStyle.Regular);
        private readonly Font quantityFont = new("Consolas", 9f, FontStyle.Regular);
        private readonly Brush bidBrush = new SolidBrush(Color.FromArgb(0, 150, 0));
        private readonly Brush askBrush = new SolidBrush(Color.FromArgb(200, 0, 0));
        private readonly Brush bidBackgroundBrush = new SolidBrush(Color.FromArgb(30, 0, 100, 0));
        private readonly Brush askBackgroundBrush = new SolidBrush(Color.FromArgb(30, 100, 0, 0));
        private readonly Brush textBrush = new SolidBrush(Color.White);
        private readonly Brush midPriceBrush = new SolidBrush(Color.Yellow);
        private readonly Pen midPriceLine = new(Color.FromArgb(150, 255, 255, 0), 2f);
        private readonly Brush backgroundBrush = new SolidBrush(Color.FromArgb(20, 20, 30));

        // Test data
        private readonly Random random = new();
        private System.Windows.Forms.Timer? testDataTimer;

        public OrderBookCanvas()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.UserPaint | 
                     ControlStyles.AllPaintingInWmPaint | 
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            MouseEnter += (s, e) => isMouseOver = true;
            MouseLeave += (s, e) => isMouseOver = false;
            MouseWheel += OnMouseWheelScroll;
        }

        public void UpdateData(PriceLevel[] newBids, PriceLevel[] newAsks)
        {
            if (newBids != null && newBids.Length > 0)
            {
                bids = newBids.OrderByDescending(b => b.Price).ToList();
            }

            if (newAsks != null && newAsks.Length > 0)
            {
                asks = newAsks.OrderByDescending(a => a.Price).ToList();
            }

            UpdateMidPrice();
            AutoCenterIfNeeded();
            Invalidate();
        }

        private void UpdateMidPrice()
        {
            if (bids.Count > 0 && asks.Count > 0)
            {
                var bestBid = bids.First().Price;
                var bestAsk = asks.Last().Price;
                midPrice = (bestBid + bestAsk) / 2;
            }
            else if (bids.Count > 0)
            {
                midPrice = bids.First().Price;
            }
            else if (asks.Count > 0)
            {
                midPrice = asks.Last().Price;
            }
        }

        private void AutoCenterIfNeeded()
        {
            if (isMouseOver) return;

            int centerY = Height / 2;
            int midPriceY = GetYPositionForPrice(midPrice);
            int offset = midPriceY - centerY;

            // Check if mid price is more than 30% away from center
            if (Math.Abs(offset) > Height * AutoCenterThreshold)
            {
                scrollOffset += offset;
            }
        }

        private int GetYPositionForPrice(decimal price)
        {
            // Find the index in the combined sorted list
            var allPrices = asks.Concat(bids).OrderByDescending(p => p.Price).ToList();
            int index = allPrices.FindIndex(p => p.Price == price);
            
            if (index < 0 && allPrices.Count > 0)
            {
                // Find closest price
                index = allPrices.Select((p, i) => new { p, i })
                    .OrderBy(x => Math.Abs(x.p.Price - price))
                    .First().i;
            }

            return index * RowHeight - scrollOffset;
        }

        private void OnMouseWheelScroll(object? sender, MouseEventArgs e)
        {
            scrollOffset -= e.Delta / 4; // Adjust scroll sensitivity
            scrollOffset = Math.Max(0, scrollOffset); // Prevent negative scroll
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.None;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            g.PixelOffsetMode = PixelOffsetMode.HighSpeed;

            // Background
            g.FillRectangle(backgroundBrush, ClientRectangle);

            if (bids.Count == 0 && asks.Count == 0)
            {
                DrawNoData(g);
                return;
            }

            // Combine and sort all levels
            var allLevels = new List<(decimal Price, decimal Quantity, bool IsBid)>();
            
            foreach (var ask in asks)
                allLevels.Add((ask.Price, ask.Quantity, false));
            
            foreach (var bid in bids)
                allLevels.Add((bid.Price, bid.Quantity, true));

            allLevels = allLevels.OrderByDescending(l => l.Price).ToList();

            // Calculate max quantity for bar scaling
            decimal maxQuantity = allLevels.Max(l => l.Quantity);

            // Draw visible rows
            int startRow = Math.Max(0, scrollOffset / RowHeight);
            int endRow = Math.Min(allLevels.Count, startRow + (Height / RowHeight) + 2);

            for (int i = startRow; i < endRow; i++)
            {
                var level = allLevels[i];
                int y = i * RowHeight - scrollOffset;

                if (y < -RowHeight || y > Height) continue;

                DrawPriceLevel(g, level.Price, level.Quantity, level.IsBid, y, maxQuantity);
            }

            // Draw mid price line
            int midY = GetYPositionForPrice(midPrice);
            if (midY >= 0 && midY <= Height)
            {
                g.DrawLine(midPriceLine, 0, midY, Width, midY);
                
                // Draw mid price text
                string midText = $"≈ {midPrice:F2}";
                var midSize = g.MeasureString(midText, priceFont);
                g.FillRectangle(backgroundBrush, Width - midSize.Width - 10, midY - midSize.Height / 2, midSize.Width + 4, midSize.Height);
                g.DrawString(midText, priceFont, midPriceBrush, Width - midSize.Width - 8, midY - midSize.Height / 2);
            }

            // Draw scroll indicator
            if (isMouseOver)
            {
                DrawScrollIndicator(g);
            }
        }

        private void DrawPriceLevel(Graphics g, decimal price, decimal quantity, bool isBid, int y, decimal maxQuantity)
        {
            // Calculate bar width
            float barWidthPercent = maxQuantity > 0 ? (float)(quantity / maxQuantity) : 0;
            int barWidth = (int)(Width * 0.3f * barWidthPercent);

            // Draw background bar
            var bgBrush = isBid ? bidBackgroundBrush : askBackgroundBrush;
            g.FillRectangle(bgBrush, 0, y, barWidth, RowHeight);

            // Draw price (left side)
            var priceBrush = isBid ? bidBrush : askBrush;
            g.DrawString(price.ToString("F2"), priceFont, priceBrush, 10, y + 3);

            // Draw quantity (right side)
            string qtyText = quantity.ToString("F4");
            var qtySize = g.MeasureString(qtyText, quantityFont);
            g.DrawString(qtyText, quantityFont, textBrush, Width - qtySize.Width - 10, y + 3);
        }

        private void DrawNoData(Graphics g)
        {
            string text = "No data";
            var size = g.MeasureString(text, priceFont);
            g.DrawString(text, priceFont, textBrush, 
                (Width - size.Width) / 2, 
                (Height - size.Height) / 2);
        }

        private void DrawScrollIndicator(Graphics g)
        {
            var totalHeight = (bids.Count + asks.Count) * RowHeight;
            if (totalHeight <= Height) return;

            int scrollBarHeight = Math.Max(20, (int)((float)Height / totalHeight * Height));
            int scrollBarY = (int)((float)scrollOffset / totalHeight * Height);

            g.FillRectangle(new SolidBrush(Color.FromArgb(100, 255, 255, 255)), 
                Width - 8, scrollBarY, 6, scrollBarHeight);
        }

        // Test data generation
        public void StartTestDataGeneration(decimal basePrice = 50000m)
        {
            GenerateTestData(basePrice);

            testDataTimer?.Stop();
            testDataTimer = new System.Windows.Forms.Timer { Interval = 100 };
            testDataTimer.Tick += (s, e) => UpdateTestData();
            testDataTimer.Start();
        }

        public void StopTestDataGeneration()
        {
            testDataTimer?.Stop();
            testDataTimer?.Dispose();
            testDataTimer = null;
        }

        private void GenerateTestData(decimal basePrice)
        {
            bids.Clear();
            asks.Clear();

            // Generate 50 bid levels
            for (int i = 0; i < 50; i++)
            {
                bids.Add(new PriceLevel
                {
                    Price = basePrice - i * 0.5m - (decimal)(random.NextDouble() * 0.5),
                    Quantity = (decimal)(random.NextDouble() * 10 + 1)
                });
            }

            // Generate 50 ask levels
            for (int i = 0; i < 50; i++)
            {
                asks.Add(new PriceLevel
                {
                    Price = basePrice + i * 0.5m + (decimal)(random.NextDouble() * 0.5),
                    Quantity = (decimal)(random.NextDouble() * 10 + 1)
                });
            }

            bids = bids.OrderByDescending(b => b.Price).ToList();
            asks = asks.OrderByDescending(a => a.Price).ToList();

            UpdateMidPrice();
            AutoCenterIfNeeded();
            Invalidate();
        }

        private void UpdateTestData()
        {
            // Randomly update quantities and add/remove levels
            if (bids.Count > 0)
            {
                int idx = random.Next(bids.Count);
                bids[idx].Quantity = (decimal)(random.NextDouble() * 10 + 1);
            }

            if (asks.Count > 0)
            {
                int idx = random.Next(asks.Count);
                asks[idx].Quantity = (decimal)(random.NextDouble() * 10 + 1);
            }

            // Simulate price movement (small drift)
            if (random.NextDouble() > 0.7)
            {
                decimal drift = (decimal)(random.NextDouble() - 0.5) * 0.1m;
                
                foreach (var bid in bids)
                    bid.Price += drift;
                
                foreach (var ask in asks)
                    ask.Price += drift;

                UpdateMidPrice();
                AutoCenterIfNeeded();
            }

            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StopTestDataGeneration();
                priceFont?.Dispose();
                quantityFont?.Dispose();
                bidBrush?.Dispose();
                askBrush?.Dispose();
                bidBackgroundBrush?.Dispose();
                askBackgroundBrush?.Dispose();
                textBrush?.Dispose();
                midPriceBrush?.Dispose();
                midPriceLine?.Dispose();
                backgroundBrush?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
