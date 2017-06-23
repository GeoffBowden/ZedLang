using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZedLang
{
    public enum State { Known, Unknown }
    public abstract class SyntacticElement
    {
        public virtual State State { get { return Value == null ? State.Unknown : State.Known; } }
        protected bool? value = null;
        public virtual bool? Value { get{ return value; } set { this.value = value; } }
        public virtual bool IsKnown { get { return State == State.Known; } }
    }
    public class NullSyntacticElement : SyntacticElement { }
    public class InvalidSyntacticElement : SyntacticElement
    {
        public InvalidSyntacticElement( string title, string message ):base ()
        {
            Title = title;
            Message = message;
        }
        public readonly string Message;
        public readonly string Title;
    }
    public class PredicateSyntacticElement : SyntacticElement
    {
        public string Predicate;
        public string Name;
    }
    //father of predicate and <name>expressions
    public abstract class ExpressionSyntacticElement : SyntacticElement
    {
        public abstract string Text { get; }
    }
    public class PredicateExpression : ExpressionSyntacticElement
    {
        public string PredicateName;
        public PredicateExpression(string PredicateName):base ()
        {
            this.PredicateName = PredicateName;
        }
        public override string Text { get { return PredicateName; } }
    }
    // expressions are recursive data structures ending in predecate expressions
    public abstract class UnaryExpression : ExpressionSyntacticElement
    {
        public ExpressionSyntacticElement expression;
    }
    public class NotExpression : UnaryExpression 
    {
        public override string Text { get { return "(not " + expression.Text + ")"; } }
    }
    public abstract class BinaryExpression : ExpressionSyntacticElement
    {
        // a predicate expression is a recursive data structure
        // a predicate expression is a [predicate|predicate expression] operation [predicate|predicate expression]  
        public ExpressionSyntacticElement lhs;
        public ExpressionSyntacticElement rhs;
    }
    public class AndExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " and " + rhs.Text + ")"; } }
    } // and
    public class OrExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " or " + rhs.Text + ")"; } }
    } // or
    public class XorExpression : BinaryExpression
    {
        public override string Text { get { return "(" + lhs.Text + " xor " + rhs.Text + ")"; } }
    } // xor
    public abstract class PropositionSyntacticElement : BinaryExpression
    {
        // syntax is x implies y where x and y are valid predicates
        // similiar are all logical connectives
        // so x could be x and z such that x and z imply y
        // same is ifandonlyif which only differs in the truth table
    }
    public class ImplicationSyntacticElement : PropositionSyntacticElement
    {
        public override string Text { get { return "If " + lhs.Text + " Then " + rhs.Text ; } }
    }
    public class IfAndOnlyIfSyntacticElement : PropositionSyntacticElement
    {
        public override string Text { get { return "If and only if " + lhs.Text + " Then " + rhs.Text ; } }
    }


    public class CommentSyntacticElement : SyntacticElement
    {   // always on a new line, syntax: note <message>
        public CommentSyntacticElement(string text):base ()
        {
            Text = text;
        }
        public readonly string Text;
    }
}
