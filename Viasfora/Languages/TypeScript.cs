﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Winterdom.Viasfora.Languages.BraceExtractors;
using Winterdom.Viasfora.Util;

namespace Winterdom.Viasfora.Languages {
  class TypeScript : CBasedLanguage {
    public const String ContentType = "TypeScript";

    static readonly String[] KEYWORDS = {
         "if", "else", "while", "do", "for", "switch",
         "break", "continue", "return", "throw"
      };
    static readonly String[] VISIBILITY = {
         "export", "public", "private"
      };
    static readonly String[] LINQ_KEYWORDS = {
         "in", "with"
      };
    protected override String[] ControlFlowDefaults {
      get { return KEYWORDS; }
    }
    protected override String[] LinqDefaults {
      get { return LINQ_KEYWORDS; }
    }
    protected override String[] VisibilityDefaults {
      get { return VISIBILITY; }
    }
    protected override String KeyName {
      get { return "TypeScript"; }
    }
    protected override String[] ContentTypes {
      get { return new String[] { ContentType }; }
    }
    public override IBraceExtractor NewBraceExtractor() {
      return new JScriptBraceExtractor(this);
    }
  }
}
