namespace FriendlyLocale.Tests.Units
{
    using FriendlyLocale.Extensions;
    using NUnit.Framework;

    [TestFixture]
    public class OskarFormatterTests
    {
        private static string Format(string format, object o)
        {
            return format.InjectNamedFormats(o);
        }

        [Test]
        public void Format_WithDoubleEscapedEndFormatBrace_ThrowsFormatException()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{foo}}}}bar", o);

            // Assert
            Assert.AreEqual("123.45}}}bar", result);
        }

        [Test]
        public void Format_WithEmptyString_ReturnsEmptyString()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format(string.Empty, o);

            // Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void Format_WithNullString_ReturnsNull()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format(null, o);

            // Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBrace_FormatsCorrectly()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{foo}}bar", o);

            // Assert
            Assert.AreEqual("123.45}bar", result);
        }

        [Test]
        public void Format_WithEndBraceFollowedByEscapedEndFormatBraceWhichTerminatesString_FormatsCorrectly()
        {
            var o = new {foo = 123.45};

            // Act
            var result = Format("{foo}}}", o);

            // Assert
            Assert.AreEqual("123.45}}", result);
        }

        [Test]
        public void Format_WithEscapeSequence_EscapesInnerCurlyBraces()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{{{foo}}}", o);

            // Assert
            Assert.AreEqual("{{123.45}}", result);
        }

        [Test]
        public void Format_WithFormatNameNotInObject_ThrowsFormatException()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{bar}", o);

            // Assert
            Assert.AreEqual("{bar}", result);
        }

        [Test]
        public void Format_WithFormatType_ReturnsFormattedExpression()
        {
            var o = new {foo = 123.45};

            // Act
            var result = Format("{foo:#.#}", o);

            // Assert
            Assert.AreEqual("123.5", result);
        }

        [Test]
        public void Format_WithNoEndFormatBrace_ReturnOriginal()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{bar", o);

            // Assert
            Assert.AreEqual("{bar", result);
        }

        [Test]
        public void Format_WithNoFormats_ReturnsFormatStringAsIs()
        {
            var o = new {foo = 123.45};

            // Act
            var result = Format("a b c", o);

            // Assert
            Assert.AreEqual("a b c", result);
        }

        [Test]
        public void StringFormat_WithDoubleEscapedCurlyBraces_DoesNotFormatString()
        {
            // Arrange
            var o = new {foo = 123.45};

            // Act
            var result = Format("{{{{foo}}}}", o);

            // Assert
            Assert.AreEqual("{{{123.45}}}", result);
        }

        [Test]
        public void StringFormat_WithMultipleExpressions_FormatsThemAll()
        {
            // Arrange
            var o = new {foo = 123.45, bar = 42, baz = "hello"};

            // Act
            var result = Format("{foo} {foo} {bar}{baz}", o);

            // Assert
            Assert.AreEqual("123.45 123.45 42hello", result);
        }
    }
}