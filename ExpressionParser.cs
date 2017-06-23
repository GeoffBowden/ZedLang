using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedLangSyntacticElements;

namespace ZedLang
{
    public static class ExpressionParser
    {
        public static readonly List<string> Expressions = new List<string>() { "not", "and", "or", "xor" };
        public static bool IsAnExpression(List<string> Expression)
        {
            CultureInfo culture = new CultureInfo("en-GB");
            foreach (string expression in Expressions)
            {
                foreach (string word in Expression)
                {
                    if (culture.CompareInfo.IndexOf(expression, word, CompareOptions.IgnoreCase) >= 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        // returns a PredicateExpression : ExpressionSyntacticElement
        //    // a predicate expression is a recursive data structure
        //    // a predicate expression is a [predicate|predicate expression] operation [predicate|predicate expression]  
        //    public PredicateExpression lhs;
        //    public Operation operation;
        //    public PredicateExpression rhs;
        //    public override bool? Value { get { return value; } set { this.value = value; } }
        //  expressions may be held in brackets, operations not
        // ad adicio: ( must be followed by a predicate and preceded by an operation
        //            ) must be followed by a operation and preceded by an predicate 
        // bracket must also balance
        //   z and not( p and q ) or ( ( not ( a and b ) ) or ( c and d ) )
        public static SyntacticElement Parse(List<string> lineElements)
        {
            if (lineElements.Count() == 0)
            {
                return new InvalidSyntacticElement("Invalid expression", "Expression has ended in an operation");
            }
            if ( lineElements.Count() == 1 )
            {
                if( lineElements[0].IsaReservedWord() )
                {
                    return new InvalidSyntacticElement("Invalid expression", "Expression has ended in an operation");
                }
                else
                {
                    return new PredicateExpression(lineElements[0]);
                }
            }
            // find the index of the first reserved word
            // return the left hand side as a string and the right hand side as a string
            // we will need to be thinking about recursion here.
            int index = IndexOfFirstKeyWord(lineElements);
            if ( index == -1 )
            {
                return new InvalidSyntacticElement("Invalid expression syntax", "No expression found");
            }
            else if ( index == -2 ) // expression surrounded by 
            {
                // strip the covering brackets
                return Parse(lineElements.Skip(1).Take(lineElements.Count() - 2).ToList());
            }
            else
            {
                ExpressionSyntacticElement ese = GetExpressionFrom(lineElements[index]);
                if (ese is NotExpression)
                {
                    if (index == 0)
                    {
                        SyntacticElement exp = Parse(lineElements.Skip(index + 1).ToList<string>());
                        if (exp is ExpressionSyntacticElement)
                        {
                            ((NotExpression)(ese)).expression = (ExpressionSyntacticElement)exp;
                        }
                        else if (exp is InvalidSyntacticElement)
                        {
                            return exp;
                        }
                        else
                        {
                            return new InvalidSyntacticElement("Invalid expression syntax", "Unknown expression found not");
                        }
                        return ese;
                    } 
                    else // the not expression is not the first token unless first token is a bracket?
                    {
                        return new InvalidSyntacticElement("Invalid expression syntax", "not must be the first token in the string unless it is (");
                    }
                }
                else
                {
                    SyntacticElement lhs = Parse(lineElements.Take(index).ToList<string>());
                    SyntacticElement rhs = Parse(lineElements.Skip(index + 1).ToList<string>());
                    if ( ( lhs is ExpressionSyntacticElement )&&
                         ( rhs is ExpressionSyntacticElement ) )
                    {
                        if (ese is AndExpression)
                        {   // 0 1 3 4 (idex) 
                            ((AndExpression)(ese)).lhs = (ExpressionSyntacticElement)lhs;
                            ((AndExpression)(ese)).rhs = (ExpressionSyntacticElement)rhs;
                        }
                        else if (ese is OrExpression)
                        {
                            ((OrExpression)(ese)).lhs = (ExpressionSyntacticElement)lhs;
                            ((OrExpression)(ese)).rhs = (ExpressionSyntacticElement)rhs;
                        }
                        else if (ese is XorExpression)
                        {
                            ((XorExpression)(ese)).lhs = (ExpressionSyntacticElement)lhs;
                            ((XorExpression)(ese)).rhs = (ExpressionSyntacticElement)rhs;
                        }
                        else
                        {
                            return new InvalidSyntacticElement("Invalid expression syntax", "Unknown expression found");
                        }
                        return ese;
                    }
                    else
                    {
                        return new InvalidSyntacticElement("Invalid expression syntax", "Unknown expression found");
                    }
                }
            }
        }

        private static int IndexOfFirstKeyWord(List<string> lineElements)
        {
            int index = 0;
            int nestedExpressionCount = 0;
            foreach( string word in lineElements)
            {
                if( word.IsAnOpenBracket())
                {
                    nestedExpressionCount += 1;
                }
                else if ( word.IsACloseBracket() )
                {
                    nestedExpressionCount -= 1;
                    if ( ( nestedExpressionCount == 0 ) && ( index == lineElements.Count -1 ) )
                    {
                        return -2;
                    } 
                }
                else if ( IsTheRootExpression( word, nestedExpressionCount ) )
                {
                    return index;
                }
                index += 1;
            }
            return -1;
        }

        private static bool IsAnExpression( string word )
        {
            foreach (string expression in Expressions)
            {
                if (string.Compare(word, expression, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsTheRootExpression(string word, int nestedExpression)
        {
            return (IsAnExpression(word)) && (nestedExpression == 0);
        }

        private static ExpressionSyntacticElement GetExpressionFrom( string operation )
        {
            if( IsAnExpression( operation ) )
            {
                if ( string.Compare( operation, "and", StringComparison.CurrentCultureIgnoreCase ) == 0 )
                {
                    return new AndExpression();
                }
                if (string.Compare(operation, "or", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return new OrExpression();
                }
                if (string.Compare(operation, "xor", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return new XorExpression();
                }
                if (string.Compare(operation, "not", StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    return new NotExpression();
                }
                throw new NotImplementedException();
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
