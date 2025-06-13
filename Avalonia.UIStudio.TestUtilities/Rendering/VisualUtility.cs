using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using Avalonia.Platform;
using Avalonia.Skia;
using Avalonia.Layout;



namespace Avalonia.UIStudio.TestUtilities.Rendering
{
    public static class VisualUtility
    {
        public static void SaveVisualToPng(Visual visual, string filePath, Size? size = null)
        {
            var measuredSize = size ?? new Size(48, 48); // fallback default size

            if (visual is Control layoutable)
            {
                layoutable.Measure(measuredSize);
                layoutable.Arrange(new Rect(measuredSize));
            }

            var pixelSize = new PixelSize((int)measuredSize.Width, (int)measuredSize.Height);
            var dpi = new Vector(96, 96);

            var rtb = new RenderTargetBitmap(pixelSize, dpi);

            using (var ctx = rtb.CreateDrawingContext(clear: true))
            {
                visual.Render(ctx);
            }

            using var fs = File.Create(filePath);
            rtb.Save(fs);
        }


    }
}
