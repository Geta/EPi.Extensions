namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Constants used in EditorHelpFor and EditButton HTML helpers.
    /// </summary>
    public class MetadataConstants
    {
        /// <summary>
        ///     Constants used in EditorHelpFor HTML helpers.
        /// </summary>
        public class EditorHelp
        {
            /// <summary>
            ///     HelpText metadata property name.
            /// </summary>
            public const string HelpTextPropertyName = "EditorHelpText";

            /// <summary>
            ///     ShowInSummary metadata property  name.
            /// </summary>
            public const string ShowInSummaryPropertyName = "EditorHelpShowInSummary";
        }

        /// <summary>
        ///     Constants used in EditButtonFor HTML Helpers.
        /// </summary>
        public class EditButton
        {
            /// <summary>
            ///     IconCssClass metadata property name.
            /// </summary>
            public const string IconCssClassPropertyName = "EditButtonIconCssClass";

            /// <summary>
            ///     ShowInGroup metadata property name.
            /// </summary>
            public const string ShowInGroupPropertyName = "EditButtonShowInGroup";

            /// <summary>
            ///     TriggerFullRefresh metadata property name.
            /// </summary>
            public const string TriggerFullRefreshPropertyName = "EditButtonTriggerFullRefresh";

            /// <summary>
            ///     ButtonLabel metadata property name.
            /// </summary>
            public const string ButtonLabel = "EditButtonLabel";
        }
    }
}