namespace ColorPickerLib.Core.Utilities
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Windows.Media;

	internal static class ColorUtilities
	{
		public static readonly Dictionary<string, Color> KnownColors = GetKnownColors();

		public static string GetColorDisplayName(this Color? color)
		{
			string colorName = KnownColors.Where(kvp => kvp.Value.Equals(color)).Select(kvp => kvp.Key).FirstOrDefault();

			if (String.IsNullOrEmpty(colorName))
				colorName = GetFormatedColorString(color, false);

			return colorName;
		}

		public static string GetColorName(this Color color)
		{
			string colorName = KnownColors.Where(kvp => kvp.Value.Equals(color)).Select(kvp => kvp.Key).FirstOrDefault();

			if (String.IsNullOrEmpty(colorName))
				colorName = color.ToString();

			return colorName;
		}

		/// <summary>
		/// Converts a <seealso cref="Color"/> value into a string expression.
		/// </summary>
		/// <param name="stringToFormat"></param>
		/// <param name="isUsingAlphaChannel"></param>
		/// <returns></returns>
		public static string FormatColorString(string stringToFormat, bool isUsingAlphaChannel)
		{
			if (!isUsingAlphaChannel && (stringToFormat.Length == 9))
				return stringToFormat.Remove(1, 2);

			return stringToFormat;
		}

		/// <summary>
		/// Converts a <seealso cref="Color"/> value into a string expression.
		/// </summary>
		/// <param name="colorToFormat"></param>
		/// <returns></returns>
		public static string GetFormatedColorString(Color? colorToFormat, bool isUsingAlphaChannel)
		{
			if ((colorToFormat == null) || !colorToFormat.HasValue)
				return string.Empty;

			var red = colorToFormat.Value.R;
			var green = colorToFormat.Value.G;
			var blue = colorToFormat.Value.B;

			if (isUsingAlphaChannel == true)
			{
				var opacity = colorToFormat.Value.A;
				return string.Format("{0:X2}{1:X2}{2:X2}{3:X2}", opacity, red, green, blue);
			}

			return string.Format("{0:X2}{1:X2}{2:X2}", red, green, blue);
		}

		private static Dictionary<string, Color> GetKnownColors()
		{
			var colorProperties = typeof(Colors).GetProperties(BindingFlags.Static | BindingFlags.Public);
			return colorProperties.ToDictionary(p => p.Name, p => (Color)p.GetValue(null, null));
		}

		// -nkarasch brought back
		/*** Replaced with static converter code in HsvColor class
				/// <summary>
				/// Converts an RGB color to an HSV color.
				/// </summary>
				/// <param name="r"></param>
				/// <param name="b"></param>
				/// <param name="g"></param>
				/// <returns></returns>
				public static HsvColor ConvertRgbToHsv(int r, int b, int g)
				{
					double delta, min;
					double h = 0, s, v;

					min = Math.Min(Math.Min(r, g), b);
					v = Math.Max(Math.Max(r, g), b);
					delta = v - min;

					if (v == 0.0)
					{
						s = 0;
					}
					else
						s = delta / v;

					if (s == 0)
						h = 0.0;
					else
					{
						if (r == v)
							h = (g - b) / delta;
						else if (g == v)
							h = 2 + (b - r) / delta;
						else if (b == v)
							h = 4 + (r - g) / delta;

						h *= 60;
						if (h < 0.0)
							h = h + 360;
					}

					return new HsvColor( h, s, v / 255);
				}

				/// <summary>
				///  Converts an HSV color to an RGB color.
				/// </summary>
				/// <param name="h"></param>
				/// <param name="s"></param>
				/// <param name="v"></param>
				/// <returns></returns>
				public static Color ConvertHsvToRgb(double h, double s, double v)
				{
					double r = 0, g = 0, b = 0;

					if (s == 0)
					{
						r = v;
						g = v;
						b = v;
					}
					else
					{
						int i;
						double f, p, q, t;

						if (h == 360)
							h = 0;
						else
							h = h / 60;

						i = (int)Math.Truncate(h);
						f = h - i;

						p = v * (1.0 - s);
						q = v * (1.0 - (s * f));
						t = v * (1.0 - (s * (1.0 - f)));

						switch (i)
						{
							case 0:
								{
									r = v;
									g = t;
									b = p;
									break;
								}
							case 1:
								{
									r = q;
									g = v;
									b = p;
									break;
								}
							case 2:
								{
									r = p;
									g = v;
									b = t;
									break;
								}
							case 3:
								{
									r = p;
									g = q;
									b = v;
									break;
								}
							case 4:
								{
									r = t;
									g = p;
									b = v;
									break;
								}
							default:
								{
									r = v;
									g = p;
									b = q;
									break;
								}
						}
					}

					return Color.FromArgb(255, (byte)(Math.Round(r * 255)), (byte)(Math.Round(g * 255)), (byte)(Math.Round(b * 255)));
				}
		***/

		/// <summary>
		/// Generates a list of colors with hues ranging from 0 to 360 and
		/// a saturation and value of 1.
		/// </summary>
		/// <returns></returns>
		public static List<Color> GenerateHsvSpectrum()
		{
			List<Color> colorsList = new List<Color>(8);

			// list of colors with hues ranging from 0 to 360
			for (int i = 0; i < 29; i++)
			{
				//                colorsList.Add(ColorUtilities.ConvertHsvToRgb(i * 12, 1, 1));
				//colorsList.Add(HsvColor.RGBFromHSV(new HsvColor(i * 12, 1, 1)));
				colorsList.Add(HsvToRgb(i * 12, 1, 1));
			}

			// saturation and value of 1
			//            colorsList.Add(ColorUtilities.ConvertHsvToRgb(0, 1, 1));
			//colorsList.Add(HsvColor.RGBFromHSV(new HsvColor(0, 1, 1)));
			colorsList.Add(HsvToRgb(0, 1, 1));

			return colorsList;
		}

		// conversion between HSB/HSV and RGB methods were taken from UweKeim's
		// https://gist.github.com/UweKeim/fb7f829b852c209557bc49c51ba14c8b
		// My goal here is to keep the Hue level stable while changing the
		// Saturation and Brightness/Value levels
		// -nkarasch

		/// <summary>
		/// Provides color conversion functionality.
		/// </summary>
		/// <remarks>
		/// http://en.wikipedia.org/wiki/HSV_color_space
		/// http://www.easyrgb.com/math.php?MATH=M19#text19
		/// </remarks>  

		public static double[] RgbToHsv(byte Red, byte Green, byte Blue)
		{
            // _NOTE #1: Even though we're dealing with a very small range of
            // numbers, the accuracy of all calculations is fairly important.
            // For this reason, I've opted to use double data types instead
            // of float, which gives us a little bit extra precision (recall
            // that precision is the number of significant digits with which
            // the result is expressed).

            double r = Red / 255d;
            double g = Green / 255d;
            double b = Blue / 255d;

            double minValue = GetMinimumValue(r, g, b);
            double maxValue = GetMaximumValue(r, g, b);
            double delta = maxValue - minValue;

			double hue = 0;
			double saturation;
            double value = maxValue;

			if (Math.Abs(maxValue - 0) < double.Epsilon || Math.Abs(delta - 0) < double.Epsilon)
			{
				hue = 0;
				saturation = 0;
			}
			else
			{
				// _NOTE #2: FXCop insists that we avoid testing for floating 
				// point equality (CA1902). Instead, we'll perform a series of
				// tests with the help of Double.Epsilon that will provide 
				// a more accurate equality evaluation.

				if (Math.Abs(minValue - 0) < double.Epsilon)
				{
					saturation = 1;
				}
				else
				{
					saturation = delta / maxValue;
				}

				if (Math.Abs(r - maxValue) < double.Epsilon)
				{
					hue = (g - b) / delta;
				}
				else if (Math.Abs(g - maxValue) < double.Epsilon)
				{
					hue = 2 + (b - r) / delta;
				}
				else if (Math.Abs(b - maxValue) < double.Epsilon)
				{
					hue = 4 + (r - g) / delta;
				}
			}

			hue *= 60;
			if (hue < 0)
			{
				hue += 360;
			}

			return new double[] {
				hue,
				saturation,
				value };
		}

		public static Color HsvToRgb(
			double Hue, double Saturation, double Value, byte Alpha = 255)
		{
			double red = 0, green = 0, blue = 0;

			double h = (Hue != 360.0) ? Hue : 0.0;
            double s = Saturation;
            double b = Value;

			if (Math.Abs(s - 0) < double.Epsilon)
			{
				red = b;
				green = b;
				blue = b;
			}
			else
			{
                // the color wheel has six sectors.

                double sectorPosition = h / 60;
                int sectorNumber = (int)Math.Floor(sectorPosition);
                double fractionalSector = sectorPosition - sectorNumber;

                double p = b * (1 - s);
                double q = b * (1 - s * fractionalSector);
                double t = b * (1 - s * (1 - fractionalSector));

				// Assign the fractional colors to r, g, and b
				// based on the sector the angle is in.
				switch (sectorNumber)
				{
					case 0:
						red = b;
						green = t;
						blue = p;
						break;

					case 1:
						red = q;
						green = b;
						blue = p;
						break;

					case 2:
						red = p;
						green = b;
						blue = t;
						break;

					case 3:
						red = p;
						green = q;
						blue = b;
						break;

					case 4:
						red = t;
						green = p;
						blue = b;
						break;

					case 5:
						red = b;
						green = p;
						blue = q;
						break;
				}
			}
            byte nRed = Convert.ToByte((int)Math.Round(red * 255));
            byte nGreen = Convert.ToByte((int)Math.Round(green * 255));
            byte nBlue = Convert.ToByte((int)Math.Round(blue * 255));

			return Color.FromArgb(Alpha, nRed, nGreen, nBlue);
		}

		/// <summary>
		/// Determines the maximum value of all of the numbers provided in the
		/// variable argument list.
		/// </summary>
		private static double GetMaximumValue(
			params double[] values)
		{
            double maxValue = values[0];

			if (values.Length >= 2)
			{
				for (int i = 1; i < values.Length; i++)
				{
                    double num = values[i];
					maxValue = Math.Max(maxValue, num);
				}
			}

			return maxValue;
		}

		/// <summary>
		/// Determines the minimum value of all of the numbers provided in the
		/// variable argument list.
		/// </summary>
		private static double GetMinimumValue(
			params double[] values)
		{
            double minValue = values[0];

			if (values.Length >= 2)
			{
				for (int i = 1; i < values.Length; i++)
				{
                    double num = values[i];
					minValue = Math.Min(minValue, num);
				}
			}

			return minValue;
		}
	}
}