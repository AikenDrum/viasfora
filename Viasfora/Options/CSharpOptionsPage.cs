﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.VisualStudio.Shell;
using Winterdom.Viasfora.Languages;

namespace Winterdom.Viasfora.Options {
  [Guid(Guids.CSharpOptions)]
  public class CSharpOptionsPage : DialogPage {
    private CSharp language = new CSharp();

    public override void SaveSettingsToStorage() {
      base.SaveSettingsToStorage();
      language.ControlFlow = ControlFlowKeywords.ToArray();
      language.Linq = LinqKeywords.ToArray();
      language.Visibility = VisibilityKeywords.ToArray();
      VsfSettings.Save();
    }
    public override void LoadSettingsFromStorage() {
      base.LoadSettingsFromStorage();
      ControlFlowKeywords = language.ControlFlow.ToList();
      LinqKeywords = language.Linq.ToList();
      VisibilityKeywords = language.Visibility.ToList();
    }

    [LocDisplayName("Control Flow")]
    [Description("Control Flow keywords to highlight")]
    [Category("CSharp")]
    [Editor(Constants.STRING_COLLECTION_EDITOR, typeof(UITypeEditor))]
    [TypeConverter(typeof(Design.StringListConverter))]
    public List<String> ControlFlowKeywords { get; set; }

    [LocDisplayName("Visibility")]
    [Description("Visibility keywords to highlight")]
    [Category("CSharp")]
    [Editor(Constants.STRING_COLLECTION_EDITOR, typeof(UITypeEditor))]
    [TypeConverter(typeof(Design.StringListConverter))]
    public List<String> VisibilityKeywords { get; set; }

    [LocDisplayName("LINQ")]
    [Description("LINQ keywords to highlight")]
    [Category("CSharp")]
    [Editor(Constants.STRING_COLLECTION_EDITOR, typeof(UITypeEditor))]
    [TypeConverter(typeof(Design.StringListConverter))]
    public List<String> LinqKeywords { get; set; }
  }
}
