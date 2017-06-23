using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLang
{
    public static class PropositionParser 
    {
        public static SyntacticElement Parse(List<string> lineElements)
        {
            // syntax is
            // proposition: <expression> <implies> <expression>
            // tests:              
            //   1st element is proposition or invalid syntactic element is returned
            //   if implies in the list returns implication object
            //   if ifandonlyif in the list returns ifandonlyif systactic object
            //   if the count of implies and ifandonlyif keywords is more than one return an invalid syntactic element
            // left hand expression is a valid expression
            // right hand expression is a valid expression
            // left hand expression is not null
            // right hand expression is not null 
            if (string.Compare("proposition", lineElements[0], StringComparison.OrdinalIgnoreCase) == 0)
            {
                return ParseProposition(lineElements);
            }
            else
            { 
                return new InvalidSyntacticElement( "No proposition header", "cannot parse proposition");
            }
        }
        private static SyntacticElement ParseSingularProposition( List<string> lineElements )
        {
            lineElements.RemoveAll(x => x == "(");
            lineElements.RemoveAll(x => x == ")");
            if (IsASinglePropositionElement(lineElements))
            {
                return new PropositionSyntacticElement(lineElements[1]);
            }
            else if (IsANotExpressionWithASingle(lineElements))
            {
                return new PropositionNegationSyntacticElement(lineElements[2]);
            }
            return new InvalidSyntacticElement("Propsition Parser Error", "No proposition symbol found");
        }

        private static SyntacticElement ParseProposition(List<string> lineElements)
        {
            if (CountOfPropositionQualifiers(lineElements) < 1)
            {
                return ParseSingularProposition(lineElements);
            }
            if (CountOfPropositionQualifiers(lineElements) > 1)
            {
                return new InvalidSyntacticElement("Too many proposition qualifiers", "can only have one ifandonlyif or implies symbol");
            }
            else
            {
                return parseStandardProposition(lineElements);
            }
        }

        private static bool IsASinglePropositionElement(List<string> lineElements)
        {
            return (lineElements.Count == 2) &&
                   (CountOfPropositionQualifiers(lineElements) == 0) &&
                   (!lineElements[1].IsaReservedWord());
        }

        private static bool IsANotExpressionWithASingle(List<string> lineElements)
        {
            return (lineElements.Count == 3) &&
                   (CountOfPropositionQualifiers(lineElements) == 0) &&
                   (lineElements[1].IsANot()) &&
                   (!lineElements[2].IsaReservedWord());
        }

        private static SyntacticElement parseStandardProposition(List<string> lineElements )
        {
            PropositionExpression proposition;
            int index = -1;
            index = IndexOfImplies(lineElements);
            if (index < 0)
            {
                index = IndexOfIfAndOnlyIf(lineElements);
                if (index < 0)
                {
                    throw new FormatException("no proposition element");
                }
                proposition = new IfAndOnlyIfSyntacticExpression();
            }
            else
            {
                proposition = new ImplicationSyntacticExpression();
            }

            // get lhs prop p and q imp z
            List<string> lhsText = lineElements.Skip(1).Take(index - 1).ToList();
            SyntacticElement lhs = ExpressionParser.Parse(lhsText);
            // check lhs
            if ( lhs is ExpressionSyntacticElement)
            {
                proposition.lhs = (ExpressionSyntacticElement)lhs;
            }
            else
            {
                return ProcessLhsError(lhs);
            }

            //           0  1  2  3  4  5  6  7     8 - X = 3
            // get rhs prop p and q imp z and k     index = 4
            //           0  1  2  3  4  5     6 - X = 3
            // get rhs prop p imp z and k     index = 2
            List<string> rhsText = lineElements.Skip(index + 1).Take(lineElements.Count() - (index + 1)).ToList();
            SyntacticElement rhs = ExpressionParser.Parse(rhsText);
            if (rhs is ExpressionSyntacticElement)
            {
                proposition.rhs = (ExpressionSyntacticElement)rhs;
            }
            else
            {
                return ProcessRhsError(rhs);
            }
            return proposition;
        }

        private static SyntacticElement ProcessLhsError( SyntacticElement lhs )
        {
            if (lhs == null)
            {
                return new InvalidSyntacticElement("No lhs on predicate", "Nothing cannot imply something");
            }
            else if (lhs is InvalidSyntacticElement)
            {
                return new InvalidSyntacticElement("Invalid left hand side of proposition", ((InvalidSyntacticElement)lhs).Message);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static SyntacticElement ProcessRhsError(SyntacticElement rhs)
        {
            if (rhs == null)
            {
                return new InvalidSyntacticElement("No rhs on predicate", "Nothing cannot imply something");
            }
            else if (rhs is InvalidSyntacticElement)
            {
                return new InvalidSyntacticElement("Invalid right hand side of proposition", ((InvalidSyntacticElement)rhs).Message);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        private static int IndexOfImplies( List<string> elements)
        {
            int index = 0;
            foreach( string element in elements )
            {
                if( string.Compare(element,"implies", StringComparison.OrdinalIgnoreCase) ==0)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        private static int IndexOfIfAndOnlyIf(List<string> elements)
        {
            int index = 0;
            foreach (string element in elements)
            {
                if (string.Compare(element, "IfAndOnlyIf", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        private static int CountOfPropositionQualifiers(List<string> elements)
        {
            int elementCount = elements.Where(x =>
                   ((string.Compare(x, "implies", StringComparison.OrdinalIgnoreCase) == 0) ||
                    (string.Compare(x, "IfAndOnlyIf", StringComparison.OrdinalIgnoreCase) == 0))
                    ).Count();
            return elementCount;

        }
    }
}
