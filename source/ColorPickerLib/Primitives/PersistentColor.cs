namespace ColorPickerLib.Primitives
{
    using ColorPickerLib.Core.Utilities;
    using System.Windows.Media;  

    public class PersistentColor
    {
        public double Hue { get; private set; }
        public double Saturation { get; private set; }
        public double Value { get; private set; }
        public Color DisplayColor { get; private set; }

        public PersistentColor(double hue, double saturation, double value, byte alpha = 255)
        {
            this.Hue = hue;
            this.Saturation = saturation;
            this.Value = value;
            this.DisplayColor = ColorUtilities.HsvToRgb(hue, saturation, value, alpha);
        }

        public PersistentColor(byte red, byte green, byte blue, byte alpha = 255)
        {
            double[] HsbValues = ColorUtilities.RgbToHsv(red, green, blue);
            this.Hue = HsbValues[0];
            this.Saturation = HsbValues[1];
            this.Value = HsbValues[2];
            DisplayColor = Color.FromArgb(alpha, red, green, blue);
        }

        public PersistentColor(Color? color)
        {
            if (color is null || !color.HasValue)
            {
                this.DisplayColor = Color.FromArgb(0, 0, 0, 255);
            }
            else
            {
                this.DisplayColor = Color.FromArgb(color.Value.A, color.Value.R, color.Value.G, color.Value.B);
            }
            double[] HsbValues = ColorUtilities.RgbToHsv(DisplayColor.R, DisplayColor.G, DisplayColor.B);
            this.Hue = HsbValues[0];
            this.Saturation = HsbValues[1];
            this.Value = HsbValues[2];
        }

        public void Update(Color? color)
        {
            if (color is null || !color.HasValue)
            {
                this.DisplayColor = Color.FromArgb(0, 0, 0, 255);
            }
            else
            {
                this.DisplayColor = color.Value;
            }
            double[] HsbValues = ColorUtilities.RgbToHsv(DisplayColor.R, DisplayColor.G, DisplayColor.B);
            this.Hue = HsbValues[0];
            this.Saturation = HsbValues[1];
            this.Value = HsbValues[2];
        }

        public void Update(byte? alpha, byte? red, byte? green, byte? blue)
        {
            byte R = red ?? DisplayColor.R;
            byte G = green ?? DisplayColor.G;
            byte B = blue ?? DisplayColor.B;
            byte A = alpha ?? DisplayColor.A;
            DisplayColor = Color.FromArgb(A, R, G, B);
            double[] HsbValues = ColorUtilities.RgbToHsv(R, G, B);
            Hue = HsbValues[0];
            Saturation = HsbValues[1];
            Value = HsbValues[2];
        }

        public void Update(double? hue, double? saturation, double? brightness)
        {
            this.Hue = hue ?? this.Hue;
            this.Saturation = saturation ?? this.Saturation;
            this.Value = brightness ?? this.Value;
            this.DisplayColor = ColorUtilities.HsvToRgb(this.Hue, this.Saturation, this.Value, DisplayColor.A);
        }

        public void Update(byte? alpha)
        {
            if (alpha.HasValue)
            {
                DisplayColor = Color.FromArgb(alpha.Value, DisplayColor.R, DisplayColor.G, DisplayColor.B);
            }
        }
    }
}