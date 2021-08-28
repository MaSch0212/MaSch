namespace MaSch.Console.Controls
{
    /// <summary>
    /// Thickness used for Margins and Paddings.
    /// </summary>
    public struct Thickness
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> struct.
        /// </summary>
        /// <param name="uniform">Value that is used for <see cref="Left"/>, <see cref="Top"/>, <see cref="Right"/> and <see cref="Bottom"/>.</param>
        public Thickness(int uniform)
            : this(uniform, uniform, uniform, uniform)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> struct.
        /// </summary>
        /// <param name="horizontal">Value that is used for <see cref="Left"/> and <see cref="Right"/>.</param>
        /// <param name="vertical">Value that is used for <see cref="Top"/> and <see cref="Bottom"/>.</param>
        public Thickness(int horizontal, int vertical)
            : this(horizontal, vertical, horizontal, vertical)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thickness"/> struct.
        /// </summary>
        /// <param name="left">Value that is used for <see cref="Left"/>.</param>
        /// <param name="top">Value that is used for <see cref="Top"/>.</param>
        /// <param name="right">Value that is used for <see cref="Right"/>.</param>
        /// <param name="bottom">Value that is used for <see cref="Bottom"/>.</param>
        public Thickness(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Gets or sets the left space.
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// Gets or sets the top space.
        /// </summary>
        public int Top { get; set; }

        /// <summary>
        /// Gets or sets the right space.
        /// </summary>
        public int Right { get; set; }

        /// <summary>
        /// Gets or sets the bottom space.
        /// </summary>
        public int Bottom { get; set; }
    }
}
