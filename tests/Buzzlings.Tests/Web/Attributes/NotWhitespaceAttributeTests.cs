using Buzzlings.Web.Attributes;

namespace Buzzlings.Tests.Web.Attributes
{
    public class NotWhitespaceAttributeTests
    {
        private readonly NotWhitespaceAttribute _notWhitespaceAttribute;

        public NotWhitespaceAttributeTests()
        {
            _notWhitespaceAttribute = new NotWhitespaceAttribute();
        }

        [Theory]
        [InlineData("Value")]
        [InlineData("Value   ")]
        [InlineData("   Va  lue   ")]
        public void IsValid_WhenValueIsNotPureWhitespace_ShouldReturnTrue(string value)
        {
            Assert.True(_notWhitespaceAttribute.IsValid(value));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("     ")]
        public void IsValid_WhenValueIsOnlyWhitespace_ShouldReturnFalse(string value)
        {
            Assert.False(_notWhitespaceAttribute.IsValid(value));
        }
    }
}
