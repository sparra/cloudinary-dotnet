﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cloudinary
{
    /// <summary>
    /// The default Transformation
    /// </summary>
    public class Transformation : ITransformation
    {
        /// <summary>
        /// Gets of sets the Width of the image
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height of the image
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets of sets Crop mode
        /// </summary>
        public CropMode? Crop { get; set; }

        /// <summary>
        /// Gets of sets Gravity of the tranformation
        /// </summary>
        public Gravity? Gravity { get; set; }

        /// <summary>
        /// Gets or sets the Format of the image
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the border radius, int.MaxValue
        /// equals a circle
        /// </summary>
        public int? Radius { get; set; }

        /// <summary>
        /// Image to use when the current image isn't available. 
        /// Format to use: publicid.format, 
        /// for example: avatar.jpg
        /// </summary>
        public string DefaultImage { get; set; }

        /// <summary>
        /// Gets the X for the fixed cropping coordinate, must 
        /// be set to SetFixedCroppingPosition
        /// </summary>
        public uint? X { get; private set; }

        /// <summary>
        /// Gets the X for the fixed cropping coordinate,
        /// be set to SetFixedCroppingPosition
        /// </summary>
        public uint? Y { get; private set; }

        /// <summary>
        /// Sets the fixed cropping position, note that
        /// the x and y values are against the original image
        /// </summary>
        /// <param name="x">the x value</param>
        /// <param name="y">the y value</param>
        public void SetFixedCroppingPosition(uint x, uint y)
        {
            X = x;
            Y = y;
        }

        public Transformation(int width, int height)
        {
            Width = width;
            Height = height;
            Crop = null;
        }

        public string GetFormat()
        {
            if (string.IsNullOrEmpty(Format))
                return "jpg";

            return Format;
        }

        public string ToCloudinary()
        {
            var cli = new StringBuilder();
            cli.AppendFormat(GetSize());

            if (X.HasValue && Y.HasValue)
                cli.AppendFormat(",x_{0},y_{1}", X.Value, Y.Value);

            if (Crop.HasValue)
                cli.AppendFormat(",c_{0}", Crop.Value.ToString().ToLowerInvariant());

            if (Gravity.HasValue)
                cli.AppendFormat(",g_{0}", Gravity.Value.ToString().ToLowerInvariant());

            if (Radius.HasValue)
            {
                string urlValue = Radius.Value.ToString(CultureInfo.InvariantCulture);

                if (Radius.Value == int.MaxValue)
                    urlValue = "max";
                    
                cli.AppendFormat(",r_{0}", urlValue);
            }

            if (!string.IsNullOrEmpty(DefaultImage))
                cli.AppendFormat(",d_{0}", DefaultImage);

            return cli.ToString();
        }

        protected virtual string GetSize()
        {
            return string.Format("w_{0},h_{1}", Width, Height);
        }
    }
}
