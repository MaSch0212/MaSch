namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Specifies the direction in which a <see cref="Tile"/> should flip.
    /// </summary>
    public enum FlipDirection
    {
        /// <summary>
        /// The tile should be pressed a bit away from the camera.
        /// </summary>
        Center,

        /// <summary>
        /// The tile should tilt to the right.
        /// </summary>
        Right,

        /// <summary>
        /// The tile should tilt to the left.
        /// </summary>
        Left,

        /// <summary>
        /// The tile should tilt to the top.
        /// </summary>
        Top,

        /// <summary>
        /// The tile should tilt to the bottom.
        /// </summary>
        Bottom,

        /// <summary>
        /// The tile should tilt to the top left.
        /// </summary>
        TopLeft,

        /// <summary>
        /// The tile should tilt to the rop right.
        /// </summary>
        TopRight,

        /// <summary>
        /// The tile should tilt to the bottom left.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// The tile should tilt to the bottom right.
        /// </summary>
        BottomRight,
    }
}
