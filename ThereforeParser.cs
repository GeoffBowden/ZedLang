using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLang
{
    public static class ThereforeParser
    {
        public static SyntacticElement Parse(List<string> lineElements)
        {
            if (string.Compare("therefore", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0)
            {
                ThereforeSyntacticElement therefore = new ThereforeSyntacticElement();
                // get lhs prop p and q imp z
                List<string> expressionText = lineElements.Skip(1).ToList();
                SyntacticElement expression = ExpressionParser.Parse(expressionText);
                // check lhs
                if (expression == null)
                {
                    return new InvalidSyntacticElement("No expression for therefore", "Nothing to be proved? why are you here?");
                }
                else if (expression is InvalidSyntacticElement)
                {
                    return new InvalidSyntacticElement("Invalid expression for therefore", ((InvalidSyntacticElement)expression).Message);
                }
                else if (expression is ExpressionSyntacticElement)
                {
                    therefore.expression = (ExpressionSyntacticElement)expression;
                }
                else
                {
                    throw new InvalidOperationException();
                }
                return therefore;
            }
            else
            {   // this is here if someone changes the design.
                return new InvalidSyntacticElement("No therefore header", "cannot parse what does not exist");
            }

        }
    }
}
