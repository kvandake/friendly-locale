namespace FriendlyLocale.Tests.Units
{
    using FriendlyLocale.Parser;
    using NUnit.Framework;

    [TestFixture]
    public class YParserTests
    {
        [Test]
        public void Check_Parser()
        {
            const string localeContent = @"
parent:
    child: value
";

            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.child");
            Assert.AreEqual("value", value);
        }

        [Test]
        public void Check_Alias()
        {
            const string localeContent = @"
buttons: &BUTTONS
  alias_button: AliasButton 

parent:
    child: value
    <<: *BUTTONS
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.alias_button");
            Assert.AreEqual("AliasButton", value);
        }

        [Test]
        public void Check_Sequence()
        {
            const string localeContent = @"
parent:
    children:
        - child_1
        - child_2
        - child_3
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.children.child_3");
            Assert.NotNull(value);
        }

        [Test]
        public void Check_Mapping()
        {
            const string localeContent = @"
parent:
    child: value
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.child.value");
            Assert.NotNull(value);
        }

        [Test]
        public void Check_Nested_Mapping()
        {
            const string localeContent = @"
parent:
    child: value
    nested_map:
        nested_key: nested_value
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.nested_map.nested_key");
            Assert.AreEqual("nested_value", value);
        }

        [Test]
        public void Check_AlsoNested_Mapping_1()
        {
            const string localeContent = @"
parent:
    child: value
    nested_map:
        nested_key: nested_value
        also_map:
            also_key: also_value
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.nested_map.also_map.also_key");
            Assert.AreEqual("also_value", value);
        }

        [Test]
        public void Check_AlsoNested_Mapping_2()
        {
            const string localeContent = @"
parent:
    child: value
    nested_map:
        also_map:
            also_key: also_value
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.nested_map.also_map.also_key");
            Assert.AreEqual("also_value", value);
        }

        [Test]
        public void Check_Hard_Mapping_1()
        {
            const string localeContent = @"
parent:
  Test1:
    Test2:
      Test3: Test3
";
            var parser = new YParser(localeContent);
            var value = parser.FindValue("parent.Test1.Test2.Test3");
            Assert.AreEqual("Test3", value);
        }

        [Test]
        public void Check_Hard_Mapping_2()
        {
            const string localeContent = @"
parent:
    Title: En TitleValue
    Locale: en
    Buttons:
        SubmitButton: En SubmitButtonValue
    Test1:
        Test2:
            Test3: Test3 Value

        Test4: Test4 Value
        # Test Comment
        Test5:
            Test6: Test5 Value
            Test7:
                Test8: Test8 Value
       
";
            var parser = new YParser(localeContent);
            var valueTest3 = parser.FindValue("parent.Test1.Test2.Test3");
            var valueTest8 = parser.FindValue("parent.Test1.Test5.Test7.Test8");
            Assert.AreEqual("Test3 Value", valueTest3);
            Assert.AreEqual("Test8 Value", valueTest8);
        }

        [Test]
        public void Check_Nested_Alias()
        {
            const string localeContent = @"
default: &DEFAULT
    default: Default Value
parent:
  Test1:
    Test2:
      Test3: Test3
    <<: *DEFAULT
";
            var parser = new YParser(localeContent);
            var valueDefault = parser.FindValue("parent.Test1.default");
            Assert.AreEqual("Default Value", valueDefault);
        }

        [Test]
        public void Check_Nested_Alias_With_Mapping()
        {
            const string localeContent = @"
default: &DEFAULT
  default: Default Value
  def_mapping:
    key_mapping1:
      nested_key_mapping:
        nested_nested_key: Nested Key Key
    key_mapping2:
      nested_key_mapping: Nested Key
    
parent:
  Test1:
    Test2:
      Test3: Test3
    <<: *DEFAULT
";
            var parser = new YParser(localeContent);
            var valueDefault = parser.FindValue("parent.Test1.def_mapping.key_mapping2.nested_key_mapping");
            Assert.AreEqual("Nested Key", valueDefault);  
        }

        [Test]
        public void Check_Two_Root_Nodes()
        {
            const string localeContent = @"
parent1:
  child1: Child1 Value

parent2:
  child2: Child2 Value

";
            var parser = new YParser(localeContent);
            var valueChild1 = parser.FindValue("parent1.child1");
            var valueChild2 = parser.FindValue("parent2.child2");
            Assert.AreEqual("Child1 Value", valueChild1);
            Assert.AreEqual("Child2 Value", valueChild2);
        }

        [Test]
        public void Check_Two_Root_Nodes_With_Alias()
        {
            const string localeContent = @"
default: &DEFAULT
  default: Default Value

parent1:
  child1: Child1 Value
  <<: *DEFAULT

parent2:
  child2: Child2 Value
  <<: *DEFAULT
";
            var parser = new YParser(localeContent);
            var valueChild1 = parser.FindValue("parent1.child1");
            var valueDefault1 = parser.FindValue("parent1.default");
            var valueChild2 = parser.FindValue("parent2.child2");
            var valueDefault2 = parser.FindValue("parent2.default");
            Assert.AreEqual("Child1 Value", valueChild1);
            Assert.AreEqual("Child2 Value", valueChild2);
            Assert.AreEqual("Default Value", valueDefault1);
            Assert.AreEqual("Default Value", valueDefault2);
        }

        [Test]
        public void Check_Skip_Comments()
        {
            const string localeContent = @"
# Comment 1
parent:
  # Comment 2
  Test1:
    # Comment 3
    Test2:
      # Comment 4
      Test3: Test3 Value
    # Comment 5
    Test4: Test4 Value
";
            var parser = new YParser(localeContent);
            var valueTest3 = parser.FindValue("parent.Test1.Test2.Test3");
            var valueTest4 = parser.FindValue("parent.Test1.Test4");
            Assert.AreEqual("Test3 Value", valueTest3);
            Assert.AreEqual("Test4 Value", valueTest4);
        }

        [Test]
        public void Check_MultipleContents()
        {
            const string localeContent1 = @"
parent:
    child: value1
";
            
            const string localeContent2 = @"
parent:
    child: value2
";
            
            var parser = new YParser(localeContent1, localeContent2);
            var value = parser.FindValue("parent.child");
            Assert.AreEqual("value2", value);
        }
    }
}