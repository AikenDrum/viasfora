﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Winterdom.Viasfora.Classifications {

  internal static class CodeClassificationDefinitions {
    [Export, Name(Constants.KEYWORD_CLASSIF_NAME)]
    internal static ClassificationTypeDefinition FlowControlClassificationType = null;

    [Export, Name(Constants.LINQ_CLASSIF_NAME)]
    internal static ClassificationTypeDefinition LinqKeywordClassificationType = null;

    [Export, Name(Constants.STRING_ESCAPE_CLASSIF_NAME)]
    internal static ClassificationTypeDefinition StringEscapeSequenceClassificationType = null;

    [Export, Name(Constants.VISIBILITY_CLASSIF_NAME)]
    internal static ClassificationTypeDefinition VisibilityKeywordClassificationType = null;

    [Export, Name(Constants.LINE_HIGHLIGHT)]
    internal static ClassificationTypeDefinition CurrentLineClassificationType = null;

    [Export, Name(Constants.COLUMN_HIGHLIGHT)]
    internal static ClassificationTypeDefinition CurrentColumnClassificationType = null;

    [Export, Name(Constants.RAINBOW_1)]
    internal static ClassificationTypeDefinition Rainbow1ClassificationType = null;

    [Export, Name(Constants.RAINBOW_2)]
    internal static ClassificationTypeDefinition Rainbow2ClassificationType = null;

    [Export, Name(Constants.RAINBOW_3)]
    internal static ClassificationTypeDefinition Rainbow3ClassificationType = null;

    [Export, Name(Constants.RAINBOW_4)]
    internal static ClassificationTypeDefinition Rainbow4ClassificationType = null;

  } 
}
