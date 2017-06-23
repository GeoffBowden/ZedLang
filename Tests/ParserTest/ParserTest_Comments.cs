using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZedLang;
using ZedLangSyntacticElements;

namespace ParserTest
{
    [TestClass]
    public class ParserTest_Comments
    {
        [TestMethod]
        public void LineParser_ValidCommentDefinition_ReturnsComment()
        {   // syntax of a comment, each comment must start a new line,
            // no inline comments are allowed. comments are denoted by
            // note: there can be no space between the note and the :
            string line;
            line = "\tnote: p and q are mental";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(CommentSyntacticElement), parseResult.GetType());
            Assert.AreEqual("p and q are mental", ((CommentSyntacticElement)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_ValidCommentDefinition_nospaceatcolon_ReturnsComment()
        {   // syntax of a comment, each comment must start a new line,
            // no inline comments are allowed. comments are denoted by
            // note: there can be no space between the note and the :
            string line;
            line = "\tnote:p and q are mental";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(CommentSyntacticElement), parseResult.GetType());
            Assert.AreEqual("p and q are mental", ((CommentSyntacticElement)parseResult).Text);
        }
        [TestMethod]
        public void LineParser_InValidCommentDefinition_ReturnsInvalidSyntacticElement()
        {   // syntax of a comment, each comment must start a new line,
            // no inline comments are allowed. comments are denoted by
            // note: there can be no space between the note and the :
            string line;
            line = "\tnote :p and q are mental";
            SyntacticElement parseResult = LineParser.Parse(line);
            Assert.AreEqual(typeof(InvalidSyntacticElement), parseResult.GetType());
        }
    }
}
