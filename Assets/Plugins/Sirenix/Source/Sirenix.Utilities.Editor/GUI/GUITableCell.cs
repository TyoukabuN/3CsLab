//-----------------------------------------------------------------------
// <copyright file="GUITableCell.cs" company="Sirenix ApS">
// Copyright (c) Sirenix ApS. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
#if UNITY_EDITOR
namespace Sirenix.Utilities.Editor
{
#pragma warning disable

    using System;
    using UnityEngine;

    /// <summary>
    /// A cell of a <see cref="GUITable"/>
    /// </summary>
    public class GUITableCell
    {
        private Rect rect;

        /// <summary>
        /// The minimum width.
        /// </summary>
        public float MinWidth = 10;

        /// <summary>
        /// <para>The width of the cell. Default is width is 0.</para>
        /// <para>The width the column is determained by the widest cell in the column.</para>
        /// <para>Width = 0 = auto.</para>
        /// </summary>
        public float Width;

        /// <summary>
        /// <para>The height of the cell. Default is height is 22.</para>
        /// <para>The height the column is determained by the tallest cell in the row.</para>
        /// </summary>
        public float Height = 22;

        /// <summary>
        /// If true, the cell will expand vertically, covering all neighbour null cells.
        /// </summary>
        public bool SpanY;

        /// <summary>
        /// If true, the cell will expand horizontally, covering all neighbour null cells.
        /// </summary>
        public bool SpanX;

        /// <summary>
        /// The table column index.
        /// </summary>
        public int X { get; internal set; }

        /// <summary>
        /// The table row index.
        /// </summary>
        public int Y { get; internal set; }

        public Action<Rect> OnGUI = null;

        /// <summary>
        /// The GUI style
        /// </summary>
        public Action<Rect> GUIStyle = null;

        internal GUITable Table;

        /// <summary>
        /// Gets the rect.
        /// </summary>
        public Rect Rect
        {
            get { return this.rect; }
            internal set { this.rect = value; }
        }

        internal void Draw()
        {
            if (this.GUIStyle != null && Event.current.type == EventType.Repaint)
            {
                this.GUIStyle.Invoke(this.rect);
            }

            if (this.OnGUI != null)
            {
                this.OnGUI.Invoke(this.rect);
            }
        }

        #region Modified By Hunter (jb) -- 2022年10月26日
        //浮动表头绘制
        internal void DrawWithRect(Rect assignedRect)
        {
            if (this.GUIStyle != null && Event.current.type == EventType.Repaint)
            {
                this.GUIStyle.Invoke(assignedRect);
            }

            if (this.OnGUI != null)
            {
                this.OnGUI.Invoke(assignedRect);
            }
        }

        #endregion
    }
}
#endif