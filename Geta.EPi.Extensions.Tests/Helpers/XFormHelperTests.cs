using EPiServer.HtmlParsing;
using FluentAssertions;
using Geta.EPi.Extensions.Helpers;
using Xunit;

namespace Geta.EPi.Extensions.Tests.Helpers
{
    public class XFormHelperTests
    {
        public class CleanupXFormHtmlMarkupTests
        {
            [Fact]
            public void it_should_return_fragment_with_same_name_when_pass_non_table_fragment()
            {
                var fragment = new ElementFragment {Name = "span"};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment);

                result.Name.Should().Be(fragment.Name);
            }

            [InlineData("table")]
            [InlineData("td")]
            [InlineData("tr")]
            [InlineData("tbody")]
            [InlineData("thead")]
            [Theory]
            public void it_should_return_div_fragment_when_pass_table_fragment(string tagName)
            {
                var fragment = new ElementFragment {Name = tagName};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment);

                result.Name.Should().Be("div");
            }

            [Fact]
            public void it_should_return_same_fragment_when_pass_non_element_fragment_with_table_name()
            {
                var fragment = new TextFragment {Name = "table"};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment);

                result.Should().BeSameAs(fragment);
                result.Name.Should().Be(fragment.Name);
            }

            [Fact]
            public void it_should_remove_valign_attribute()
            {
                var fragment = new ElementFragment {Name = "table"};
                fragment.Attributes.Add(new AttributeFragment {Name = "valign"});

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment);

                result.As<ElementFragment>().Attributes.Should().NotContain(x => x.Name == "valign");
            }

            [Fact]
            public void it_should_not_add_class_attribute_when_pass_class_attribute_false()
            {
                var fragment = new ElementFragment {Name = "table"};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment, false);

                result.As<ElementFragment>().Attributes.Should().NotContain(x => x.Name == "class");
            }

            [Fact]
            public void it_should_add_class_attribute_when_pass_class_attribute_true()
            {
                var fragment = new ElementFragment {Name = "table"};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment, true);

                result.As<ElementFragment>()
                    .Attributes.Should()
                    .Contain(x => x.Name == "class");
            }

            [InlineData("table", "xform-table")]
            [InlineData("tr", "xform-row")]
            [InlineData("td", "xform-col")]
            [InlineData("tbody", "xform-body")]
            [InlineData("thead", "xform-thead")]
            [Theory]
            public void it_should_add_table_class_attribute_for_table_element(string tagName, string expectedClassName)
            {
                var fragment = new ElementFragment {Name = tagName};

                var result = XFormHelper.CleanupXFormHtmlMarkup(fragment);

                result.As<ElementFragment>()
                    .Attributes.Should()
                    .Contain(x => x.Name == "class" && x.Value == expectedClassName);
            }
        }
    }
}