﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Text.Formatting;

namespace Winterdom.Viasfora.Text {
  public class CurrentLineAdornment {
    public const String CUR_LINE_TAG = "currentLine";
    private IAdornmentLayer layer;
    private IWpfTextView view;
    private IClassificationFormatMap formatMap;
    private IClassificationType formatType;
    private Brush fillBrush;
    private Pen borderPen;
    private Image currentHighlight = null;

    public CurrentLineAdornment(
          IWpfTextView view, IClassificationFormatMap formatMap,
          IClassificationType formatType) {
      this.view = view;
      this.formatMap = formatMap;
      this.formatType = formatType;
      layer = view.GetAdornmentLayer(Constants.LINE_HIGHLIGHT);

      view.Caret.PositionChanged += OnCaretPositionChanged;
      view.ViewportWidthChanged += OnViewportWidthChanged;
      view.LayoutChanged += OnLayoutChanged;
      view.ViewportLeftChanged += OnViewportLeftChanged;
      view.Closed += OnViewClosed;
      VsfSettings.SettingsUpdated += OnSettingsUpdated;
      formatMap.ClassificationFormatMappingChanged +=
         OnClassificationFormatMappingChanged;

      CreateDrawingObjects();
    }

    void OnSettingsUpdated(object sender, EventArgs e) {
      this.currentHighlight = null;
      CreateDrawingObjects();
      RedrawAdornments();
    }
    void OnViewClosed(object sender, EventArgs e) {
      view.Caret.PositionChanged -= OnCaretPositionChanged;
      view.ViewportWidthChanged -= OnViewportWidthChanged;
      view.LayoutChanged -= OnLayoutChanged;
      view.ViewportLeftChanged -= OnViewportLeftChanged;
      view.Closed -= OnViewClosed;
      VsfSettings.SettingsUpdated -= OnSettingsUpdated;
    }
    void OnViewportLeftChanged(object sender, EventArgs e) {
      RedrawAdornments();
    }
    void OnViewportWidthChanged(object sender, EventArgs e) {
      RedrawAdornments();
    }
    void OnClassificationFormatMappingChanged(object sender, EventArgs e) {
      // the user changed something in Fonts and Colors, so
      // recreate our adornments
      this.currentHighlight = null;
      CreateDrawingObjects();
    }
    void OnCaretPositionChanged(object sender, CaretPositionChangedEventArgs e) {
      ITextViewLine newLine = GetLineByPos(e.NewPosition);
      ITextViewLine oldLine = GetLineByPos(e.OldPosition);
      if ( newLine != oldLine ) {
        layer.RemoveAdornmentsByTag(CUR_LINE_TAG);
        this.CreateVisuals(newLine);
      }
    }
    void OnLayoutChanged(object sender, TextViewLayoutChangedEventArgs e) {
      SnapshotPoint caret = view.Caret.Position.BufferPosition;
      foreach ( var line in e.NewOrReformattedLines ) {
        if ( line.ContainsBufferPosition(caret) ) {
          this.currentHighlight = null; // force recalculation
          this.CreateVisuals(line);
          break;
        }
      }
    }

    private void CreateDrawingObjects() {
      // this gets the color settings configured by the
      // user in Fonts and Colors (or the default in out
      // classification type).
      TextFormattingRunProperties format =
         formatMap.GetTextProperties(formatType);

      fillBrush = format.BackgroundBrush;
      Brush penBrush = format.ForegroundBrush;
      borderPen = new Pen(penBrush, VsfSettings.HighlightLineWidth);
      borderPen.Freeze();
      RedrawAdornments();
    }
    private void RedrawAdornments() {
      if ( view.TextViewLines != null ) {
        if ( currentHighlight != null ) {
          layer.RemoveAdornment(currentHighlight);
        }
        this.currentHighlight = null; // force redraw
        var caret = view.Caret.Position;
        ITextViewLine line = GetLineByPos(caret);
        this.CreateVisuals(line);
      }
    }
    private ITextViewLine GetLineByPos(CaretPosition pos) {
      SnapshotPoint point = pos.BufferPosition;
      if ( point.Snapshot != view.TextSnapshot ) {
        point = point.TranslateTo(view.TextSnapshot, PointTrackingMode.Positive);
      }
      return view.GetTextViewLineContainingBufferPosition(point);
    }
    private void CreateVisuals(ITextViewLine line) {
      if ( !VsfSettings.CurrentLineHighlightEnabled ) {
        return; // not enabled
      }
      IWpfTextViewLineCollection textViewLines = view.TextViewLines;
      if ( textViewLines == null )
        return; // not ready yet.
      SnapshotSpan span = line.Extent;
      Rect rc = new Rect(
         new Point(view.ViewportLeft, line.TextTop),
         new Point(Math.Max(view.ViewportRight - 2, line.TextRight), line.TextBottom)
      );

      if ( NeedsNewImage(rc) ) {
        Geometry g = new RectangleGeometry(rc, 0.5, 0.5);
        GeometryDrawing drawing = new GeometryDrawing(fillBrush, borderPen, g);
        drawing.Freeze();
        DrawingImage drawingImage = new DrawingImage(drawing);
        drawingImage.Freeze();
        Image image = new Image();
        // work around WPF rounding bug
        image.UseLayoutRounding = false;
        image.Source = drawingImage;
        currentHighlight = image;
      }

      //Align the image with the top of the bounds of the text geometry
      Canvas.SetLeft(currentHighlight, rc.Left);
      Canvas.SetTop(currentHighlight, rc.Top);

      layer.AddAdornment(
         AdornmentPositioningBehavior.TextRelative, span,
         CUR_LINE_TAG, currentHighlight, null
      );
    }
    private bool NeedsNewImage(Rect rc) {
      if ( currentHighlight == null )
        return true;
      if ( AreClose(currentHighlight.Width, rc.Width) )
        return true;
      return AreClose(currentHighlight.Height, rc.Height);
    }
    private bool AreClose(double d1, double d2) {
      double diff = d1 - d2;
      return Math.Abs(diff) < 0.1;
    }
  }

}
