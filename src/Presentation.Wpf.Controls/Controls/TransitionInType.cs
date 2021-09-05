using System;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Specifies the transitions to use to fade in content. Multiple can be used at the same time.
    /// </summary>
    [Flags]
    public enum TransitionInType
    {
        /// <summary>
        /// No animation is used.
        /// </summary>
        None = 0b0,

        /// <summary>
        /// The Opacity is animated.
        /// </summary>
        Fade = 0b1,

        /// <summary>
        /// A blur effect is added and animated.
        /// </summary>
        Blur = 0b10,

        /// <summary>
        /// The control starts very zoomed in and animates to normal size.
        /// </summary>
        ZoomFromUser = 0b100,

        /// <summary>
        /// The control starts very zoomed out and animates to normal size.
        /// </summary>
        ZoomToUser = 0b1000,

        /// <summary>
        /// The control moves from the left side into view.
        /// </summary>
        SlideInFromLeft = 0b1_0000_0000,

        /// <summary>
        /// The control moves from the top side into view.
        /// </summary>
        SlideInFromTop = 0b10_0000_0000,

        /// <summary>
        /// The control moves from the right side into view.
        /// </summary>
        SlideInFromRight = 0b100_0000_0000,

        /// <summary>
        /// The control moves from the bottom side into view.
        /// </summary>
        SlideInFromBottom = 0b1000_0000_0000,

        /// <summary>
        /// When used in combination with a slide transition, the control starts only move half of the size.
        /// </summary>
        SlideOnlyHalf = 0b1_0000_0000_0000,

        /// <summary>
        /// When used in combination with a slide transition, the control starts only move a quater of the size.
        /// </summary>
        SlideOnlyQuater = 0b10_0000_0000_0000,
    }
}
