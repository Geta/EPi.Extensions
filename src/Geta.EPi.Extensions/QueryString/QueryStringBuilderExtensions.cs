namespace Geta.EPi.Extensions.QueryString
{
    /// <summary>
    /// Extensions for QueryStringBuilder
    /// </summary>
    public static class QueryStringBuilderExtensions
    {
        /// <summary>
        /// Adds query string parameter to query URL encoded if condition is met.
        /// </summary>
        /// <param name="builder">Instance of QueryStringBuilder.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public static QueryStringBuilder AddIf(
            this QueryStringBuilder builder,
            bool condition,
            string name,
            string value)
        {
            return condition ? builder.Add(name, value) : builder;
        }

        /// <summary>
        /// Adds query string parameter to query URL encoded if condition is met.
        /// </summary>
        /// <param name="builder">Instance of QueryStringBuilder.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public static QueryStringBuilder AddIf(
            this QueryStringBuilder builder,
            bool condition,
            string name,
            object value)
        {
            return condition ? builder.Add(name, value) : builder;
        }

        /// <summary>
        /// Adds a segment at the end of the URL encoded if condition is met.
        /// </summary>
        /// <param name="builder">Instance of QueryStringBuilder.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="segment">Name of the segment.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public static QueryStringBuilder AddSegmentIf(
            this QueryStringBuilder builder,
            bool condition,
            string segment)
        {
            return condition ? builder.AddSegment(segment) : builder;
        }

        /// <summary>
        /// Removes query string parameter from query if condition is met.
        /// </summary>
        /// <param name="builder">Instance of QueryStringBuilder.</param>
        /// <param name="condition">Condition.</param>
        /// <param name="name">Name of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public static QueryStringBuilder RemoveIf(
            this QueryStringBuilder builder,
            bool condition,
            string name)
        {
            return condition ? builder.Remove(name) : builder;
        }
    }
}