using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLang
{
    public static class LineParser
    {
        public static SyntacticElement Parse( string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return new NullSyntacticElement();
            }
            // force spacing:
            line = line.Replace(">", " > ").Replace( "=", " = " ).Replace( ":", ": " ).
                        Replace( "(", " ( " ).Replace( ")", " ) " );
            List<string> lineElements = line.BreakLine();
            if ( lineElements.Count == 0 )
            {
                return new NullSyntacticElement();
            }
            if (string.Compare("predicate", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0)
            {
                return PredicateParser.Parse(lineElements);
            }
            if ( string.Compare("proposition", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0 )
            {
                return PropositionParser.Parse(lineElements);
            }
            if (string.Compare("therefore", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0)
            {
                return ThereforeParser.Parse(lineElements);
            }
            if ( string.Compare("note:", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0 )
            {
                return new CommentSyntacticElement(string.Join(" ", lineElements.Skip(1)));
            }
            if ( ExpressionParser.IsAnExpression( lineElements ) )
            {
                return ExpressionParser.Parse(lineElements);
            }
            if ((lineElements.Count() == 1)&&(!lineElements[0].IsaReservedWord()))
            {
                return new PredicateExpression(lineElements[0]);
            }
            return new InvalidSyntacticElement("Parser error", "Invalid Instruction" );
        } // public static SyntacticElement Parse
    }

}
