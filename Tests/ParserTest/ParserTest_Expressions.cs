using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZedLang;
using System.Collections.Generic;
using ZedLangSyntacticElements;

namespace ParserTest
{
    [TestClass]
    public class ParserTest_Expressions
    {
        [TestMethod]
        public void ExpressionParser_SimplePredicate_ReturnsExpression()
        {
            string line = "bob";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(parseResult.GetType(), typeof(PredicateExpression));
            Assert.AreEqual("bob", ((ExpressionSyntacticElement)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_DoublePredicate_ReturnsInvalidSyntacticElement()
        {
            string line = "bob dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_SimpleAnd_ReturnsExpression()
        {
            string line = "bob and dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(AndExpression), parseResult.GetType());
            Assert.AreEqual("(bob and dave)", ((AndExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleOr_ReturnsExpression()
        {
            string line = "bob or dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(OrExpression), parseResult.GetType());
            Assert.AreEqual("(bob or dave)", ((OrExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleXor_ReturnsExpression()
        {
            string line = "bob xor dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(XorExpression), parseResult.GetType());
            Assert.AreEqual("(bob xor dave)", ((XorExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleNot_ReturnsExpression()
        {
            string line = "not dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(NotExpression), parseResult.GetType());
            Assert.AreEqual("(not dave)", ((NotExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleNotWithBrackets_ReturnsExpression()
        {
            string line = "not( dave)";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(NotExpression), parseResult.GetType());
            Assert.AreEqual("(not dave)", ((NotExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleNotInBrackets_ReturnsExpression()
        {
            string line = "(not dave)";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(NotExpression), parseResult.GetType());
            Assert.AreEqual("(not dave)", ((NotExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_SimpleNot_WithRHS_ReturnsInvalidSyntacticExpression()
        {
            string line = "bob not dave";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_JustBrackets_ReturnsInvalidSyntax()
        {
            string line = "()";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_PredicateNotExpression_ReturnsInvalidSyntax()
        {
            string line = "P (not(Q))";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_SimpleNotInMultiBrackets_ReturnsExpression()
        {
            string line = "((not)(dave))";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_bracketedPandQorbracketedAandB_ReturnsExpression()
        {
            string line = "(P and Q) or (A and B)";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(OrExpression), parseResult.GetType());
            Assert.AreEqual("((P and Q) or (A and B))", ((OrExpression)parseResult).Text);
        }
        [TestMethod]
        public void ExpressionParser_AandBPandBQ_ReturnsExpression()
        {
            string line = "A and ( P and ) Q";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_AandBnoEndBrace_ReturnsExpression()
        {
            string line = "(A and B";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
        [TestMethod]
        public void ExpressionParser_AandBnoStartBrace_ReturnsExpression()
        {
            string line = "A and B)";
            SyntacticElement parseResult;
            parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }

    }
}
