using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DungeonSim
{
    /// <summary>
    /// Draws a hexagonal grid of explored hexes with minimal summary info (coord + biome).
    /// Uses flat-top hexagons and axial coordinates (q, r).
    /// </summary>
    public class HexGridPanel : Panel
    {
        private HexMap? _hexMap;
        private const int HexSize = 28;
        private static readonly float Sqrt3 = (float)Math.Sqrt(3);

        public HexMap? HexMap
        {
            get => _hexMap;
            set { _hexMap = value; Invalidate(); }
        }

        public HexGridPanel()
        {
            BackColor = Color.Black;
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_hexMap == null) return;

            var explored = _hexMap.GetAllExploredHexes();
            if (explored.Count == 0) return;

            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Find bounds in axial coords to center the grid
            int minQ = explored.Min(h => h.Coordinate.q);
            int maxQ = explored.Max(h => h.Coordinate.q);
            int minR = explored.Min(h => h.Coordinate.r);
            int maxR = explored.Max(h => h.Coordinate.r);

            // Flat-top axial to pixel: x = size*sqrt3*(q + r/2), y = size*3/2*r
            float scale = HexSize;
            float hexWidth = Sqrt3 * scale;
            float halfHeight = scale * 1.5f;

            float minPx = float.MaxValue, minPy = float.MaxValue, maxPx = float.MinValue, maxPy = float.MinValue;
            foreach (var h in explored)
            {
                float px = scale * Sqrt3 * (h.Coordinate.q + h.Coordinate.r / 2f);
                float py = scale * 1.5f * h.Coordinate.r;
                minPx = Math.Min(minPx, px); maxPx = Math.Max(maxPx, px);
                minPy = Math.Min(minPy, py); maxPy = Math.Max(maxPy, py);
            }
            float totalW = maxPx - minPx + hexWidth;
            float totalH = maxPy - minPy + 2 * halfHeight;
            float offsetX = Math.Max(0, (Width - totalW) / 2f) - minPx;
            float offsetY = Math.Max(0, (Height - totalH) / 2f) - minPy;

            using (var pen = new Pen(Color.White, 1.5f))
            using (var fillBrush = new SolidBrush(Color.FromArgb(30, 30, 30)))
            using (var textBrush = new SolidBrush(Color.White))
            using (var font = new Font("Consolas", 7f))
            {
                foreach (var hex in explored)
                {
                    float cx = offsetX + scale * Sqrt3 * (hex.Coordinate.q + hex.Coordinate.r / 2f) + hexWidth / 2f;
                    float cy = offsetY + scale * 1.5f * hex.Coordinate.r + halfHeight;

                    var points = GetHexPoints(cx, cy, flatTop: true);
                    g.FillPolygon(fillBrush, points);
                    g.DrawPolygon(pen, points);

                    bool isCurrent = hex.Coordinate == _hexMap.CurrentPartyLocation;
                    bool isCapital = hex.Coordinate == _hexMap.CapitalLocation;
                    string label = $"{hex.Coordinate.q},{hex.Coordinate.r}";
                    string biomeAbbrev = GetBiomeAbbrev(hex.Biome);
                    if (isCapital) label += " C";
                    if (isCurrent) label += " *";

                    var text = label + Environment.NewLine + biomeAbbrev;
                    float halfWidth = hexWidth / 2f;
                    var rect = new RectangleF(cx - halfWidth + 2, cy - 10, hexWidth - 4, 22);
                    var sf = new StringFormat
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Center
                    };
                    g.DrawString(text, font, textBrush, rect, sf);
                }
            }
        }

        private static PointF[] GetHexPoints(float cx, float cy, bool flatTop)
        {
            var points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                float angle = (float)(Math.PI / 3 * i);
                if (flatTop) angle -= (float)(Math.PI / 6);
                points[i] = new PointF(
                    cx + HexSize * (float)Math.Cos(angle),
                    cy + HexSize * (float)Math.Sin(angle)
                );
            }
            return points;
        }

        private static string GetBiomeAbbrev(BiomeType biome)
        {
            return biome switch
            {
                BiomeType.Hills => "Hil",
                BiomeType.Plains => "Pla",
                BiomeType.Mountains => "Mnt",
                BiomeType.Forest => "For",
                BiomeType.Desert => "Des",
                BiomeType.Tundra => "Tun",
                BiomeType.Canyon => "Can",
                BiomeType.Lake => "Lak",
                BiomeType.Volcano => "Vol",
                BiomeType.Sinkhole => "Sink",
                BiomeType.SameAsPrevious => "Same",
                _ => biome.ToString().Length >= 3 ? biome.ToString().Substring(0, 3) : biome.ToString()
            };
        }
    }
}
