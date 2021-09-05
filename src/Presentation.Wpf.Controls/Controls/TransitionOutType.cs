using System;

namespace MaSch.Presentation.Wpf.Controls
{
    /// <summary>
    /// Specifies the transitions to use to fade out content. Multiple can be used at the same time.
    /// </summary>
    [Flags]
    public enum TransitionOutType
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
        /// The control animates to zoom in.
        /// </summary>
        ZoomFromUser = 0b100,

        /// <summary>
        /// The control animates to zoom out.
        /// </summary>
        ZoomToUser = 0b1000,

        /// <summary>
        /// The control moves to the left side out of view.
        /// </summary>
        SlideOutToLeft = 0b1_0000_0000,

        /// <summary>
        /// The control moves to the top side out of view.
        /// </summary>
        SlideOutToTop = 0b10_0000_0000,

        /// <summary>
        /// The control moves to the right side out of view.
        /// </summary>
        SlideOutToRight = 0b100_0000_0000,

        /// <summary>
        /// The control moves to the bottom side out of view.
        /// </summary>
        SlideOutToBottom = 0b1000_0000_0000,

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
