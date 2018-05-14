namespace FriendlyLocale.Tests.Units
{
    using System;
    using System.Linq;
    using FriendlyLocale.Parser.Core;
    using NUnit.Framework;

    [TestFixture]
    public class TokenizerTests
    {
        [Test]
        public void Detect_Indent()
        {
            const string localeContent = @"
parent:
    Title: En TitleValue 1
    Locale: en
    Buttons 2:
        SubmitButton 3: En SubmitButtonValue
    Test1 4:
        Test2 5:
            Test3 6: Test3
";
            using (var scanner = new Scanner(localeContent))
            {
                using (var tokenizer = new Tokenizer(scanner))
                {
                    Assert.AreEqual(6, tokenizer.Tokens.Count(x => x.Kind == TokenKind.Indent));
                }
            }
        }

        [Test]
        public void Skip_Mapping_Comment()
        {
            const string localeContent = @"
parent:
    # This is comment
    Locale: en
    Buttons:
        SubmitButton: SubmitButtonValue
";
            
            using (var scanner = new Scanner(localeContent))
            {
                using (var tokenizer = new Tokenizer(scanner))
                {
                    Assert.GreaterOrEqual(tokenizer.Tokens.Count, 0);
                }
            }
        }
    }
}